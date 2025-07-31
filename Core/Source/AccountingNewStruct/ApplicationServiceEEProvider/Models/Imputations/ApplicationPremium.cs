using System;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// Aplicación de prima
    /// </summary>
    public class ApplicationPremium : TransactionType
    {
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
        /// Nombre del pagador
        /// </summary>
        
        public string Name { get; set; }

        /// <summary>
        /// Número de cuota a pagar
        /// </summary>
        public int PaymentNumber { get; set; }

        /// <summary>
        /// Identificador de la moneda
        /// </summary>
        public int Currencyid { get; set; }
        /// <summary>
        /// Identificador de la moneda
        /// </summary>
        public string NameCurrency { get; set; }

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
        /// Indica si es pago por comisión
        /// </summary>
        public bool IsCommissionPaid { get; set; }

        /// <summary>
        /// Indica si es pago por coaseguro
        /// </summary>
        public bool IsCoinsurancePremiumPaid { get; set; }

        /// <summary>
        /// Estado de la cuota
        /// </summary>
        public int QuotaStatusId { get; set; }

        /// <summary>
        /// Comision descontada
        /// </summary>
        public decimal DiscountCommission { get; set; }
    }
}
