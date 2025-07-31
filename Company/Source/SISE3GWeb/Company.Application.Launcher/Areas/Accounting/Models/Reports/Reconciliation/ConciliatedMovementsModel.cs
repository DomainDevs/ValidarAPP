using System;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports.Reconciliation
{
    public class ConciliatedMovementsModel
    {
        public int ConciliationId { get; set; }
        public DateTime DateConciliation { get; set; }
        public string Type { get; set; }

        public DateTime DateBankMovements { get; set; }
        public string VoucherBankMovements { get; set; }
        public int ConciliationMovementTypeBankMovements { get; set; }
        public decimal AmountBankMovements { get; set; }

        public DateTime DateDailyAccountingMovement { get; set; }
        public string VoucherDailyAccountingMovement { get; set; }
        public int ConciliationMovementTypeDailyAccountingMovement { get; set; }
        public decimal AmountDailyAccountingMovement { get; set; }

        public DateTime DateAccountingMovement { get; set; }
        public string VoucherAccountingMovement { get; set; }
        public int ConciliationMovementTypeAccountingMovement { get; set; }
        public decimal AmountAccountingMovement { get; set; }

        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
    }
}