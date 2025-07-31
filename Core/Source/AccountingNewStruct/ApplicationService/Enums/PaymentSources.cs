using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.Enums
{
    [DataContract]
    [Flags]
    public enum PaymentSources
    {
        [EnumMember]
        ClaimsPaymentRequest = 1,
        [EnumMember]
        Salvage = 2,
        [EnumMember]
        Recovery = 3,
        [EnumMember]
        PaymentRequest = 4
    }
}
