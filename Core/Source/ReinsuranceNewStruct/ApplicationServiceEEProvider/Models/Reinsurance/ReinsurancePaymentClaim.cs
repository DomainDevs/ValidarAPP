using Sistran.Core.Application.ReinsuranceServices.EEProvider.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    /// <summary>
    /// Modelo de Reaseguro de Pago / Siniestro
    /// </summary>
    [DataContract]
    public class ReinsurancePaymentClaim
    {
        /// <summary>
        /// Id
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// ReinsuranceNumber
        /// </summary>        
        [DataMember]
        public int ReinsuranceNumber { get; set; }

        /// <summary>
        /// Movements
        /// </summary>        
        [DataMember]
        public Movements Movement { get; set; }

        /// <summary>
        /// ProcessDate
        /// </summary>        
        [DataMember]
        public DateTime ProcessDate { get; set; }

        /// <summary>
        /// ReinsuranceDate
        /// </summary>        
        [DataMember]
        public DateTime ReinsuranceDate { get; set; }

        /// <summary>
        /// IsAutomatic
        /// </summary>        
        [DataMember]
        public bool IsAutomatic { get; set; }

        /// <summary>
        /// UserId 
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// ReinsurancePaymentClaimLayers
        /// </summary>        
        [DataMember]
        public List<ReinsurancePaymentClaimLayer> ReinsurancePaymentClaimLayers { get; set; }

        [DataMember]
        public int ProcessId { get; set; }

        [DataMember]
        public bool ProcessReinsured { get; set; }
    }
}
