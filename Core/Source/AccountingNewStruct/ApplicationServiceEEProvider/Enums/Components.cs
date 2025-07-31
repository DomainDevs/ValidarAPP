using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Enums
{
    [DataContract]
    [Flags]
    public enum Components
    {
        [EnumMember]
        Prime = 1,
        [EnumMember]
        AdministrativeFee = 2,
        [EnumMember]
        FinancialFee = 3,
        [EnumMember]
        IssuanceFee = 5,
        [EnumMember]
        Tax = 10,
        [EnumMember]
        Bonus = 12
    }
}
