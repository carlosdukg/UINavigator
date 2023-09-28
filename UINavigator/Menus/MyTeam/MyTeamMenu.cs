using OpenQA.Selenium;
using System.Collections.Generic;
using UINavigator.Models.Enums.MyTeam;

namespace UINavigator.Menus.MyTeam
{
    public class MyTeamMenu
    {
        private readonly IWebDriver _driver;
        const string MyEmployees = "167";

        public MyTeamMenu(IWebDriver driver)
        {
            _driver = driver;
        }

        public void NavigateToPath(string path)
        {
            var navigationPoints = path.Split('>');
            var menuElements = EmployeeAdminMenuOptions();
            foreach (var navigation in navigationPoints)
            {
                if (menuElements.ContainsKey(navigation))
                {
                    menuElements[navigation].Click();
                }
            }
        }

        public Dictionary<string, IWebElement> EmployeeAdminMenuOptions()
        {
            return new Dictionary<string, IWebElement>
            {
                { MyTeamOptionMenu.MyEmployees.ToString(), _driver.FindElement(By.Id(MyEmployees)) }
            };
        }
    }
}
