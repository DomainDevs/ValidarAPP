using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.UnderwritingOperatingQuotaServices.Enums
{
    [DataContract]
    [Flags]
    public enum EnumEventOperationQuota
    {
        [EnumMember]
        ASSIGN_INDIVIDUAL_OPERATION_QUOTA = 1,

        [EnumMember]
        APPLY_ENDORSEMENT = 2,

        [EnumMember]
        APPLY_MODIFY_ENDORSEMENT = 3,

        [EnumMember]
        APPLY_CANCELLATION_ENDORSEMENT = 4,

        [EnumMember]
        APPLY_EFFECTIVE_EXTENSION_ENDORSEMENT = 5,

        [EnumMember]
        APPLY_RENEWAL_ENDORSMENT = 6,

        [EnumMember]
        APPLI_LAST_CANCELLATION_ENDORSEMENT=7,

        [EnumMember]
        APPLY_CHANGE_TERM_ENDORSEMENT = 8,

        [EnumMember]
        APPLY_AGENT_CHANGE_ENDORSEMENT = 9,

        [EnumMember]
        APPLY_REINSURANCE_ENDORSEMENT = 10,

    }
}
