using System;
using BenchmarkDotNet.Running;

namespace DataStructures.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<MetricsCounterBenchmark>();
        }
    }
}
