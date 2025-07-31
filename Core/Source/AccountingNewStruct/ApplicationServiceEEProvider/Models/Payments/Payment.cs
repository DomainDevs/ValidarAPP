using System.Runtime.Serialization;
using System.Collections.Generic;
using Sistran.Core.Application.CommonService.Models;
using System;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments
{


    /// <summary>
    ///     Pago
    /// </summary>
    [DataContract]
    public class Payment
    {

        /// <summary>
        ///     Identificador
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Método de pago
        /// </summary>
        public PaymentMethod PaymentMethod { get; set; }

        /// <summary>
        ///     Importe de pago
        /// </summary>
        public Amount Amount { get; set; }

        /// <summary>
        /// LocalAmount 
        /// </summary>        
        public Amount LocalAmount { get; set; }

        /// <summary>
        /// ExchangeRate
        /// </summary>
        public ExchangeRate ExchangeRate { get; set; }

        /// <summary>
        ///     Estado del Pago
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        ///     Total Impuestos
        /// </summary>
        public decimal Tax { get; set; }

        /// <summary>
        ///     Total Retenciones
        /// </summary>
        public decimal Retention { get; set; }

        /// <summary>
        ///     Impuestos a aplicar
        /// </summary>
        public List<PaymentTax> Taxes { get; set; }

        /// <summary>
        /// Numero del pago
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// IssuingBankCode 
        /// </summary>
        public int IssuingBankCode { get; set; }

        /// <summary>
        /// DatePayment 
        /// </summary>
        public DateTime? DatePayment { get; set; }

        /// <summary>
        /// Holder 
        /// </summary>
        public string Holder { get; set; }

        /// <summary>
        /// IssuingAccountNumber 
        /// </summary>
        public string IssuingAccountNumber { get; set; }


        /// <summary>
        /// Retentions 
        /// </summary>
        public decimal Retentions { get; set; }

        /// <summary>
        /// collect id 
        /// </summary>
        public int CollectId { get; set; }

        /// <summary>
        /// Voucher identifier
        /// </summary>
        public string Voucher { get; set; }
    }
}
