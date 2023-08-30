using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using UINavigator.Common;
using McMaster.Extensions.CommandLineUtils;
using UINavigator.Common.Contracts;
using UINavigator.Services;

var services = new ServiceCollection()
                .AddSingleton<IChromeWebDriver, ChromeWebDriver>()
                .AddSingleton<IMemCache, MemCache>()
                .AddSingleton<IWebDriver, ChromeDriver>()
                .AddSingleton<IUtilitiesService, Utilities>()
                .AddSingleton<ILoginService, Login>()
                .AddSingleton<ICustomerSelectorService, CustomerSelector>()
                .AddSingleton<INavigationService, Navigation>();

services.AddMemoryCache();

var app = new CommandLineApplication
{
    Name = "ultipro"
};

app.HelpOption();
var location = app.Option("-l|--location <LOCATION>", "UltiPro APP Location", CommandOptionType.SingleValue);
var username = app.Option("-u|--username <USERNAME>", "UltiPro APP Username", CommandOptionType.SingleValue);
var password = app.Option("-p|--password <PASSWORD>", "UltiPro APP Password", CommandOptionType.SingleValue);
var closeDriver = app.Option("-wd|--web-driver <WEBDRIVER>", "Chrome Web Driver Close Flag", CommandOptionType.SingleValue);
closeDriver.DefaultValue = "n";

app.OnExecute(() =>
{
    var provider = services.BuildServiceProvider();
    var navigationService = provider.GetService<INavigationService>();
    var loginUrl = $"https://{location.Value()}web.dlas1.ucloud.int/";

    var navigator = navigationService?.Login(username.Value(), password.Value(), loginUrl, false);
    var navDriver = navigator?.WebDriver();

    if (closeDriver?.Value() is not null && string.Equals(closeDriver.Value()?.Trim(), "y"))
    {
        navDriver?.Close();
    }
});

return app.Execute(args);
