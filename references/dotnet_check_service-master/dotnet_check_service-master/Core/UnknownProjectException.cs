using System;

namespace DotNetTestService.Core
{
    public sealed class UnknownProjectException : Exception
    {
        public UnknownProjectException(int projectNo) : base($"Unknown project #{projectNo}")
        {
        }
    }
}