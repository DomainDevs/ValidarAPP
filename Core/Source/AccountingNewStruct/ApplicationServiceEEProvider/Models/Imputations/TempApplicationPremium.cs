using System;
using System.Collections.Generic;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// Aplicación de prima
    /// </summary>
    public class TempApplicationPremium
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador de la aplicación
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// Identificador del endoso
        /// </summary>
        public int EndorsementId { get; set; }

        /// <summary>
        /// Identificador del pagador
        /// </summary>
        public int PayerId { get; set; }

        /// <summary>
        /// Número de cuota a pagar
        /// </summary>
        public int PaymentNumber { get; set; }

        /// <summary>
        /// Identificador de la moneda
        /// </summary>
        public int Currencyid { get; set; }

        /// <summary>
        /// Tasa de cambio
        /// </summary>
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// Fecha contable
        /// </summary>
        public DateTime AccountingDate { get; set; }

        /// <summary>
        /// Fecha de registro
        /// </summary>
        public DateTime RegisterDate { get; set; }

        /// <summary>
        /// Monto
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Monto en moneda local
        /// </summary>
        public decimal LocalAmount { get; set; }

        /// <summary>
        /// Monto adicional
        /// </summary>
        public decimal MainAmount { get; set; }

        /// <summary>
        /// Monto adicional en moneda local
        /// </summary>
        public decimal MainLocalAmount { get; set; }

        /// <summary>
        /// Estado de la cuota
        /// </summary>
        public int QuotaStatusId { get; set; }

        /// <summary>
        /// Discounted commission
        /// </summary>
        public decimal DiscountedCommission { get; set; }

        /// <summary>
        /// Commision list
        /// </summary>
        public List<ApplicationPremiumCommision> Commissions { get; set; }

        /// <summary>
        /// iva
        /// </summary>
        public decimal Tax { get; set; }
    }
}
