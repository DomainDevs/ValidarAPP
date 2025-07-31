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
    public enum EnumEventEconomicGroup
    {
        [EnumMember]
        CREATE_ECONOMIC_GROUP = 1,

        [EnumMember]
        ENABLED_INDIVIDUAL_TO_ECONOMIC_GROUP = 2,

        [EnumMember]
        DISABLED_INDIVIDUAL_TO_ECONOMIC_GROUP = 3
    }
}
