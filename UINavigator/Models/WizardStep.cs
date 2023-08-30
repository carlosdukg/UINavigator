using UINavigator.Models.UIModels;

namespace UINavigator.Models
{
    public class WizardStep
    {
        public string? Name { get; set; }

        public bool? MoveNext { get; set; }

        public int? DelayInSeconds { get; set; }

        public List<UIControl>? Controls { get; set; }

    }
}
