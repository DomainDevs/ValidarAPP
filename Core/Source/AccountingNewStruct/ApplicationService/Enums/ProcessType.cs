using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.Enums
{
    [DataContract]
    [Flags]
    public enum ProcessType
    {
        [EnumMember]
        GenerationCoupon = 1,
        [EnumMember]
        Prenotification = 2,
        [EnumMember]
        GenerationText = 3,
        [EnumMember]
        GenerationExcel = 4,
        [EnumMember]
        Apply = 5
    }
}
