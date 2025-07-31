using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.Enums
{
    [DataContract]
    [Flags]
    public enum PaymentRequestTypes 
    {
        [EnumMember]
        Payment = 1,
        [EnumMember]
        Recovery = 2,
        [EnumMember]
        Void = 3
    };
}
