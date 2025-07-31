using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.Enums
{
    [DataContract]
    [Flags]
    public enum PaymentTicketStatus
    {
        [EnumMember]
        Canceled = 0,
        [EnumMember]
        Active = 1,
        [EnumMember]
        Paid = 2,
    }
}
