﻿using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using UINavigator.Models;
using System.Reflection;
using UINavigator.Models.UIModels;
using UINavigator.Common.Contracts;

namespace UINavigator.Common
{
    /// <summary>
    /// Selenium web driver extensions.
    /// </summary>
    public static class WebDriverExtensions
    {
        private const string LastFrame = "last";

        /// <summary>
        /// Set HTML UI controls.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="steps"></param>
        /// <param name="utilities"></param>
        public static void SetUIControls(this IWebDriver driver, List<UIWizardStep?> steps, IUtilitiesService utilities)
        {
            foreach (var step in steps)
            {
                SetUIControl(driver, step, utilities);
            }
        }

        /// <summary>
        /// Set HTML UI control.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="step"></param>
        /// <param name="utilities"></param>
        public static void SetUIControl(this IWebDriver driver, UIWizardStep? step, IUtilitiesService? utilities)
        {
            if (step == null || utilities == null)
            {
                return;
            }
            var validControls = step.Controls?.Where(c => c != null);

            if (validControls != null)
            {
                foreach (var control in validControls)
                {
                    try
                    {
                        ProcessControlAction(control, utilities, driver);
                    }
                    catch (ElementNotInteractableException)
                    {
                        //TODO: log error
                    }
                    catch (NoSuchElementException)
                    {
                        //TODO: log error
                    }

                    if (control.DelayInSeconds != null)
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(control.DelayInSeconds.Value));
                    }
                }
            }

            if (step.MoveNext.HasValue && step.MoveNext.Value)
            {
                driver.MoveNext();
            }

            if (step.MovePrev.HasValue && step.MovePrev.Value)
            {
                driver.MovePrev();
            }

            if (step.DelayInSeconds != null)
            {
                Thread.Sleep(TimeSpan.FromSeconds(step.DelayInSeconds.Value));
            }
        }

        /// <summary>
        /// Set HTML UI control.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="utilities"></param>
        /// <param name="control"></param>
        public static void SetUIControl(this IWebDriver driver, IUtilitiesService utilities, UIControl control)
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

                ProcessControlAction(control, utilities, driver);
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

        private static void ProcessControlAction(UIControl control, IUtilitiesService utilities, IWebDriver driver)
        {
            switch (control.Type)
            {
                case ControlType.Input:
                    {
                        var input = control.Id == null ? driver.FindElement(By.Id(control.Name)) : driver.FindElement(By.Id(control.Id));
                        if (control.Value != null && control.Value.StartsWith("method:"))
                        {
                            var methodName = control.Value.Replace("method:", "").Trim();
                            Type myEmpType = utilities.GetType();
                            MethodInfo? ctrlMethod = myEmpType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);

                            var paramIndex = control.Value.IndexOf("#");
                            var param = paramIndex == -1 ? "" : control.Value.Substring(control.Value.IndexOf("#") + 1).Trim();

                            var controlValue = string.Empty;
                            if (ctrlMethod != null)
                            {
                                if (!string.IsNullOrWhiteSpace(param))
                                {
                                    controlValue = (string?)ctrlMethod.Invoke(utilities, new[] { param });
                                }
                                else
                                {
                                    controlValue = (string?)ctrlMethod.Invoke(utilities, null);
                                }
                            }

                            Thread.Sleep(TimeSpan.FromSeconds(1));
                            input.SendKeys(controlValue);

                            // exit out the input to fire events if needed
                            driver.FindElement(By.XPath("//html")).Click();
                        }
                        else
                        {
                            if (control.Value == "")
                            {
                                input.Clear();
                            }
                            else
                            {
                                input.Clear();
                                input.SendKeys(control.Value);

                                // exit out the input to fire events if needed
                                driver.FindElement(By.XPath("//html")).Click();
                            }
                        }
                        break;
                    }
                case ControlType.Dropdown:
                    {
                        var dropdown = control.Id == null ? driver.FindElement(By.Id(control.Name)) : driver.FindElement(By.Id(control.Id));
                        var dropdownElement = new SelectElement(dropdown);
                        dropdownElement.SelectByValue(control.Value);
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
                        var searchButton = driver.FindElement(By.Id("GridView1_filterButton"));
                        searchButton.Click();

                        // TODO: add search entry

                        Thread.Sleep(TimeSpan.FromMilliseconds(1000));

                        // active page grid
                        var masterGrid = driver.FindElement(By.Id(control.Id));
                        var masterBody = masterGrid.FindElement(By.TagName("tbody"));
                        var tableRows = masterBody.FindElements(By.TagName("tr"));
                        IWebElement? custLink = null;
                        foreach (var row in tableRows.ToList())
                        {
                            var rowColumns = row.FindElements(By.TagName("td"));
                            var controlValues = control.Value?.Split(":");
                            IWebElement codeColumn;
                            var searchText = string.Empty;
                            if (controlValues == null || !controlValues.Any() || controlValues.Count() == 1)
                            {
                                codeColumn = rowColumns[0];
                                searchText = control.Value;
                            }
                            else
                            {
                                var index = int.Parse(controlValues[0]);
                                codeColumn = rowColumns[index];
                                searchText = controlValues[1];
                            }

                            if (codeColumn != null && string.Equals(codeColumn.Text.Trim(), searchText, StringComparison.OrdinalIgnoreCase))
                            {
                                custLink = rowColumns[0].FindElement(By.TagName("a"));
                                break;
                            }
                        }

                        if (custLink != null)
                        {
                            custLink.Click();
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
