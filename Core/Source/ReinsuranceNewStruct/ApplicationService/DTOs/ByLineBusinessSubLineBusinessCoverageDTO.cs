using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    /// <summary>
    /// Tipo de asociación de lineas por Ramo Tecnico -> Subramo Tecnico -> Cobertura
    /// </summary>
    [DataContract]
    public class ByLineBusinessSubLineBusinessCoverageDTO : LineAssociationTypeDTO
    {
        /// <summary>
        /// Ramo
        /// </summary>
        [DataMember]
        public LineBusinessDTO LineBusiness { get; set; }

        /// <summary>
        /// SubRamo
        /// </summary>
        [DataMember]
        public SubLineBusinessDTO SubLineBusiness { get; set; }

        /// <summary>
        /// Cobertura
        /// </summary>
        [DataMember]
        public List<CoverageDTO> Coverage { get; set; }
    }
}