using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class MassiveDataGenerateDTO 
    {
        [DataMember]
        public int PayerId { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public int PolicyId { get; set; }       
        [DataMember]
        public int EndorsementId { get; set; }
        [DataMember]
        public int Quote { get; set; }
        [DataMember]
        public int PaymentIndividualId { get; set; }//tomador
        [DataMember]
        public string PaymentIndividualName { get; set; }//tomador
        [DataMember]
        public int PaymentMethodCode { get; set; }
        [DataMember]
        public string IssueDate { get; set; }
        [DataMember]
        public int BranchId { get; set; }
        [DataMember]
        public int Rows { get; set; }
        [DataMember]
        public int PolicyNumber { get; set; }
        [DataMember]
        public int EndorsementNumber { get; set; }
        [DataMember]
        public int PrefixId { get; set; }
        [DataMember]
        public int CurrencyId { get; set; }
        [DataMember]
        public int AgentId { get; set; }
        [DataMember]
        public string AgentName { get; set; }
        [DataMember]
        public string BeneficiaryName { get; set; }
        [DataMember]
        public string BeneficiaryDocumentNumber { get; set; }
        [DataMember]
        public decimal ExchangeRate { get; set; }
    }
}