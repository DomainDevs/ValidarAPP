using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports.Reconciliation
{
    public class ReconciliationBalanceModel
    {
        public int Id { get; set; }
        public int Section { get; set; }
        public string Description { get; set; }
        public string Debits { get; set; }
        public string Credits { get; set; }
        public int BankId { get; set; }
        public string BankName { get; set; }
        public int AccountTypeId { get; set; }
        public string AccountTypeName { get; set; }
        public string AccountNumber { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public string UserName { get; set; }
    }
}