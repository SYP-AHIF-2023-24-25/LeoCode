using System;

namespace DotNetTestService.Model
{
    public sealed class StatsResponse
    {
        public DateTime ServiceStart { get; set; }
        public int TotalCheckRuns { get; set; }
        public int PassedRuns { get; set; }
        public int FailedRuns { get; set; }
        public int ActiveRuns { get; set; }
        public TimeSpan AvgRunTime { get; set; }
        public double PassRation => DivByTotalRuns(PassedRuns);
        public double FailRation => DivByTotalRuns(FailedRuns);

        private double DivByTotalRuns(int value)
        {
            if (TotalCheckRuns <= 0)
            {
                return 0;
            }

            return value / (double) TotalCheckRuns;
        }
    }
}