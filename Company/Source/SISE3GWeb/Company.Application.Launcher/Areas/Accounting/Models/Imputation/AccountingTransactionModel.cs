using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Imputation
{
    [KnownType("AccountingTransactionModel")]
    public class AccountingTransactionModel
    {
        public int TempDailyAccountingCode { get; set; }
        public int TempImputationCode { get; set; }
        public int BranchCode { get; set; }
        public int SalesPointCode { get; set; }
        public int CompanyCode { get; set; }
        public int PaymentConceptId { get; set; }
        public int BeneficiaryId { get; set; }
        public int BookAccountCode { get; set; }

        //Agregado para enviar código de cuenta contable en BE
        public string BookAccountNumber { get; set; }
        public int AccountingNature { get; set; }
        public int CurrencyId { get; set; }
        public decimal IncomeAmount { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public int BankReconciliationId { get; set; }
        public int ReceiptNumber { get; set; }
        public DateTime ReceiptDate { get; set; }
        public decimal PostdatedAmount { get; set; }

        //Agregado para grabar centros de costos y códigos de análisis

        public List<CostCenterModel> CostCenters { get; set; }

        public List<AnalysisModel> Analyses { get; set; }
    }

    /// <summary>
    /// CostCenterModel
    /// </summary>
    public class CostCenterModel
    {
        public int CostCenterId { get; set; }
        public string Description { get; set; }
        public decimal Percentage { get; set; }
    }

    /// <summary>
    /// AnalysisCodeModel
    /// </summary>
    public class AnalysisModel
    {
        public int AnalysisCode { get; set; }
        public string AnalysisDescription { get; set; }
        public int AnalysisConcept { get; set; }
        public string AnalysisConceptDescription { get; set; }
        public string AnalysisKey { get; set; }
        public string Description { get; set; }
        
    }
}