using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Transports.Endorsement.CreditNote.ApplicationServices.DTOs
{
    /// <summary>
    /// Informacion resumen del endoso
    /// </summary>
    [DataContract]
    public class SummaryDTO
    {
        /// <summary>
        /// Monto asegurado
        /// </summary>
        [DataMember]
        public Decimal AmountInsured { get; set; }

        /// <summary>
        /// Prima
        /// </summary>
        [DataMember]
        public Decimal Premium { get; set; }

        /// <summary>
        /// Gastos
        /// </summary>
        [DataMember]
        public Decimal Expenses { get; set; }

        /// <summary>
        /// Recargos
        /// </summary>
        [DataMember]
        public Decimal Surcharges { get; set; }

        /// <summary>
        /// Descuentos
        /// </summary>
        [DataMember]
        public Decimal Discounts { get; set; }

        /// <summary>
        /// Impuestos
        /// </summary>
        [DataMember]
        public Decimal Taxes { get; set; }

        /// <summary>
        /// Prima total
        /// </summary>
        [DataMember]
        public Decimal FullPremium { get; set; }

        /// <summary>
        /// Cantidad de riesgos asociados 
        /// </summary>
        [DataMember]
        public Decimal RiskCount { get; set; }
    }
}
