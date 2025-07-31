using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    /// <summary>
    /// Tipo de asociación de lineas por Ramo -> Subramo -> Riesgo
    /// </summary>
    [DataContract]
    public class ByLineBusinessSubLineBusinessInsuredObjectDTO : LineAssociationTypeDTO
    {
        /// <summary>
        /// Ramo Tecnico
        /// </summary>
        [DataMember]
        public LineBusinessDTO LineBusiness { get; set; }

        /// <summary>
        /// SubRamo Tecnico
        /// </summary>
        [DataMember]
        public SubLineBusinessDTO SubLineBusiness { get; set; }

        /// <summary>
        /// Objeto de Seguro
        /// </summary>
        [DataMember]
        public List<InsuredObjectDTO> InsuredObject { get; set; }
    }
}

