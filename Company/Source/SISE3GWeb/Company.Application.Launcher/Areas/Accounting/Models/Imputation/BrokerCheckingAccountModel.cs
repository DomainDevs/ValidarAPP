using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Imputation
{
    [KnownType("BrokerCheckingAccountModel")]
    public class BrokerCheckingAccountModel
    {
        public int ImputationId { get; set; }
        public List<BrokerCheckingAccountItemModel> BrokersCheckingAccountTransactionItems { get; set; }
    }

    [KnownType("BrokerCheckingAccountItemModel")]
    public class BrokerCheckingAccountItemModel 
    {
        public int BrokerCheckingAccountItemId { get; set; }
        public int AgentTypeId { get; set; }
        public int AgentId { get; set; }
        public int AgentAgencyId { get; set; }
        public int AccountingNature { get; set; }
        public int BranchId { get; set; }
        public int SalePointId { get; set; }
        public int AccountingCompanyId { get; set; }
        public DateTime AccountingDate { get; set; }
        public int CheckingAccountConceptId { get; set; }
        public int CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal IncomeAmount { get; set; }
        public decimal Amount { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public int BillId { get; set; }
        public int BrokerCheckingAccountId { get; set; }
        public int PolicyId { get; set; }
        public int PrefixId { get; set; }
        public int InsuredId { get; set; }
        public int CommissionType { get; set; }
        public decimal CommissionPercentage { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal DiscountedCommission { get; set; }
        public decimal CommissionBalance { get; set; }
        public List<BrokerCheckingAccountItemModel> BrokersCheckingAccountTransactionChild { get; set; }
    }
}