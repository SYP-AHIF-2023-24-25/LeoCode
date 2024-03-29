﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace LeoCodeBackend
{
    public class ResultFileHelperTypescript
    {
        public string formatData(string json)
        {
            TestResults testResults = JsonConvert.DeserializeObject<TestResults>(json);

            var customResults = new CustomResults
            {
                Summary = new Summary
                {
                    TotalTests = testResults.Stats.Tests,
                    PassedTests = testResults.Stats.Passes,
                    FailedTests = testResults.Stats.Failures
                },
                TestResults = testResults.Passes
                .Select(test => new TestResult
                {
                    TestName = $"T{test.CurrentRetry + 1}_{test.Title.Replace(" ", "_").ToLower()}",
                    Outcome = "Passed",
                    ErrorMessage = ""
                })
                .Concat(testResults.Failures
                    .Select(test => new TestResult
                    {
                        TestName = $"T{test.CurrentRetry + 1}_{test.Title.Replace(" ", "_").ToLower()}",
                        Outcome = "Failed",
                        ErrorMessage = test.Err.Message
                    }))
                .ToList()
            };
            return JsonConvert.SerializeObject(customResults, Newtonsoft.Json.Formatting.Indented);
        }
    }

    public class TestResults
    {
        public Stats Stats { get; set; }
        public List<Test> Tests { get; set; }
        public List<object> Pending { get; set; }
        public List<Failure> Failures { get; set; }
        public List<Pass> Passes { get; set; }
    }

    public class Stats
    {
        public int Suites { get; set; }
        public int Tests { get; set; }
        public int Passes { get; set; }
        public int Pending { get; set; }
        public int Failures { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Duration { get; set; }
    }

    public class Test
    {
        public string Title { get; set; }
        public string FullTitle { get; set; }
        public string File { get; set; }
        public int Duration { get; set; }
        public int CurrentRetry { get; set; }
        public string Speed { get; set; }
        public Err Err { get; set; }
    }

    public class Err
    {
        public string Message { get; set; }
        public bool ShowDiff { get; set; }
        public string Actual { get; set; }
        public string Expected { get; set; }
        public string Operator { get; set; }
        public string Stack { get; set; }
    }

    public class Failure
    {
        public string Title { get; set; }
        public string FullTitle { get; set; }
        public string File { get; set; }
        public int Duration { get; set; }
        public int CurrentRetry { get; set; }
        public Err Err { get; set; }
    }

    public class Pass
    {
        public string Title { get; set; }
        public string FullTitle { get; set; }
        public string File { get; set; }
        public int Duration { get; set; }
        public int CurrentRetry { get; set; }
        public string Speed { get; set; }
        public Err Err { get; set; }
    }
}