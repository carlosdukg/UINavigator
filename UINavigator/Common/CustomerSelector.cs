using OpenQA.Selenium;
using UINavigator.Common.Contracts;

namespace UINavigator.Common
{
    /// <inheritdoc/>
    public class CustomerSelector : ICustomerSelectorService
    {
        private readonly IWebDriver _driver;

        /// <summary>
        /// Creates a new instance of the CustomerSelector.
        /// </summary>
        /// <param name="driver"></param>
        public CustomerSelector(IWebDriver driver)
        {
            _driver = driver;
        }

        /// <inheritdoc/>
        public void SelectCustomerCodeWithComponent(string customerCode)
        {
            var searchBox = _driver.FindElement(By.Id("masterCoGrid_TextEntryFilterControlInputBox_0"));
            searchBox.SendKeys(customerCode);

            var searchButton = _driver.FindElement(By.Id("masterCoGrid_filterButton"));
            searchButton.Click();

            // first customer grid
            var masterGrid = _driver.FindElement(By.Id("ctl00_Content_masterCoGrid"));
            var tableRows = masterGrid.FindElements(By.TagName("tr"));
            IWebElement? custLink = null;
            foreach(var row in tableRows.ToList().Skip(1))
            {
                var rowColumns = row.FindElements(By.TagName("td"));
                var codeColumn = rowColumns[0];
                if (codeColumn != null && string.Equals(codeColumn.Text.Trim(), customerCode, StringComparison.OrdinalIgnoreCase))
                {
                    custLink = rowColumns[1]?.FindElement(By.TagName("a"));
                    break;
                }
            }

            if (custLink != null)
            {
                custLink.Click();
            }
            else
            {
                return;
            }

            // second customer grid
            var contentGrid = _driver.FindElement(By.Id("ctl00_Content_componentCoGrid"));
            var contenTableRows = contentGrid.FindElements(By.TagName("tr"));
            IWebElement? contentLink = null;
            foreach (var row in contenTableRows.ToList().Skip(1))
            {
                var rowColumns = row.FindElements(By.TagName("td"));
                var codeColumn = rowColumns[0];
                var codeValue = codeColumn.Text.Trim();
                if (codeColumn != null && codeValue.StartsWith(customerCode, StringComparison.OrdinalIgnoreCase))
                {
                    contentLink = rowColumns[1]?.FindElement(By.TagName("a"));
                    break;
                }
            }

            if (contentLink != null)
            {
                contentLink.Click();
            }
            else
            {
                return;
            }
        }
    }
}
