using Sistran.Core.Application.CommonService.Models;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    /// <summary>
    /// ReinsurancePaymentClaimAllocation
    /// </summary>
    [DataContract]
    public class ReinsurancePaymentClaimAllocation
    {
        /// <summary>
        /// Id
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// EstimationType
        /// </summary>
        [DataMember]
        public EstimationType EstimationType { get; set; }

        /// <summary>
        /// Currency 
        /// </summary>
        [DataMember]
        public Currency Currency { get; set; }

        /// <summary>
        /// Facultative 
        /// </summary>
        [DataMember]
        public bool Facultative { get; set; }

        /// <summary>
        /// LevelCompanyId
        /// </summary>
        [DataMember]
        public int LevelCompanyId { get; set; }

        /// <summary>
        /// ReinsuranceSourceId
        /// </summary>
        [DataMember]
        public int ReinsuranceSourceId { get; set; }


        /// <summary>
        /// Amount 
        /// </summary>
        [DataMember]
        public Amount Amount { get; set; }
    }
}
