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
    public enum LineBusinessKeys
    {
        [EnumMember]
        RESPONSABILIDAD_CIVIL = 6,
        [EnumMember]
        CUMPLIMIENTO = 5,
        [EnumMember]
        CAUCION_JUDICIAL = 2,
        [EnumMember]
        ARRENDAMIENTOS = 32
    }
}
