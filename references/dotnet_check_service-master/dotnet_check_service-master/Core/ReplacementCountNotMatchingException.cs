using System;

namespace DotNetTestService.Core
{
    public sealed class ReplacementCountNotMatchingException : Exception
    {
        public ReplacementCountNotMatchingException(int expected, int actual) 
            : base($"{expected} code replacements were expected, but {actual} have been provided") {}
    }
}