using OpenQA.Selenium;
using UINavigator.Models.Enums.Employee;

namespace UINavigator.Menus.Employee
{
    public class EmployeeMenu : IMenuItem
    {
        private readonly IWebDriver _driver;
        const string CareerEducationId = "912";
        const string EmployeeVaccinationTestId = "5001522";
        const string PayId = "476";
        const string DirectDepositId = "481";
        const string JobsId = "455";
        const string JobSummaryId = "456";
        const string CompensationId = "463";
        const string JobHistoryId = "791";
        const string PersonalId = "338";
        const string EmployeeSummaryId = "17";

        private List<MenuElement> _menuElements;
        private readonly string _name;

        List<MenuElement> IMenuItem.Items { get => _menuElements; set => _menuElements = value; }

        public string?Id;

        public string Name => _name;

        string? IMenuItem.Id => throw new NotImplementedException();

        public EmployeeMenu(IWebDriver driver)
        {
            _driver = driver;
            _name = "EmployeeMenu";
            _menuElements = new List<MenuElement>
            {
                new MenuElement
                {
                    Id = CareerEducationId,
                    Name = "CareerEducation"
                },
                new MenuElement
                {
                    Id = PayId,
                    Name = "Pay"
                },
                new MenuElement
                {
                    Id = DirectDepositId,
                    Name = "DirectDeposit"
                },
                new MenuElement
                {
                    Id = JobSummaryId,
                    Name = "JobSummary"
                },
                new MenuElement
                {
                    Id = JobHistoryId,
                    Name = "JobHistory"
                },
                new MenuElement
                {
                    Id = PersonalId,
                    Name = "Personal"
                },
                new MenuElement
                {
                    Id = EmployeeSummaryId,
                    Name = "EmployeeSummary"
                }
            };
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
                 { EmployeeMenuOption.CareerEducation.ToString(), GetElement(CareerEducationId) },
                 { EmployeeMenuOption.EmployeeVaccinationTest.ToString(), GetElement(EmployeeVaccinationTestId) },
                 { EmployeeMenuOption.Pay.ToString(), GetElement(PayId) },
                 { EmployeeMenuOption.DirectDeposit.ToString(), GetElement(DirectDepositId) },
                 { EmployeeMenuOption.Jobs.ToString(), GetElement(JobsId) },
                 { EmployeeMenuOption.JobSummary.ToString(), GetElement(JobSummaryId) },
                 { EmployeeMenuOption.JobHistory.ToString(), GetElement(JobHistoryId) },
                 { EmployeeMenuOption.Personal.ToString(), GetElement(PersonalId) },
                 { EmployeeMenuOption.EmployeeSummary.ToString(), GetElement(EmployeeSummaryId) },
                 { EmployeeMenuOption.Compensation.ToString(), GetElement(CompensationId) }
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
