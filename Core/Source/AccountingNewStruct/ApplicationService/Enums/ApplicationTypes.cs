using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.Enums
{
    [DataContract]
    [Flags]
    public enum ApplicationTypes
    {
        [EnumMember]
        Collect = 2,
        [EnumMember]
        JournalEntry = 8,
        [EnumMember]
        PreLiquidation = 10,
        [EnumMember]
        PaymentOrder = 1
    }
}
