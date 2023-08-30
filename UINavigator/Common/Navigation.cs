using OpenQA.Selenium;
using UINavigator.Common.Contracts;
using UINavigator.Models.UIModels;

namespace UINavigator.Common
{
    /// <inheritdoc/>
    public class Navigation : INavigationService
    {
        private readonly IWebDriver _driver;
        private readonly ILoginService _login;

        /// <summary>
        /// Creates a new instance of Navigation
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="login"></param>
        public Navigation(IWebDriver driver, ILoginService login)
        {
            _driver = driver;
            _login = login;
        }

        /// <inheritdoc/>
        public Navigation Login(string? username, string? password, string? location, bool? isSSOUser)
        {
            _login.OpenBrowserAndLogin(username, password, location, isSSOUser);
            return this;
        }

        /// <inheritdoc/>
        public Navigation Path(UINavigation navigation)
        {
            if(string.IsNullOrWhiteSpace(navigation.Path) || string.IsNullOrWhiteSpace(navigation.Handler))
            {
                return this;
            }
            var menuObj = navigation.Handler;
            var menuType = Type.GetType(menuObj);
            if (menuType != null)
            {
                dynamic? menu = Activator.CreateInstance(menuType, _driver);
                menu?.NavigateToPath(navigation.Path);
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
