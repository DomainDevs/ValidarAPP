using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    /// <summary>
    /// Aplicación de prima
    /// </summary>
    [DataContract]
    public class TempApplicationPremiumDTO
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
        /// Estado de la cuota
        /// </summary>
		[DataMember]
        public int QuotaStatusId { get; set; }

        /// <summary>
        /// Comisiones
        /// </summary>
        [DataMember]
        public List<TempApplicationPremiumCommissDTO> Commissions { get; set; }

        /// <summary>
        /// impuestos
        /// </summary>
        [DataMember]
        public decimal Tax { get; set; }

        /// <summary>
        /// identificador de usuario
        /// </summary>
        [DataMember]
        public decimal userId { get; set; }
        /// <summary>
        /// NoExpenses: tiene o no gastos
        /// </summary>
        [DataMember]
        public bool NoExpenses { get; set; }
    }
}