using OpenQA.Selenium;
using UINavigator.Contracts;
using UINavigator.Menus;
using UINavigator.Models.Enums.Site;
using UINavigator.Models.UIModels;

namespace UINavigator.Services
{
    /// <inheritdoc/>
    public class NavigationService : INavigationService
    {
        private readonly IWebDriver _driver;
        private readonly ILoginService _login;

        /// <summary>
        /// Creates a new instance of Navigation
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="login"></param>
        public NavigationService(IWebDriver driver, ILoginService login)
        {
            _driver = driver;
            _login = login;
        }

        /// <inheritdoc/>
        public NavigationService Login(string? username, string? password, string? location, bool? isSSOUser)
        {
            _login.OpenBrowserAndLogin(username, password, location, isSSOUser);
            return this;
        }

        /// <inheritdoc/>
        public NavigationService Path(UINavigation navigation)
        {
            if (string.IsNullOrWhiteSpace(navigation.MainMenu))
            {
                throw new ArgumentNullException(nameof(navigation.MainMenu));
            }

            var tOption = TopMenuOption.Unknown;
            _ = Enum.TryParse(navigation.MainMenu, out tOption);

            var topMenu = TopMenu.GetTopMenu(tOption);
            _driver.FindElement(By.Id(topMenu?.Id)).Click();

            if (string.IsNullOrWhiteSpace(navigation.Path))
            {
                return this;
            }

            var navigationPoints = navigation.Path.Split('>');
            foreach (var point in navigationPoints)
            {
                var item = topMenu?.Items.SingleOrDefault(m => m?.Name?.ToLower() == point.ToLower());
                if (item != null)
                {
                    _driver.FindElement(By.Id(item.Id)).Click();
                }
            }

            return this;
        }

        /// <inheritdoc/>
        public IWebDriver WebDriver()
        {
            return _driver;
        }
    }
}
