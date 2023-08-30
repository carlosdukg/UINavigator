using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UINavigator.Models.Enums.Administration;

namespace UINavigator.Menus.Administration
{
    public class AdministrationMenu
    {
        private readonly IWebDriver _driver;
        const string EmployeeAdmin = "346";
        const string MyEmployees = "424";

        public AdministrationMenu(IWebDriver driver)
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
                 { AdministrationMenuOption.EmployeeAdmin.ToString(), _driver.FindElement(By.Id(EmployeeAdmin)) },
                 { AdministrationMenuOption.MyEmployees.ToString(), _driver.FindElement(By.Id(MyEmployees)) }
            };
        }
    }
}
