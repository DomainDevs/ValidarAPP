using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Bill
{
    public class ReceiptSearchModel
    {
        public int BillCode { get; set; }
        public int BillStatus { get; set; }
        public string StatusDescription { get; set; }
        public string PaymentsTotal { get; set; }
        public string AccountName { get; set; }
        public string RegisterDate { get; set; }
        public string AccountingDate { get; set; }
        public int PayerId { get; set; }
        public string Payer { get; set; }
        public int BranchCode { get; set; }
        public string BranchDescription { get; set; }
        public int BillControlCode { get; set; }
        public string DocumentNumber { get; set; }
        public string Comments { get; set; }
        public int JournalEntryId { get; set; }
        public int TechnicalTransaction { get; set; }
    }
}