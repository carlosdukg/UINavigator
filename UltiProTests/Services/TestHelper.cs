using Newtonsoft.Json;
using OpenQA.Selenium;
using UINavigator.Common;
using UINavigator.Models.UIModels;
using UINavigator.Common.Contracts;
using OpenQA.Selenium.Support.UI;
using System.Reflection;
using UINavigator.Models.Enums;

namespace UltiProTests.Services
{
    public static class TestHelper
    {
        public static async Task<UITest?> LoadUITest(string location)
        {
            using StreamReader stream = new(location);
            var data = await stream.ReadToEndAsync();

            var receipe = JsonConvert.DeserializeObject<UITest>(data);
            return receipe;
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
                            ProcessControlActions(action.Controls, webDriver, utils);
                        });
                        break;
                    case UIActionType.PopUp: // check action for pages, and clearly define whats a page or a pop-up
                        await Task.Run(() =>
                        {
                            ProcessControlActions(action.Controls, webDriver, utils);
                        });
                        break;
                    case UIActionType.Wizard:
                        await Task.Run(() =>
                        {
                            ProcessWizardControlActions(action, webDriver, utils);
                        });
                        break;
                }
            }
        }

        public static void ProcessUIActions(List<UIAction>? actions, IWebDriver? webDriver, IUtilitiesService? utils, Navigation? pageNav)
        {
            if (actions == null || webDriver == null)
            {
                return;
            }

            // execute actions
            foreach (var action in actions)
            {
                switch (action.Type)
                {
                    case UIActionType.Navigate:
                        if (action.Navigation == null)
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
                        ProcessControlActions(action.Controls, webDriver, utils);
                        break;
                    case UIActionType.PopUp: // check action for pages, and clearly define whats a page or a pop-up
                        ProcessControlActions(action.Controls, webDriver, utils);
                        break;
                    case UIActionType.Wizard:
                        ProcessWizardControlActions(action, webDriver, utils);
                        break;
                }
            }
        }

        private static void ProcessControlActions(List<UIControl>? controls, IWebDriver? webDriver, IUtilitiesService? utils)
        {
            if (webDriver == null || utils == null)
            {
                return;
            }

            if (controls == null || !controls.Any())
            {
                return;
            }

            foreach (var control in controls)
            {
                webDriver.SetUIControl(utils, control);

                ValidateControlValue(control, webDriver);

                ValidateVisibleControls(control.ValidateControls?.VisibleControls, webDriver);

                ValidateHiddenControls(control.ValidateControls?.HiddenControls, webDriver);

                ValidateRequiredControls(control.ValidateControls?.RequiredControls, webDriver);

                ValidateNotRequiredControls(control.ValidateControls?.NotRequiredControls, webDriver);

                ValidateDisabledControls(control.ValidateControls?.DisabledControls, webDriver);

                ValidateEnabledControls(control.ValidateControls?.EnabledControls, webDriver);

                ValidatePageMessages(control, webDriver);

                ValidateValidationObject(control, webDriver, utils);
            }
        }

        private static void ProcessWizardControlActions(UIAction? action, IWebDriver? webDriver, IUtilitiesService? utils)
        {
            if (action == null || webDriver == null || utils == null)
            {
                return;
            }

            var wizardSteps = utils?.GetWizardSteps(action);
            if (wizardSteps == null || !wizardSteps.Any())
            {
                return;
            }

            foreach (var step in wizardSteps)
            {
                if (step == null)
                {
                    continue;
                }

                ProcessControlActions(step.Controls, webDriver, utils);

                if (step.MoveNext.HasValue && step.MoveNext.Value)
                {
                    webDriver.MoveNext();
                }
                if (step.MovePrev.HasValue && step.MovePrev.Value)
                {
                    webDriver.MovePrev();
                }
                if (step.DelayInSeconds != null)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(step.DelayInSeconds.Value));
                }
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

        private static void ValidateEnabledControls(IEnumerable<string>? enabledControls, IWebDriver webDriver)
        {
            if (enabledControls != null && enabledControls.Any())
            {
                foreach (var enabledControl in enabledControls)
                {
                    var coreDisabledAttr = webDriver.FindElement(By.Id(enabledControl)).GetAttribute("core-disabled");
                    Assert.IsTrue(coreDisabledAttr == null);

                    var disabledAttr = webDriver.FindElement(By.Id(enabledControl)).GetAttribute("disabled");
                    Assert.IsTrue(disabledAttr == null);
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

        private static void ValidateValidationObject(UIControl control, IWebDriver webDriver, IUtilitiesService utils)
        {
            if (control?.ValidateControls?.ValidationObject != null)
            {
                var methodName = control?.ValidateControls?.ValidationObject?.MethodName?.Trim();
                var methodParams = control?.ValidateControls?.ValidationObject?.MethodControlParams;

                Type utilsType = utils.GetType();
                if (!string.IsNullOrWhiteSpace(methodName))
                {
                    MethodInfo? ctrlMethod = utilsType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static);
                    object? expectedControlValue;
                    if (control?.ValidateControls.ValidationObject.MethodReturnType == DataTypes.Double)
                    {
                        if (methodParams != null)
                        {
                            var parsedParams = methodParams.Select(x => x.Type == ControlType.Any 
                                || x.Type == ControlType.Span ? 
                                GetHtmlValue(x.Id, webDriver) : 
                                GetInputValue(x.Id, webDriver)).ToArray();
                            expectedControlValue = (double?)ctrlMethod?.Invoke(utils, parsedParams);
                        }
                        else
                        {
                            expectedControlValue = (double?)ctrlMethod?.Invoke(utils, null);
                        }

                        var controlToValidate = control?.ValidateControls.ValidationObject.ControlToValidateId;
                        var controlToValidateValue = controlToValidate?.Type == ControlType.Any || controlToValidate?.Type == ControlType.Span ?
                            GetHtmlValue(controlToValidate?.Id, webDriver) :
                            GetInputValue(controlToValidate?.Id, webDriver);

                        Assert.IsTrue((double?)expectedControlValue == 
                            double.Parse(controlToValidateValue), $"expected value:{(double)expectedControlValue}, control value:{double.Parse(controlToValidateValue)}");
                    }
                    else if (control?.ValidateControls.ValidationObject.MethodReturnType == DataTypes.Int)
                    {
                        if (methodParams != null)
                        {
                            var parsedParams = methodParams.Select(x => GetHtmlValue(x.Id, webDriver)).ToArray();
                            expectedControlValue = (int?)ctrlMethod?.Invoke(utilsType, new[] { parsedParams });
                        }
                        else
                        {
                            expectedControlValue = (int?)ctrlMethod?.Invoke(utilsType, null);
                        }
                        var controlToValidate = control?.ValidateControls.ValidationObject.ControlToValidateId;
                        var controlToValidateValue = GetHtmlValue(controlToValidate?.Id, webDriver);

                        Assert.IsTrue((int?)expectedControlValue == int.Parse(controlToValidateValue));
                    }
                    else if (control?.ValidateControls.ValidationObject.MethodReturnType == DataTypes.String)
                    {
                        if (methodParams != null)
                        {
                            var parsedParams = methodParams.Select(x => GetHtmlValue(x.Id, webDriver)).ToArray();
                            expectedControlValue = (string?)ctrlMethod?.Invoke(utils, parsedParams);
                        }
                        else
                        {
                            expectedControlValue = (string?)ctrlMethod?.Invoke(utils, null);
                        }
                        var controlToValidate = control?.ValidateControls.ValidationObject.ControlToValidateId;
                        var controlToValidateValue = GetHtmlValue(controlToValidate?.Id, webDriver);

                        Assert.IsTrue(string.Equals(expectedControlValue, controlToValidateValue));
                    }
                    else if (control?.ValidateControls.ValidationObject.MethodReturnType == DataTypes.Bool)
                    {
                        if (methodParams != null)
                        {
                            var parsedParams = methodParams.Select(x => x.Type == ControlType.Any 
                                || x.Type == ControlType.Span ? 
                                    GetHtmlValue(x.Id, webDriver) : 
                                    GetInputValue(x.Id, webDriver)).ToArray();
                            expectedControlValue = (bool?)ctrlMethod?.Invoke(utils, parsedParams);
                        }
                        else
                        {
                            expectedControlValue = (bool?)ctrlMethod?.Invoke(utils, null);
                        }

                        Assert.IsTrue((bool?)expectedControlValue);
                    }
                }
            }
        }

        private static void ValidateControlValue(UIControl control, IWebDriver webDriver)
        {
            switch (control.Type)
            {
                case ControlType.Dropdown:
                    DropDownValueValidation(control, webDriver);
                    break;
                case ControlType.Div:
                    HtmlValueValidation(control, webDriver);
                    break;
                case ControlType.UrlLocation:
                    UrlLocationValidation(control, webDriver);
                    break;
                case ControlType.Span:
                    HtmlValueValidation(control, webDriver);
                    break;
                case ControlType.Any:
                    HtmlValueValidation(control, webDriver);
                    break;
            }
        }

        private static void DropDownValueValidation(UIControl control, IWebDriver webDriver)
        {
            var dropdown = control.Id == null ? webDriver.FindElement(By.Id(control.Name)) : webDriver.FindElement(By.Id(control.Id));
            var dropdownElement = new SelectElement(dropdown);

            if (control.ValidateControls?.ControlValues != null && control.ValidateControls.ControlValues.Any())
            {
                if (control.ValidateControls.ControlValues.Count == 1)
                {
                    var option = dropdownElement.SelectedOption;
                    var optionSelected = option.Text;
                    var validateValue = control.ValidateControls?.ControlValues.FirstOrDefault();
                    Assert.IsTrue(option.Text == validateValue, $"select option:{validateValue} not found");
                }
                else
                {
                    foreach (string value in control.ValidateControls.ControlValues)
                    {
                        Assert.IsTrue(dropdownElement.Options.Any(o => o.Text == value), $"select option:{value} not found");
                    }
                }
            }
        }

        private static void HtmlValueValidation(UIControl control, IWebDriver webDriver)
        {
            if (control?.ValidateControls?.ControlValue != null)
            {
                var htmlCtrl = webDriver.FindElement(By.Id(control.Id));
                var htmlCtrlText = htmlCtrl.Text;
                Assert.IsTrue(htmlCtrlText.Contains(control.ValidateControls.ControlValue), $"HTML text {htmlCtrlText} not found in control {control.Id}");
            }
        }

        private static void UrlLocationValidation(UIControl control, IWebDriver webDriver)
        {
            var handles = webDriver.WindowHandles;
            webDriver.SwitchTo().Window(handles.Last());

            var driverUrl = webDriver.Url;

            Assert.IsTrue(driverUrl == control.Value);
        }

        private static string GetInputValue(string? id, IWebDriver webDriver)
        {
            if (id == null)
            {
                return string.Empty;
            }

            var htmlCtrl = webDriver.FindElement(By.Id(id));
            var htmlCtrlValue = htmlCtrl.GetAttribute("value").Contains('$') ? htmlCtrl.GetAttribute("value")[1..] : htmlCtrl.GetAttribute("value");
            return htmlCtrlValue;
        }

        private static string GetHtmlValue(string? id, IWebDriver webDriver)
        {
            if(id == null)
            {
                return string.Empty;
            }
            
            var htmlCtrl = webDriver.FindElement(By.Id(id));
            var htmlCtrlText = htmlCtrl.Text.Contains('$') ? htmlCtrl.Text[1..] : htmlCtrl.Text;
            return htmlCtrlText;
        }
    }
}
