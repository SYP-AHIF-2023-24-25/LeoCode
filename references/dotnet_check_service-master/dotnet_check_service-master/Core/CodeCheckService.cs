using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotNetTestService.Core.CheckRuns;
using DotNetTestService.Core.FileHandling;
using DotNetTestService.Model;
using Microsoft.Extensions.Logging;

namespace DotNetTestService.Core
{
    public sealed class CodeCheckService : ICodeCheckService
    {
        private readonly ICheckRunService _checkRunService;
        private readonly ILogger<CodeCheckService> _logger;
        private readonly ConcurrentDictionary<int, ProjectDefinition> _projectDefinitions;
        private readonly IProjectDefProvider _projectDefProvider;

        public CodeCheckService(IProjectDefProvider projectDefProvider, ICheckRunService checkRunService,
            ILogger<CodeCheckService> logger)
        {
            this._projectDefProvider = projectDefProvider;
            this._checkRunService = checkRunService;
            this._logger = logger;
            this._projectDefinitions = new();
        }

        public async Task<Guid> SubmitCodeForCheck(int projectNo, List<CodeReplacement> codeReplacements)
        {
            var def = await GetProjectDefinition(projectNo);
            if (def.ReplacementCount != codeReplacements.Count)
            {
                throw new ReplacementCountNotMatchingException(def.ReplacementCount, codeReplacements.Count);
            }

            var runId = this._checkRunService.RunCheck(def, codeReplacements);
            return runId;
        }

        public IRun? CheckRunStatus(Guid runId)
        {
            return this._checkRunService.GetRunInfo(runId);
        }

        public async Task<ImmutableDictionary<string, ImmutableList<string>>> GetTemplateFiles(int projectNo)
        {
            var def = await GetProjectDefinition(projectNo);
            var templateDir = def.TemplateDirPath;
            var sfp = new SourceFileProcessor(templateDir);
            var files = await sfp.ReadFiles();
            var readmeFile = Util.GetFilesInDir(templateDir, ".md")
                .FirstOrDefault(f => f.ToLowerInvariant().Contains("readme"));
            if (readmeFile != null)
            {
                files.Add(Path.GetFileName(readmeFile), (await File.ReadAllLinesAsync(readmeFile)).ToList());
            }

            return files
                .Select(kv => (FileName: kv.Key, Lines: kv.Value.ToImmutableList()))
                .Where(t => !t.FileName.Contains("Test"))
                .ToImmutableDictionary(t => t.FileName, t => t.Lines);
        }

        private async Task<ProjectDefinition> GetProjectDefinition(int projectNo)
        {
            if (!this._projectDefinitions.ContainsKey(projectNo))
            {
                try
                {
                    var projectDef = await this._projectDefProvider.TryGetProjectDef(projectNo);
                    this._projectDefinitions.AddOrUpdate(projectNo, projectDef, (n, d) => d);
                }
                catch (ArgumentException ex)
                {
                    this._logger.LogWarning(ex, $"Unknown project #{projectNo} submitted");
                }
            }

            if (!this._projectDefinitions.TryGetValue(projectNo, out var def))
            {
                throw new UnknownProjectException(projectNo);
            }

            return def;
        }
    }
}