
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.UndewritingIntegrationServices.DTOs
{
    /// <summary>
    /// Plan Financiero
    /// </summary>
    [DataContract]
    public class FinancialPlanDTO
    {
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public DateTime ExpirationDate { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public decimal AmountPending { get; set; }
        [DataMember]
        public decimal Premium { get; set; }
        [DataMember]
        public decimal Tax { get; set; }
        [DataMember]
        public decimal Expenses { get; set; }
        [DataMember]
        public decimal ExpensesOther { get; set; }
        [DataMember]
        public decimal Financial { get; set; }
        [DataMember]
        public decimal TotalAmount { get; set; }

        [DataMember]
        public decimal Percentage { get; set; }

        [DataMember]
        public short StateQuota { get; set; }

        /// <summary>
        /// Gets or sets the payer payment identifier.
        /// </summary>
        /// <value>
        /// The payer payment identifier.
        /// </value>
        [DataMember]
        public int PayerPaymentId { get; set; }

    }
}