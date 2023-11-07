using UINavigator.Models.UI;

namespace UINavigator.Models.UIModels
{
    public class UIControl
    {
        /// <summary>
        /// Html control id.
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// Html control name.
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// UI control type
        /// </summary>
        public ControlType Type { get; set; }
        /// <summary>
        /// Control value.
        /// </summary>
        public string? Value { get; set; }
        /// <summary>
        /// Set control value using javascript.
        /// </summary>
        public bool SetValueWithJScript { get; set; }
        /// <summary>
        /// Use custom method logic to set control value.
        /// </summary>
        public UISetValueMehod? SetValueMethod { get; set; }
        /// <summary>
        /// Complex control value, 
        /// object definition will be evaluated based on the control type.
        /// </summary>
        public object? ObjectValue { get; set; }
        /// <summary>
        /// Execute delay before setting control value/action.
        /// </summary>
        public int? DelayBeforeValue { get; set; }
        /// <summary>
        /// Delay after setting control value/acion.
        /// </summary>
        public int? DelayAfterRender { get; set; }
        /// <summary>
        /// Validate other controls present in the same page.
        /// </summary>
        public UIValidate? ValidateControls { get; set; }
        /// <summary>
        /// Validate error message/s display.
        /// </summary>
        public List<string>? ErrorMessages { get; set; }
        /// <summary>
        /// Validate information message/s display.
        /// </summary>
        public List<string>? InfoMessages { get; set; }
        /// <summary>
        /// Validate warning message/s display.
        /// </summary>
        public List<string>? WarningMessages { get; set; }
    }
}