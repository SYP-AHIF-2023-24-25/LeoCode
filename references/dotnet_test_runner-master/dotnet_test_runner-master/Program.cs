using System;
using System.Threading.Tasks;

namespace TestRunner
{
    class Program
    {
        async static Task Main(string[] args)
        {
            Console.WriteLine("Starting!");
            var res = await (new DockerHelper()).RunTestContainer();
            if (res != null)
            {
                Console.WriteLine($"Passed tests percentage: {res.PassPercentage}");
            } else
            {
                Console.WriteLine("no results file!");
            }
            Console.WriteLine("Done!");
            Console.ReadKey();
        }
    }
}
