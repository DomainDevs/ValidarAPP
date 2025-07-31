using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Imputation
{
    [KnownType("InsuredLoanModel")]
    public class InsuredLoanModel
    {
        public int ImputationId { get; set; }
        public List<InsuredLoanItemModel> InsuredLoansTransactionItems { get; set; }
    }

    [KnownType("InsuredLoanItemModel")]
    public class InsuredLoanItemModel
    {
        public int InsuredLoanItemId { get; set; }
        public string Description { get; set; }
        public int ImputationId { get; set; }
        public int LoanNumber { get; set; }
        public int IndividualId { get; set; }
        public string InsuredDocumentNumber { get; set; }
        public string InsuredName { get; set; }
        public int AccountingNature { get; set; }
        public int CurrencyId { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal IncomeAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal Capital { get; set; }
        public decimal CurrentInterest { get; set; }
        public decimal PreviousInterest { get; set; }
        public List<InsuredLoanItemModel> InsuredLoansTransactionChild { get; set; }
    }
}