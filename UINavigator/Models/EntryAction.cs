using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UINavigator.Models
{
    public class EntryAction
    {
        public string? Type { get; set; }
        public bool IsWizarrd { get; set; }
        public List<UIWizardStep>? WizardSteps { get; set; }
    }
}
