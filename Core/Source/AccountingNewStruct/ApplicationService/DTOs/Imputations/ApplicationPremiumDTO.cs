using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    /// <summary>
    /// Aplicación de prima
    /// </summary>
    [DataContract]
    public class ApplicationPremiumDTO
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Identificador de la aplicación
        /// </summary>
        [DataMember]
        public int ApplicationId { get; set; }

        /// <summary>
        /// Identificador del endoso
        /// </summary>
        [DataMember]
        public int EndorsementId { get; set; }

        /// <summary>
        /// Identificador del pagador
        /// </summary>
        [DataMember]
        public int PayerId { get; set; }
        /// <summary>
        /// Nombre del pagador
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// Número de cuota a pagar
        /// </summary>
        [DataMember]
        public int PaymentNumber { get; set; }

        /// <summary>
        /// Identificador de la moneda
        /// </summary>
        [DataMember]
        public int Currencyid { get; set; }
        /// <summary>
        /// Identificador de la moneda
        /// </summary>
        [DataMember]
        public string NameCurrency { get; set; }

        /// <summary>
        /// Tasa de cambio
        /// </summary>
        [DataMember]
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// Fecha contable
        /// </summary>
        [DataMember]
        public DateTime AccountingDate { get; set; }

        /// <summary>
        /// Fecha de registro
        /// </summary>
        [DataMember]
        public DateTime RegisterDate { get; set; }

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

        /// <summary>
        /// Indica si es pago por comisión
        /// </summary>
        [DataMember]
        public bool IsCommissionPaid { get; set; }

        /// <summary>
        /// Indica si es pago por coaseguro
        /// </summary>
        [DataMember]
        public bool IsCoinsurancePremiumPaid { get; set; }

        /// <summary>
        /// Estado de la cuota
        /// </summary>
        [DataMember]
        public int QuotaStatusId { get; set; }
        

    }
}
