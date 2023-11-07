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
        public UIValueMehod? SetValueMethod { get; set; }
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
        public int? DelayAfterValue { get; set; }
        /// <summary>
        /// Control value/s to validate
        /// </summary>
        public UIValidateControl? ValidateControlValue {get; set;}       
        /// <summary>
        /// Other controls present in the same page to be validated.
        /// </summary>
        public UIValidateControls? ValidateOtherControls { get; set; }
        /// <summary>
        /// Error message/s to display.
        /// </summary>
        public List<string>? ErrorMessages { get; set; }
        /// <summary>
        /// Information message/s to display.
        /// </summary>
        public List<string>? InfoMessages { get; set; }
        /// <summary>
        /// Warning message/s to display.
        /// </summary>
        public List<string>? WarningMessages { get; set; }
    }
}