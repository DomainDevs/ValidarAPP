using System;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// Aplicación de prima
    /// </summary>
    public class TempApplicationPremiumComponent
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TempApplicationPremiumCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ComponentCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ComponentTinyDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ComponentCurrencyCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal ExchangeRate { get; set; }

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

    }
}
