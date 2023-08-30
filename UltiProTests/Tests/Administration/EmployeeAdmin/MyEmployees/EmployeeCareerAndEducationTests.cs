using OpenQA.Selenium;
using Microsoft.Extensions.Caching.Memory;
using UINavigator.Common;
using UINavigator.Common.Contracts;
using UltiProTests.Services;

namespace UltiProTests.Tests.AdministrationTopMenu.EmployeeAdmin.MyEmployees
{
    [TestClass]
    public class EmployeeCareerAndEducationTests
    {
        private IWebDriver? _driver;
        private ChromeWebDriver? _chormeDriver;
        private IUtilitiesService? _utilities;
        Navigation? _navigate;

        [TestInitialize]
        public void Initialize()
        {
            _chormeDriver = new ChromeWebDriver();
            _driver = _chormeDriver.GetDriver();

            var cacheOptions = new MemoryCacheOptions
            {
                SizeLimit = 1024
            };
            var cache = new MemCache(new MemoryCache(cacheOptions));
            _utilities = new Utilities(cache);

            ICustomerSelectorService customerSelector = new CustomerSelector(_driver);
            ILoginService login = new Login(_driver, customerSelector);
            _navigate = new Navigation(_driver, login);
        }

        [TestCleanup]
        public void TearDown()
        {
            if (_driver != null)
            {
                _driver.Quit();
            }
        }

        [TestMethod]
        public async Task MyEmployees_Employee_CareerAndEducation_AddVaccineTest_Tuberculosis_PeridicTesting_TBResults_Selection_Views()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/Administration/EmployeeAdmin/Pages/Benefits/AddVaccine/my-employees-add-vaccine-test-tb-pt-tb-results.json");
            if (_driver == null)
            {
                Assert.Fail();
            }

            //*** navigate and login ***//               
            _navigate?
                .Login(uiTest?.Login?.Username, uiTest?.Login?.Password, uiTest?.Login?.Url, uiTest?.Login?.IsSSOUser);

            //*** execute UI actions ***//
            TestHelper.ProcessUIActions(uiTest?.Actions, _driver, _utilities, _navigate);

        }

        [TestMethod]
        public async Task MyEmployees_Employee_CareerAndEducation_AddVaccineTest_Influenza_IL_ConsentDate_Selection_Views()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/Administration/EmployeeAdmin/Pages/Benefits/AddVaccine/my-employees-add-vaccine-test-INF-IL-consent-date.json");
            if (_driver == null)
            {
                Assert.Fail();
            }

            //*** navigate and login ***//                      
            _navigate?
                .Login(uiTest?.Login?.Username, uiTest?.Login?.Password, uiTest?.Login?.Url, uiTest?.Login?.IsSSOUser);

            // execute UI actions
            TestHelper.ProcessUIActions(uiTest?.Actions, _driver, _utilities, _navigate);
        }

        [TestMethod]
        public async Task MyEmployees_Employee_CareerAndEducation_AddVaccineTest_Influenza_IL_DeclineDate_Selection_Views()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/Administration/EmployeeAdmin/Pages/Benefits/AddVaccine/my-employees-add-vaccine-test-INF-IL-decline-date_valid.json");
            if (_driver == null)
            {
                Assert.Fail();
            }

            //*** navigate and login ***//                      
            _navigate?
                .Login(uiTest?.Login?.Username, uiTest?.Login?.Password, uiTest?.Login?.Url, uiTest?.Login?.IsSSOUser);

            // execute UI actions
            TestHelper.ProcessUIActions(uiTest?.Actions, _driver, _utilities, _navigate);
        }

        [TestMethod]
        public async Task MyEmployees_Employee_CareerAndEducation_AddVaccineTest_Influenza_IL_DeclineDate_Save_Valid_Selection()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/Administration/EmployeeAdmin/Pages/Benefits/AddVaccine/my-employees-add-vaccine-test-INF-IL_valid.json");
            if (_driver == null)
            {
                Assert.Fail();
            }

            //*** navigate and login ***//                      
            _navigate?
                .Login(uiTest?.Login?.Username, uiTest?.Login?.Password, uiTest?.Login?.Url, uiTest?.Login?.IsSSOUser);

            // execute UI actions
            TestHelper.ProcessUIActions(uiTest?.Actions, _driver, _utilities, _navigate);
        }

        [TestMethod]
        public async Task MyEmployees_Employee_CareerAndEducation_AddVaccineTest_Influenza_IL_DeclineDate_Save_Missing_All_Required()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/Administration/EmployeeAdmin/Pages/Benefits/AddVaccine/my-employees-add-vaccine-test-inf-il-decline-date_required_all.json");
            if (_driver == null)
            {
                Assert.Fail();
            }

            //*** navigate and login ***//                      
            _navigate?
                .Login(uiTest?.Login?.Username, uiTest?.Login?.Password, uiTest?.Login?.Url, uiTest?.Login?.IsSSOUser);

            // execute UI actions
            TestHelper.ProcessUIActions(uiTest?.Actions, _driver, _utilities, _navigate);
        }

        [TestMethod]
        public async Task MyEmployees_Employee_CareerAndEducation_AddVaccineTest_Influenza_IL_DeclineDate_Save_Missing_Dates()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/Administration/EmployeeAdmin/Pages/Benefits/AddVaccine/my-employees-add-vaccine-test-INF-IL-decline-date_required_dates.json");
            if (_driver == null)
            {
                Assert.Fail();
            }

            //*** navigate and login ***//                      
            _navigate?
                .Login(uiTest?.Login?.Username, uiTest?.Login?.Password, uiTest?.Login?.Url, uiTest?.Login?.IsSSOUser);

            // execute UI actions
            TestHelper.ProcessUIActions(uiTest?.Actions, _driver, _utilities, _navigate);
        }

        [TestMethod]
        public async Task MyEmployees_Employee_CareerAndEducation_AddVaccineTest_Influenza_IL_DeclineDate_Save_Missing_Attestation()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/Administration/EmployeeAdmin/Pages/Benefits/AddVaccine/my-employees-add-vaccine-test-inf-il-decline-date.json");
            if (_driver == null)
            {
                Assert.Fail();
            }

            //*** navigate and login ***//                      
            _navigate?
                .Login(uiTest?.Login?.Username, uiTest?.Login?.Password, uiTest?.Login?.Url, uiTest?.Login?.IsSSOUser);

            // execute UI actions
            TestHelper.ProcessUIActions(uiTest?.Actions, _driver, _utilities, _navigate);
        }
    }
}