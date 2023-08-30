using OpenQA.Selenium;

namespace UINavigator.Services
{
    /// <summary>
    /// Chrome broser web driver.
    /// </summary>
    public interface IChromeWebDriver
    {
        IWebDriver GetDriver();
    }
}