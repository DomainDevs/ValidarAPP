using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Services.UtilitiesServices.Enums
{
    /// <summary>
    /// Tipo de Busqueda Asegurado
    /// </summary>
    [DataContract]
    public enum InsuredSearchType
    {
        /// <summary>
        /// Numero documento
        /// </summary>
        [EnumMember]
        DocumentNumber = 1,

        /// <summary>
        /// Individuo
        /// </summary>
        [EnumMember]
        IndividualId = 2,

        /// <summary>
        /// Codigo individio
        /// </summary>
        [EnumMember]
        CodeIndividual = 3
    }
}