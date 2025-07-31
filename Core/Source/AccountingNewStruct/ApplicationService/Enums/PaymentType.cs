using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.Enums
{
    [DataContract]
    [Flags]
    public enum PaymentType
    {
        [EnumMember]
        Check = 1,
        [EnumMember]
        Transfer = 4, 
    }
}
