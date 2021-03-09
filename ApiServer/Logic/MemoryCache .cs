using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Logic
{
    public class MemoryCache<T>
    {
        // this will be defined using T that needs to be locked
        public static void StartTransaction(string key)
        {
            var transactionLock = Lock<T>.Create(key);
            transactionLock.Wait();
        }
        public static void EndTransaction(string key)
        {
            var transactionLock = Lock<T>.Get(key);
            transactionLock.Release();
        }
    }
}
