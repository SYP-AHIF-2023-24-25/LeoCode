using System;
using System.Collections.Generic;
using DotNetTestService.Model;

namespace DotNetTestService.Core.CheckRuns
{
    public interface ICheckRunService
    {
        Guid RunCheck(ProjectDefinition projectDef, List<CodeReplacement> codeReplacements);
        IRun? GetRunInfo(Guid runId);
    }
}