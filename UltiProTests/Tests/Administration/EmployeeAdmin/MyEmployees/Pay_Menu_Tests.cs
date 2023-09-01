using OpenQA.Selenium;
using Microsoft.Extensions.Caching.Memory;
using UINavigator.Common;
using UINavigator.Common.Contracts;
using UltiProTests.Services;

namespace UltiProTests.Tests.AdministrationTopMenu.EmployeeAdmin.MyEmployees
{
    [TestClass]
    public class Pay_Menu_Tests
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
        public async Task Direct_Deposit_Routing_Numbers_Mismatch()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/Administration/EmployeeAdmin/Pages/Pay/direct_deposit-routing_number-mismatch-error-USL1001.json");
            
            if (_driver == null)
            {
                Assert.Fail();
            }
            if (uiTest == null)
            {
                Assert.Fail();
            }

            //*** navigate and login ***//               
            _navigate?
                .Login(uiTest.Login?.Username, uiTest.Login?.Password, uiTest.Login?.Url, uiTest.Login?.IsSSOUser);

            //*** execute UI actions ***//
            TestHelper.ProcessUIActions(uiTest.Actions, _driver, _utilities, _navigate);
        }

        [TestMethod]
        public async Task Direct_Deposit_Account_Numbers_Mismatch()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/Administration/EmployeeAdmin/Pages/Pay/direct_deposit-account_number-mismatch-error-USL1001.json");

            if (_driver == null)
            {
                Assert.Fail();
            }
            if (uiTest == null)
            {
                Assert.Fail();
            }

            //*** navigate and login ***//               
            _navigate?
                .Login(uiTest.Login?.Username, uiTest.Login?.Password, uiTest.Login?.Url, uiTest.Login?.IsSSOUser);

            //*** execute UI actions ***//
            TestHelper.ProcessUIActions(uiTest.Actions, _driver, _utilities, _navigate);
        }
    }
}