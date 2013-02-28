﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Phone.Testing;

using NoppaClient;
using NoppaClient.DataModel;

namespace UnitTests
{
    [TestClass]
    public class NoppaApiClientTest : WorkItemTest
    {
        /**
         * Integration tests to see that our API calls works as expected
         */

        [TestMethod]
        [Asynchronous]
        public async Task TestGetAllOrganizations()
        {
            NoppaApiClient client = new NoppaApiClient(APIKeyHolder.Key);

            List<Organization> organizations = await client.GetAllOrganizations();

            Assert.IsTrue(organizations.Exists( (item) => item.Id == "CHEM" ));
            Assert.IsTrue(organizations.Exists( (item) => item.Id == "ECON" ));
            Assert.IsTrue(organizations.Exists( (item) => item.Id == "ELEC" ));
            Assert.IsTrue(organizations.Exists( (item) => item.Id == "ENG" ));
            Assert.IsTrue(organizations.Exists( (item) => item.Id == "ERI" ));
            Assert.IsTrue(organizations.Exists( (item) => item.Id == "SCI" ));
            Assert.IsTrue(organizations.Exists( (item) => item.Id == "TaiK" ));

            EnqueueTestComplete();
        }
    }
}