using System.Diagnostics;

namespace csharp_runner
{
    public class executeTests
    {
        public static async Task<string> runCSharp(string exerciseName, string templateFilePath, string filePathForRandomDirectory, string code, string fileName)
        {
            string solutionDir = createTempDir(filePathForRandomDirectory);
            await CopyAsync(templateFilePath, solutionDir);
            solutionDir = $@"{solutionDir}/{exerciseName}";
            await ReplaceCodeAsync(solutionDir, code, fileName, exerciseName);

            int exitCode = await RunCommandsAsync(solutionDir, "restore");
            exitCode = await RunCommandsAsync(solutionDir, "test -l:trx;LogFileName=TestOutput.xml");

            string testOutput = await File.ReadAllTextAsync(Path.Combine(solutionDir, $"{exerciseName}Tests/TestResults/TestOutput.xml"));
            return testOutput;
        }

        public static async Task<string> testTemplate(string path, string exerciseName)
        {
            await RunCommandsAsync(path, "restore");
            await RunCommandsAsync(path, "test -l:trx;LogFileName=TestOutput.xml");
            string testOutput = await File.ReadAllTextAsync(Path.Combine(path, $"{exerciseName}Tests/TestResults/TestOutput.xml"));
            return testOutput;
        }

        private static async Task<int> RunCommandsAsync(string solutionDir, string command)
        {
            try
            {
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = $"{command}",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        WorkingDirectory = solutionDir
                    }
                };

                var outputBuilder = new System.Text.StringBuilder();
                var errorBuilder = new System.Text.StringBuilder();

                process.OutputDataReceived += (sender, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        outputBuilder.AppendLine(args.Data);
                    }
                };

                process.ErrorDataReceived += (sender, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        errorBuilder.AppendLine(args.Data);
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                await process.WaitForExitAsync();

                int exitCode = process.ExitCode;

                string output = outputBuilder.ToString();
                string error = errorBuilder.ToString();

                if (!string.IsNullOrEmpty(error))
                {
                    Console.WriteLine("Dotnet test errors:");
                    Console.WriteLine(error);
                }

                return exitCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return -1;
            }
        }

        private static async Task ReplaceCodeAsync(string solutionDir, string code, string fileName, string exerciseName)
        {
            string filePath = $@"{solutionDir}/{exerciseName}/{fileName}";
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                await writer.WriteLineAsync(code);
            }
        }

        private static string createTempDir(string filePathForRandomDirectory)
        {
            string randomFolderName = Path.GetRandomFileName();
            string fullPath = Path.Combine(filePathForRandomDirectory, randomFolderName);
            Directory.CreateDirectory(fullPath);

            Console.WriteLine($"Random Ordner generiert {fullPath}");

            return fullPath;
        }

        static async Task CopyAsync(string sourceDir, string targetDir)
        {
            Process process = new Process();
            process.StartInfo.FileName = "/bin/cp";
            process.StartInfo.Arguments = $"-r {sourceDir} {targetDir}";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            process.Start();

            Task<string> outputReader = process.StandardOutput.ReadToEndAsync();
            Task<string> errorReader = process.StandardError.ReadToEndAsync();

            await Task.WhenAll(outputReader, errorReader);

            string output = await outputReader;
            string error = await errorReader;

            Console.WriteLine("Output:");
            Console.WriteLine(output);

            Console.WriteLine("Error:");
            Console.WriteLine(error);

            process.WaitForExit();
        }
    }
}
