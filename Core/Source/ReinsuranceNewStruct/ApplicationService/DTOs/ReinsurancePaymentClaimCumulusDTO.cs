using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class ReinsurancePaymentClaimCumulusDTO
    {
        /// <summary>
        /// Id
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

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
        /// ReinsurancePaymentClaimAllocation
        /// </summary>        
        [DataMember]
        public List<ReinsurancePaymentClaimAllocationDTO> ReinsurancePaymentClaimAllocation { get; set; }
    }
}
