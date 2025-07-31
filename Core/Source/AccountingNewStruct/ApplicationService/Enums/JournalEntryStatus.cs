using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.Enums
{
    [DataContract]
    [Flags]
    public enum JournalEntryStatus
    {
        [EnumMember]
        Canceled = 0,
        [EnumMember]
        Applied = 1
    }
}
