using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    [DataContract]
    public class AccountingListParametersDTO
    {
        /// <summary>
        /// PaymentMethodTypeCode
        /// </summary>
        [DataMember]
        public int PaymentMethodTypeCode { get; set; }

        /// <summary>
        /// CurrencyCode
        /// </summary>
        [DataMember]
        public int CurrencyCode { get; set; }
        
        /// <summary>
        /// Amount
        /// </summary>
        [DataMember]
        public decimal Amount { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        [DataMember]
        public decimal LocalAmount { get; set; }

        /// <summary>
        /// ExchangeRate
        /// </summary>
        [DataMember]
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// PayerId
        /// </summary>
        [DataMember]
        public int PayerId { get; set; }

        /// <summary>
        /// PaymentCode
        /// </summary>
        [DataMember]
        public int PaymentCode { get; set; }

        /// <summary>
        /// BranchCode
        /// </summary>
        [DataMember]
        public int BranchCode { get; set; }

        /// <summary>
        /// AccountingDate
        /// </summary>
        [DataMember]
        public DateTime AccountingDate { get; set; }

        /// <summary>
        /// Identificador de la cuenta contable
        /// </summary>
        [DataMember]
        public int AccountingAccountId { get; set; }

        /// <summary>
        /// Naturaleza del movimiento
        /// </summary>
        [DataMember]
        public int AccountingNature { get; set; }

        /// <summary>
        /// Número de recibo
        /// </summary>
        [DataMember]
        public int? RecieptNumber { get; set; }

        /// <summary>
        /// Fecha de recibo
        /// </summary>
        [DataMember]
        public DateTime? RecieptDate { get; set; }

        /// <summary>
        /// Identificador del banco de conciliación
        /// </summary>
        [DataMember]
        public int? BankReconciliationId { get; set; }
    }
}
