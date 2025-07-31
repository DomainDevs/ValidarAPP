using System;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models
{
    public class BillReport
    {
        /// <summary>
        /// Technical transaction
        /// </summary>
        public int TechnicalTransaction { get; set; }

        /// <summary>
        /// Branch name
        /// </summary>
        public string BranchName { get; set; }

        /// <summary>
        /// Document number for current client
        /// </summary>
        public string ClientDocumentNumber { get; set; }

        /// <summary>
        /// Payer name
        /// </summary>
        public string PayerName { get; set; }

        /// <summary>
        /// Payment date
        /// </summary>
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// Payer document number
        /// </summary>
        public string PayerDocumentNumber { get; set; }

        /// <summary>
        /// Total
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Total description in letters
        /// </summary>
        public string TotalInLetters { get; set; }

        /// <summary>
        /// Concept description
        /// </summary>
        public string ConceptDescription { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Currency description
        /// </summary>
        public string CurrencyDescription { get; set; }

        /// <summary>
        /// Payment method
        /// </summary>
        public string PaymentMethod { get; set; }
    }
}
