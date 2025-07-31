using System.Runtime.Serialization;
using System;
namespace Sistran.Core.Application.OperationQuotaServices.EEProvider.Enums
{
    [DataContract]
    [Flags]
    public enum EnumInicialEvent
    {
        [EnumMember]
        INICIAL_EVENT = 0
    }
}
