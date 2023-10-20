using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotNetTestService.Core.FileHandling
{
    public static class Util
    {
        public static IEnumerable<string> GetFilesInDir(string dirPath, string? filterExtension = null)
        {
            var subDirFiles = Directory.EnumerateDirectories(dirPath)
                .Select(d => GetFilesInDir(d, filterExtension))
                .SelectMany(e => e);
            var dirFiles = Directory.EnumerateFiles(dirPath);
            var allFiles = subDirFiles.Concat(dirFiles);
            if (filterExtension != null)
            {
                return allFiles.Select(p => (path: p, fi: new FileInfo(p)))
                    .Where(t => t.fi.Extension == filterExtension)
                    .Select(t => t.path);
            }

            return allFiles;
        }
    }
}