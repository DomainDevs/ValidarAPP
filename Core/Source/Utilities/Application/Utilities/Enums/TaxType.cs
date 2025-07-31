using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.Utilities.Enums
{
    [DataContract]
    [Flags]
    public enum TaxType
    {
        /// <summary>
        /// Constante porcentaje
        /// </summary>
        [EnumMember]
        Percentage = 1,

        /// <summary>
        /// Constante pormilaje
        /// </summary>        
        [EnumMember]
        Permilage = 2,

        /// <summary>
        /// Constante valor fijo
        /// </summary>        
        [EnumMember]
        FixedValue = 3
    }
}
