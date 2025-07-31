namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models
{
    public class AccountingAccountFilter
    {
        public int UserId { get; set; }

        public int IndividualId { get; set; }

        public int BranchId { get; set; }

        public int ConceptId { get; set; }

        public string ConceptDescription { get; set; }

        public string AccountingDescription { get; set; }

        public string AccountingNumber { get; set; }
    }
}
