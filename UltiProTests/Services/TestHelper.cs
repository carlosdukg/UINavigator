using Newtonsoft.Json;
using OpenQA.Selenium;
using UINavigator.Common;
using UINavigator.Models.UIModels;
using UINavigator.Common.Contracts;
using OpenQA.Selenium.Support.UI;

namespace UltiProTests.Services
{
    public static class TestHelper
    {
        public static async Task<UITest?> LoadUITest(string location)
        {
            using StreamReader stream = new(location);
            var data = await stream.ReadToEndAsync();

            try
            {
                var receipe = JsonConvert.DeserializeObject<UITest>(data);
                return receipe;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task ProcessUIActionsAsync(
            List<UIAction>? actions,
            IWebDriver? webDriver,
            IUtilitiesService? utils,
            Navigation? pageNav)
        {
            if (actions == null)
            {
                return;
            }

            // execute actions
            foreach (var action in actions)
            {
                if (action == null)
                {
                    continue;
                }

                switch (action.Type)
                {
                    case UIActionType.Navigate:
                        await Task.Run(() =>
                        {
                            if (action.Navigation != null && action.Navigation.IsPopUp)
                            {
                                var windowHandles = webDriver?.WindowHandles;
                                webDriver?.SwitchTo().Window(windowHandles?.Last());
                                pageNav?.Path(action.Navigation);
                            }
                            else if (action.Navigation != null)
                            {
                                pageNav?.Path(action.Navigation);
                            }
                        });
                        break;
                    case UIActionType.Page:
                        await Task.Run(() =>
                        {
                            ProcessTestControlActions(action, webDriver, utils);
                        });
                        break;
                    case UIActionType.PopUp: // check action for pages, and clearly define whats a page or a pop-up
                        await Task.Run(() =>
                        {
                            ProcessTestControlActions(action, webDriver, utils);
                        });
                        break;
                    case UIActionType.Wizard:
                        await Task.Run(() =>
                        {
                            // execute wizard steps
                            var wizardSteps = utils?.GetWizardSteps(action);

                            if (wizardSteps != null)
                            {
                                foreach (var step in wizardSteps)
                                {
                                    webDriver?.SetUIControl(step, utils);
                                }
                            }
                        });
                        break;
                }
            }
        }

        public static void ProcessUIActions(List<UIAction>? actions, IWebDriver? webDriver, IUtilitiesService? utils, Navigation? pageNav)
        {
            if(actions == null || webDriver == null)
            {
                return;
            }

            // execute actions
            foreach (var action in actions)
            {
                switch (action.Type)
                {
                    case UIActionType.Navigate:
                        if(action.Navigation == null)
                        {
                            break;
                        }
                        if (action.Navigation.IsPopUp)
                        {
                            var windowHandles = webDriver.WindowHandles;
                            webDriver.SwitchTo().Window(windowHandles.Last());
                            pageNav?.Path(action.Navigation);
                        }
                        else
                        {
                            pageNav?.Path(action.Navigation);
                        }
                        break;
                    case UIActionType.Page:
                        ProcessTestControlActions(action, webDriver, utils);
                        break;
                    case UIActionType.PopUp: // check action for pages, and clearly define whats a page or a pop-up
                        ProcessTestControlActions(action, webDriver, utils);
                        break;
                    case UIActionType.Wizard:
                        // execute wizard steps
                        var wizardSteps = utils?.GetWizardSteps(action);
                        if (wizardSteps != null)
                        {
                            foreach (var step in wizardSteps)
                            {
                                webDriver.SetUIControl(step, utils);
                            }
                        }
                        break;
                }
            }
        }

        private static void ProcessTestControlActions(UIAction? action, IWebDriver? webDriver, IUtilitiesService? utils)
        {
            if (action?.Controls == null || webDriver == null || utils == null)
            {
                return;
            }

            foreach (var control in action.Controls)
            {
                webDriver.SetUIControl(utils, control);

                ValidateControlValue(control, webDriver);

                ValidateVisibleControls(control.ValidateControls?.VisibleControls, webDriver);

                ValidateHiddenControls(control.ValidateControls?.HiddenControls, webDriver);

                ValidateRequiredControls(control.ValidateControls?.RequiredControls, webDriver);

                ValidateNotRequiredControls(control.ValidateControls?.NotRequiredControls, webDriver);

                ValidateDisabledControls(control.ValidateControls?.DisabledControls, webDriver);

                ValidatePageMessages(control, webDriver);
            }
        }

        private static void ValidateVisibleControls(IEnumerable<string>? visibleControls, IWebDriver webDriver)
        {
            if (visibleControls != null && visibleControls.Any())
            {
                foreach (var displayControl in visibleControls)
                {
                    Assert.IsTrue(webDriver.FindElement(By.Id(displayControl)).Displayed);
                }
            }
        }

        private static void ValidateHiddenControls(IEnumerable<string>? hiddenControls, IWebDriver webDriver)
        {
            if (hiddenControls != null && hiddenControls.Any())
            {
                foreach (var hiddenControl in hiddenControls)
                {
                    var controlElement = webDriver.FindElement(By.Id(hiddenControl));
                    Assert.IsFalse(controlElement.Displayed);
                    Assert.IsTrue(string.IsNullOrWhiteSpace(controlElement.GetAttribute("value")));
                }
            }
        }

        private static void ValidateRequiredControls(IEnumerable<string>? requiredControls, IWebDriver webDriver)
        {
            if (requiredControls != null && requiredControls.Any())
            {
                foreach (var requiredControl in requiredControls)
                {
                    var attribute = webDriver.FindElement(By.Id(requiredControl)).GetAttribute("required");
                    Assert.IsTrue(string.Equals(attribute, "true", StringComparison.OrdinalIgnoreCase));
                }
            }
        }

        private static void ValidateNotRequiredControls(IEnumerable<string>? notRequiredControls, IWebDriver webDriver)
        {
            if (notRequiredControls != null && notRequiredControls.Any())
            {
                foreach (var notRequiredControl in notRequiredControls)
                {
                    var attribute = webDriver.FindElement(By.Id(notRequiredControl)).GetAttribute("required");
                    Assert.IsTrue(attribute == null || string.Equals(attribute, "false", StringComparison.OrdinalIgnoreCase));
                }
            }
        }

        private static void ValidateDisabledControls(IEnumerable<string>? disabledControls, IWebDriver webDriver)
        {
            if (disabledControls != null && disabledControls.Any())
            {
                foreach (var disabledControl in disabledControls)
                {
                    var coreDisabledAttr = webDriver.FindElement(By.Id(disabledControl)).GetAttribute("core-disabled");
                    if (coreDisabledAttr != null)
                    {
                        Assert.IsTrue(string.Equals(coreDisabledAttr, "true", StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        var disabledAttr = webDriver.FindElement(By.Id(disabledControl)).GetAttribute("disabled");
                        Assert.IsTrue(disabledAttr != null);
                    }
                }
            }
        }

        private static void ValidatePageMessages(UIControl control, IWebDriver webDriver)
        {
            if (control.ErrorMessages != null && control.ErrorMessages.Any())
            {
                var errorDiv = webDriver.FindElement(By.Id("ctl00_errMsg"));
                var listUl = errorDiv.FindElement(By.TagName("ul"));
                var links = listUl.FindElements(By.TagName("li"));

                foreach (var errorMessage in control.ErrorMessages)
                {
                    if (!links.Any(l => string.Equals(l.Text, errorMessage, StringComparison.OrdinalIgnoreCase)))
                        Assert.Fail($"Error message not found: {errorMessage}");
                }
            }
            if (control.InfoMessages != null && control.InfoMessages.Any())
            {
                if (control.InfoMessages.Count == 0 && control.InfoMessages[0] == "empty")
                {
                    try
                    {
                        var infoDiv = webDriver.FindElement(By.Id("ctl00_infoMsg"));
                        var listUl = infoDiv.FindElement(By.TagName("ul"));
                        Assert.Fail("There are information messages"); // TODO: improve
                    }
                    catch (Exception)
                    {

                    }
                }
                else
                {
                    var infoDiv = webDriver.FindElement(By.Id("ctl00_infoMsg"));
                    var listUl = infoDiv.FindElement(By.TagName("ul"));
                    var links = listUl.FindElements(By.TagName("li"));

                    foreach (var infoMessage in control.InfoMessages)
                    {
                        if (!links.Any(l => string.Equals(l?.Text, infoMessage, StringComparison.OrdinalIgnoreCase)))
                            Assert.Fail($"Info message not found: {infoMessage}");
                    }
                }
            }
        }

        private static void ValidateControlValue(UIControl control, IWebDriver webDriver)
        {
            if (control.ValidateControls?.ControlValues != null && control.ValidateControls.ControlValues.Any())
            {
                switch (control.Type)
                {
                    case ControlType.Dropdown:
                        DropDownValueValidation(control, webDriver);
                        break;
                    case ControlType.Div:
                        DivValueValidation(control, webDriver);
                        break;
                    case ControlType.UrlLocation:
                        UrlLocationValidation(control, webDriver);
                        break;
                }
            }
        }

        private static void DropDownValueValidation(UIControl control, IWebDriver webDriver)
        {
            var dropdown = control.Id == null ? webDriver.FindElement(By.Id(control.Name)) : webDriver.FindElement(By.Id(control.Id));
            var dropdownElement = new SelectElement(dropdown);

            if (control.ValidateControls?.ControlValues != null && control.ValidateControls.ControlValues.Any())
            {
                foreach (string value in control.ValidateControls.ControlValues)
                {
                    Assert.IsTrue(dropdownElement.Options.Any(o => o.Text == value), $"select option:{value} not found");
                }
            }
        }

        private static void DivValueValidation(UIControl control, IWebDriver webDriver)
        {
            var infoDiv = webDriver.FindElement(By.Id(control.Id));
            if (control.Value != null)
            {
                Assert.IsTrue(infoDiv.Text.Contains(control.Value));
            }
        }

        private static void UrlLocationValidation(UIControl control, IWebDriver webDriver)
        {
            var handles = webDriver.WindowHandles;
            webDriver.SwitchTo().Window(handles.Last());

            var driverUrl = webDriver.Url;

            Assert.IsTrue(driverUrl == control.Value);
        }
    }
}
