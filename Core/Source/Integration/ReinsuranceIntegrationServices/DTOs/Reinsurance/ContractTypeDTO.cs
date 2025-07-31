using System.Runtime.Serialization;

namespace Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance
{
    /// <summary>
    /// Modelo de Tipo de Contrato
    /// </summary>
    [DataContract]
    public class ContractTypeDTO
    {
        /// <summary>
        /// Clave primaria del modelo
        /// </summary>
        [DataMember]
        public int ContractTypeId { get; set; }

        /// <summary>
        /// Descripción del tipo de contrato
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Estado del tipo de contrato
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }

        /// <summary>
        /// Modelo de funcionalidad del contrato
        /// </summary>
        [DataMember]
        public ContractFunctionalityDTO ContractFunctionality { get; set; }
    }
}
