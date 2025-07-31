using System;
using System.Collections.Generic;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.BankReconciliation
{
    public class BankReconciliationModel
    {
        public int Id { get; set; }
        public DateTime DateTo { get; set; }
        public DateTime ConciliationDate { get; set; }
        public int AccountBankId { get; set; }
        public List<BankStatementModel> Statements { get; set; }

    }
}