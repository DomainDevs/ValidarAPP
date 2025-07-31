using System;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports
{
    public class ParameterModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsObject { get; set; }
        public int Branch { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public DateTime ImputationDate { get; set; }
        public int BankId { get; set; }
        public string StatusCheck { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int CostCenterId { get; set; }
        public string AccountingNumberFrom { get; set; }
        public string AccountingNumberTo { get; set; }
        public int EntryId { get; set; }
        public int MonthTo { get; set; }
        public int Accumulated { get; set; }
        public int All { get; set; }
        public int Assets { get; set; }
        public int Liabilities { get; set; }
        public int Patrimony { get; set; }
        public int MemorandumAccounts { get; set; }
        public int Income { get; set; }
        public int Expenses { get; set; }
        public int ContingentAccount { get; set; }
        public int ContingentAccountTwo { get; set; }
        public int MemorandumAccountsTwo { get; set; }
        public int AnalysisType { get; set; }
        public int AnalysisByConcept { get; set; }
        public int CurrencyCode { get; set; }

        public string ProcedureName { get; set; }
        public int Prefix { get; set; }
        public string User { get; set; }
        public float DollarExchangeRate { get; set; }
        public float EuroExchangeRate { get; set; }
        public int Insured { get; set; }
        public int Agent { get; set; }
        public string StatusCollectedByPortfolio { get; set; }
        public string StatusListCollections { get; set; }
        public string StatusPremium { get; set; }
        public int PointSale { get; set; }
        public int Currency { get; set; }
        public int Day { get; set; }
        public int PaymentTicketCode { get; set; }
        public int paymentMethodTypeCode { get; set; }
        public string BranchName { get; set; }
        public string CurrencyName { get; set; }
        public string MonthFormat { get; set; }
        public int AgentType { get; set; }

        public string InsuredName { get; set; }
        public string PrefixName { get; set; }
        public string Concept { get; set; }
        public int DocumentNumber { get; set; }
        public decimal Amount { get; set; }

        public int AutomaticEntries { get; set; }
        public int ManualEntries { get; set; }
        public int Operation { get; set; }

        public int LineBusiness { get; set; }
        public DateTime DueDate { get; set; }
        public int RangeId { get; set; }
        public int OperationType { get; set; }

        // Reportes masivos
        public int MassiveReportId { get; set; }
        public int ReportTypeId { get; set; }
        public int TotalRecords { get; set; }
    }
}