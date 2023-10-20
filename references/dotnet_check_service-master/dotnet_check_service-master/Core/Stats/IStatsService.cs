using System;
using DotNetTestService.Model;

namespace DotNetTestService.Core.Stats
{
    public interface IStatsService
    {
        StatsResponse GetStats();
        void SetCurrentActiveGetter(Func<int> currentActiveRunsGetter);
        void AddRunStats(TimeSpan runDuration, bool passed);
    }
}