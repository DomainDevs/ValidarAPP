using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.UnderwritingOperatingQuotaServices.Enums
{
    [DataContract]
    [Flags]
    public enum EnumEventConsortium
    {
        [EnumMember]
        CREATE_CONSORTIUM = 1,

        [EnumMember]
        ASSIGN_INDIVIDUAL_TO_CONSORTIUM = 2,

        [EnumMember]
        MODIFY_INDIVIDUAL_TO_CONSORTIUM= 3,

        [EnumMember]
        DISABLED_INDIVIDUAL_TO_CONSORTIUM = 4
    }
}
