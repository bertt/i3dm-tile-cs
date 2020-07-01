using BenchmarkDotNet.Running;
using System;

namespace i3dm.tile.benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<I3dmBenchmarks>();
            Console.Write(summary);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

        }
    }
}
