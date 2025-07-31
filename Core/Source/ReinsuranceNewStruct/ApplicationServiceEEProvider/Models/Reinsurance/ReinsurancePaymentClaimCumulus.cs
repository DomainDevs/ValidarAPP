
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    /// <summary>
    /// Modelo de Reaseguro de Pago / Siniestro
    /// </summary>
    [DataContract]
    public class ReinsurancePaymentClaimCumulus
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
        public Line Line { get; set; }        

        /// <summary>
        /// CumulusKey 
        /// </summary>
        [DataMember]
        public string CumulusKey { get; set; }

        /// <summary>
        /// ReinsurancePaymentClaimAllocation
        /// </summary>        
        [DataMember]
        public List<ReinsurancePaymentClaimAllocation> ReinsurancePaymentClaimAllocation { get; set; }
    }
}
