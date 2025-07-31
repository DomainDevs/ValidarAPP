using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.OperationQuotaServices.EEProvider.Enums
{
    [DataContract]
    [Flags]
    public enum EnumMovement
    {
        [EnumMember]
        CUPO = 1,

        [EnumMember]
        CUMULO = 2,

        [EnumMember]
        TOTAL = 3

    }
}
