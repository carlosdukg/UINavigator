using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using UINavigator.Services;

public class ChromeWebDriver : IChromeWebDriver
{
    public IWebDriver GetDriver()
    {
        ChromeOptions options = new ChromeOptions();
        options.AddArgument("start-maximized");
        options.AddArgument("test-type");
        options.AddArgument("disable-notifications");
        options.AddUserProfilePreference("autofill.profile_enabled", false);
        options.AcceptInsecureCertificates = true;
        //ChromeOptions handleSSL = new ChromeOptions
        //{

        //    //Using the accept insecure cert method with true as parameter
        //    //to accept the untrusted certificate
        //    AcceptInsecureCertificates = true,
        //};
        var driver = new ChromeDriver(options);
        /*string fileName = "chromedriver";
        string path = Path.Combine(Environment.CurrentDirectory, @"Drivers\", fileName);
        return new ChromeDriver(path, handleSSL);*/
        return driver;
    }
}