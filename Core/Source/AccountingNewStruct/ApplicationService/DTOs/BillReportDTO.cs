using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    [DataContract]
    public class BillReportDTO
    {
        /// <summary>
        /// Technical transaction
        /// </summary>
        [DataMember]
        public int TechnicalTransaction { get; set; }

        /// <summary>
        /// Branch name
        /// </summary>
        [DataMember]
        public string BranchName { get; set; }

        /// <summary>
        /// Document number for current client
        /// </summary>
        [DataMember]
        public string ClientDocumentNumber { get; set; }

        /// <summary>
        /// Payer name
        /// </summary>
        [DataMember]
        public string PayerName { get; set; }

        /// <summary>
        /// Payment date
        /// </summary>
        [DataMember]
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// Payer document number
        /// </summary>
        [DataMember]
        public string PayerDocumentNumber { get; set; }

        /// <summary>
        /// Total
        /// </summary>
        [DataMember]
        public decimal Total { get; set; }

        /// <summary>
        /// Total description in letters
        /// </summary>
        [DataMember]
        public string TotalInLetters { get; set; }

        /// <summary>
        /// Concept description
        /// </summary>
        [DataMember]
        public string ConceptDescription { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Currency description
        /// </summary>
        [DataMember]
        public string CurrencyDescription { get; set; }

        /// <summary>
        /// Payment method
        /// </summary>
        [DataMember]
        public string PaymentMethod { get; set; }
    }
}
