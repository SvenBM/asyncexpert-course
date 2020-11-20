using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AwaitableExercises.Core
{
    public static class BoolExtensions
    {
        public static BoolAwaiter GetAwaiter(this bool b)
        {
            return new BoolAwaiter(b);
        }
    }

    public class BoolAwaiter : INotifyCompletion
    {
        private readonly bool b;

        public bool IsCompleted => false;

        public BoolAwaiter(bool b)
        {
            this.b = b;
        }

        public void OnCompleted(Action continuation)
        {
            _ = Task.Run(() => continuation?.Invoke());
        }

        public bool GetResult() => b;
    }
}
