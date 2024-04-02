using System.Diagnostics;

namespace csharp_runner
{
    public class executeTests
    {
        public static async Task<string> runCSharp(string exerciseName, string templateFilePath, string code, string fileName)
        {
            string solutionDir = createTempDirAndCopyTemplate(templateFilePath, exerciseName);

            replaceCode(solutionDir, code, fileName);

            runCommands(solutionDir, "restore");
            runCommands(solutionDir, "test -l:trx;LogFileName=TestOutput.xml");

            string testOutput = await File.ReadAllTextAsync(Path.Combine(solutionDir, "TestOutput.xml"));
            Console.WriteLine(testOutput);
            return testOutput;
        }

        private static void runCommands(string solutionDir, string command)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"{command}",
                WorkingDirectory = solutionDir,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = psi })
            {
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();
            }
        }

        private static async void replaceCode(string solutionDir, string code, string fileName)
        {
            await File.WriteAllTextAsync(Path.Combine(solutionDir, fileName), code);
        }

        private static string createTempDirAndCopyTemplate(string templateFilePath, string exerciseName)
        {
            string solutionDir = Directory.CreateTempSubdirectory().Name;
            File.Copy(templateFilePath, solutionDir);
            return solutionDir;
        }
    }
}