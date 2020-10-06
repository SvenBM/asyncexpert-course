using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace Dotnetos.AsyncExpert.Homework.Module01.Benchmark
{
    [DisassemblyDiagnoser(exportCombinedDisassemblyReport: true)]
    public class FibonacciCalc
    {
        // HOMEWORK:
        // 1. Write implementations for RecursiveWithMemoization and Iterative solutions
        // 2. Add MemoryDiagnoser to the benchmark
        // 3. Run with release configuration and compare results
        // 4. Open disassembler report and compare machine code
        // 
        // You can use the discussion panel to compare your results with other students

        [Benchmark(Baseline = true)]
        [ArgumentsSource(nameof(Data))]
        public ulong Recursive(ulong n)
        {
            if (n == 1 || n == 2) return 1;
            return Recursive(n - 2) + Recursive(n - 1);
        }

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public ulong RecursiveWithMemoization(ulong n)
        {
            var results = new ulong[n];
            results[0] = 1;
            results[1] = 1;
            return RecursiveMemo(n, results);
        }

        private ulong RecursiveMemo(ulong n, ulong[] results)
        {
            if (results[n - 1] != 0)
                return results[n - 1];

            var res = RecursiveMemo(n - 2, results) + RecursiveMemo(n - 1, results);
            results[n - 1] = res;
            return res;
        }
        
        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public ulong Iterative(ulong n)
        {
            ulong preCurr = 1, curr = 1;
            for (ulong i = 3; i <= n; i++)
            {
                var tmp = preCurr + curr;
                preCurr = curr;
                curr = tmp;
            }
            return curr;
        }

        public IEnumerable<ulong> Data()
        {
            yield return 15;
            yield return 35;
        }
    }
}
