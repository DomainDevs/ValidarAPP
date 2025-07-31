using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Services.UtilitiesServices.Enums
{
    [Flags]
    public enum AsynchronousProcessType
    {
        [EnumMember]
        Collective = 1,
        [EnumMember]
        Massive = 2,
        [EnumMember]
        Renewal = 3
    }

    [Flags]
    public enum AsynchronousProcessStatus
    {
        [EnumMember]
        CollectiveInitial = 1,
        [EnumMember]
        CollectiveFinished = 2,
        [EnumMember]
        MassiveInitial = 3,
        [EnumMember]
        MassiveFinished = 4
    }

}
