using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace DotNetTestService.Core.FileHandling
{
    public sealed class ProjectDefProvider : IProjectDefProvider
    {
        private readonly AppSettings _appSettings;

        public ProjectDefProvider(IOptions<AppSettings> config)
        {
            this._appSettings = config.Value;
        }

        public async Task<ProjectDefinition> TryGetProjectDef(int projectNo)
        {
            var templateDir = Path.Combine(this._appSettings.RootTemplateDir, projectNo.ToString());
            if (!Directory.Exists(templateDir))
            {
                throw new ArgumentException($"No template for project {projectNo} found (search path = {templateDir})");
            }

            var sfp = new SourceFileProcessor(templateDir);
            var replacementCount = await sfp.CountReplacements();
            return new(projectNo, templateDir, replacementCount);
        }
    }
}