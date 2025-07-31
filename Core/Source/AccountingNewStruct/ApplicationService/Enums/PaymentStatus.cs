using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.Enums
{
    [DataContract]
    [Flags]
    public enum PaymentStatus
    {
        [EnumMember]
        Canceled = 0,
        [EnumMember]
        Active = 1,
        [EnumMember]
        InternalBallot = 2,
        [EnumMember]
        Deposited = 3,
        [EnumMember]
        Rejected = 4,
        [EnumMember]
        Legalized = 5,
        [EnumMember]
        Regularized = 6,
        [EnumMember]
        Exchanged = 7
    }
}
