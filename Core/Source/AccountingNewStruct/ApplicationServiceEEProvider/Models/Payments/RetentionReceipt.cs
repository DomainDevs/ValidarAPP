using System;
using System.Runtime.Serialization;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting;


namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments
{   

    /// <summary>
    ///     Recibo de Retencion
    /// </summary>
    [DataContract]
    public class RetentionReceipt : Payment
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
        public Policy Policy { get; set; }
        /// <summary>
        /// RetentionConcept
        /// </summary>
        [DataMember]
        public Models.Retentions.RetentionConcept RetentionConcept { get; set; }
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
