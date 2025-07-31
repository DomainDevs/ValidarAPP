using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.Enums
{
    [DataContract]
    [Flags]
    public enum ComponentTypes
    {
        [EnumMember]
        I = 1,
        [EnumMember]
        P = 2,
        [EnumMember]
        G = 3
    }
}
