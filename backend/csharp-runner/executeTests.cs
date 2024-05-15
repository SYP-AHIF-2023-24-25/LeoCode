using System.Diagnostics;

namespace csharp_runner
{
    public class executeTests
    {
        public static async Task<string> runCSharp(string exerciseName, string templateFilePath, string filePathForRandomDirectory, string code, string fileName)
        {
            Console.WriteLine("in Methode runCSharp in execute Tests");
            string solutionDir = createTempDir(filePathForRandomDirectory);
            Console.WriteLine($"Solution Directory: {solutionDir}");
            await CopyAsync(templateFilePath, solutionDir);
            int exitCode = await RunCommandsAsyncCommandLine(solutionDir, $"ln -s /usr/src/app/config/nuget.config {solutionDir}/{exerciseName}/nuget.config");
            solutionDir = $@"{solutionDir}/{exerciseName}";
            await ReplaceCodeAsync(solutionDir, code, fileName, exerciseName);

            Console.WriteLine("dotnet restore anfangen");
            Console.Write($"Solution dir für ausführen von dotnet restore: {solutionDir}");
            exitCode = await RunCommandsAsyncForDotnet(solutionDir, "test -l:trx;LogFileName=TestOutput.xml");
            Console.WriteLine($"tests ausführen fertig und code ist: {exitCode}");

            string testOutput = await File.ReadAllTextAsync(Path.Combine(solutionDir, $"{exerciseName}Tests/TestResults/TestOutput.xml"));
            Console.WriteLine($"TestOutput: {testOutput}");
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

        private static async Task<int> RunCommandsAsyncForDotnet(string solutionDir, string command)
        {
            try
            {
                // Erstellen Sie den Prozess zur Ausführung des dotnet test-Befehls
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = $"{command}", // Argumente für den dotnet test-Befehl
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        WorkingDirectory = solutionDir // Arbeitsverzeichnis festlegen
                    }
                };

                // Ereignishandler für die Ausgabe festlegen
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

                // Starten Sie den Prozess und beginnen Sie mit dem Lesen von stdout/stderr
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                // Warten Sie, bis der Prozess beendet ist
                await process.WaitForExitAsync();

                // Konsolenrückgabewert abrufen
                int exitCode = process.ExitCode;

                // Ergebnisse verarbeiten (optional)
                string output = outputBuilder.ToString();
                string error = errorBuilder.ToString();

                // Hier können Sie die Ausgabe verarbeiten oder sie zurückgeben
                Console.WriteLine("Dotnet test output:");
                Console.WriteLine(output);

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
                return -1; // Rückgabewert für Fehler
            }
        }

        private static async Task<int> RunCommandsAsyncCommandLine(string solutionDir, string command)
        {
            try
            {
                // Erstellen Sie den Prozess zur Ausführung des dotnet test-Befehls
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "/bin/sh",
                        Arguments = $"-c \"{command}\"", // Befehl für das Symbolic Link setzen                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        WorkingDirectory = solutionDir // Arbeitsverzeichnis festlegen
                    }
                };

                // Ereignishandler für die Ausgabe festlegen
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

                // Starten Sie den Prozess und beginnen Sie mit dem Lesen von stdout/stderr
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                // Warten Sie, bis der Prozess beendet ist
                await process.WaitForExitAsync();

                // Konsolenrückgabewert abrufen
                int exitCode = process.ExitCode;

                // Ergebnisse verarbeiten (optional)
                string output = outputBuilder.ToString();
                string error = errorBuilder.ToString();

                // Hier können Sie die Ausgabe verarbeiten oder sie zurückgeben
                Console.WriteLine("Dotnet test output:");
                Console.WriteLine(output);

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
                return -1; // Rückgabewert für Fehler
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
