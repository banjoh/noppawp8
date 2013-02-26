using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoppaClient.DataModel;

namespace UnitTests
{
    [TestClass]
    public class DataModelTests
    {
        [TestMethod]
        public void TestInitClasses()
        {
            // Dictionary class
            Dictionary<NoppaClient.DataModel.Language, string> dict = 
                new Dictionary<NoppaClient.DataModel.Language, string>();
            dict.Add(NoppaClient.DataModel.Language.English, "english name");
            dict.Add(NoppaClient.DataModel.Language.Finnish, "finnish name");
            dict.Add(NoppaClient.DataModel.Language.Swedish, "swedish name");

            NoppaClient.DataModel.Department dip = new NoppaClient.DataModel.Department("DEPART", "T1234", dict);

            Assert.IsTrue("DEPART" == dip.Id);
            Assert.IsTrue("T1234" == dip.OrgId);

            NoppaClient.Settings.Language = NoppaClient.DataModel.Language.English;
            Assert.IsTrue("english name" == dip.Name);

            NoppaClient.Settings.Language = NoppaClient.DataModel.Language.Finnish;
            Assert.IsTrue("finnish name" == dip.Name);

            NoppaClient.Settings.Language = NoppaClient.DataModel.Language.Swedish;
            Assert.IsTrue("swedish name" == dip.Name);

            // Organiszation class
            Dictionary<NoppaClient.DataModel.Language, string> dict1 =
                new Dictionary<NoppaClient.DataModel.Language, string>();
            dict1.Add(NoppaClient.DataModel.Language.English, "english name");
            dict1.Add(NoppaClient.DataModel.Language.Finnish, "finnish name");
            dict1.Add(NoppaClient.DataModel.Language.Swedish, "swedish name");

            NoppaClient.DataModel.Organization org = new NoppaClient.DataModel.Organization("CHEM", dict1);

            Assert.IsTrue("CHEM" == org.Id);

            NoppaClient.Settings.Language = NoppaClient.DataModel.Language.English;
            Assert.IsTrue("english name" == org.Name);

            NoppaClient.Settings.Language = NoppaClient.DataModel.Language.Finnish;
            Assert.IsTrue("finnish name" == org.Name);

            NoppaClient.Settings.Language = NoppaClient.DataModel.Language.Swedish;
            Assert.IsTrue("swedish name" == org.Name);

            // Course class
        }
    }
}
