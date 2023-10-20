using System;

namespace DotNetTestService.Core.CheckRuns
{
    public interface IRun
    {
        Guid Id { get; }
        RunStatus Status { get; }
        FailReason? FailReason { get; }
        int? PassPercentage { get; }
        bool? AllPassed { get; }
        public TimeSpan Duration { get; }
    }
}