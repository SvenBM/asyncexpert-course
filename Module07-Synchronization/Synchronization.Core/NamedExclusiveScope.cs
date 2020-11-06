using System;
using System.Collections.Generic;
using System.Threading;

namespace Synchronization.Core
{
    /*
     * Implement very simple wrapper around Mutex or Semaphore (remember both implement WaitHandle) to
     * provide a exclusive region created by `using` clause.
     *
     * Created region may be system-wide or not, depending on the constructor parameter.
     *
     * Any try to get a second systemwide scope should throw an `System.InvalidOperationException` with `Unable to get a global lock {name}.`
     */
    public class NamedExclusiveScope : IDisposable
    {
        private readonly Semaphore globalSemaphore;
        
        private static readonly object LocalAccess = new object();
        private static readonly Dictionary<string, Semaphore> LocalSemaphores = 
            new Dictionary<string, Semaphore>();

        private readonly Semaphore localSemaphore;

        public NamedExclusiveScope(string name, bool isSystemWide)
        {
            if(isSystemWide)
            {
                this.globalSemaphore = new Semaphore(0, 1, name, out var created);
                if (created) return;
                this.globalSemaphore = null;
                throw new InvalidOperationException($"Unable to get a global lock {name}.");
            }

            lock (LocalAccess)
            {
                if(!LocalSemaphores.TryGetValue(name, out this.localSemaphore))
                    LocalSemaphores.Add(name, this.localSemaphore = new Semaphore(1,1));
            }

            this.localSemaphore.WaitOne();
        }

        public void Dispose()
        {
            this.globalSemaphore?.Release();
            this.globalSemaphore?.Dispose();
            this.localSemaphore?.Release();
        }
    }
}
