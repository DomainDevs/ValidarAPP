using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class PaymentOrdersCommissionDTO 
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string ProcessNumber { get; set; }
        [DataMember]
        public int BranchId { get; set; }
        [DataMember]
        public string BranchName { get; set; }
        [DataMember]
        public int CompanyId { get; set; }
        [DataMember]
        public string CompanyName { get; set; }
        [DataMember]
        public DateTime EstimatedDatePayment { get; set; }
        [DataMember]
        public string PaymentOrderNumber { get; set; }
        [DataMember]
        public string AgentDocumentNumber { get; set; }
        [DataMember]
        public string AgentName { get; set; }
        [DataMember]
        public string AgentDocNumberName { get; set; }
        [DataMember]
        public int CurrencyId { get; set; }
        [DataMember]
        public string CurrencyName { get; set; }
        [DataMember]
        public int PaymentSourceId { get; set; }
        [DataMember]
        public decimal ExchangeRate { get; set; }
        [DataMember]
        public decimal IncomeAmount { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
    }
}
