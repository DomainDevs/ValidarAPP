using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.Enums
{
    [DataContract]
    [Flags]
    public enum CoveredRiskType
    {
        [EnumMember]
        Vehicle = 1,
        [EnumMember]
        Location = 2,
        [EnumMember]
        Surety = 7,
        [EnumMember]
        Transport = 8,
        [EnumMember]
        Aircraft = 9,
        [EnumMember]
        Aeronavigation = 100
    }
}
