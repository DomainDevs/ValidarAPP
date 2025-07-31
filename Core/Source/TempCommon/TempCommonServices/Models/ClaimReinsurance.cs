using System;
using System.Runtime.Serialization;

using Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.TempCommonServices.Models
{
    /// <summary>
    /// ReinsuranceClaim 
    /// </summary>
    [DataContract]
    public class ClaimReinsurance
    {
        /// <summary>
        /// Id 
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        
        /// <summary>
        /// EndorsmentId
        /// </summary>
        [DataMember]
        public int EndorsmentId { get; set; }

        /// <summary>
        /// ClaimNumber
        /// </summary>
        [DataMember]
        public int ClaimNumber { get; set; }

        /// <summary>
        /// ClaimDate
        /// </summary>
        [DataMember]
        public DateTime ClaimDate { get; set; }

        /// <summary>
        /// RiskNumber
        /// </summary>
        [DataMember]
        public int RiskNumber { get; set; }

        /// <summary>
        /// CoverageNumber
        /// </summary>
        [DataMember]
        public int CoverageNumber { get; set; }

        /// <summary>
        /// Currency
        /// </summary>
        [DataMember]
        public Currency Currency { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        [DataMember]
        public Amount Amount { get; set; }
    }
}
