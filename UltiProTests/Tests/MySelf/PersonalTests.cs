using Microsoft.Extensions.Caching.Memory;
using UINavigator.Common.Contracts;
using UINavigator.Common;
using OpenQA.Selenium;
using UltiProTests.Services;

namespace UltiProTests.Tests.MySelfTopMenu
{
    [TestClass]
    public class PersonalTests
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
        public async Task Personal_AddressAndNameChange_Page_Load()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/MySelf/Personal/personal-address-name-change-page-load-USL1001.json");

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
        public async Task Personal_AddressAndNameChange_Address_Change()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/MySelf/Personal/personal-address-change-USL1001.json");

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
        public async Task Personal_AddressAndNameChange_Smart_Address_Change()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/MySelf/Personal/personal-smart-address-change-USL1001.json");

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
        public async Task Personal_AddressAndNameChange_Name_Change()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/MySelf/Personal/personal-name-change-USL1001.json");

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
        public async Task Personal_AddressAndNameChange_NameAndAddress_Change()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/MySelf/Personal/personal-name-address-change-USL1001.json");

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
