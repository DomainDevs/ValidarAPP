using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;




namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class SearchAgentsItemsDTO 
    {
        [DataMember]
        public int BrokerCheckingAccountItemId { get; set; }
        [DataMember]
        public int ImputationId { get; set; }
        [DataMember]
        public int BrokerParentId { get; set; }
        [DataMember]
        public int BranchCode { get; set; }
        [DataMember]
        public string BranchName { get; set; }
        [DataMember]
        public int SalePointCode { get; set; }
        [DataMember]
        public string SalePointName { get; set; }
        [DataMember]
        public int PrefixCode { get; set; }
        [DataMember]
        public string PrefixName { get; set; }
        [DataMember]
        public string PolicyDocumentNumber { get; set; }
        [DataMember]
        public double PolicyId { get; set; }
        [DataMember]
        public string InsuredDocNumber { get; set; }
        [DataMember]
        public string InsuredName { get; set; }
        [DataMember]
        public double InsuredId { get; set; }
        [DataMember]
        public string CurrencyName { get; set; }
        [DataMember]
        public int CurrencyCode { get; set; }
        [DataMember]
        public double Amount { get; set; }
        [DataMember]
        public decimal IncomeAmount { get; set; }
        [DataMember]
        public decimal ExchangeRate { get; set; }
        [DataMember]
        public int CommissionTypeCode { get; set; }
        [DataMember]
        public string CommissionTypeName { get; set; }
        [DataMember]
        public decimal CommissionPercentage { get; set; }
        [DataMember]
        public decimal CommissionAmount { get; set; }
        [DataMember]
        public decimal CommissionDiscounted { get; set; }
        [DataMember]
        public decimal CommissionBalance { get; set; }

        [DataMember]
        public int Rows { get; set; }      

    }
}
