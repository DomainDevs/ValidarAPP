using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.Enums
{
    [DataContract]
    [Flags]
    public enum AccountingNature
    {
        [EnumMember]
        [Description("Credit")]
        Credit = 1,
        [EnumMember]
        [Description("Debit")]
        Debit = 2
    }
}
