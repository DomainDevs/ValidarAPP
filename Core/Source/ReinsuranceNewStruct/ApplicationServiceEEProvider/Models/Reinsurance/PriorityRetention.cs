
using System;
using System.Runtime.Serialization;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Integration.CommonServices.DTOs;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    /// <summary>
    /// Modelo para Retenciones prioritarias
    /// </summary>
    [DataContract]
    public class PriorityRetention
    {
        /// <summary>
        /// Cantidad, Moneda
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Valido desde
        /// </summary>
        [DataMember]
        public DateTime ValidityFrom { get; set; }
        /// <summary>
        /// Valido hasta
        /// </summary>
        [DataMember]
        public DateTime ValidityTo { get; set; }
        /// <summary>
        /// Monto, Moneda
        /// </summary>
        [DataMember]
        public decimal PriorityRetentionAmount { get; set; }
        /// <summary>
        /// Ramo
        /// </summary>
        [DataMember]
        public Prefix Prefix { get; set; }
        /// <summary>
        /// Enabled
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }
    }
}
