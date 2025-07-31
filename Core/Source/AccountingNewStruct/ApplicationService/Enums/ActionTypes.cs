using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.Enums
{
    [DataContract]
    [Flags]
    public enum ActionTypes
    {
        [EnumMember]
        CreatePayment = 1,
        [EnumMember]
        ExchangePayment = 2,
        [EnumMember]
        PaymentInternalBallot =3,
        [EnumMember]
        PaymentBallotDeposit = 4,
        [EnumMember]
        RejectionPayment = 5,
        [EnumMember]
        PayRegularized = 6,
        [EnumMember]
        PayLegalized = 7 ,
        [EnumMember]
        ReversePaymentInternalBallot = 8
    }
}
