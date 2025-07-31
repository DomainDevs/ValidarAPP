using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.AccountingServices.Enums
{
    [DataContract]
    [Flags]
    public enum CollectStatus
    {
        [EnumMember]
        Canceled = 0, 
        [EnumMember]
        Active = 1,
        [EnumMember]
        PartiallyApplied = 2,
        [EnumMember]
        Applied = 3,
        [EnumMember]
        Reversed = 4
    }
}
