using System;
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
        private readonly Mutex mutex;
        private readonly Semaphore semaphore;

        public NamedExclusiveScope(string name, bool isSystemWide)
        {
            if(isSystemWide)
            {
                if (Mutex.TryOpenExisting(name, out this.mutex))
                    throw new InvalidOperationException($"Unable to get a global lock {name}.");

                this.mutex = new Mutex(false, name);
                this.mutex.WaitOne();
            }
            else
            {
                if(!Semaphore.TryOpenExisting(name, out this.semaphore))
                    this.semaphore = new Semaphore(1,1, name);
                this.semaphore.WaitOne();
            }
        }

        public void Dispose()
        {
            this.mutex?.ReleaseMutex();
            this.mutex?.Dispose();
            this.semaphore?.Release();
            this.semaphore?.Dispose();
        }
    }
}
