using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance
{
    /// <summary>
    /// Capa del Reaseguro
    /// </summary>
    [DataContract]
    public class ReinsuranceLayerDTO
    {
        /// <summary>
        /// ReinsuranceLayerId
        /// </summary>
        [DataMember]
        public int ReinsuranceLayerId { get; set; }

        /// <summary>
        /// LayerNumber 
        /// </summary>
        [DataMember]
        public int LayerNumber { get; set; }

        /// <summary>
		/// LayerPercentage
		/// </summary>
		[DataMember]
        public decimal LayerPercentage { get; set; }

        /// <summary>
        /// PremiumPercentage
        /// </summary>
        [DataMember]
        public decimal PremiumPercentage { get; set; }


        /// <summary>
        /// ReinsuranceLines
        /// </summary>
        [DataMember]
        public List<ReinsuranceLineDTO> ReinsuranceLines { get; set; }

        /// <summary>
        /// SumPercentage
        /// </summary>
        [DataMember]

        public decimal SumPercentage { get; set; }

        [DataMember]
        public int ReinsSourceId { get; set; }

        [DataMember]
        public int TemporaryIssueId { get; set; }

        [DataMember]
        public int TempReinsuranceProcessId { get; set; }
    }
}
