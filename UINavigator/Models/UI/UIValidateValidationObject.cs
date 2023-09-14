
using UINavigator.Models.Enums;

namespace UINavigator.Models.UI
{
    public class UIValidateValidationObject
    {
        public string? MethodName { get; set; }

        public UIValidateValidationObjectControl[]? MethodControlParams { get; set; }

        public DataTypes? MethodReturnType { get; set; }

        public UIValidateValidationObjectControl? ControlToValidateId { get; set; }

    }
}
