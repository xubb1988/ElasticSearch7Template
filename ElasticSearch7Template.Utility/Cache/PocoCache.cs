using System;
using System.Collections.Generic;
using System.Threading;

namespace ElasticSearch7Template.Utility.Cache
{

    internal class ManagedCache
    {
    }
    public class PocoCache<TKey, TValue>
    {
        private readonly bool useManaged;

        private PocoCache(bool useManaged)
        {
            this.useManaged = useManaged;
        }

        /// <summary>
        /// Creates a cache that uses static storage
        /// </summary>
        /// <returns></returns>
        public static PocoCache<TKey, TValue> CreateStaticCache()
        {
            return new PocoCache<TKey, TValue>(false);
        }

        public static PocoCache<TKey, TValue> CreateManagedCache()
        {
            return new PocoCache<TKey, TValue>(true);
        }

        readonly Dictionary<TKey, TValue> map = new Dictionary<TKey, TValue>();
        readonly ReaderWriterLockSlim readerWriterLock = new ReaderWriterLockSlim();
        readonly ManagedCache managedCache = new ManagedCache();

        public int Count
        {
            get
            {
                return map.Count;
            }
        }

        public TValue Get(TKey key, Func<TValue> factory)
        {
            // Check cache
            readerWriterLock.EnterReadLock();
            TValue val;
            try
            {
                if (map.TryGetValue(key, out val))
                    return val;
            }
            finally
            {
                readerWriterLock.ExitReadLock();
            }

            // Cache it
            readerWriterLock.EnterWriteLock();
            try
            {
                // Check again
                if (map.TryGetValue(key, out val))
                    return val;

                // Create it
                val = factory();

                // Store it
                map.Add(key, val);

                // Done
                return val;
            }
            finally
            {
                readerWriterLock.ExitWriteLock();
            }
        }

        public bool AddIfNotExists(TKey key, TValue value)
        {
            // Cache it
            readerWriterLock.EnterWriteLock();
            try
            {
                // Check again
                TValue val;
                if (map.TryGetValue(key, out val))
                    return true;

                // Create it
                val = value;

                // Store it
                map.Add(key, val);

                // Done
                return false;
            }
            finally
            {
                readerWriterLock.ExitWriteLock();
            }
        }

        public void Flush()
        {
            // Cache it
            readerWriterLock.EnterWriteLock();
            try
            {
                map.Clear();
            }
            finally
            {
                readerWriterLock.ExitWriteLock();
            }

        }
    }
}
