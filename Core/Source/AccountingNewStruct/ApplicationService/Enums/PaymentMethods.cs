using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.Enums
{
    /// <summary>
    ///     Metodos de Pago
    /// </summary>
    [DataContract]
    [Flags]
    public enum PaymentMethods
    {
        [EnumMember]
        Default = 0,
        [EnumMember]
        PostdatedCheck = 1,
        [EnumMember]
        CurrentCheck = 2,
        [EnumMember]
        Cash = 3,
        [EnumMember]
        CreditableCreditCard = 4,
        [EnumMember]
        UncreditableCreditCard = 5,
        [EnumMember]
        DepositVoucher = 6,
        [EnumMember]
        DirectConection = 7,
        [EnumMember]
        Transfer = 8,
        [EnumMember]
        RetentionReceipt = 10,
        [EnumMember]
        ElectronicPayment = 11,
        [EnumMember]
        PaymentArea = 12,
        [EnumMember]
        PaymentCard = 13,
        [EnumMember]
        ConsignmentByCheck = 25
    };
}
