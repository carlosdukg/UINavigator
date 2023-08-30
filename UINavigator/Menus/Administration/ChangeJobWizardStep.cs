using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UINavigator.Menus.Administration
{
    public enum AddEmployeeWizardStep
    {
        Unknown,
        Start,
        Personal,
        Dates,
        Payroll,
        Attendance,
        DirectDeposit,
        FederalIncome,
        StateTax,
        LocalTax,
        PTO
    }
}
