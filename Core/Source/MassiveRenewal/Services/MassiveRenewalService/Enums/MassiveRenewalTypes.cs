using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.MassiveRenewalServices.Enums
{
    [Flags]
    public enum ProcessRenewalStatus
    {
        [EnumMember]
        Inicialized = 5,
        [EnumMember]
        Temporals = 6,
        [EnumMember]
        Finalized = 7
    }

    [Flags]
    public enum ProcessTemporalStatus
    {
        [EnumMember]
        Query = 1,
        [EnumMember]
        Creation = 2,
        [EnumMember]
        Tariff = 3,
        [EnumMember]
        ExecutionEvents = 4,
        [EnumMember]
        Finalized = 5,
        [EnumMember]
        Issuance = 6,
        [EnumMember]
        Excluded = 7
    }
}