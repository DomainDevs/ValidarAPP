using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    /// <summary>
    /// Tipo de asociación de lineas por Tipo de Operacion -->  Ramo 
    /// </summary>
    [DataContract]
    public class ByOperationTypePrefixDTO : LineAssociationTypeDTO
    {
        
        /// <summary>
        /// Tipos de Operacion
        /// </summary>
        [DataMember]        
        public int BusinessType { get; set; } 

        /// <summary>
        /// Ramos
        /// </summary>
        [DataMember]
        public List<PrefixDTO> Prefixes { get; set; } 
    }
}