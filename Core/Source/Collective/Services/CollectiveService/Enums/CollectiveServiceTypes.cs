using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.CollectiveServices.Enums
{
    [Flags]
    public enum CollectiveLoadStatus
    {
        [EnumMember]
        Validating = 1,
        [EnumMember]
        Validated = 2,
        [EnumMember]
        Tariffing = 3,
        [EnumMember]
        Tariffed = 4,
        [EnumMember]
        Issuing = 5,
        [EnumMember]
        Issued = 6
    }

    [Flags]
    public enum CollectiveLoadProcessStatus
    {
        [EnumMember]
        Validation = 1,
        [EnumMember]
        Tariff = 2,
        [EnumMember]
        Events = 3,
        [EnumMember]
        Issuance = 4,
        [EnumMember]
        Finalized = 5
    }
}