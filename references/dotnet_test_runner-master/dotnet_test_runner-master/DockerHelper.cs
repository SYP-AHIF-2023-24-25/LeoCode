using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRunner
{
    public sealed class DockerHelper
    {
        public async Task<Results?> RunTestContainer()
        {
            var cwd = Directory.GetCurrentDirectory();
            cwd = @"C:\temp\coderunner\dotnet_test_in_docker";
            var processInfo = new ProcessStartInfo("docker", $"run --rm -v {cwd}:/usr/src/project dotnetrunner");

            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardOutput = true;
            processInfo.RedirectStandardError = true;

            var proc = new Process
            {
                StartInfo = processInfo,
                EnableRaisingEvents = true
            };
            proc.OutputDataReceived += HandleOutput;

            proc.Start();
            proc.BeginOutputReadLine();
            await proc.WaitForExitAsync();

            var code = proc.ExitCode;
            proc.Dispose();

            var resultsFile = Directory.EnumerateFiles($"{cwd}/results", "*.trx").FirstOrDefault();
            if (resultsFile == null)
            {
                resultsFile = Path.Combine(cwd, "na.trx");
            }
            var rh = new ResultFileHelper(resultsFile);
            return rh.GetResults();
        }

        private void HandleOutput(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }
}
