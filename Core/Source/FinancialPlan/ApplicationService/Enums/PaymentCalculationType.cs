using System;

namespace Sistran.Core.Application.FinancialPlanServices.Enums
{
    [Flags]
    public enum PaymentCalculationType
    {
        Day = 1,
        Fortnight = 2,
        Month = 3
    }
}
