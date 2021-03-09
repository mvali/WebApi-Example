using System;
using System.Collections.Concurrent;
using System.Threading;

namespace ApiServer.Logic
{
    // using MemoryCache to store objects in memory for a fast response.
    // we do not want to lock all of the cache but only an object in the cache

    // store locks somewhere. Used throughout the system, will be as Singleton.

    internal sealed class LockDictionary : ConcurrentDictionary<string, SemaphoreSlim>
    {
        private static readonly Lazy<LockDictionary> lazy = new Lazy<LockDictionary>(() => Create());
        internal static LockDictionary Instance
        {
            get { return lazy.Value; }
        }
        private LockDictionary(ConcurrentDictionary<string, SemaphoreSlim> dictionary) : base(dictionary)
        { }
        private static LockDictionary Create()
        {
            return new LockDictionary(new ConcurrentDictionary<string, SemaphoreSlim>());
        }
    }
}
