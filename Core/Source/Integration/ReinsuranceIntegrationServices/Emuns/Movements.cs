using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.ReinsuranceIntegrationServices.Enums
{
    [DataContract]
    [Flags]
    public enum Movements
    {
        [EnumMember]
        Original = 1,
        [EnumMember]
        Counterpart = 2,
        [EnumMember]
        Adjustment = 3
    }
}