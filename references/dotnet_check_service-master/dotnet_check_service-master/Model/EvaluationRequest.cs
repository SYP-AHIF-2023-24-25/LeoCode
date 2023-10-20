using System.Collections.Generic;

namespace DotNetTestService.Model
{
    public sealed class EvaluationRequest
    {
        public int ProjectNo { get; set; }
        public List<CodeReplacement> CodeReplacements { get; set; } = default!;
    }
}