using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoppaClient.DataModel;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UnitTests
{
    [TestClass]
    public class DataModelTests
    {
        NoppaClient.Settings settings { get; set; }

        [TestInitialize]
        public void Setup()
        {
            settings = new NoppaClient.Settings();
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

            Organization org = JsonConvert.DeserializeObject<Organization>(json);

            Assert.IsTrue("CHEM" == org.Id);

            settings.Language = Language.English;
            Assert.IsTrue("School of Chemical Technology" == org.Name);

            settings.Language = Language.Finnish;
            Assert.IsTrue("Kemian tekniikan korkeakoulu" == org.Name);

            settings.Language = Language.Swedish;
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

            Department dip = JsonConvert.DeserializeObject<Department>(json);

            Assert.IsTrue("T2020" == dip.Id);
            Assert.IsTrue("ENG" == dip.OrgId);

            settings.Language = Language.English;
            Assert.IsTrue("Department of Energy Technology" == dip.Name);

            settings.Language = Language.Finnish;
            Assert.IsTrue("Energiatekniikan laitos" == dip.Name);

            settings.Language = Language.Swedish;
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

            Course c = JsonConvert.DeserializeObject<Course>(json);

            Assert.IsTrue("ENE.kand" == c.Id);
            Assert.IsTrue("T2020" == c.DepartmentId);
            Assert.IsTrue("Kandidaatintyö ja seminaari" == c.Name);
            Assert.IsTrue("http://noppa-api-dev.aalto.fi/noppa/kurssi/ENE.kand" == c.Url);
            Assert.IsTrue("fi" == c.Language);
            Assert.IsTrue("https://oodi.aalto.fi/a/opintjakstied.jsp?Kieli=1&Tunniste=ENE.kand&html=1" == c.OodiUrl);
        }
    }
}
