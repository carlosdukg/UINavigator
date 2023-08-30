using OpenQA.Selenium;
using UINavigator.Models.Enums.Employee;

namespace UINavigator.Menus.Administration
{
    public class EmployeeMenu
    {
        private readonly IWebDriver _driver;
        const string CareerEducation = "912";
        const string EmployeeVaccinationTest = "5001522";
        const string Pay = "476";
        const string DirectDeposit = "481";
        const string Jobs = "455";
        const string JobSummary = "456";
        const string JobHistory = "791";
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
                 { EmployeeMenuOption.CareerEducation.ToString(), GetElement(CareerEducation) },
                 { EmployeeMenuOption.EmployeeVaccinationTest.ToString(), GetElement(EmployeeVaccinationTest) },
                 { EmployeeMenuOption.Pay.ToString(), GetElement(Pay) },
                 { EmployeeMenuOption.DirectDeposit.ToString(), GetElement(DirectDeposit) },
                 { EmployeeMenuOption.Jobs.ToString(), GetElement(Jobs) },
                 { EmployeeMenuOption.JobSummary.ToString(), GetElement(JobSummary) },
                 { EmployeeMenuOption.JobHistory.ToString(), GetElement(JobHistory) },
                 { EmployeeMenuOption.Personal.ToString(), GetElement(Personal) },
                 { EmployeeMenuOption.EmployeeSummary.ToString(), GetElement(EmployeeSummary) }
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
