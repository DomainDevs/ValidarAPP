using System.Runtime.Serialization;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.PaymentRequest
{
    [DataContract]
    public class AccountingTax
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the Concepto Contable
        /// </summary>
        /// <value>
        /// Concepto Contable
        /// </value>
        [DataMember]
        public int AccountingConceptId { get; set; }

        [DataMember]
        public int AccountingAccountId { get; set; }

        [DataMember]
        public string AccountingAccountNumber { get; set; }

        [DataMember]
        public int AccountingNatureId { get; set; }
    }
}
