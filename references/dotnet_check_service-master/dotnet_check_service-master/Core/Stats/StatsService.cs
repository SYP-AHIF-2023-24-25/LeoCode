using System;
using DotNetTestService.Model;

namespace DotNetTestService.Core.Stats
{
    public sealed class StatsService : IStatsService
    {
        private readonly object _mutex = new();
        private Func<int>? _currentActiveRunsGetter;
        private DateTime _firstRun = DateTime.MinValue;
        private int _noOfRuns;
        private int _passedRuns;
        private TimeSpan _totalRunTime = TimeSpan.Zero;

        private TimeSpan AvgRunTime
        {
            get
            {
                if (this._noOfRuns == 0)
                {
                    return TimeSpan.Zero;
                }

                var totalTicks = this._totalRunTime.Ticks;
                var avgTicks = totalTicks / this._noOfRuns;
                return TimeSpan.FromTicks(avgTicks);
            }
        }

        private int ActiveRuns => this._currentActiveRunsGetter?.Invoke() ?? 0;

        public StatsResponse GetStats()
        {
            lock (this._mutex)
            {
                return new()
                {
                    TotalCheckRuns = this._noOfRuns,
                    PassedRuns = this._passedRuns,
                    FailedRuns = this._noOfRuns - this._passedRuns,
                    ActiveRuns = ActiveRuns,
                    AvgRunTime = AvgRunTime,
                    ServiceStart = this._firstRun
                };
            }
        }

        public void SetCurrentActiveGetter(Func<int> currentActiveRunsGetter)
        {
            lock (this._mutex)
            {
                this._currentActiveRunsGetter = currentActiveRunsGetter;
            }
        }

        public void AddRunStats(TimeSpan runDuration, bool passed)
        {
            lock (this._mutex)
            {
                this._totalRunTime += runDuration;
                this._noOfRuns++;
                if (passed)
                {
                    this._passedRuns++;
                }

                if (this._firstRun == DateTime.MinValue)
                {
                    this._firstRun = DateTime.Now;
                }
            }
        }
    }
}