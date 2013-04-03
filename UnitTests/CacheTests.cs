using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Phone.Testing;

using NoppaClient;

namespace UnitTests
{
    [TestClass]
    public class CacheTests
    {
        [TestInitialize]
        public void Setup()
        {
            Cache.EmptyCache();
        }
        
        [TestMethod]
        public void TestAddingItem()
        {
            string k = "http//some.address.com/item1";
            string d = "my data";

            Cache.Add(k, d, Cache.PolicyLevel.Short);

            Assert.IsTrue(Cache.Exists(k));
            Assert.IsTrue(d == Cache.Get(k));

            string k2 = "http//some.address.com/itemforever";
            string d2 = "my forever data";
            Cache.Add(k2, d2, Cache.PolicyLevel.Long);

            Assert.IsTrue(Cache.Exists(k2));
            Assert.IsTrue(d2 == Cache.Get(k2));

            string k3 = "http//some.address.com/item-none";
            Cache.Add(k3, d, Cache.PolicyLevel.BypassCache);

            Assert.IsFalse(Cache.Exists(k3));

            bool caughtException = false;
            try
            {
                string d4 = "text";
                Cache.Add(k2, d4, Cache.PolicyLevel.Long);
            }
            catch
            {
                caughtException = true;
            }
            Assert.IsTrue(caughtException);
        }

        [TestMethod]
        public void TestRemovingItem()
        {
            string k = "http//some.address.com/item1";
            string d = "my data";

            Cache.Add(k, d, Cache.PolicyLevel.Short);
            Assert.IsTrue(Cache.Exists(k));

            Cache.Remove(k);
            Assert.IsFalse(Cache.Exists(k));

            bool caughtException = false;
            try
            {
                Cache.Remove(k);
            }
            catch
            {
                caughtException = true;
            }
            Assert.IsTrue(caughtException);
        }

        [TestMethod]
        public void TestReloadItem()
        {
            string k = "http//some.address.com/item1";
            string d = "my data";

            Cache.Add(k, d, Cache.PolicyLevel.Short);
            Assert.IsTrue(d == Cache.Get(k));
            
            string d2 = "my other data to replace previous data";
            Cache.Add(k, d2, Cache.PolicyLevel.Reload);
            Assert.IsTrue(Cache.Exists(k));
            Assert.IsTrue(Cache.Get(k) == d2);
        }

        [TestMethod]
        public void TestCacheItemToBinary()
        {
            string json = @"{
                'course_id': 'ENE.kand',
                'dept_id': 'T2020',
                'name': 'Kandidaatintyö ja seminaari',
                'course_url': 'http://noppa-api-dev.aalto.fi/noppa/kurssi/ENE.kand',
                'course_url_oodi': 'https://oodi.aalto.fi/a/opintjakstied.jsp?Kieli=1&Tunniste=ENE.kand&html=1',
                'noppa_language': 'fi',
                'links': [
                    {
                        'rel': 'self',
                        'title': 'self',
                        'uri': 'http://noppa-api-dev.aalto.fi/api/v1/courses/ENE.kand'
                    }
                ]

            }";

            string req = "http://noppa-api-dev.aalto.fi/api/v1/courses?key=cdda4ae4833c0114005de5b5c4371bb8&org_id=eng";

            DateTime dt = DateTime.Now;
            dt.AddDays(1);

            byte[] itemArr = Cache.CacheItem.ToBinary(req, new Cache.CacheItem(json, dt, Cache.PolicyLevel.Short));

            List<byte> l = new List<byte>();

            // DateTime (Int64)
            byte[] date = BitConverter.GetBytes(dt.ToBinary());
            foreach (byte b in date) 
            {
                l.Add(b);
            }

            // Policy (Int32)
            byte[] p = BitConverter.GetBytes((int)Cache.PolicyLevel.Short);
            foreach (byte b in p)
            {
                l.Add(b);
            }

            // Key length (Int32)
            byte[] keyLen = BitConverter.GetBytes(req.Length);
            foreach (byte b in keyLen)
            {
                l.Add(b);
            }

            // Key
            foreach (char b in req)
            {
                l.Add((byte)b);
            }

            // Data
            foreach (char b in json)
            {
                l.Add((byte)b);
            }

            byte[] expectedArr = l.ToArray();
            Assert.IsTrue(expectedArr.Length == itemArr.Length);
            for (int i = 0; i < itemArr.Length; i++)
            {
                Assert.IsTrue(itemArr[i] == expectedArr[i]);
            }
        }

        [TestMethod]
        public void TestCacheItemFromBinary()
        {
            string json = @"{
                'course_id': 'ENE.kand',
                'dept_id': 'T2020',
                'name': 'Kandidaatintyö ja seminaari',
                'course_url': 'http://noppa-api-dev.aalto.fi/noppa/kurssi/ENE.kand',
                'course_url_oodi': 'https://oodi.aalto.fi/a/opintjakstied.jsp?Kieli=1&Tunniste=ENE.kand&html=1',
                'noppa_language': 'fi',
                'links': [
                    {
                        'rel': 'self',
                        'title': 'self',
                        'uri': 'http://noppa-api-dev.aalto.fi/api/v1/courses/ENE.kand'
                    }
                ]

            }";

            string req = "http://noppa-api-dev.aalto.fi/api/v1/courses?key=cdda4ae4833c0114005de5b5c4371bb8&org_id=eng";

            DateTime dt = DateTime.Now;
            dt.AddDays(1);

            List<byte> l = new List<byte>();

            // DateTime (Int64)
            byte[] date = BitConverter.GetBytes(dt.ToBinary());
            foreach (byte b in date)
            {
                l.Add(b);
            }

