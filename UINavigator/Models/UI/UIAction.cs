
using System.Collections.Generic;

namespace UINavigator.Models.UIModels
{
    public class UIAction
    {
        public UIActionType Type { get; set; }

        public UINavigation Navigation { get; set; }

        public List<UIControl> Controls { get; set; }

        public List<UIWizardStep> WizardSteps { get; set; }
    }
}
