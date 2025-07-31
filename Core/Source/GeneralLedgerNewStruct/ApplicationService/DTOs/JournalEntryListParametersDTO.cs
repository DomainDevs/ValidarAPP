using System.Runtime.Serialization;

namespace Sistran.Core.Application.GeneralLedgerServices.DTOs
{
    [DataContract]
    public class JournalEntryListParametersDTO
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
    }
}
