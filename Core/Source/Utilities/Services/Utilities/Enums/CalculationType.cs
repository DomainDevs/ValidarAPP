using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Services.UtilitiesServices.Enums
{

    [Flags]
    public enum CalculationType
    {
        /// <summary>
        /// Constante Prorrata
        /// </summary>
        [EnumMember]
        Prorate = 1,

        /// <summary>
        /// Constante Directo
        /// </summary>
        [EnumMember]
        Direct = 2,

        /// <summary>
        /// Constante Corto Plazo
        /// </summary>
        [EnumMember]
        ShortTerm = 3
    }
}
