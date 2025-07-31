using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    /// <summary>
    /// ReinsurancePaymentClaimLayer
    /// </summary>
    [DataContract]
    public class ReinsurancePaymentClaimLayer
    {
        /// <summary>
        /// Id
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// LayerNumber
        /// </summary>        
        [DataMember]
        public int LayerNumber { get; set; }

        /// <summary>
        /// ReinsurancePaymentClaimCumulus
        /// </summary>        
        [DataMember]
        public List<ReinsurancePaymentClaimCumulus> ReinsurancePaymentClaimCumulus { get; set; }
    }
}
