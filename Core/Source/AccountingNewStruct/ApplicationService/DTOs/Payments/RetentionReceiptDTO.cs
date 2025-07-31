using Sistran.Core.Application.AccountingServices.DTOs.Retentions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Payments
{
    /// <summary>
    ///     Recibo de Retencion
    /// </summary>
    [DataContract]
    public class RetentionReceiptDTO : PaymentDTO
    {
        /// <summary>
        ///     Numero de Serie de la fatura afectada
        /// </summary>
        [DataMember]
        public string SerialNumber { get; set; }

        /// <summary>
        ///     Numero de factura
        /// </summary>
        [DataMember]
        public string BillNumber { get; set; }

        /// <summary>
        ///     Número de Autorización
        /// </summary>
        [DataMember]
        public string AuthorizationNumber { get; set; }

        /// <summary>
        ///     Número de comprobante
        /// </summary>
        [DataMember]
        public string VoucherNumber { get; set; }

        /// <summary>
        ///     Número de serie del comprobante
        /// </summary>
        [DataMember]
        public string SerialVoucherNumber { get; set; }

        /// <summary>
        ///     Fecha
        /// </summary>
        [DataMember]
        public DateTime Date { get; set; }
        /// <summary>
        /// Policy
        /// </summary>
        [DataMember]
        public PolicyDTO Policy { get; set; }
        /// <summary>
        /// RetentionConcept
        /// </summary>
        [DataMember]
        public RetentionConceptDTO RetentionConcept { get; set; }
        /// <summary>
        /// InvoiceDate
        /// </summary>
        [DataMember]
        public DateTime InvoiceDate { get; set; }
        /// <summary>
        /// IssueDate
        /// </summary>
        [DataMember]
        public DateTime IssueDate { get; set; }
        /// <summary>
        /// ExpirationDate
        /// </summary>
        [DataMember]
        public DateTime ExpirationDate { get; set; }
    }
}
