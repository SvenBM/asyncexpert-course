using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures
{
    public class ConcurrentDictionaryOnlyMetricsCounter : IMetricsCounter
    {
        // Implement this class using only ConcurrentDictionary.
        // Use methods that change the state atomically to ensure that everything is counted properly.
        // This task does not require using any Interlocked, or Volatile methods. The only required API is provided by the ConcurrentDictionary

        private readonly ConcurrentDictionary<string, int> dictionary = new ConcurrentDictionary<string, int>();

        public IEnumerator<KeyValuePair<string, int>> GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }

        public void Increment(string key)
        {
            this.dictionary.AddOrUpdate(key, 1, (s, i) => i + 1);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}