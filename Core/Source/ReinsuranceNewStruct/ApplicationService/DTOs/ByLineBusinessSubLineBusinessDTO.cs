using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    /// <summary>
    /// Tipo de asociación de lineas por Ramo Tecnico -> Subramo Tecnico
    /// </summary>
    [DataContract]
    public class ByLineBusinessSubLineBusinessDTO : LineAssociationTypeDTO
    {
        /// <summary>
        /// Ramo Tecnico
        /// </summary>
        [DataMember]
        public LineBusinessDTO LineBusiness { get; set; }

      
        /// <summary>
        /// SubRamos Tecmicos
        /// </summary>
        [DataMember]
        public List<SubLineBusinessDTO> SubLineBusiness { get; set; }
    }
}