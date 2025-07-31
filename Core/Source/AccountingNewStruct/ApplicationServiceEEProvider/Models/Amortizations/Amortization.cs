using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting;
using Sistran.Core.Application.AccountingServices.Enums;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Amortizations
{
    /// <summary>
    /// Amortization:   Amortizacion
    /// </summary>
    [DataContract]
    public class Amortization
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
        public Amount  PositiveAppliedTotal  { get; set; }

        /// <summary>
        /// NegativeAppliedTotal: Total Negativo Aplicado 
        /// </summary>
        [DataMember]
        public Amount NegativeAppliedTotal { get; set; }

        /// <summary>
        /// AmortizationStatus: Estado 
        /// </summary>        
        [DataMember]
        public AmortizationStatus AmortizationStatus { get; set; }

        /// <summary>
        /// Policies: Lista de polizas con amortizaciones
        /// </summary>        
        [DataMember]
        public List<Policy> Policies { get; set; }

        
    }
}
