using Microsoft.Extensions.Caching.Memory;
using OpenQA.Selenium;
using UINavigator.Common.Contracts;
using UINavigator.Common;
using UltiProTests.Services;

namespace UltiProTests.Tests.AdministrationTopMenu.EmployeeAdmin.MyEmployees
{
    [TestClass]
    public class Job_Menu_Tests
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
        public async Task Change_Job_HROPS()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/Administration/EmployeeAdmin/Employee/Jobs/change-job-1.json");

            if (_driver == null)
            {
                Assert.Fail("Null selenium driver");
            }
            if (uiTest == null)
            {
                Assert.Fail("Null test template");
            }

            //*** navigate and login ***//               
            _navigate?
                .Login(uiTest.Login?.Username, uiTest.Login?.Password, uiTest.Login?.Url, uiTest.Login?.IsSSOUser);

            //*** execute UI actions ***//
            await TestHelper.ProcessUIActionsAsync(uiTest.Actions, _driver, _utilities, _navigate);
        }

        [TestMethod]
        public async Task Change_Job_NON_HROPS()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/Administration/EmployeeAdmin/Employee/Jobs/change-job-2.json");

            if (_driver == null)
            {
                Assert.Fail("Null selenium driver");
            }
            if (uiTest == null)
            {
                Assert.Fail("Null test template");
            }

            //*** navigate and login ***//               
            _navigate?
                .Login(uiTest.Login?.Username, uiTest.Login?.Password, uiTest.Login?.Url, uiTest.Login?.IsSSOUser);

            //*** execute UI actions ***//
            await TestHelper.ProcessUIActionsAsync(uiTest.Actions, _driver, _utilities, _navigate);
        }

        [TestMethod]
        public async Task Add_Job_History_HROPS()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/Administration/EmployeeAdmin/Employee/Jobs/add-job-history-1.json");

            if (_driver == null)
            {
                Assert.Fail("Null selenium driver");
            }
            if (uiTest == null)
            {
                Assert.Fail("Null test template");
            }

            //*** navigate and login ***//               
            _navigate?
                .Login(uiTest.Login?.Username, uiTest.Login?.Password, uiTest.Login?.Url, uiTest.Login?.IsSSOUser);

            //*** execute UI actions ***//
            await TestHelper.ProcessUIActionsAsync(uiTest.Actions, _driver, _utilities, _navigate);
        }

        [TestMethod]
        public async Task Add_Job_History_NON_HROPS()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/Administration/EmployeeAdmin/Employee/Jobs/add-job-history-2.json");

            if (_driver == null)
            {
                Assert.Fail("Null selenium driver");
            }
            if (uiTest == null)
            {
                Assert.Fail("Null test template");
            }

            //*** navigate and login ***//               
            _navigate?
                .Login(uiTest.Login?.Username, uiTest.Login?.Password, uiTest.Login?.Url, uiTest.Login?.IsSSOUser);

            //*** execute UI actions ***//
            await TestHelper.ProcessUIActionsAsync(uiTest.Actions, _driver, _utilities, _navigate);
        }

        #region CAT1022

        [TestMethod]
        public async Task Job_Summary_Salary()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/Administration/EmployeeAdmin/Employee/Jobs/CAT1022/job-summary-not-in-paygroup.json");

            if (_driver == null)
            {
                Assert.Fail("Null selenium driver");
            }
            if (uiTest == null)
            {
                Assert.Fail("Null test template");
            }

            //*** navigate and login ***//               
            _navigate?
                .Login(uiTest.Login?.Username, uiTest.Login?.Password, uiTest.Login?.Url, uiTest.Login?.IsSSOUser);

            //*** execute UI actions ***//
            await TestHelper.ProcessUIActionsAsync(uiTest.Actions, _driver, _utilities, _navigate);
        }

        [TestMethod]
        public async Task Job_Summary_Salary_Pay_Groupp()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/Administration/EmployeeAdmin/Employee/Jobs/CAT1022/job-summary-in-paygroup.json");

            if (_driver == null)
            {
                Assert.Fail("Null selenium driver");
            }
            if (uiTest == null)
            {
                Assert.Fail("Null test template");
            }

            //*** navigate and login ***//               
            _navigate?
                .Login(uiTest.Login?.Username, uiTest.Login?.Password, uiTest.Login?.Url, uiTest.Login?.IsSSOUser);

            //*** execute UI actions ***//
            await TestHelper.ProcessUIActionsAsync(uiTest.Actions, _driver, _utilities, _navigate);
        }

        [TestMethod]
        public async Task Change_Job_And_Salary_Pay_Group()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/Administration/EmployeeAdmin/Employee/Jobs/CAT1022/change-job-salary.json");

            if (_driver == null)
            {
                Assert.Fail("Null selenium driver");
            }
            if (uiTest == null)
            {
                Assert.Fail("Null test template");
            }

            //*** navigate and login ***//               
            _navigate?
                .Login(uiTest.Login?.Username, uiTest.Login?.Password, uiTest.Login?.Url, uiTest.Login?.IsSSOUser);

            //*** execute UI actions ***//
            await TestHelper.ProcessUIActionsAsync(uiTest.Actions, _driver, _utilities, _navigate);
        }

        [TestMethod]
        public async Task Change_Salary_Pay_Group()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/Administration/EmployeeAdmin/Employee/Jobs/CAT1022/change-salary.json");

            if (_driver == null)
            {
                Assert.Fail("Null selenium driver");
            }
            if (uiTest == null)
            {
                Assert.Fail("Null test template");
            }

            //*** navigate and login ***//               
            _navigate?
                .Login(uiTest.Login?.Username, uiTest.Login?.Password, uiTest.Login?.Url, uiTest.Login?.IsSSOUser);

            //*** execute UI actions ***//
            await TestHelper.ProcessUIActionsAsync(uiTest.Actions, _driver, _utilities, _navigate);
        }

        [TestMethod]
        public async Task Job_Detail_Pay_Group()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/Administration/EmployeeAdmin/Employee/Jobs/CAT1022/job-history-detail-in-paygroup.json");

            if (_driver == null)
            {
                Assert.Fail("Null selenium driver");
            }
            if (uiTest == null)
            {
                Assert.Fail("Null test template");
            }

            //*** navigate and login ***//               
            _navigate?
                .Login(uiTest.Login?.Username, uiTest.Login?.Password, uiTest.Login?.Url, uiTest.Login?.IsSSOUser);

            //*** execute UI actions ***//
            await TestHelper.ProcessUIActionsAsync(uiTest.Actions, _driver, _utilities, _navigate);
        }

        #endregion
    }
}
