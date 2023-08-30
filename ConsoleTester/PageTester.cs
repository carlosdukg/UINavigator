using OpenQA.Selenium;
using System;
using UINavigator.Services;

namespace UITester
{
    public class PageTester
    {
        private readonly IWebDriver _driver;

        public PageTester(IChromeWebDriver driver)
        {
            _driver = driver.GetDriver();
        }

        public void OpenBrowserAndLogin()
        {
            _driver.Navigate().GoToUrl("https://localhost/Login.aspx");
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);

            Login();

            _driver.Quit();
        }

        private void Login()
        {
            var loginUserName = _driver.FindElement(By.Id("ctl00_Content_Login1_UserName"));
            loginUserName.SendKeys("ssouser");
            var loginPassword = _driver.FindElement(By.Id("ctl00_Content_Login1_Password"));
            loginPassword.SendKeys("password");

            var loginButton = _driver.FindElement(By.Id("ctl00_Content_Login1_LoginButton"));
            loginButton.Click();
        }
    }
}