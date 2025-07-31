using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.Enums
{
    [DataContract]
    [Flags]
    public enum Modules
    {
        [EnumMember]
        Issuance = 1,
        [EnumMember]
        Claim = 2,
        [EnumMember]
        Payment = 3,
    }
}
