using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Enums
{
    [DataContract]
    [Flags]
    public enum CumulusDetailKeys
    {
        [EnumMember]
        REINS_CUMULUS_DETAIL_INDIVIDUAL,
        [EnumMember]
        REINS_CUMULUS_DETAIL_CONSORTIUM,
        [EnumMember]
        REINS_CUMULUS_DETAIL_ECONOMIC_GROUP,
       
    }
}
