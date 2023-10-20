using System.Threading.Tasks;

namespace DotNetTestService.Core.FileHandling
{
    public interface IProjectDefProvider
    {
        Task<ProjectDefinition> TryGetProjectDef(int projectNo);
    }
}