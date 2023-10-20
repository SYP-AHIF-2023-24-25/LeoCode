using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DotNetTestService.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DotNetTestService.Core.FileHandling
{
    public sealed class RunFileHandler : IRunFileHandler
    {
        private readonly ILogger<RunFileHandler> _logger;
        private readonly AppSettings _settings;

        public RunFileHandler(IOptions<AppSettings> config, ILogger<RunFileHandler> logger)
        {
            this._logger = logger;
            this._settings = config.Value;
        }

        public string PrepareWorkDir(string templatePath, int runNo)
        {
            CheckDirsExists(templatePath);
            var workDirPath = PrepareWorkDir(runNo);
            CopyFiles(templatePath, workDirPath);
            return workDirPath;
        }

        public Task InjectCodeReplacements(string workDirPath, IEnumerable<CodeReplacement> replacements)
        {
            var sfp = new SourceFileProcessor(workDirPath);
            return sfp.PerformReplacements(replacements);
        }

        public void CleanUpWorkDir(string workDirPath)
        {
            Directory.Delete(workDirPath, true);
        }

        private static void CopyFiles(string sourceDir, string targetDir)
        {
            var sourceFiles = Util.GetFilesInDir(sourceDir);
            foreach (var sourceFile in sourceFiles)
            {
                var targetFile = sourceFile.Replace(sourceDir, targetDir);
                var fi = new FileInfo(targetFile);
                if (fi.Directory == null)
                {
                    throw new InvalidOperationException("target file directory null");
                }

                fi.Directory.Create();
                File.Copy(sourceFile, targetFile, true);
            }
        }

        private string PrepareWorkDir(int runNo)
        {
            var workDirPath = Path.Combine(this._settings.RootWorkDir, runNo.ToString());
            if (Directory.Exists(workDirPath))
            {
                this._logger.LogWarning(
                    $"Removing already existing working directory for run # {runNo} before starting run");
                CleanUpWorkDir(workDirPath);
            }

            Directory.CreateDirectory(workDirPath);
            return workDirPath;
        }

        private void CheckDirsExists(string templatePath)
        {
            if (!Directory.Exists(templatePath))
            {
                throw new ArgumentException($"Provided template dir {templatePath} does not exist");
            }

            if (Directory.Exists(this._settings.RootWorkDir))
            {
                return;
            }

            throw new ArgumentException($"Provided root working dir {this._settings.RootWorkDir} does not exist");
        }
    }
}