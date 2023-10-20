using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DotNetTestService.Core.CheckRuns
{
    public sealed class DockerHelper
    {
        private readonly string _cachePath;
        private readonly ILogger<DockerHelper> _logger;
        private readonly int _runNo;
        private readonly int _timeoutMinutes;

        public DockerHelper(ILogger<DockerHelper> logger, int runNo, int timeoutMinutes, string cachePath)
        {
            this._logger = logger;
            this._runNo = runNo;
            this._timeoutMinutes = timeoutMinutes;
            this._cachePath = cachePath;
        }

        public async Task<(bool timeout, Results? results)> RunTestContainer(string workDir)
        {
            var cancelled = false;
            workDir = workDir.TrimEnd('\\').TrimEnd('/');
            var containerName = $"check_{this._runNo}";
            var src = new CancellationTokenSource(TimeSpan.FromMinutes(this._timeoutMinutes));
            var processInfo =
                new ProcessStartInfo("docker",
                    $"run --rm --name {containerName} -v {workDir}:/usr/src/project -v {this._cachePath}:/usr/cache mhaslinger/dotnetrunner")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

            var proc = new Process
            {
                StartInfo = processInfo,
                EnableRaisingEvents = true
            };
            proc.OutputDataReceived += HandleOutput;

            var sw = Stopwatch.StartNew();
            proc.Start();
            proc.BeginOutputReadLine();

            try
            {
                await proc.WaitForExitAsync(src.Token);
            } catch (OperationCanceledException)
            {
                cancelled = true;
                Log("Stopping docker container due to timeout", LogLevel.Warn);
                await StopDockerContainer(containerName);
            }

            // required to deal with final stdout messages ?!
            // proc.WaitForExit(500);

            var code = proc.ExitCode;
            Log($"Docker process exited with status code {code}", code != 0 ? LogLevel.Warn : LogLevel.Info);
            proc.Dispose();
            Log($"Docker task completed in {sw.Elapsed}", LogLevel.Info);

            var resultsFile = Directory.EnumerateFiles($"{workDir}/results", "*.trx").FirstOrDefault()
                              ?? Path.Combine(workDir, "na.trx");

            var rh = new ResultFileHelper(resultsFile);
            return (cancelled, rh.GetResults());
        }

        private static async Task StopDockerContainer(string name)
        {
            var processInfo =
                new ProcessStartInfo("docker", $"rm -f {name}")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false
                };

            var proc = new Process
            {
                StartInfo = processInfo,
                EnableRaisingEvents = true
            };

            proc.Start();
            await proc.WaitForExitAsync();
            proc.Dispose();
        }

        private void Log(string message, LogLevel level = LogLevel.Debug)
        {
            message = $"[Run #{this._runNo}][Docker] {message}";
            switch (level)
            {
                case LogLevel.Info:
                    this._logger.LogInformation(message);
                    break;
                case LogLevel.Warn:
                    this._logger.LogWarning(message);
                    break;
                case LogLevel.Debug:
                    this._logger.LogInformation(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }

        private void HandleOutput(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Data))
            {
                return;
            }

            Log(e.Data);
        }

        private enum LogLevel
        {
            Info,
            Warn,
            Debug
        }
    }
}