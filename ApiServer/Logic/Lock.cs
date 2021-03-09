using System.Threading;

namespace ApiServer.Logic
{
    public static class Lock<T>
    {
        private static LockDictionary _locks = LockDictionary.Instance;
        private static readonly object _cacheLock = new object();

        public static SemaphoreSlim Get(string key)
        {
            lock (_cacheLock)
            {
                if (!_locks.ContainsKey(GetLockKey(key)))
                    return null;
                return _locks[GetLockKey(key)];
            }
        }
        public static SemaphoreSlim Create(string key)
        {
            lock (_cacheLock)
            {
                if (!_locks.ContainsKey(GetLockKey(key)))
                    _locks.TryAdd(GetLockKey(key),
                        new SemaphoreSlim(1, 1));

                return _locks[GetLockKey(key)];
            }
        }
        public static void Remove(string key)
        {
            lock (_cacheLock)
            {
                SemaphoreSlim removedObject;
                if (_locks.ContainsKey(GetLockKey(key)))
                    _locks.TryRemove(GetLockKey(key), out removedObject);
            }
        }
        private static string GetLockKey(string key)
        {
            return $"{typeof(T).Name}_{key}";
        }

    }
}
