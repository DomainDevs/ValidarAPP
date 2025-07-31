using System.Runtime.Serialization;

namespace Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance
{
    /// <summary>
    /// Modelo de Líneas de contrato
    /// </summary>
    [DataContract]
    public class ContractLineDTO
    {
        /// <summary>
        /// Identificador único del modelo
        /// </summary>
        [DataMember]
        public int ContractLineId { get; set; }

        /// <summary>
        /// Modelo de Contrato
        /// </summary>
        [DataMember]
        public ContractDTO Contract { get; set; }

        /// <summary>
        /// Prioridad
        /// </summary>
        [DataMember]
        public int Priority { get; set; }
    }
}
