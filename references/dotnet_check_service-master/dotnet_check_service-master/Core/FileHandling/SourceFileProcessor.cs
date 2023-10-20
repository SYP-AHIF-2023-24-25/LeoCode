using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DotNetTestService.Model;

namespace DotNetTestService.Core.FileHandling
{
    public sealed class SourceFileProcessor
    {
        private static readonly Regex replacementPattern = new(@"<!<REPLACEMENT - (\d\d)>!>", RegexOptions.Compiled);
        private readonly string _rootDir;

        public SourceFileProcessor(string rootDir)
        {
            if (!Directory.Exists(rootDir))
            {
                throw new ArgumentException("directory does not exist", nameof(rootDir));
            }

            this._rootDir = rootDir;
        }

        public async Task<int> CountReplacements()
        {
            return await ProcessFiles();
        }

        public Task<int> PerformReplacements(IEnumerable<CodeReplacement> codeReplacements)
        {
            return ProcessFiles(codeReplacements);
        }

        private async Task<int> ProcessFiles(IEnumerable<CodeReplacement>? codeReplacements = null,
            IDictionary<string, List<string>>? fileContents = null)
        {
            static string FixNewLines(string raw) => raw.Replace("\\n", Environment.NewLine);

            var replacementDic = codeReplacements?.ToDictionary(cr => cr.SectionNo, cr => FixNewLines(cr.RawCode));
            var files = Util.GetFilesInDir(this._rootDir, ".cs");
            var cnt = 0;
            foreach (var file in files)
            {
                var lines = fileContents != null
                    ? new List<string>(400)
                    : null;
                cnt += await ProcessFile(file, replacementDic, lines);
                if (lines != null)
                {
                    var fileName = Path.GetFileName(file);
                    fileContents!.Add(fileName, lines);
                }
            }

            return cnt;
        }

        private static async Task<int> ProcessFile(string filePath,
            IReadOnlyDictionary<int, string>? replacements = null, List<string>? storeLines = null)
        {
            var doReplace = replacements != null;
            var replacementDone = false;
            var cnt = 0;
            var lines = await File.ReadAllLinesAsync(filePath);
            storeLines?.AddRange(lines);
            var newLines = doReplace
                ? new List<string>(lines.Length + 10)
                : new(0);
            foreach (var line in lines)
            {
                var match = replacementPattern.Match(line);
                if (!match.Success)
                {
                    if (doReplace)
                    {
                        newLines.Add(line);
                    }

                    continue;
                }

                cnt++;
                if (!doReplace)
                {
                    continue;
                }

                var repNo = int.Parse(match.Groups[1].Value);
                if (!replacements!.TryGetValue(repNo, out var replacementLines))
                {
                    throw new InvalidOperationException($"Replacement section {repNo} not available");
                }

                replacementDone = true;
                newLines.Add(replacementLines);
            }
            
            if (doReplace && replacementDone)
            {
                await File.WriteAllLinesAsync(filePath, newLines);
            }

            return cnt;
        }

        public async Task<IDictionary<string, List<string>>> ReadFiles()
        {
            var dic = new Dictionary<string, List<string>>();
            await ProcessFiles(fileContents: dic);
            return dic;
        }
    }
}