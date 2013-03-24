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
        NoppaClient.Settings Settings { get; set; }

        [TestInitialize]
        public void Setup()
        {
            Settings = new NoppaClient.Settings();
        }

        [TestMethod]
        public void TestOrganisationParsing()
        {
            string json = @"{
                'org_id': 'CHEM',
                'name_fi': 'Kemian tekniikan korkeakoulu',
                'name_sv': 'Högskolan för kemiteknik',
                'name_en': 'School of Chemical Technology',
                'links': [
                    {
                        'rel': 'self',
                        'title': 'self',
                        'uri': 'http://noppa-api-dev.aalto.fi/api/v1/organizations/CHEM'
                    },
                    {
                        'rel': 'related',
                        'title': 'departments',
                        'uri': 'http://noppa-api-dev.aalto.fi/api/v1/departments?org_id=CHEM'
                    }
                ]

            }";

            Organization org = new NoppaClient.DataModel.Organization(json);

            Assert.IsTrue("CHEM" == org.Id);

            Settings.Language = Language.English;
            Assert.IsTrue("School of Chemical Technology" == org.Name);

            Settings.Language = Language.Finnish;
            Assert.IsTrue("Kemian tekniikan korkeakoulu" == org.Name);

            Settings.Language = Language.Swedish;
            Assert.IsTrue("Högskolan för kemiteknik" == org.Name);
        }

        [TestMethod]
        public void TestDepartmentParsing()
        {
            string json = @"{
                'dept_id': 'T2020',
                'org_id': 'ENG',
                'name_fi': 'Energiatekniikan laitos',
                'name_sv': 'Institutionen för energiteknik',
                'name_en': 'Department of Energy Technology',
                'links': [
                    {
                        'rel': 'self',
                        'title': 'self',
                        'uri': 'http://noppa-api-dev.aalto.fi/api/v1/departments/T2020'
                    },
                    {
                        'rel': 'related',
                        'title': 'courses',
                        'uri': 'http://noppa-api-dev.aalto.fi/api/v1/courses?dept_id=T2020'
                    }
                ]

            }";

            NoppaClient.DataModel.Department dip = new NoppaClient.DataModel.Department(json);

            Assert.IsTrue("T2020" == dip.Id);
            Assert.IsTrue("ENG" == dip.OrgId);

            Settings.Language = Language.English;
            Assert.IsTrue("Department of Energy Technology" == dip.Name);

            Settings.Language = Language.Finnish;
            Assert.IsTrue("Energiatekniikan laitos" == dip.Name);

            Settings.Language = Language.Swedish;
            Assert.IsTrue("Institutionen för energiteknik" == dip.Name);
        }

        [TestMethod]
        public void TestCourseParsing()
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

            NoppaClient.DataModel.Course c = new NoppaClient.DataModel.Course(json);

            Assert.IsTrue("ENE.kand" == c.Id);
            Assert.IsTrue("T2020" == c.DepId);
            Assert.IsTrue("Kandidaatintyö ja seminaari" == c.Name);
            Assert.IsTrue(new Uri("http://noppa-api-dev.aalto.fi/noppa/kurssi/ENE.kand") == c.CourseUrl);
            Assert.IsTrue(new Uri("https://oodi.aalto.fi/a/opintjakstied.jsp?Kieli=1&Tunniste=ENE.kand&html=1") == c.OodiUrl);
            Assert.IsTrue(NoppaClient.DataModel.Language.Finnish == c.Language);
        }
    }
}
