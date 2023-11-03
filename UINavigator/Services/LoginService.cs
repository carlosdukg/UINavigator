using OpenQA.Selenium;
using UINavigator.Contracts;

namespace UINavigator.Services
{
    /// <inheritdoc/>
    public class LoginService : ILoginService
    {
        private readonly IWebDriver _driver;
        private readonly ICustomerSelectorService _customerSelector;

        /// <summary>
        /// Create a new instance of Login
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="customerSelector"></param>
        public LoginService(IWebDriver driver, ICustomerSelectorService customerSelector)
        {
            _driver = driver;
            _customerSelector = customerSelector;
        }

        /// <inheritdoc/>
        public void OpenBrowser(string location)
        {
            _driver.Navigate().GoToUrl(location);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);
        }

        /// <inheritdoc/>
        public void OpenBrowserAndLogin(string? username, string? password, string? location, bool? isSSOUser = false)
        {
            _driver.Navigate().GoToUrl(location);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);

            LoginWithUser(username, password, isSSOUser);
        }

        private void LoginWithUser(string? username, string? password, bool? isSSOUser)
        {
            var loginUserName = _driver.FindElement(By.Id("ctl00_Content_Login1_UserName"));
            loginUserName.SendKeys(username);
            var loginPassword = _driver.FindElement(By.Id("ctl00_Content_Login1_Password"));
            loginPassword.SendKeys(password);

            var loginButton = _driver.FindElement(By.Id("ctl00_Content_Login1_LoginButton"));
            loginButton.Click();

            if (isSSOUser != null && isSSOUser.Value)
            {
                _customerSelector.SelectCustomerCodeWithComponent("USG");
            }
        }
    }
}
