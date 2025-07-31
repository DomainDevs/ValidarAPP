using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UnderwritingServices.Enums
{
    [DataContract]
    public enum ClaimsUnderwritingKeys
    {
        [EnumMember]
        CLM_INSURED_PERSON_TYPE,

        [EnumMember]
        CLM_SURETY_PERSON_TYPE,

        [EnumMember]
        CLM_HOLDER_PERSON_TYPE
    }
}
