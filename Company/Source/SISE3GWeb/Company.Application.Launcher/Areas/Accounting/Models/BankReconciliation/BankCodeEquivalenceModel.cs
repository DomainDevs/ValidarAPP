using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.BankReconciliation
{
    public class BankCodeEquivalenceModel
    {
        public int Id { get; set; }
        public int BankId { get; set; }
        public string BankCode { get; set; }
        public string BankMovementId { get; set; }
        public bool HasVoucher { get; set; }        
    }
}