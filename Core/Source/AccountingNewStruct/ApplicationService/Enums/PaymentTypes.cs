using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.Enums
{
    [DataContract]
    [Flags]
    public enum PaymentTypes
    {
        [EnumMember]
        Transfer = 1,
    }
}
