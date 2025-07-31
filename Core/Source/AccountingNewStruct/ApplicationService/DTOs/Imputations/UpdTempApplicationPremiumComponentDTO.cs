using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    /// <summary>
    /// Aplicación de prima
    /// </summary>
    [DataContract]
    public class UpdTempApplicationPremiumComponentDTO
    {
        

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int TempApplicationPremiumCode { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int ComponentCurrencyCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// Monto
        /// </summary>
        [DataMember]
        public decimal PremiumAmount { get; set; }

        /// <summary>
        /// Monto
        /// </summary>
        [DataMember]
        public decimal ExpensesLocalAmount { get; set; }

        /// <summary>
        /// Impuestos 
        /// </summary>
        [DataMember]
        public decimal TaxLocalAmount { get; set; }
    }
}
