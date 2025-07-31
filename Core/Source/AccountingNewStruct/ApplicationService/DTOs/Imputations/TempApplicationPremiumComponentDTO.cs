using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    /// <summary>
    /// Aplicación de prima
    /// </summary>
    [DataContract]
    public class TempApplicationPremiumComponentDTO
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int TempApplicationPremiumCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int ComponentCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string ComponentTinyDescription { get; set; }

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
        public decimal Amount { get; set; }

        /// <summary>
        /// Monto en moneda local
        /// </summary>
        [DataMember]
        public decimal LocalAmount { get; set; }

        /// <summary>
        /// Monto adicional
        /// </summary>
        [DataMember]
        public decimal MainAmount { get; set; }

        /// <summary>
        /// Monto adicional en moneda local
        /// </summary>
        [DataMember]
        public decimal MainLocalAmount { get; set; }

    }
}
