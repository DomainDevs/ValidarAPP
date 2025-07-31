using System.Runtime.Serialization;

namespace Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance
{
    /// <summary>
    /// ResettlementType
    /// </summary>
    [DataContract]
    public class ResettlementTypeDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripción del tipo de contrato
        /// </summary>
        [DataMember]
        public string Description { get; set; }

    }
}
