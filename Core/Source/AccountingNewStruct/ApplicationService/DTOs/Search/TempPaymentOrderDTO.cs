using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class TempPaymentOrderDTO 
    {
        [DataMember]
        public int TempPaymentOrderId { get; set; }
        [DataMember]
        public int BranchId { get; set; }
        [DataMember]
        public string BranchName { get; set; }
        [DataMember]
        public int BranchPayId { get; set; }
        [DataMember]
        public string BranchPayName { get; set; }
        [DataMember]
        public int CompanyId { get; set; }
        [DataMember]
        public string CompanyName { get; set; }
        [DataMember]
        public int CurrencyId { get; set; }
        [DataMember]
        public string CurrencyName { get; set; }
        [DataMember]
        public int PersonTypeId { get; set; }
        [DataMember]
        public string PersonTypeName { get; set; }
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public string BeneficiaryDocumentNumber { get; set; }
        [DataMember]
        public string BeneficiaryName { get; set; }
        [DataMember]
        public string EstimatedPaymentDate { get; set; }
        [DataMember]
        public string PayTo { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public int PaymentMethodId { get; set; }
        [DataMember]
        public string PaymentMethodName { get; set; }
        [DataMember]
        public int PaymentSourceId { get; set; }
        [DataMember]
        public string PaymentSourceName { get; set; }
        [DataMember]
        public decimal Exchange { get; set; }
        [DataMember]
        public decimal IncomeAmount { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public string PaymentDate { get; set; }
        [DataMember]
        public string BeneficiaryType { get; set; }
        [DataMember]
        public int AccountBankId { get; set; }
        [DataMember]
        public string Observation { get; set; }

        [DataMember]
        public int IndividualTypeId { get; set; }
    }
}
