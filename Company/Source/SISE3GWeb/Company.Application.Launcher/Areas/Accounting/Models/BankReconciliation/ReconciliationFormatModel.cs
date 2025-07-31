using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.BankReconciliation
{
    public class ReconciliationFormatModel
    {
        public int Id { get; set; }

        public int BankAccountCompanyId { get; set; }

        public int FormatId { get; set; }

        public string OperationType { get; set; }
    }
}