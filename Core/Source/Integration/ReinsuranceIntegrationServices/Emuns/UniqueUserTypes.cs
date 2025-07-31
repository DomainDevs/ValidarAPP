using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.ReinsuranceIntegrationServices.Enums
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
