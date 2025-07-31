using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance
{
    /// <summary>
    /// Modelo de Líneas de Contrato
    /// </summary>
    [DataContract]
    public class LineDTO
    {
        /// <summary>
        /// Identificador único de la línea
        /// </summary>
        [DataMember]
        public int LineId { get; set; }

        /// <summary>
        /// Descripción de la línea
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Modelo de Tipo de Cúmulo
        /// </summary>
        [DataMember]
        public CumulusTypeDTO CumulusType { get; set; }

        /// <summary>
        /// Listado de líneas de contrato
        /// </summary>
        [DataMember]
        public List<ContractLineDTO> ContractLines { get; set; }
    }
}
