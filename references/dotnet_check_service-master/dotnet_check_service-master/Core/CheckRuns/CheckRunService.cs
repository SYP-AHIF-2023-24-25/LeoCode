using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotNetTestService.Core.FileHandling;
using DotNetTestService.Core.Stats;
using DotNetTestService.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DotNetTestService.Core.CheckRuns
{
    public sealed class CheckRunService : ICheckRunService, IDisposable
    {
        private readonly AppSettings _appSettings;
        private readonly Timer _cleanupTimer;
        private readonly ILogger<DockerHelper> _dockerLogger;
        private readonly IRunFileHandler _runFileHandler;
        private readonly ILogger<Run> _runLogger;
        private readonly ConcurrentDictionary<int, Task> _runningChecks;
        private readonly ConcurrentDictionary<Guid, Run> _runs;
        private readonly IStatsService _statsService;
        private int _lastRunNo;

        public CheckRunService(IRunFileHandler runFileHandler, ILogger<Run> logger, ILogger<DockerHelper> dockerLogger,
            IStatsService statsService, IOptions<AppSettings> settings)
        {
            this._runFileHandler = runFileHandler;
            this._runLogger = logger;
            this._dockerLogger = dockerLogger;
            this._statsService = statsService;
            this._lastRunNo = 0;
            this._runs = new();
            this._runningChecks = new();
            this._cleanupTimer = new(CleanupTimerTick, this, TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(45));
            this._statsService.SetCurrentActiveGetter(() => this._runningChecks.Count);
            this._appSettings = settings.Value;
        }

        public Guid RunCheck(ProjectDefinition projectDef, List<CodeReplacement> codeReplacements)
        {
            var guid = Guid.NewGuid();
            var runNo = GetRunNo();
            var (projectNo, templateDirPath, _) = projectDef;
            var run = new Run(guid, projectNo, runNo, SanitizeCodeReplacements(codeReplacements), this._runFileHandler,
                this._runLogger, this._dockerLogger, this._statsService, this._appSettings);
            this._runs.AddOrUpdate(guid, run, (g, r) => run);
            run.Start(this._runningChecks, templateDirPath);
            return guid;
        }

        public IRun? GetRunInfo(Guid runId)
        {
            return this._runs.TryGetValue(runId, out var run) ? run : null;
        }

        public void Dispose()
        {
            this._cleanupTimer.Dispose();
        }

        private void CleanupTimerTick(object? state)
        {
            var toRemove = this._runs.Values
                .Where(r => r.Status is RunStatus.Completed or RunStatus.Failed)
                .Where(r => r.TimeDone is null || r.TimeDone.Value.AddHours(1) < DateTime.Now)
                .Select(r => r.Id)
                .ToList();
            foreach (var runToRemove in toRemove)
            {
                this._runs.Remove(runToRemove, out _);
            }
        }

        private static IReadOnlyCollection<CodeReplacement> SanitizeCodeReplacements(IEnumerable<CodeReplacement> replacements)
        {
            return replacements.Select(cr =>
            {
                cr.RawCode = cr.RawCode.Replace("\\r\\n", Environment.NewLine).Replace("\\n", Environment.NewLine);
                return cr;
            }).ToList();
        }

        private int GetRunNo() => Interlocked.Increment(ref this._lastRunNo);
    }
}