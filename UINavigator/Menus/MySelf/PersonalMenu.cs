using OpenQA.Selenium;
using UINavigator.Models.Enums.MySelf.Personal;

namespace UINavigator.Menus.MySelf
{
    public class PersonalMenu
    {
        private readonly IWebDriver _driver;
        const string Personal = "81";
        const string NameAddressAndTelephone = "83";

        public PersonalMenu(IWebDriver driver)
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
                { PersonalMenuOption.Personal.ToString(), _driver.FindElement(By.Id(Personal)) },
                { PersonalMenuOption.AddressNameChange.ToString(), _driver.FindElement(By.Id(NameAddressAndTelephone)) }
            };
        }
    }
}
