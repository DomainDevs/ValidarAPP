using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class OtherPaymentsRequestReportHeaderDTO 
    {
        [DataMember]
        public int PaymentRequestId { get; set; }
		
        [DataMember]
        public int Number { get; set; }
		
        [DataMember]
        public string EstimatedDate { get; set; }
		
        [DataMember]
        public int PersonTypeId { get; set; }
		
        [DataMember]
        public string PersonTypeDescription { get; set; }
		
        [DataMember]
        public int IndividualId { get; set; }
		
        [DataMember]
        public string DocumentNumber { get; set; }
		
        [DataMember]
        public string Name { get; set; }
		
        [DataMember]
        public int CurrencyId { get; set; }
		
        [DataMember]
        public string CurrencyDescription { get; set; }
		
        [DataMember]
        public string RegistrationDate { get; set; }
		
        [DataMember]
        public decimal TotalAmount { get; set; }
		
        [DataMember]
        public int UserId { get; set; }
		
        [DataMember]
        public string UserAccountName { get; set; }
		
        [DataMember]
        public int PaymentMethodId { get; set; }
		
        [DataMember]
        public string PaymentMethodDescription { get; set; }
		
        [DataMember]
        public string PaymentRequestDescription { get; set; }
		
        [DataMember]
        public int CollectId { get; set; }
		
        [DataMember]
        public List<OtherPaymentRequestReportDetails> OtherPaymentRequestReportDetails { get; set; }
        
    }

    [DataContract]
    public class OtherPaymentRequestReportDetails //: IDto
    {
        [DataMember]
        public int VoucherTypeId { get; set; }
        [DataMember]
        public string VoucherTypeDescription { get; set; }
        [DataMember]
        public string VoucherNumber { get; set; }
        [DataMember]
        public decimal TotalAmount { get; set; }
        [DataMember]
        public decimal Taxes { get; set; }
        [DataMember]
        public decimal Retentions { get; set; }
    }
}
