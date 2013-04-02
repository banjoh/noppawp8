using System;
using System.Collections.Generic;
using System.IO;

namespace NoppaClient
{
    public static class Cache
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
            public DateTime TTL { get; private set; }

            public CacheItem(string i, DateTime d)
            {
                Item = i;
                TTL = d;
            }

            public static byte[] ToBinary(string key, CacheItem item)
            {
                List<byte> l = new List<byte>();

                // DateTime (Int64)
                byte[] date = BitConverter.GetBytes(item.TTL.ToBinary());
                foreach (byte b in date)
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

            public static Tuple<string, CacheItem> FromBinary(byte[] b)
            {
                int currentIdx = 0;
                // DateTime (Int64)
                DateTime dt = DateTime.FromBinary(BitConverter.ToInt64(b, 0));
                currentIdx += 8;

                // Key length (Int32)
                int keyLen = BitConverter.ToInt32(b, 8);
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

                return Tuple.Create(key, new CacheItem(data, dt));
            }
        }

        private static readonly Dictionary<string, CacheItem> _cache = new Dictionary<string, CacheItem>();
        private static readonly object _lock = new Object();

        public static void Serialize(Stream s)
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
                    _cache.Add(t.Item1, t.Item2);
            }
        }

        public static void Deserialize(Stream s)
        {
            // Serialize cached items to filesystem
            s.Flush();
            DateTime now = DateTime.Now;

            foreach (string key in _cache.Keys)
            {
                // Old items should not be persisted
                CacheItem c = _cache[key];
                if (c.TTL < now)
                    continue;

                byte[] data = CacheItem.ToBinary(key, c);

                // Data length (Int23). Always the first value
                byte[] len = BitConverter.GetBytes(data.Length);

                s.Write(len, 0, 4);
                s.Write(data, 0, data.Length);
            }
        }

        private static DateTime TimeToLive(PolicyType p)
        {
            DateTime dt = default(DateTime);

            if (p == PolicyType.Temporary)
            {
                // Store item for 1 day
                // TODO: Have it configurable
                dt = DateTime.Now.AddDays(1);
            }
            return dt;
        }
        #endregion

        #region Accessor methods
        public static void EmptyCache()
        {
            lock (_lock)
            {
                _cache.Clear();
            }
        }

        public static bool Exists(string key)
        {
            lock (_lock)
            {
                return _cache.ContainsKey(key);
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

        public static void Add(string key, string value, PolicyType policy = PolicyType.None)
        {
            if (Exists(key) == true)
                throw new ApplicationException(String.Format("An object with key '{0}' already exists", key));

            lock (_lock)
            {
                if (policy != PolicyType.None)
                    _cache.Add(key, new CacheItem(value, TimeToLive(policy)));
            }
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

        public static void Replace(string key, string value, PolicyType policy = PolicyType.None)
        {
            lock (_lock)
            {
                if (_cache.ContainsKey(key) == true)
                    _cache.Remove(key);

                if (policy != PolicyType.None)
                    _cache.Add(key, new CacheItem(value, TimeToLive(policy)));
            }
        }
        #endregion
    }
}
