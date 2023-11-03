using UINavigator.Models.UI;

namespace UINavigator.Models.UIModels
{
    public class UIControl
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public ControlType Type { get; set; }

        public string? Value { get; set; }

        public bool SetValueWithJScript { get; set; }

        public object? ObjectValue { get; set; }

        public int? DelayBeforeInSeconds { get; set; }

        public int? DelayInSeconds { get; set; }

        public UIValidate? ValidateControls { get; set; }

        public List<string>? ErrorMessages { get; set; }

        public List<string>? InfoMessages { get; set; }

        public List<string>? WarningMessages { get; set; }
    }
}