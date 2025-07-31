using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    /// <summary>
    /// Tipo de asociación de lineas por Ramo -> Riesgo
    /// </summary>
    [DataContract]
    public class ByLineBusinessInsuredObjectDTO : LineAssociationTypeDTO
    {
        /// <summary>
        /// Ramo Tecnico
        /// </summary>
        [DataMember]
        public LineBusinessDTO LineBusiness { get; set; }

        /// <summary>
        /// Objeto de Seguro
        /// </summary>
        [DataMember]
        public List<InsuredObjectDTO> InsuredObject { get; set; }
    }
}