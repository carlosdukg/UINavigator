using UINavigator.Common.Contracts;
using UINavigator.Models;
using UINavigator.Models.UIModels;

namespace UINavigator.Common
{
    /// <inheritdoc/>
    public class Utilities : IUtilitiesService
    {
        /// <summary>
        /// Creates a new instance of Utilities.
        /// </summary>
        /// <param name="cache"></param>
        public Utilities() { }

        /// <inheritdoc/>
        public UIWizardStep? GetWizardStep(Enum stepName, EntryAction entryAction)
        {
            var stepActions = entryAction?
                .WizardSteps?
                .SingleOrDefault(s => string.Equals(s.Name, stepName.ToString(), StringComparison.OrdinalIgnoreCase));

            return stepActions;
        }

        /// <inheritdoc/>
        public IEnumerable<UIWizardStep?> GetWizardSteps(UIAction entryAction)
        {
            var steps = entryAction?.WizardSteps;

            return steps ?? Enumerable.Empty<UIWizardStep?>();
        }
    }
}
