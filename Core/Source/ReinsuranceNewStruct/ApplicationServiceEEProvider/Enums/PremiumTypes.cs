using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Enums
{
    [DataContract]
    [Flags]
    public enum PremiumTypes
    {
        [EnumMember]
        MinimumPremium = 1, 
         [EnumMember]
        DepositPremium = 2,
        [EnumMember]
        MinimumAndDepositPremium = 3,

    }
}
