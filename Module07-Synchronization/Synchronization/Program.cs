using System;
using System.Threading;
using System.Threading.Tasks;
using Synchronization.Core;

namespace Synchronization
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var scopeName = "default";
            var isSystemWide = false;
            if (args.Length == 2)
            {
                scopeName = args[0];
                isSystemWide = bool.Parse(args[1]);
            }
            using (new NamedExclusiveScope(scopeName, isSystemWide))
            {
                Console.WriteLine("Hello world!");
                //Thread.Sleep(300);
                await Task.Delay(300);
            }
        }
    }
}
