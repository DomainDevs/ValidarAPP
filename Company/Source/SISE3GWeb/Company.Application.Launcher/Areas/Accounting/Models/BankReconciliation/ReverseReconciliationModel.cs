using System;
using System.Collections.Generic;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.BankReconciliation
{
    public class ReverseReconciliationModel
    {
        public int Id { get; set; }
        public DateTime ReverseDate { get; set; }
        public List<BankReconciliationModel> Conciliations { get; set; }
    }
}