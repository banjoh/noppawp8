using System;
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

            foreach (var org in organizations)
            {
                foreach (Language lang in Enum.GetValues(typeof(Language)))
                {
                    Settings.Language = lang;
                    System.Diagnostics.Debug.WriteLine(String.Format("{0}: {1}", org.Id, org.Name));
                }
            }

            EnqueueTestComplete();
        }
    }
}
