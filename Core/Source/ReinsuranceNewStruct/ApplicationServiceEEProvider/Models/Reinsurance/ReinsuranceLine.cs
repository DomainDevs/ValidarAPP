using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    /// <summary>
    /// Linea del Reaseguro
    /// </summary>
     [DataContract]
    public class ReinsuranceLine
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
        public Line Line { get; set; }

        /// <summary>
        /// CumulusKey
        /// </summary>
        [DataMember]
        public string CumulusKey { get; set; }

        /// <summary>
        /// ReinsuranceAllocations
        /// </summary>
        [DataMember]
        public List<ReinsuranceAllocation> ReinsuranceAllocations { get; set; }


        /// <summary>
        /// ReinsuranceCumulusRiskCoverages
        /// </summary>
        [DataMember]
		public List<ReinsuranceCumulusRiskCoverage> ReinsuranceCumulusRiskCoverages { get; set; }


	}
}
