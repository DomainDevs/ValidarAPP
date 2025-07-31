using System;

namespace Sistran.Core.Application.FinancialPlanServices.Enums
{
    [Flags]
    public enum ComponentType
    {
        Premium = 1,
        Expenses = 2,
        Surcharges = 3,
        Discounts = 4,
        Taxes = 5
    }
}
