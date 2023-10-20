using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DotNetTestService.Core.CheckRuns
{
    public sealed class ResultFileHelper
    {
        private const string TOTAL_ATT = "total";
        private const string PASSED_ATT = "passed";
        private readonly ISet<string> _attributes = new[] {TOTAL_ATT, PASSED_ATT}.ToHashSet();

        private readonly string _filePath;

        public ResultFileHelper(string resultFilePath)
        {
            this._filePath = resultFilePath;
        }

        public Results? GetResults()
        {
            if (!File.Exists(this._filePath))
            {
                return new(false, -1, -1);
            }

            var doc = XDocument.Load(this._filePath);
            var res = doc.Descendants()
                .Where(e => e.Name.LocalName == "Counters")
                .SelectMany(d => d.Attributes())
                .Where(a => this._attributes.Contains(a.Name.LocalName))
                .ToDictionary(a => a.Name.LocalName, a => SafeParse(a.Value));
            if (res.Values.Any(v => v < 0))
            {
                return null;
            }

            return new(true, res[TOTAL_ATT], res[PASSED_ATT]);
        }

        private static int SafeParse(string val)
        {
            if (string.IsNullOrEmpty(val) || !int.TryParse(val, out var value))
            {
                return -1;
            }

            return value;
        }
    }

    public record Results(bool Compiled, int NoOfTests, int PassedTests)
    {
        public bool AllPassed => Compiled
                                 && !InvalidResults
                                 && NoOfTests == PassedTests;

        public int PassPercentage => !Compiled || InvalidResults
            ? 0
            : (int) (Math.Round(PassedTests / (double) NoOfTests, 2) * 100);

        private bool InvalidResults => NoOfTests == -1 || PassedTests == -1;
    }
}