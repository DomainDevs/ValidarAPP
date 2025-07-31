using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    /// <summary>
    /// Tipo de asociación de lineas: por Ramo Técnico
    /// </summary>
    [DataContract]
    public class ByLineBusinessDTO : LineAssociationTypeDTO
    {
        /// <summary>
        /// Ramo Técnico
        /// </summary>
        [DataMember]
        public List<LineBusinessDTO> LineBusiness { get; set; }
    }
}