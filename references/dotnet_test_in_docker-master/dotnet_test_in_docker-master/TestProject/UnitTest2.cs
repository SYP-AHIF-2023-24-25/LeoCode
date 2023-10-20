using FluentAssertions;
using ToBeDockerTested;
using Xunit;

namespace TestProject
{
    public class UnitTest2
    {
        [Fact]
        public void TestCalc()
        {
            const int V1 = 5;
            const int V2 = 3;
            
            var sl = new SomeLogic();

            var result = sl.Calc(V1, V2);

            result.Should().Be(V1 + V2);
        }
    }
}
