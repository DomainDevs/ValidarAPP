using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.Enums
{
    [DataContract]
    [Flags]
    public enum ImputationItemTypes
    {
        [EnumMember]
        PremiumsReceivable = 1,

        [EnumMember]
        DepositPremiums = 2,

        [EnumMember]
        CommissionRetained = 3,

        [EnumMember]
        CheckingAccountBrokers = 4,

        [EnumMember]
        CheckingAccountCoinsurances = 5,

        [EnumMember]
        CheckingAccountReinsurances = 6,

        [EnumMember]
        Accounting = 7,

        [EnumMember]
        PaymentSuppliers = 8,

        [EnumMember]
        PaymentClaims = 9,

        [EnumMember]
        InsuredLoans = 10
    }

   
}
