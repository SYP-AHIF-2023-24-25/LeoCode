using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using DotNetTestService.Core.CheckRuns;
using DotNetTestService.Model;

namespace DotNetTestService.Core
{
    public interface ICodeCheckService
    {
        Task<Guid> SubmitCodeForCheck(int projectNo, List<CodeReplacement> codeReplacements);
        IRun? CheckRunStatus(Guid runId);
        Task<ImmutableDictionary<string, ImmutableList<string>>> GetTemplateFiles(int projectNo);
    }
}