using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.Enums
{
    [DataContract]
    [Flags]
    public enum CollectControlStatus
    {
        [EnumMember]
        Close = 0,
        [EnumMember]
        Open = 1
    }
}
