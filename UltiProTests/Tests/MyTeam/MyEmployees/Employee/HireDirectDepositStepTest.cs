using Microsoft.Extensions.Caching.Memory;
using OpenQA.Selenium;
using UINavigator.Common.Contracts;
using UINavigator.Common;
using UltiProTests.Services;

namespace UltiProTests.Tests.MyTeamTopMenu.MyEmployees.Employee
{
    [TestClass]
    public class HireDirectDepositStepTest
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
        public async Task Hire_Direct_Deposit_Sted_Add_Data_DIS1013()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/MyTeam/MyEmployees/add-employee-add-direct-deposit.json");
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
                .Login(uiTest?.Login?.Username, uiTest?.Login?.Password, uiTest?.Login?.Url, uiTest?.Login?.IsSSOUser);

            //*** execute UI actions ***//
            await TestHelper.ProcessUIActionsAsync(uiTest?.Actions, _driver, _utilities, _navigate);
        }

        [TestMethod]
        public async Task Hire_Direct_Deposit_Sted_Edit_Data_DIS1013()
        {
            //*** arrange ***//
            var uiTest = await TestHelper
                .LoadUITest(@"DataTemplates/MyTeam/MyEmployees/add-employee-edit-direct-deposit.json");
            if (_driver == null)
            {
                Assert.Fail();
            }

            //*** navigate and login ***//               
            _navigate?
                .Login(uiTest?.Login?.Username, uiTest?.Login?.Password, uiTest?.Login?.Url, uiTest?.Login?.IsSSOUser);

            //*** execute UI actions ***//
            await TestHelper.ProcessUIActionsAsync(uiTest?.Actions, _driver, _utilities, _navigate);
        }
    }
}
