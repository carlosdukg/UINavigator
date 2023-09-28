using OpenQA.Selenium;
using System.Collections.Generic;
using UINavigator.Models.Enums.Site;

namespace UINavigator.Menus
{
    public class TopMenu
    {
        private readonly IWebDriver _driver;
        const string Administration = "menu_admin";
        const string MyTeam = "menu_my_team";
        const string MySelf = "menu_myself";
        const string SystemConfiguration = "menu_sys_cfg";
        const string Favorites = "menu_favorites";
        public TopMenu(IWebDriver driver)
        {
            _driver = driver;
        }

        public void NavigateToPath(string path)
        {
            var navigationPoints = path.Split('>');
            var menuElements = TopMenuOptions();
            foreach (var navigation in navigationPoints)
            {
                if (menuElements.ContainsKey(navigation))
                {
                    menuElements[navigation].Click();
                }
            }
        }

        public Dictionary<string, IWebElement> TopMenuOptions()
        {
            return new Dictionary<string, IWebElement>
            {
                { TopMenuOption.Administration.ToString(), _driver.FindElement(By.Id(Administration)) },
                { TopMenuOption.MyTeam.ToString(), _driver.FindElement(By.Id(MyTeam)) },
                { TopMenuOption.MySelf.ToString(), _driver.FindElement(By.Id(MySelf)) },
                //{ TopMenuOption.SystemConfiguration.ToString(), _driver.FindElement(By.Id(SystemConfiguration)) },
                { TopMenuOption.Favorites.ToString(), _driver.FindElement(By.Id(Favorites)) }
            };
        }
    }
}
