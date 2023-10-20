using System;
using DotNetTestService.Core.CheckRuns;

namespace DotNetTestService.Model
{
    public sealed class CheckStatusResponse
    {
        public RunStatus Status { get; set; }
        public FailReason? FailReason { get; set; }
        public int? PassPercentage { get; set; }
        public TimeSpan Duration { get; set; }
        public bool AllPassed { get; set; }
    }
}