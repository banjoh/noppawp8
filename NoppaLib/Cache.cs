using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.CompilerServices;
using System.ComponentModel;

[assembly: InternalsVisibleTo("UnitTests")]
namespace NoppaLib
{
    public class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // This is usually called within a property setter or getter, and therefore propert name is filled
        // in by the compiler for you. If it is called from elsewhere, it has to be explicitly provided.
        protected bool SetProperty<T>(ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
            {
                return false;
            }
            currentValue = newValue;
            NotifyPropertyChanged(propertyName);

            return true;
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class Cache : BindableBase
    {
        #region Policy level definition
        public enum PolicyLevel
        {
            /// <summary>
            /// Do not cache item
            /// </summary>
            BypassCache,
            /// <summary>
            /// Cache for 1 hour
            /// </summary>
            Short,
            /// <summary>
            /// Cache for 1 week
            /// </summary>
            Long,
            /// <summary>
            /// Replace an existing item with the reloaded on.
            /// </summary>
            Reload
        }

        internal static DateTime TimeToLive(PolicyLevel p)
        {
            switch (p)
            {
                case PolicyLevel.Long:
                    return DateTime.Now.AddMonths(1);
                case PolicyLevel.Short:
                    return DateTime.Now.AddHours(1);
                case PolicyLevel.BypassCache:
                    throw new ApplicationException("BypassCache policy level should not request for a TTL object");
                case PolicyLevel.Reload:
                    throw new ApplicationException("Reload policy level should not request for a TTL object");
                default:
                    return default(DateTime);
            }
        }
        #endregion

        #region Cache item defintion
        internal class CacheItem
        {
            public string Item { get; private set; }
            public DateTime TTL { get; private set; }
            public PolicyLevel Policy { get; private set; }

            internal CacheItem(string i, DateTime d, PolicyLevel p)
            {
                Item = i;
                TTL = d;
                Policy = p;
            }

            internal static byte[] ToBinary(string key, CacheItem item)
            {
                List<byte> l = new List<byte>();

                // DateTime (Int64)
                byte[] date = BitConverter.GetBytes(item.TTL.ToBinary());
                foreach (byte b in date)
                {
                    l.Add(b);
                }

                // Policy (Int32)
                byte[] p = BitConverter.GetBytes((int)item.Policy);
                foreach (byte b in p)
                {
                    l.Add(b);
                }

                // Key length (Int32)
                byte[] keyLen = BitConverter.GetBytes(key.Length);
                foreach (byte b in keyLen)
                {
                    l.Add(b);
                }

                // Key
                foreach (char b in key)
                {
                    l.Add((byte)b);
                }

                // Data
                foreach (byte b in item.Item)
                {
                    l.Add((byte)b);
                }

                return l.ToArray();
            }

            internal static Tuple<string, CacheItem> FromBinary(byte[] b)
            {
                int currentIdx = 0;
                // DateTime (Int64)
                DateTime dt = DateTime.FromBinary(BitConverter.ToInt64(b, 0));
                currentIdx += 8;

                // Policy (Int32)
                Cache.PolicyLevel policy = (Cache.PolicyLevel)BitConverter.ToInt32(b, 8);
                currentIdx += 4;

                // Key length (Int32)
                int keyLen = BitConverter.ToInt32(b, 12);
                currentIdx += 4;

                // Key
                char[] arr = new char[keyLen];
                for (int i = 0; i < keyLen; i++)
                {
                    arr[i] = (char)b[currentIdx++];
                }
                string key = new string(arr);

                // Data
                int remainder = b.Length - currentIdx;
                arr = new char[remainder];
                for (int i = 0; i < remainder; i++)
                {
                    arr[i] = (char)b[currentIdx++];
                }
                string data = new string(arr);

                return Tuple.Create(key, new CacheItem(data, dt, policy));
            }
        }
        #endregion

        #region Member varaibles and helper methods
        private static readonly Dictionary<string, CacheItem> _cache = new Dictionary<string, CacheItem>();
        private static readonly object _lock = new Object();
        public static readonly string CACHEFILE = "cachefile.cache";
        private static Cache _instance = null;

        public static Cache Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Cache();
                }
                return _instance;
            }
        }

        public static void Deserialize(Stream s)
        {
            if (s.Length <= 0)
                return;

            // Deserialize stored cached items
            byte[] bytes = new byte[s.Length];
            int numBytesToRead = (int)s.Length;
            int numBytesRead = 0;
            while (numBytesToRead > 0)
            {
                // Read may return anything from 0 to 10. 
                int n = s.Read(bytes, numBytesRead, numBytesToRead - numBytesRead);
                // The end of the file is reached. 
                if (n == 0)
                {
                    break;
                }
                numBytesRead += n;
                numBytesToRead -= n;
            }

            //Parse the array of bytes
            int fullLength = bytes.Length;
            int currentIdx = 0;
            DateTime now = DateTime.Now;
            while (currentIdx < fullLength - 1)
            {
                // Data length (Int23)
                int dataLen = BitConverter.ToInt32(bytes, currentIdx);
                currentIdx += 4;

                byte[] data = new byte[dataLen];
                for (int i = 0; i < dataLen; i++)
                {
                    data[i] = bytes[currentIdx++];
                }

                Tuple<string, CacheItem> t = CacheItem.FromBinary(data);

                // Only add cache items that are not older that NOW
                if (t.Item2.TTL > now)
                    lock (_lock)
                    {
                        _cache.Add(t.Item1, t.Item2);
                    }
            }

            Instance.NotifyPropertyChanged("IsNotEmpty");
        }

        public static void Serialize(Stream s)
        {
            // Serialize cached items to stream
            DateTime now = DateTime.Now;

            Dictionary<string, CacheItem>.KeyCollection keys;
            lock (_lock)
            {
                keys = _cache.Keys;
            }

            foreach (string key in keys)
            {
                // Old items should not be persisted
                CacheItem c = null;
                lock (_lock)
                {
                    // Another thread may remove a cache item
                    // so check its existence first before accessing
                    if (_cache.ContainsKey(key))
                        c = _cache[key];
                }
                if (c == null || c.TTL < now)
                    continue;

                byte[] data = CacheItem.ToBinary(key, c);

                // Data length (Int23). Always the first value
                byte[] len = BitConverter.GetBytes(data.Length);

                s.Write(len, 0, 4);
                s.Write(data, 0, data.Length);
            }
        }

        #endregion

        #region Accessor methods
        public static void EmptyCache()
        {
            lock (_lock)
            {
                _cache.Clear();
                Instance.NotifyPropertyChanged("IsNotEmpty");
            }
        }

        public static bool IsNotEmpty
        {
            get
            {
                return _cache.Count > 0;
            }
        }

        public static bool Exists(string key)
        {
            lock (_lock)
            {
                if (_cache.ContainsKey(key))
                {
                    return _cache[key].TTL > DateTime.Now;
                }
                return false;
            }
        }

        public static string Get(string key)
        {
            if (Exists(key) == false)
                throw new ApplicationException(String.Format("An object with key '{0}' does not exists", key));

            lock (_lock)
            {
                return _cache[key].Item;
            }
        }

        public static void Add(string key, string value, PolicyLevel policy)
        {
            if (Exists(key) == true && policy != PolicyLevel.Reload)
                throw new ApplicationException(String.Format("An object with key '{0}' already exists", key));

            lock (_lock)
            {
                if (policy == PolicyLevel.Reload)
                {
                    // Default to Short policy
                    Cache.PolicyLevel oldPolicy = Cache.PolicyLevel.Short;

                    // If the item exists, remove it first and use it's policy
                    if (_cache.ContainsKey(key))
                    {
                        oldPolicy = _cache[key].Policy;
                        _cache.Remove(key);

                    }
                    _cache.Add(key, new CacheItem(value, TimeToLive(oldPolicy), oldPolicy));
                }
                else if (policy != PolicyLevel.BypassCache)
                {
                    _cache.Add(key, new CacheItem(value, TimeToLive(policy), policy));
                }
            }

            Instance.NotifyPropertyChanged("IsNotEmpty");
        }

        public static void Remove(string key)
        {
            if (Exists(key) == false)
                throw new ApplicationException(String.Format("An object with key '{0}' does not exist in cache", key));

            lock (_lock)
            {
                _cache.Remove(key);
            }
        }
        #endregion
    }
}
