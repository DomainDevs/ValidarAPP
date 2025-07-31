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
    public enum PrefixTypes
    {
        [EnumMember]
        RESPONSABILIDAD_CIVIL = 15,
        [EnumMember]
        CUMPLIMIENTO = 30,
        [EnumMember]
        CAUCION_JUDICIAL = 31,
        [EnumMember]
        ARRENDAMIENTOS = 32
    }
}
