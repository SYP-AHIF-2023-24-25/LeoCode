using System.Collections.Immutable;

namespace DotNetTestService.Model
{
    public sealed class ProjectTemplateResponse
    {
        public int ProjectNo { get; init; }
        public ImmutableDictionary<string, ImmutableList<string>> Files { get; init; } = default!;
    }
}