using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.BankReconciliation
{
    public class BankMovementModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public bool IsDebitBank { get; set; }
        public bool IsDebitCompany { get; set; }
        public int AccountingNatureCompany { get; set; }
    }
}