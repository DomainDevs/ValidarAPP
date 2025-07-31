using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Services.UtilitiesServices.Enums
{
    [DataContract]
    public enum IndividualType
    {
        [EnumMember]
        Person = 1,

        [EnumMember]
        Company = 2,

        [EnumMember]
        ProspectNatural = 3,

        [EnumMember]
        ProspectLegal = 4
    }
}