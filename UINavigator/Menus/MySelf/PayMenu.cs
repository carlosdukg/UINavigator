using OpenQA.Selenium;
using UINavigator.Models.Enums.MySelf.Pay;

namespace UINavigator.Menus.MySelf
{
    public class PayMenu
    {
        private readonly IWebDriver _driver;
        const string Pay = "95";
        const string DirectDeposit = "19";

        public PayMenu(IWebDriver driver)
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
                { PayMenuOption.Pay.ToString(), _driver.FindElement(By.Id(Pay)) },
                { PayMenuOption.DirectDeposit.ToString(), _driver.FindElement(By.Id(DirectDeposit)) }
            };
        }
    }
}
