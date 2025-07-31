using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.MassiveUnderwritingServices.Enums
{  
    [Flags]
    public enum MassiveLoadProcessStatus
    {
        [EnumMember]
        Validation = 1,
        [EnumMember]
        Validated = 2,
        [EnumMember]
        Tariff = 3,
        [EnumMember]
        Events = 4,
        [EnumMember]
        Issuance = 5,
        [EnumMember]
        Finalized = 6
    }
}