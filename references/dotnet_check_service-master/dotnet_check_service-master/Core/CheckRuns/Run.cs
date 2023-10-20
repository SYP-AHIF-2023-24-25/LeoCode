using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using DotNetTestService.Core.FileHandling;
using DotNetTestService.Core.Stats;
using DotNetTestService.Model;
using Microsoft.Extensions.Logging;

namespace DotNetTestService.Core.CheckRuns
{
    public sealed class Run : IRun
    {
        private readonly ILogger<DockerHelper> _dockerLogger;
        private readonly IRunFileHandler _runFileHandler;
        private readonly ILogger<Run> _runLogger;
        private readonly AppSettings _settings;
        private readonly IStatsService _statsService;
        private TimeSpan? _duration;
        private DateTime? _startTime;

        public Run(Guid id, int projectNo, int runNo, IReadOnlyCollection<CodeReplacement> codeReplacements,
            IRunFileHandler runFileHandler, ILogger<Run> runLogger, ILogger<DockerHelper> dockerLogger,
            IStatsService statsService, AppSettings settings)
        {
            Id = id;
            ProjectNo = projectNo;
            RunNo = runNo;
            CodeReplacements = codeReplacements;
            this._runFileHandler = runFileHandler;
            this._runLogger = runLogger;
            this._dockerLogger = dockerLogger;
            this._statsService = statsService;
            this._settings = settings;
        }

        public int ProjectNo { get; }
        public int RunNo { get; }
        private IReadOnlyCollection<CodeReplacement> CodeReplacements { get; }
        public Guid Id { get; }
        public RunStatus Status { get; private set; }
        public FailReason? FailReason { get; private set; }
        public int? PassPercentage { get; private set; }
        public bool? AllPassed { get; private set; }
        public DateTime? TimeDone { get; private set; }

        public TimeSpan Duration
        {
            get
            {
                if (this._duration != null)
                {
                    return this._duration.Value;
                }

                if (this._startTime == null)
                {
                    return TimeSpan.Zero;
                }

                return DateTime.Now - this._startTime.Value;
            }
            private set => this._duration = value;
        }

        public void Start(ConcurrentDictionary<int, Task> runningChecks, string templateDirPath)
        {
            var task = Task.Run(async () =>
            {
                this._startTime = DateTime.Now;
                var sw = Stopwatch.StartNew();
                try
                {
                    string workDir = default!;
                    Status = RunStatus.NotStarted;
                    try
                    {
                        workDir = this._runFileHandler.PrepareWorkDir(templateDirPath, RunNo);
                        await this._runFileHandler.InjectCodeReplacements(workDir, CodeReplacements);
                        await RunCheck(workDir);
                        Status = RunStatus.Completed;
                    }
                    catch (Exception ex)
                    {
                        Status = RunStatus.Failed;
                        FailReason = CheckRuns.FailReason.Unknown;
                        this._runLogger.LogError(ex, "Error while running code compile and test task");
                    }
                    finally
                    {
                        try
                        {
                            this._runFileHandler.CleanUpWorkDir(workDir);
                        }
                        catch (Exception ex)
                        {
                            Status = RunStatus.Failed;
                            FailReason = CheckRuns.FailReason.Unknown;
                            this._runLogger.LogError(ex, "Failed to clean working directory");
                        }
                    }
                }
                finally
                {
                    if (runningChecks.ContainsKey(RunNo))
                    {
                        runningChecks.Remove(RunNo, out _);
                    }

                    var elapsed = sw.Elapsed;
                    this._statsService.AddRunStats(elapsed, Status == RunStatus.Completed);
                    Duration = elapsed;
                    TimeDone = DateTime.Now;
                }
            });
            runningChecks.AddOrUpdate(RunNo, task, (n, t) => task);
        }

        private async Task RunCheck(string workDir)
        {
            Status = RunStatus.Running;
            var (timeout, results) =
                await new DockerHelper(this._dockerLogger, RunNo, this._settings.TimeoutMinutes,
                    this._settings.PackageCacheDir).RunTestContainer(workDir);
            Status = RunStatus.Failed;
            PassPercentage = null;
            AllPassed = null;
            if (timeout)
            {
                FailReason = CheckRuns.FailReason.Timeout;
                return;
            }

            if (results == null)
            {
                FailReason = CheckRuns.FailReason.Unknown;
                return;
            }

            if (!results.Compiled)
            {
                FailReason = CheckRuns.FailReason.CompileError;
                return;
            }

            Status = RunStatus.Completed;
            FailReason = null;
            PassPercentage = results.PassPercentage;
            AllPassed = results.AllPassed;
        }
    }
}