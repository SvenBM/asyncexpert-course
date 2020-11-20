using System;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace DataStructures.Benchmark
{
    [MemoryDiagnoser]
    public class MetricsCounterBenchmark
    {
        private const int KeyCount = 16;
        private const int ValueCount = 100000;
        private const int ConcurrentWriters = 2;

        [Params(typeof(LockingMetricsCounter), typeof(ConcurrentDictionaryOnlyMetricsCounter), typeof(ConcurrentDictionaryWithCounterMetricsCounter))]
        public Type Type;

        private Task[] tasks;
        private TaskCompletionSource<object> starter;

        [IterationSetup]
        public void Setup()
        {
            var originalKeys = Enumerable.Range(0, KeyCount).Select(i => i.ToString()).ToArray();
            var keys = Enumerable.Repeat(originalKeys, ConcurrentWriters).SelectMany(m => m).ToArray();

            starter = new TaskCompletionSource<object>();
            var counter = (IMetricsCounter)Activator.CreateInstance(Type);

            // run two tasks per key
            tasks = keys.Select(key => Task.Run(async () =>
            {
                await starter.Task;
                for (var i = 0; i < ValueCount; i++)
                {
                    counter.Increment(key);
                }
            })).ToArray();
        }

        [Benchmark]
        public async Task Run()
        {
            starter.SetResult(starter);
            await Task.WhenAll(tasks);
        }
    }
}
