using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using System;
using System.Runtime.Serialization;



namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class SearchParameterPaymentOrdersDTO
    {
        [DataMember]
        public BranchDTO Branch { get; set; }
        [DataMember]
        public PaymentMethodDTO PaymentMethod { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string StartDate { get; set; }
        [DataMember]
        public string EndDate { get; set; }
        [DataMember]
        public string PaymentOrderNumber { get; set; }
        [DataMember]
        public PersonTypeDTO PersonType { get; set; }
        [DataMember]
        public string BeneficiaryDocumentNumber { get; set; }
        [DataMember]
        public string BeneficiaryFullName { get; set; }
        [DataMember]
        public DateTime PaymentDate { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public bool? IsDelivered { get; set; }
        [DataMember]
        public bool? IsReconciled { get; set; }
        [DataMember]
        public bool? IsAccounting { get; set; }
        [DataMember]
        public int AccountBankId { get; set; }
        [DataMember]
        public string CheckNumber { get; set; }

    }
}
