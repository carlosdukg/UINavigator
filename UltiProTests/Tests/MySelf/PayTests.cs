using Microsoft.Extensions.Caching.Memory;
using OpenQA.Selenium;
using UINavigator.Common.Contracts;
using UINavigator.Common;
using UltiProTests.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace UltiProTests.Tests.MySelfTopMenu
{
    [TestClass]
    public class PayTests
    {
        private IWebDriver _driver;
        private ChromeWebDriver _chormeDriver;
        private IUtilitiesService _utilities;
        Navigation _navigate;

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
                .LoadUITest(@"DataTemplates/MySelf/Pay/USL1001/direct_deposit-routing_number-mismatch-error-USL1001.json");

            if (_driver == null)
            {
                Assert.Fail();
            }
            if (uiTest == null)
            {
                Assert.Fail();
            }

            //*** navigate and login ***//               
            _navigate
                .Login(uiTest.Login.Username, uiTest.Login.Password, uiTest.Login.Url, uiTest.Login.IsSSOUser);

            //*** execute UI actions ***//
            TestHelper.ProcessUIActions(uiTest.Actions, _driver, _utilities, _navigate);
        }

        [TestMethod]
        public async Task Direct_Deposit_Account_Numbers_Mismatch()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/MySelf/Pay/USL1001/direct_deposit-account_number-mismatch-error-USL1001.json");

            if (_driver == null)
            {
                Assert.Fail();
            }
            if (uiTest == null)
            {
                Assert.Fail();
            }

            //*** navigate and login ***//               
            _navigate
                .Login(uiTest.Login.Username, uiTest.Login.Password, uiTest.Login.Url, uiTest.Login.IsSSOUser);

            //*** execute UI actions ***//
            TestHelper.ProcessUIActions(uiTest.Actions, _driver, _utilities, _navigate);
        }

        [TestMethod]
        public async Task Direct_Deposit_Edit_Page_Load()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/MySelf/Pay/USL1001/direct_deposit-edit-page-load-USL1001.json");

            if (_driver == null)
            {
                Assert.Fail();
            }
            if (uiTest == null)
            {
                Assert.Fail();
            }

            //*** navigate and login ***//               
            _navigate
                .Login(uiTest.Login.Username, uiTest.Login.Password, uiTest.Login.Url, uiTest.Login.IsSSOUser);

            //*** execute UI actions ***//
            TestHelper.ProcessUIActions(uiTest.Actions, _driver, _utilities, _navigate);
        }

        [TestMethod]
        public async Task Direct_Deposit_Add_Successful()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/MySelf/Pay/USL1001/direct_deposit-valid-create-USL1001.json");

            if (_driver == null)
            {
                Assert.Fail();
            }
            if (uiTest == null)
            {
                Assert.Fail();
            }

            //*** navigate and login ***//               
            _navigate
                .Login(uiTest.Login.Username, uiTest.Login.Password, uiTest.Login.Url, uiTest.Login.IsSSOUser);

            //*** execute UI actions ***//
            TestHelper.ProcessUIActions(uiTest.Actions, _driver, _utilities, _navigate);
        }

        [TestMethod]
        public async Task Direct_Deposit_Edit_Page_Show_Routing_Confirmation_On_Change()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/MySelf/Pay/USL1001/direct_deposit-edit-page-display-conf-routing-number-USL1001.json");

            if (_driver == null)
            {
                Assert.Fail();
            }
            if (uiTest == null)
            {
                Assert.Fail();
            }

            //*** navigate and login ***//               
            _navigate
                .Login(uiTest.Login.Username, uiTest.Login.Password, uiTest.Login.Url, uiTest.Login.IsSSOUser);

            //*** execute UI actions ***//
            TestHelper.ProcessUIActions(uiTest.Actions, _driver, _utilities, _navigate);
        }

        [TestMethod]
        public async Task Direct_Deposit_Edit_Page_Show_Account_Confirmation_On_Change()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/MySelf/Pay/USL1001/direct_deposit-edit-page-display-conf-account-number-USL1001.json");

            if (_driver == null)
            {
                Assert.Fail();
            }
            if (uiTest == null)
            {
                Assert.Fail();
            }

            //*** navigate and login ***//               
            _navigate
                .Login(uiTest.Login.Username, uiTest.Login.Password, uiTest.Login.Url, uiTest.Login.IsSSOUser);

            //*** execute UI actions ***//
            TestHelper.ProcessUIActions(uiTest.Actions, _driver, _utilities, _navigate);
        }

        [TestMethod]
        public async Task MySelf_Direct_Deposit_Add_DIS1013()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/MySelf/Pay/DIS1013/direct_deposit-add-test.json");

            if (_driver == null)
            {
                Assert.Fail();
            }
            if (uiTest == null)
            {
                Assert.Fail();
            }

            //*** navigate and login ***//               
            _navigate
                .Login(uiTest.Login.Username, uiTest.Login.Password, uiTest.Login.Url, uiTest.Login.IsSSOUser);

            //*** execute UI actions ***//
            TestHelper.ProcessUIActions(uiTest.Actions, _driver, _utilities, _navigate);
        }

        [TestMethod]
        public async Task MySelf_Direct_Deposit_Edit_DIS1013()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/MySelf/Pay/DIS1013/direct_deposit-edit-test.json");

            if (_driver == null)
            {
                Assert.Fail();
            }
            if (uiTest == null)
            {
                Assert.Fail();
            }

            //*** navigate and login ***//               
            _navigate
                .Login(uiTest.Login.Username, uiTest.Login.Password, uiTest.Login.Url, uiTest.Login.IsSSOUser);

            //*** execute UI actions ***//
            TestHelper.ProcessUIActions(uiTest.Actions, _driver, _utilities, _navigate);
        }
    }
}
