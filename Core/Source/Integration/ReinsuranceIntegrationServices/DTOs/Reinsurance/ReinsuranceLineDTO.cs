using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance
{
    /// <summary>
    /// Linea del Reaseguro
    /// </summary>
    [DataContract]
    public class ReinsuranceLineDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int ReinsuranceLineId { get; set; }

        /// <summary>
        /// Line
        /// </summary>
        [DataMember]
        public LineDTO Line { get; set; }

        /// <summary>
        /// CumulusKey
        /// </summary>
        [DataMember]
        public string CumulusKey { get; set; }

        /// <summary>
        /// ReinsuranceAllocations
        /// </summary>
        [DataMember]
        public List<ReinsuranceAllocationDTO> ReinsuranceAllocations { get; set; }

        /// <summary>
        /// ReinsuranceCumulusRiskCoverages
        /// </summary>
        [DataMember]
        public List<ReinsuranceCumulusRiskCoverageDTO> ReinsuranceCumulusRiskCoverages { get; set; }
    }
}
