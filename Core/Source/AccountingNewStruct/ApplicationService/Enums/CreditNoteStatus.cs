using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.Enums
{
    [DataContract]
    [Flags]
    public enum CreditNoteStatus
    { 
        [EnumMember]
        Actived = 1,
        [EnumMember]
        Applied = 2,
        [EnumMember]
        Rejected = 3,
        [EnumMember]
        NoData = 4
    }
}
