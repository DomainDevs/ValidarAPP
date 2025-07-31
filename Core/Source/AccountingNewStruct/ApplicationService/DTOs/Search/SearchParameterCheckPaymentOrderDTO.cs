using System;
using System.Runtime.Serialization;
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using Sistran.Core.Application.AccountingServices.DTOs.Payments;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class SearchParameterCheckPaymentOrderDTO
    {
        [DataMember]
        public PaymentMethodDTO PaymentSource { get; set; }
        [DataMember]
        public DateTime EstimatedPaymentDate { get; set; }
        [DataMember]
        public int NumberCheck { get; set; }
        [DataMember]
        public BankAccountCompanyDTO BankAccountCompany { get; set; }
        [DataMember]
        public int IsPrinted { get; set; }
        [DataMember]
        public int PaymentOrderNumber { get; set; }
        [DataMember]
        public int CheckFrom { get; set; }
        [DataMember]
        public int CheckTo { get; set; }
        [DataMember]
        public string BeneficiaryFullName { get; set; }
        [DataMember]
        public int DeliveryType { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
    }
}
