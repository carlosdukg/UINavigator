﻿
namespace UINavigator.Models.UI
{
    public class UIValidateControls
    {
        public string[]? VisibleControls { get; set; }

        public string[]? HiddenControls { get; set; }

        public string[]? RequiredControls { get; set; }

        public string[]? NotRequiredControls { get; set; }

        public string[]? DisabledControls { get; set; }

        public string[]? EnabledControls { get; set; }

        public UIValidateValidationObject? ValidationObject { get; set; }
    }
}
