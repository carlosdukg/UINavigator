using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UINavigator.Models.Enums.Employee;

namespace UINavigator.Menus.MyTeam
{
    public class EmployeeMenu
    {
        private readonly IWebDriver _driver;
        const string Jobs = "127";
        const string JobSummary = "946";
        const string Compensation = "168";
        const string JobHistory = "802";
        const string Personal = "338";
        const string EmployeeSummary = "17";

        public EmployeeMenu(IWebDriver driver)
        {
            _driver = driver;
        }

        public void NavigateToPath(string path)
        {
            var navigationPoints = path.Split('>');
            var menuElements = EmployeeMenuOptions();
            foreach (var navigation in navigationPoints)
            {
                if (menuElements.ContainsKey(navigation))
                {
                    if (menuElements[navigation] != null) // refactor
                    {
                        menuElements[navigation]?.Click();
                    }
                }
            }
        }

        public Dictionary<string, IWebElement?> EmployeeMenuOptions()
        {
            return new Dictionary<string, IWebElement?>
            {
                 { EmployeeMenuOption.Jobs.ToString(), GetElement(Jobs) },
                 { EmployeeMenuOption.JobSummary.ToString(), GetElement(JobSummary) },
                 { EmployeeMenuOption.JobHistory.ToString(), GetElement(JobHistory) },
                 { EmployeeMenuOption.Personal.ToString(), GetElement(Personal) },
                 { EmployeeMenuOption.EmployeeSummary.ToString(), GetElement(EmployeeSummary) },
                 { EmployeeMenuOption.Compensation.ToString(), GetElement(Compensation) }
            };
        }

        private IWebElement? GetElement(string elementId)
        {
            try
            {
                return _driver.FindElement(By.Id(elementId));
            }
            catch (Exception)
            {
                // refactor to find only elements needed by the test
                return null;
            }
        }
    }
}
