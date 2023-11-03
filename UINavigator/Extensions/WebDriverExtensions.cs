using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System.Reflection;
using UINavigator.Models.UIModels;
using UINavigator.Models.UI;
using Newtonsoft.Json.Linq;

namespace UINavigator.Extensions
{
    /// <summary>
    /// Selenium web driver extensions.
    /// </summary>
    public static class WebDriverExtensions
    {
        private const string LastFrame = "last";

        /// <summary>
        /// Set HTML UI control.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="utilities"></param>
        /// <param name="control"></param>
        public static void SetUIControl(this IWebDriver driver, UIControl control)
        {
            try
            {
                if (control == null)
                {
                    return;
                }

                if (control.DelayInSeconds != null)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(control.DelayInSeconds.Value));
                }

                ProcessControlAction(control, driver);
            }
            catch (ElementNotInteractableException)
            {
                //TODO: log error
            }
            catch (NoSuchElementException)
            {
                //TODO: log error
            }
        }

        /// <summary>
        /// Clicks move next(Next) web app button.
        /// </summary>
        /// <param name="driver"></param>
        public static void MoveNext(this IWebDriver driver)
        {
            var nextButton = driver.FindElement(By.Id("ctl00_btnNext"));
            nextButton.Click();
        }

        /// <summary>
        /// Clicks move previous(Prev) web app button.
        /// </summary>
        /// <param name="driver"></param>
        public static void MovePrev(this IWebDriver driver)
        {
            var nextButton = driver.FindElement(By.Id("ctl00_btnPrev"));
            nextButton.Click();
        }

        /// <summary>
        /// Clicks add(Add) web app button.
        /// </summary>
        /// <param name="driver"></param>
        public static void ClickAddButton(this IWebDriver driver)
        {
            var addButton = driver.FindElement(By.Id("ctl00_btnAdd"));
            addButton.Click();
        }

        /// <summary>
        /// Clicks search(Search) web app button.
        /// </summary>
        /// <param name="driver"></param>
        public static void ClickSearchButton(this IWebDriver driver)
        {
            var addButton = driver.FindElement(By.Id("GridView1_filterButton"));
            addButton.Click();
        }

        /// <summary>
        /// Clicks save(Save) web app button.
        /// </summary>
        /// <param name="driver"></param>
        public static void ClickSaveButton(this IWebDriver driver)
        {
            var saveButton = driver.FindElement(By.Id("ctl00_btnSave"));
            saveButton.Click();
        }

        /// <summary>
        /// Selects grid link based on control id, location and text.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="gridId"></param>
        /// <param name="rowColumn"></param>
        /// <param name="rowLevel"></param>
        /// <param name="elementId"></param>
        /// <param name="elementText"></param>
        /// <param name="elementTagName"></param>
        public static void SelectGridElmentAtRowColumnPostion(this IWebDriver driver,
            string gridId,
            int rowColumn,
            int? rowLevel = null,
            string? elementId = null,
            string? elementText = null,
            string? elementTagName = null)
        {
            var masterGrid = driver.FindElement(By.Id(gridId));
            var tableBody = masterGrid.FindElement(By.TagName("tbody"));
            var tableRows = tableBody.FindElements(By.TagName("tr"));
            IWebElement? webElement = null;
            var rowCount = 0;
            foreach (var row in tableRows.ToList())
            {
                if (rowLevel != null && rowCount == rowLevel + 1)
                {
                    break;
                }
                var rowColumns = row.FindElements(By.TagName("td"));
                var column = rowColumns[rowColumn];
                if (elementText == null)
                {
                    webElement = FindElement(column, elementId, elementTagName);
                    if (rowLevel == null && webElement != null)
                    {
                        break;
                    }
                }
                else
                {
                    if (column != null && string.Equals(column.Text.Trim(), elementText, StringComparison.OrdinalIgnoreCase))
                    {
                        webElement = FindElement(column, elementId, elementTagName);
                        break;
                    }
                }
            }

            if (webElement != null)
            {
                webElement.Click();
            }
            else
            {
                return;
            }
        }

        private static void ProcessControlAction(UIControl control, IWebDriver driver)
        {
            if (control.DelayBeforeInSeconds != null)
            {
                Thread.Sleep(TimeSpan.FromSeconds(control.DelayBeforeInSeconds.Value));
            }

            switch (control.Type)
            {
                case ControlType.Input:
                    {
                        var input = control.Id == null ? driver.FindElement(By.Id(control.Name)) : driver.FindElement(By.Id(control.Id));

                        if (control.Value == "" && string.IsNullOrWhiteSpace(control.Setter) && control.MethodsClass == null)
                        {
                            input.Clear();
                        }
                        else if (control.Value != null && control.SetValueWithJScript)
                        {
                            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                            input.SendKeys(Keys.Return);
                            js.ExecuteScript($"document.getElementById('{control.Id}').value = '${control.Value}'");
                            js.ExecuteScript($"document.getElementById('{control.Id}').dispatchEvent(new Event('change'))");
                            Thread.Sleep(TimeSpan.FromSeconds(1));
                            driver.FindElement(By.XPath("//html")).Click();
                        }
                        else if (!string.IsNullOrWhiteSpace(control.Setter) && control.MethodsClass != null)
                        {
                            var methodName = control.Setter.Trim();
                            Type myEmpType = control.MethodsClass.GetType();
                            MethodInfo? ctrlMethod = myEmpType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);

                            var controlValue = string.Empty;
                            if (ctrlMethod != null)
                            {
                                if (control.SetterPatemeters != null && control.SetterPatemeters.Any())
                                {
                                    controlValue = (string?)ctrlMethod.Invoke(control.MethodsClass, control.SetterPatemeters.ToArray());
                                }
                                else
                                {
                                    controlValue = (string?)ctrlMethod.Invoke(control.MethodsClass, null);
                                }
                            }

                            Thread.Sleep(TimeSpan.FromSeconds(1));
                            input.SendKeys(controlValue);

                            // exit out the input to fire events if needed
                            driver.FindElement(By.XPath("//html")).Click();
                        }
                        else if (control.Value != null)
                        {
                            input.Clear();
                            Thread.Sleep(TimeSpan.FromSeconds(1));
                            input.SendKeys(control.Value);

                            // exit out the input to fire events if needed
                            driver.FindElement(By.XPath("//html")).Click();
                        }
                        break;
                    }
                case ControlType.Dropdown:
                    {
                        var dropdown = control.Id == null ? driver.FindElement(By.Id(control.Name)) : driver.FindElement(By.Id(control.Id));
                        var dropdownElement = new SelectElement(dropdown);
                        if (control.Value != null)
                        {
                            dropdownElement.SelectByValue(control.Value);
                        }
                        break;
                    }
                case ControlType.Radio:
                    {
                        var radio = control.Id == null ? driver.FindElement(By.Id(control.Name)) : driver.FindElement(By.Id(control.Id));
                        radio.Click();
                        break;
                    }
                case ControlType.ButtonClick:
                    {
                        var button = control.Id == null ? driver.FindElement(By.Id(control.Name)) : driver.FindElement(By.Id(control.Id));
                        button.Click();
                        break;
                    }
                case ControlType.GridSearchAndSelect:
                    {
                        if (control.ObjectValue != null)
                        {
                            var gridValue = ((JObject)control.ObjectValue).ToObject<GridValue>();
                            if (gridValue?.Search?.FindByContorlId != null && !string.IsNullOrWhiteSpace(gridValue?.Search?.FindByControlValue))
                            {
                                var findByControl = driver.FindElement(By.Id(gridValue?.Search?.FindByContorlId));
                                var findByDropdown = new SelectElement(findByControl);
                                findByDropdown.SelectByValue(gridValue?.Search.FindByControlValue);
                            }

                            if (gridValue?.Search?.OperatorControlId != null && !string.IsNullOrWhiteSpace(gridValue?.Search?.OperatorControlValue))
                            {
                                var operatorControl = driver.FindElement(By.Id(gridValue?.Search?.OperatorControlId));
                                var operatorDropdown = new SelectElement(operatorControl);
                                operatorDropdown.SelectByValue(gridValue?.Search?.OperatorControlValue);
                            }

                            if (gridValue?.Search?.FromControlId != null && !string.IsNullOrWhiteSpace(gridValue?.Search?.FromControlValue))
                            {
                                var operatorControl = driver.FindElement(By.Id(gridValue?.Search?.OperatorControlId));
                                var operatorDropdown = new SelectElement(operatorControl);
                                operatorDropdown.SelectByValue(gridValue?.Search?.OperatorControlValue);
                            }

                            if (gridValue?.Search?.ToControlId != null && !string.IsNullOrWhiteSpace(gridValue?.Search?.ToControlValue))
                            {
                                var operatorControl = driver.FindElement(By.Id(gridValue?.Search?.OperatorControlId));
                                var operatorDropdown = new SelectElement(operatorControl);
                                operatorDropdown.SelectByValue(gridValue?.Search?.OperatorControlValue);
                            }

                            if (gridValue?.Search?.SearchInputControlId != null && !string.IsNullOrWhiteSpace(gridValue?.Search?.SearchInputControlValue))
                            {
                                var searchInput = driver.FindElement(By.Id(gridValue?.Search?.SearchInputControlId));
                                searchInput.SendKeys(gridValue?.Search?.SearchInputControlValue);
                            }

                            var searchButton = driver.FindElement(By.Id(gridValue?.Search?.SearchButtonControlId));
                            searchButton.Click();

                            // delay to allow the grid to bind its data
                            Thread.Sleep(TimeSpan.FromMilliseconds(1000));

                            // active page grid
                            var masterGrid = driver.FindElement(By.Id(control.Id));
                            var masterBody = masterGrid.FindElement(By.TagName("tbody"));
                            var tableRows = masterBody.FindElements(By.TagName("tr"));
                            IWebElement? custLink = null;
                            foreach (var row in tableRows.ToList())
                            {
                                var rowColumns = row.FindElements(By.TagName("td"));

                                IWebElement codeColumn;
                                var selectText = gridValue?.Select?.Value;
                                var selectColIndex = gridValue?.Select?.ColIndex == null ? 0 : gridValue.Select.ColIndex;
                                codeColumn = rowColumns[selectColIndex];

                                if (codeColumn != null && string.Equals(codeColumn.Text.Trim(), selectText, StringComparison.OrdinalIgnoreCase))
                                {
                                    // TODO: refactor: assumed that the first column contains the link to the next page, and the control is a html anchor
                                    custLink = rowColumns[0].FindElement(By.TagName("a"));
                                    break;
                                }
                            }

                            if (custLink != null)
                            {
                                custLink.Click();
                            }
                        }
                        break;
                    }
                case ControlType.GridViewAndSelect:
                    {
                        // active page grid
                        var masterGrid = driver.FindElement(By.Id(control.Id));
                        var masterBody = masterGrid.FindElement(By.TagName("tbody"));
                        var tableRows = masterBody.FindElements(By.TagName("tr"));
                        foreach (var row in tableRows.ToList())
                        {
                            var rowColumns = row.FindElements(By.TagName("td"));
                            foreach (var column in rowColumns.ToList())
                            {
                                if (column != null && string.Equals(column.Text.Trim(), control.Value, StringComparison.OrdinalIgnoreCase))
                                {
                                    IWebElement custLink = column.FindElement(By.TagName("a"));
                                    custLink.Click();
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case ControlType.SwitchFrame:
                    {
                        if (control.Value?.ToLower() == LastFrame.ToLower())
                        {
                            var windowHandles = driver.WindowHandles;
                            driver.SwitchTo().Window(windowHandles.Last());
                        }
                        else
                        {
                            driver.SwitchTo().Frame(driver.FindElement(By.XPath($"//iframe[@id='{control.Id}']")));
                        }
                        break;
                    }
                case ControlType.Calendar:
                    {
                        var input = control.Id == null ? driver.FindElement(By.Id(control.Name)) : driver.FindElement(By.Id(control.Id));
                        if (control.Value == "")
                        {
                            input.SendKeys(Keys.Control + "a");
                            input.SendKeys(Keys.Delete);
                        }
                        else
                        {
                            input.SendKeys(control.Value);
                        }
                        // exit out the calendar input to fire events if needed
                        driver.FindElement(By.XPath("//html")).Click();
                        break;
                    }
                case ControlType.CheckBox:
                    {
                        var checkBox = control.Id == null ? driver.FindElement(By.Id(control.Name)) : driver.FindElement(By.Id(control.Id));
                        checkBox.Click();
                        break;
                    }
                case ControlType.AddressSearch:
                    {
                        var input = control.Id == null ? driver.FindElement(By.Id(control.Name)) : driver.FindElement(By.Id(control.Id));
                        input.Clear();
                        input.SendKeys(control.Value);

                        Thread.Sleep(2000);

                        input.SendKeys(Keys.Return);
                        input.SendKeys(Keys.Return);
                        break;
                    }
                case ControlType.RightMenuLink:
                    {
                        var selections = control.Value?.Split(":");
                        if (selections != null)
                        {
                            var ulElementIndex = int.Parse(selections[0]);
                            var liElementIndex = int.Parse(selections[1]);

                            var thingsICanDoContainer = driver?.FindElement(By.Id("ctl00_thingsICanDoContentBoxBodyDiv"));
                            var ulPanels = thingsICanDoContainer?.FindElements(By.TagName("ul"));
                            var ulPanel = ulPanels?[ulElementIndex];

                            var ulPanelList = ulPanel?.FindElements(By.TagName("li"));

                            var listItem = ulPanelList?[liElementIndex];
                            listItem?.Click();
                        }
                        break;
                    }
                case ControlType.LookUp:
                    {
                        var lookUpData = control.Value?.Split(":");
                        if (lookUpData != null)
                        {
                            var index = int.Parse(lookUpData[0]);
                            var htmlElementTag = lookUpData[1];

                            var lookUpArea = control.Id == null ? driver.FindElement(By.Id(control.Name)) : driver.FindElement(By.Id(control.Id));
                            var htmlElements = lookUpArea?.FindElements(By.TagName(htmlElementTag));
                            var htmlElement = htmlElements?[index];

                            htmlElement?.Click();
                        }
                        break;
                    }
            }
        }

        private static IWebElement? FindElement(IWebElement element, string? elementId, string? elementTagName)
        {
            IWebElement? webElement = null;
            if (elementId != null)
            {
                webElement = element?.FindElement(By.Id(elementId));
            }
            else if (elementTagName != null)
            {
                webElement = element?.FindElement(By.TagName(elementTagName));
            }
            return webElement;
        }
    }
}
