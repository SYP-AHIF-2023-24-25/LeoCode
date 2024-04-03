using System.Diagnostics;

namespace csharp_runner
{
    public class executeTests
    {
        public static async Task<string> runCSharp(string exerciseName, string templateFilePath, string filePathForRandomDirectory, string code, string fileName)
        {
            Console.WriteLine("in Methode runCSharp in execute Tests");
            string solutionDir = createTempDirAndCopyTemplate(templateFilePath, exerciseName, filePathForRandomDirectory);
            Console.WriteLine($"Solution Directory: {solutionDir}");
            replaceCode(solutionDir, code, fileName);

            runCommands(solutionDir, "restore");
            runCommands(solutionDir, "test -l:trx;LogFileName=TestOutput.xml");

            string testOutput = await File.ReadAllTextAsync(Path.Combine(solutionDir, "TestOutput.xml"));
            Console.WriteLine($"TestOutput: {testOutput}");
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

        private static string createTempDirAndCopyTemplate(string templateFilePath, string exerciseName, string filePathForRandomDirectory)
        {
            // Erstelle einen zufälligen Ordnernamen
            string randomFolderName = Path.GetRandomFileName();
            string fullPath = Path.Combine(filePathForRandomDirectory, randomFolderName);
            Directory.CreateDirectory(fullPath);

            Console.WriteLine($"Random Ordner generiert {fullPath}");
            
            CopyAll(new DirectoryInfo(templateFilePath), new DirectoryInfo(fullPath));

            Console.WriteLine("Inhalt erfolgreich kopiert.");

            return fullPath;
        }

        static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            // Kopieren aller Dateien
            foreach (FileInfo file in source.GetFiles())
            {
                string dest = Path.Combine(target.FullName, file.Name);
                file.CopyTo(dest, true);
            }

            // Kopieren aller Unterverzeichnisse
            foreach (DirectoryInfo subDir in source.GetDirectories())
            {
                // Ignorieren von bestimmten Ordnern (z.B. .vs)
                if (subDir.Name.ToLower() == ".vs" || subDir.Name.ToLower() == "obj")
                {
                    continue;
                }
                    

                string dest = Path.Combine(target.FullName, subDir.Name);
                CopyAll(subDir, new DirectoryInfo(dest));
            }
        }
    }
}
