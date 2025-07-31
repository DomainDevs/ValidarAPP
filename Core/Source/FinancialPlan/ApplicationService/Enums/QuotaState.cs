using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.FinancialPlanServices.Enums
{
    [Flags]
    public enum QuotaState
    {
        [EnumMember]
        Pending = 1,
        [EnumMember]
        Complete = 2,
        [EnumMember]
        Partial = 3
    }
}
