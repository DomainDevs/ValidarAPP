using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Services.UtilitiesServices.Enums
{
    [DataContract]
    public enum CustomerType
    {
        [EnumMember]
        Individual = 1,

        [EnumMember]
        Prospect = 2
    }
}