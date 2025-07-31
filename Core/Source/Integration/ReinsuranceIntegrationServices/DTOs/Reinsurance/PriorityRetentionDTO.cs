using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance
{
    /// <summary>
    /// Modelo de retencion prioritaria
    /// </summary>
    [DataContract]
    public class PriorityRetentionDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// ValidityFrom
        /// </summary>
        [DataMember]
        public DateTime ValidityFrom { get; set; }

        /// <summary>
        /// ValidityTo
        /// </summary>
        [DataMember]
        public DateTime ValidityTo { get; set; }

        /// <summary>
        /// PriorityRetentionAmount
        /// </summary>
        [DataMember]
        public decimal PriorityRetentionAmount { get; set; }

        /// <summary>
        /// LineBusiness
        /// </summary>
        [DataMember]
        public PrefixDTO Prefix { get; set; }
        /// <summary>
        /// Enable
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }

    }
}
