using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Cache.Init();
        }

        [TestCleanup]
        public void TearDown()
        {
            Cache.EmptyCache();
            Cache.Deinit();
        }

        [TestMethod]
        public void TestAddingItem()
        {
            string k = "http//some.address.com/item1";
            string d = "my data";

            Cache.Add(k, d);

            Assert.IsTrue(Cache.Exists(k));
            Assert.IsTrue(d == Cache.Get(k));
        }

        [TestMethod]
        public void TestRemovingItem()
        {
            string k = "http//some.address.com/item1";
            string d = "my data";

            Cache.Add(k, d);
            Assert.IsTrue(Cache.Exists(k));

            Cache.Remove(k);
            Assert.IsFalse(Cache.Exists(k));
        }

        [TestMethod]
        public void TestReplaceItem()
        {
            string k = "http//some.address.com/item1";
            string d = "my data";

            Cache.Add(k, d);
            Assert.IsTrue(d == Cache.Get(k));
            
            string d2 = "my other data to replace previous data";
            Cache.Replace(k, d2);
            Assert.IsTrue(Cache.Exists(k));
            Assert.IsTrue(Cache.Get(k) == d2);
        }

        //Synchronisation tests
    }
}
