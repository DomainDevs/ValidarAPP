using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.TaxServices.Enums
{
    public enum RateType
    {
        [EnumMember]
        Percentage = 1,

        [EnumMember]
        Milleage = 2,

        [EnumMember]
        Amount = 3
    }
}
