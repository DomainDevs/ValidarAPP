using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Amortizations
{
    /// <summary>
    /// Amortization:   Amortizacion
    /// </summary>
    [DataContract]
    public class AmortizationDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Date: Fecha de proceso 
        /// </summary>        
        [DataMember]
        public DateTime Date { get; set; }

        /// <summary>
        /// User: Usuario 
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// PositiveAppliedTotal: Total Positivo Aplicado 
        /// </summary>
        [DataMember]
        public AmountDTO  PositiveAppliedTotal  { get; set; }

        /// <summary>
        /// NegativeAppliedTotal: Total Negativo Aplicado 
        /// </summary>
        [DataMember]
        public AmountDTO NegativeAppliedTotal { get; set; }

        /// <summary>
        /// AmortizationStatus: Estado 
        /// </summary>        
        [DataMember]
        public int AmortizationStatus { get; set; }

        /// <summary>
        /// Policies: Lista de polizas con amortizaciones
        /// </summary>        
        [DataMember]
        public List<PolicyDTO> Policies { get; set; }

        
    }
}
