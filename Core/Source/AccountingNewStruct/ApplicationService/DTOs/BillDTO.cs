using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs
{

    [DataContract]
    public class BillDTO
    {
        [DataMember]
        public int BillId { get; set; }
        [DataMember]
        public int BillingConceptId { get; set; }
        [DataMember]
        public int BillControlId { get; set; }
        [DataMember]
        public DateTime RegisterDate { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public decimal PaymentsTotal { get; set; }
        [DataMember]
        public int PayerId { get; set; }
        [DataMember]
        public int SourcePaymentId { get; set; }
        [DataMember]
        public List<PaymentSummaryDTO> PaymentSummary { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public int PayerTypeId { get; set; }
        [DataMember]
        public int PayerDocumentTypeId { get; set; }
        [DataMember]
        public string PayerDocumentNumber { get; set; }
        [DataMember]
        public string PayerName { get; set; }
    }
}
