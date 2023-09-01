using System;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using UINavigator.Common.Contracts;
using UINavigator.Models;
using UINavigator.Models.UIModels;
using UINavigator.Services;

namespace UINavigator.Common
{
    /// <inheritdoc/>
    public class Utilities : IUtilitiesService
    {
        private readonly IMemCache _cache;

        /// <summary>
        /// Creates a new instance of Utilities.
        /// </summary>
        /// <param name="cache"></param>
        public Utilities(IMemCache cache)
        {
            _cache = cache;
        }

        /// <inheritdoc/>
        public string GenerateSSN()
        {
            var ssn = _cache.Get<string>("ssn");
            if (ssn == null)
            {
                var random = new Random();
                ssn = $"{random.Next(10000).ToString("D4").Substring(0, 3)}" +
                    $"-{random.Next(10000).ToString("D4").Substring(0, 2)}" +
                    $"-{random.Next(10000).ToString("D4").Substring(0, 4)}";
                _cache.Set("ssn", ssn);
            }
            return ssn;
        }

        public string GenerateSIN()
        {
            var sin = _cache.Get<string>("sin");
            if (sin == null)
            {
                var r = new Random();
                int[] s = new int[9];

                for (int i = 0; i < 9; i++)
                    s[i] = r.Next(0, 10);

                sin = $"{s[0]}{s[1]}{s[2]} {s[3]}{s[4]}{s[5]} {s[6]}{s[7]}{s[8]}";
            }
            return sin;
        }

        public string GenerateEMPNum()
        {
            var empnum = _cache.Get<string>("empnum");
            if (empnum == null)
            {
                empnum = "1010";
                _cache.Set("empnum", empnum);
            }
            else
            {
                var newNum = int.Parse(empnum);
                newNum += 1;
                empnum = newNum.ToString();
                _cache.Set("empnum", empnum);
            }
            
            return empnum;
        }

        public double CalculateAnnualSalaryC(string? hour, string? rate)
        {
            if (hour != null && rate != null)
            {
                return double.Parse(hour) * double.Parse(rate) * 22;
            }
            return 0;
        }

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

        public string GetCache()
        {
            var value = _cache.Get<string>("ssn");
            if (value == null)
            {
                value = "";
            }
            return value;
        }
    }
}
