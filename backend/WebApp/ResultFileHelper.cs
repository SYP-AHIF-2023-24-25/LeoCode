using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace LeoCode
{
    public sealed class ResultFileHelper
    {
        private const string TOTAL_ATT = "tests";
        private const string PASSED_ATT = "passes";

        private readonly string _filePath;
        private readonly ISet<string> _attributes = (new[] { TOTAL_ATT, PASSED_ATT }).ToHashSet();

        public ResultFileHelper(string resultFilePath)
        {
            _filePath = resultFilePath;
        }

        public Results? GetResults()
        {
            if (!File.Exists(_filePath))
            {
                return new(false, -1, -1);
            }
            
            var doc = XDocument.Load(_filePath);
            return new(true, 0, 0);
            /*var res = doc.Descendants()
                .Where(e => e.Name.LocalName == "Counters")
                .SelectMany(d => d.Attributes())
                .Where(a => _attributes.Contains(a.Name.LocalName))
                .ToDictionary(a => a.Name.LocalName, a => SafeParse(a.Value));
            if (res.Values.Any(v => v < 0))
            {
                return null;
            }
            return new(true, res[TOTAL_ATT], res[PASSED_ATT]);*/
        }

        private static int SafeParse(string val)
        {
            if (string.IsNullOrEmpty(val) || !int.TryParse(val, out int value))
            {
                return -1;
            }
            return value;
        }

    }

    public record Results(bool Compiled, int NoOfTests, int PassedTests)
    {
        public bool AllPassed => Compiled
            && (!InvalidResults)
            && NoOfTests == PassedTests;
        public int PassPercentage => (!Compiled || InvalidResults)
            ? 0
            : (int)(Math.Round(PassedTests / (double)NoOfTests, 2) * 100);
        private bool InvalidResults => NoOfTests == -1 || PassedTests == -1;
    }
}
