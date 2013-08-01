using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Phone.Testing;

using NoppaLib;
using NoppaLib.DataModel;
using NoppaClient;

namespace UnitTests
{
    [TestClass]
    public class NoppaAPITests : WorkItemTest
    {
        /**
         * Integration tests to see that our API calls works as expected
         */

        [TestMethod]
        [Asynchronous]
        public async void TestGetAllOrganizations()
        {
            List<Organization> organizations = await NoppaAPI.GetAllOrganizations();

            Assert.IsNotNull(organizations, "Cannot fetch organizations, either not found or timed out");

            Assert.IsTrue(organizations.Exists( (item) => item.Id == "CHEM" ));
            Assert.IsTrue(organizations.Exists( (item) => item.Id == "ECON" ));
            Assert.IsTrue(organizations.Exists( (item) => item.Id == "ELEC" ));
            Assert.IsTrue(organizations.Exists( (item) => item.Id == "ENG" ));
            Assert.IsTrue(organizations.Exists( (item) => item.Id == "ERI" ));
            Assert.IsTrue(organizations.Exists( (item) => item.Id == "SCI" ));
            Assert.IsTrue(organizations.Exists( (item) => item.Id == "TaiK" ));

            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public async void TestGetDepartment()
        {
            string dept = "T4070";
            Department department = await NoppaAPI.GetDepartment(dept);

            Assert.IsNotNull(department, "Cannot fetch department, either not found or timed out");
            Assert.AreEqual("ELEC", department.OrgId);
            Assert.AreEqual(dept, department.Id);
            Assert.AreEqual("Department of Communications and Networking", department.name_en);

            EnqueueTestComplete();
        }
    }
}
