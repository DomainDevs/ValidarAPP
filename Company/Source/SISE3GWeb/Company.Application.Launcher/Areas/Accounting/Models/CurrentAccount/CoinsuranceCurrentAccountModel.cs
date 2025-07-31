using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.CurrentAccount
{
    [Serializable]
    [KnownType("CoinsuranceCurrentAccountReportModel")]
    public class CoinsuranceCurrentAccountModel
    {
        [DataMember]
        public string PaymentDate { get; set; }
        [DataMember]
        public int BillNumber { get; set; }
        [DataMember]
        public int BranchId { get; set; }
        [DataMember]
        public int PrefixId { get; set; }
        [DataMember]
        public int SubPrefixId { get; set; }
        [DataMember]
        public string PolicyNumber { get; set; }
        [DataMember]
        public string EndorsmentNumber { get; set; }
        [DataMember]
        public int InsuredId { get; set; }
        [DataMember]
        public string InsuredName { get; set; }
        [DataMember]
        public int AgentId { get; set; }
        [DataMember]
        public string AgentName { get; set; }
        [DataMember]
        public string CurrencyDescription { get; set; }
        [DataMember]
        public decimal IssuancePrimeAmount { get; set; }
        [DataMember]
        public decimal PrimeAmount { get; set; }
        [DataMember]
        public int CompanyParticipation { get; set; }
        [DataMember]
        public decimal IssuancePremiumAmount { get; set; }
        [DataMember]
        public decimal PremiumAmount { get; set; }
        [DataMember]
        public decimal PolicyChange { get; set; }
        [DataMember]
        public decimal IssuanceExpensesAmount { get; set; }
        [DataMember]
        public decimal ExpensesAmount { get; set; }
        [DataMember]
        public decimal IssuanceExpensesTax { get; set; }
        [DataMember]
        public decimal ExpensesTax { get; set; }
        [DataMember]
        public decimal IssuanceAmount { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public string BranchDescription { get; set; }
        [DataMember]
        public string CompanyDescription { get; set; }
    }
}