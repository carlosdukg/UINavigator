using OpenQA.Selenium;

namespace UINavigator.Contracts
{
    /// <summary>
    /// Chrome broser web driver.
    /// </summary>
    public interface IChromeWebDriverService
    {
        IWebDriver GetDriver();
    }
}