            // Policy (Int32)
            byte[] p = BitConverter.GetBytes((int)Cache.PolicyLevel.Short);
            foreach (byte b in p)
            {
                l.Add(b);
            }

            // Key length (Int32)
            byte[] keyLen = BitConverter.GetBytes(req.Length);
            foreach (byte b in keyLen)
            {
                l.Add(b);
            }

            // Key
            foreach (char b in req)
            {
                l.Add((byte)b);
            }

            // Data
            foreach (char b in json)
            {
                l.Add((byte)b);
            }

            byte[] byteArr = l.ToArray();

            Tuple<string, Cache.CacheItem> t = Cache.CacheItem.FromBinary(byteArr);

            Assert.IsTrue(t.Item1 == req);
            Assert.IsTrue(t.Item2.TTL == dt);
            Assert.IsTrue(t.Item2.Item == json);
        }

        [TestMethod]
        public void TestDeserializeCache()
        {
            string json = @"{
                'course_id': 'ENE.kand',
                'dept_id': 'T2020',
                'name': 'Kandidaatintyö ja seminaari',
                'course_url': 'http://noppa-api-dev.aalto.fi/noppa/kurssi/ENE.kand',
                'course_url_oodi': 'https://oodi.aalto.fi/a/opintjakstied.jsp?Kieli=1&Tunniste=ENE.kand&html=1',
                'noppa_language': 'fi',
                'links': [
                    {
                        'rel': 'self',
                        'title': 'self',
                        'uri': 'http://noppa-api-dev.aalto.fi/api/v1/courses/ENE.kand'
                    }
                ]

            }";

            string req = "http://noppa-api-dev.aalto.fi/api/v1/courses?key=cdda4ae4833c0114005de5b5c4371bb8&org_id=eng";

            DateTime dt = DateTime.Now.AddDays(1);

            List<byte> l = new List<byte>();

            // DateTime (Int64)
            byte[] date = BitConverter.GetBytes(dt.ToBinary());
            foreach (byte b in date)
            {
                l.Add(b);
            }

            // Policy (Int32)
            byte[] p = BitConverter.GetBytes((int)Cache.PolicyLevel.Short);
            foreach (byte b in p)
            {
                l.Add(b);
            }

            // Key length (Int32)
            byte[] keyLen = BitConverter.GetBytes(req.Length);
            foreach (byte b in keyLen)
            {
                l.Add(b);
            }

            // Key
            foreach (char b in req)
            {
                l.Add((byte)b);
            }

            // Data
            foreach (char b in json)
            {
                l.Add((byte)b);
            }

            // Data length (Int23). Always the first value
            byte[] len = BitConverter.GetBytes(l.ToArray().Length);
            for (int i = 0; i < 4; i++)
            {
                l.Insert(i, len[i]);
            }

            byte[] byteArr = l.ToArray();

            MemoryStream s = new MemoryStream(byteArr);
            Assert.IsFalse(Cache.Exists(req));
            
            Cache.Deserialize(s);
            s.Dispose();

            Assert.IsTrue(Cache.Exists(req));
            Assert.IsTrue(Cache.Get(req) == json);
        }

        [TestMethod]
        public void TestSerializeCache()
        {
            string json = @"{
                'course_id': 'ENE.kand',
                'dept_id': 'T2020',
                'name': 'Kandidaatintyö ja seminaari',
                'course_url': 'http://noppa-api-dev.aalto.fi/noppa/kurssi/ENE.kand',
                'course_url_oodi': 'https://oodi.aalto.fi/a/opintjakstied.jsp?Kieli=1&Tunniste=ENE.kand&html=1',
                'noppa_language': 'fi',
                'links': [
                    {
                        'rel': 'self',
                        'title': 'self',
                        'uri': 'http://noppa-api-dev.aalto.fi/api/v1/courses/ENE.kand'
                    }
                ]

            }";

            string req = "http://noppa-api-dev.aalto.fi/api/v1/courses?key=cdda4ae4833c0114005de5b5c4371bb8&org_id=eng";

            DateTime dt = DateTime.Now.AddDays(1);

            List<byte> l = new List<byte>();

            // DateTime (Int64)
            byte[] date = BitConverter.GetBytes(dt.ToBinary());
            foreach (byte b in date)
            {
                l.Add(b);
            }

            // Policy (Int32)
            byte[] p = BitConverter.GetBytes((int)Cache.PolicyLevel.Short);
            foreach (byte b in p)
            {
                l.Add(b);
            }

            // Key length (Int32)
            byte[] keyLen = BitConverter.GetBytes(req.Length);
            foreach (byte b in keyLen)
            {
                l.Add(b);
            }

            // Key
            foreach (char b in req)
            {
                l.Add((byte)b);
            }

            // Data
            foreach (char b in json)
            {
                l.Add((byte)b);
            }

            // Data length (Int23). Always the first value
            byte[] len = BitConverter.GetBytes(l.ToArray().Length);
            for (int i = 0; i < 4; i++)
            {
                l.Insert(i, len[i]);
            }

            byte[] byteArr = l.ToArray();

            Cache.Add(req, json, Cache.PolicyLevel.Short);

            MemoryStream s = new MemoryStream();

            Cache.Serialize(s);
            s.Dispose();

            byte[] arr = s.ToArray();

            Assert.IsTrue(arr.Length == byteArr.Length);
            for (int i = 0; i < arr.Length; i++)
            {
                Assert.IsTrue(arr[i] == byteArr[i]);
            }
        }

        //Synchronisation tests
    }
}
