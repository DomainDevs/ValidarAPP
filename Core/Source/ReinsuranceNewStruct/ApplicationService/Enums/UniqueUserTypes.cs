using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.Enums
{
    [Flags]
    public enum UniqueUserTypes
    {
        [EnumMember]
        Integrated = 1,
        [EnumMember]
        Standard = 2
    }
}
