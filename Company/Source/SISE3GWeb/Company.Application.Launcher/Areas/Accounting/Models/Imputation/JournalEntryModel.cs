using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Imputation
{
    [KnownType("JournalEntryModel")]
    public class JournalEntryModel
    {
        public int JournalEntryItemId { get; set; }
        public DateTime AccountingDate { get; set; }
        public int BranchId { get; set; }
        public int CompanyId { get; set; }
        public string Comments { get; set; }
        public string Description { get; set; }
        public int IndividualId { get; set; }
        public int PersonTypeId { get; set; }
        public int SalePointId { get; set; }
        public int StatusId { get; set; }
    }
}