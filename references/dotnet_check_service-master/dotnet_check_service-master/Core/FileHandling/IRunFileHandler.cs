using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetTestService.Model;

namespace DotNetTestService.Core.FileHandling
{
    public interface IRunFileHandler
    {
        string PrepareWorkDir(string templatePath, int runNo);
        Task InjectCodeReplacements(string workDirPath, IEnumerable<CodeReplacement> replacements);
        void CleanUpWorkDir(string workDirPath);
    }
}