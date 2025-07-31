using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.Enums
{

    [DataContract]
    [Flags]
    public enum CoInsuranceTypes
    {
        [EnumMember]
        Accepted = 2,
        [EnumMember]
        Given = 3,
    }
}
