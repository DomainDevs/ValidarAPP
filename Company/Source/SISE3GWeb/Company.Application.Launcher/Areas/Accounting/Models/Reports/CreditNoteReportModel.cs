using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports
{
    [KnownType("CreditNoteReportModel")]
    public class CreditNoteReportModel
    {
        public int BranchId { get; set; }
        public string BranchDescription { get; set; }
        public int PrefixId { get; set; }
        public string PrefixDescription { get; set; }
        public int PolicyId { get; set; }
        public string PolicyDocumentNumber { get; set; }
        public int EndorsementId { get; set; }
        public string EndorsementDocumentNumber { get; set; }
        public int PayerIndividualId { get; set; }
        public string PayerName { get; set; }
        public int PaymentNumer { get; set; }
        public decimal Amount { get; set; }
        public int ImputationId { get; set; }
    }
}