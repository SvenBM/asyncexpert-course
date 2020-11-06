using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DataStructures
{
    public class ConcurrentDictionaryWithCounterMetricsCounter : IMetricsCounter
    {
        // Implement this class using ConcurrentDictionary and the provided AtomicCounter class.
        // AtomicCounter should be created only once per key, then its Increment method should be used.
        
        private readonly ConcurrentDictionary<string, AtomicCounter> dictionary = new ConcurrentDictionary<string, AtomicCounter>();

        public IEnumerator<KeyValuePair<string, int>> GetEnumerator()
        {
            return this.dictionary.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Count).GetEnumerator();
        }

        public void Increment(string key)
        {
            var counter = this.dictionary.GetOrAdd(key, s => new AtomicCounter());
            counter.Increment();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public class AtomicCounter
        {
            int value;

            public void Increment() => Interlocked.Increment(ref value);

            public int Count => Volatile.Read(ref value);
        }
    }
}