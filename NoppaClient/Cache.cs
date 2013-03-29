using System;
using System.Collections.Generic;

namespace NoppaClient
{
    public class Cache
    {
        #region Member varaibles and helper methods
        public enum PolicyType
        {
            None,
            Forever,
            Temporary
        }

        public class CacheItem
        {
            public string Item { get; private set; }
            public PolicyType Policy { get; private set; }
            public DateTime TTL { get; private set; }

            public CacheItem(string i, PolicyType p)
            {
                Item = i;
                Policy = p;

                if (p == PolicyType.Temporary)
                {
                    // Store item for 1 day
                    // TODO: Have it configurable
                    DateTime dt = DateTime.Now;
                    dt.AddDays(1);
                    TTL = dt;
                }
            }
        }

        private readonly Dictionary<string, CacheItem> _cache = new Dictionary<string, CacheItem>();
        private readonly object _lock = new Object();
        private bool _initialized = false;
        private static Cache _instance;

        // Not thread safe
        private static Cache Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Cache();

                return _instance;
            }
        }

        private static bool Initialized
        { 
            get
            {
                lock (Instance._lock)
                {
                    return Instance._initialized;
                }
            }
            set
            {
                lock (Instance._lock)
                {
                    Instance._initialized = value;
                }
            }
        }

        ~Cache()
        {
            // Extra check just incase this function is not called before destruction
            Deinit();
        }

        private static void TestInitialized()
        {
            if (Initialized == false)
                throw new ApplicationException("Cache has not been initialized. Call Cache::Init() first");
        }

        public static void Init()
        {
            // Deserialize stored cached items
            Initialized = true;
        }

        public static void Deinit()
        {
            // Serialize cached items to filesystem
            Initialized = false;
        }
        #endregion

        #region Accessor methods
        public static void EmptyCache()
        {
            TestInitialized();

            lock (Instance._lock)
            {
                Instance._cache.Clear();
            }
        }

        public static bool Exists(string key)
        {
            TestInitialized();

            lock (Instance._lock)
            {
                return Instance._cache.ContainsKey(key);
            }
        }

        public static string Get(string key)
        {
            TestInitialized();

            if (Exists(key) == false)
                throw new ApplicationException(String.Format("An object with key '{0}' does not exists", key));

            lock (Instance._lock)
            {
                return Instance._cache[key].Item;
            }
        }

        public static void Add(string key, string value, PolicyType policy = PolicyType.None)
        {
            TestInitialized();

            if (Exists(key) == true)
                throw new ApplicationException(String.Format("An object with key '{0}' already exists", key));

            lock (Instance._lock)
            {
                Instance._cache.Add(key, new CacheItem(value, policy));
            }
        }

        public static void Remove(string key)
        {
            TestInitialized();

            if (Exists(key) == false)
                throw new ApplicationException(String.Format("An object with key '{0}' does not exist in cache", key));

            lock (Instance._lock)
            {
                Instance._cache.Remove(key);
            }
        }

        public static void Replace(string key, string value, PolicyType policy = PolicyType.None)
        {
            TestInitialized();

            lock (Instance._lock)
            {
                if (Instance._cache.ContainsKey(key) == true)
                    Instance._cache.Remove(key);

                Instance._cache.Add(key, new CacheItem(value, policy));
            }
        }
        #endregion
    }
}
