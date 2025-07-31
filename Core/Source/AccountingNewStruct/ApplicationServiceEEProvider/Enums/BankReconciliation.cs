using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Enums
{
    [DataContract]
    [Flags]
    public enum BankReconciliation
    {
        [EnumMember]
        No = 1,
        [EnumMember]
        Yes = 2
    }
}
