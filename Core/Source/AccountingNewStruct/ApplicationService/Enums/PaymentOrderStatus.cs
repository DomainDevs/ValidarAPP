using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.Enums
{
    /// <summary>
    /// Estados de órdenes de pago
    /// </summary>
    [DataContract]
    [Flags]
    public enum PaymentOrderStatus //enum para estados de ordenes de pago
    {
        [EnumMember]
        Canceled  =  0,
        [EnumMember]
        Active = 1,
        [EnumMember]
        Authorized = 2,
        [EnumMember]
        Applied = 3,
        [EnumMember]
        Paid = 4,
        [EnumMember]
        Rejected = 5,
        [EnumMember]
        Forwarded = 6
    }
}
