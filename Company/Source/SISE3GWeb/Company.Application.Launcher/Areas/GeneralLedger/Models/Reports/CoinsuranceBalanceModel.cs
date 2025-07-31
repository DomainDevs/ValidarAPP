using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models.Reports
{
    [Serializable]
    [KnownType("CoinsuranceBalanceModel")]
    public class CoinsuranceBalanceModel
    {
        [DataMember]
        public int OperationId { get; set; }

        [DataMember]
        public int CoinsuranceCheckingAccountCd { get; set; }

        [DataMember]
        public int CurrencyCd { get; set; }

        [DataMember]
        public int LineBusinessCd { get; set; }

        [DataMember]
        public string LineBusinessDescription { get; set; }

        [DataMember]
        public int SubLineBusinessCd { get; set; }

        [DataMember]
        public string SubLineBusinessDescription { get; set; }

        [DataMember]
        public int CoinsuredCompanyId { get; set; }

        [DataMember]
        public DateTime AccountingDate { get; set; }

        [DataMember]
        public int TransactionNumber { get; set; }

        [DataMember]
        public int DailyEntryId { get; set; }

        [DataMember]
        public decimal ExchangeRate { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public decimal PolicyincomeAmount { get; set; }

        [DataMember]
        public decimal PolicyAmount { get; set; }

        [DataMember]
        public decimal AdministrativeExpensesIncomeAmount { get; set; }

        [DataMember]
        public decimal AdministrativeExpensesAmount { get; set; }

        [DataMember]
        public decimal AdministrativeExpensesTaxincomeAmount { get; set; }

        [DataMember]
        public decimal AdministrativeExpensesTaxAmount { get; set; }

        [DataMember]
        public decimal IncomeAmount { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public int PolicyId { get; set; }

        [DataMember]
        public int MovementCd { get; set; }

        [DataMember]
        public string CompanyTradeName { get; set; }

        [DataMember]
        public decimal PrimeIncomeAmount { get; set; }

        [DataMember]
        public decimal PrimeAmount { get; set; }

        [DataMember]
        public int BranchCd { get; set; }

        [DataMember]
        public string BranchDescription { get; set; }


        [DataMember]
        public decimal PolicyNumber { get; set; }

        [DataMember]
        public decimal EndorsementNumber { get; set; }

        [DataMember]
        public decimal PolicyHolderId { get; set; }


        [DataMember]
        public string PolicyHolderName { get; set; }

        [DataMember]
        public decimal PolicyExchangeRate { get; set; }

        [DataMember]
        public int BusinessTypeCd { get; set; }

        [DataMember]
        public string Surname { get; set; }

        [DataMember]
        public string MotherLastName { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public decimal AgentTaxIncomeAmount { get; set; }

        [DataMember]
        public decimal AgentTaxAmount { get; set; }

        [DataMember]
        public decimal CompanyCommissionPercentage { get; set; }

        [DataMember]
        public decimal CompanyParticipationPercentage { get; set; }

        [DataMember]
        public int PrefixCd { get; set; }

        [DataMember]
        public string PrefixDescription { get; set; }


        [DataMember]
        public decimal ClaimPaymentTaxIncomeAmount { get; set; }

        [DataMember]
        public decimal ClaimPaymentTaxAmount { get; set; }

        [DataMember]
        public int Year { get; set; }

        [DataMember]
        public string Month { get; set; }

        [DataMember]
        public decimal Balance { get; set; }
    }
}