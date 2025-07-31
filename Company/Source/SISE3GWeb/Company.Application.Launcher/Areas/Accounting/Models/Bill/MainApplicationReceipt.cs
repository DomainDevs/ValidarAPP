using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Bill
{
    [KnownType("MainApplicationReceipt")]
    public class MainApplicationReceipt
    {
        public string Message { get; set; }

        public string BillId { get; set; }

        public string AccountingCompanyId { get; set; }

        public string BillingConceptId { get; set; }

        public string Date { get; set; }

        public string Description { get; set; }

        public string IsTemporal { get; set; }

        public string Number { get; set; }

        public string PayerIndividualId { get; set; }

        public string PaymentsTotal { get; set; }

        public string StatusId { get; set; }

        public string UserId { get; set; }

        public string TechnicalTransaction { get; set; }

        public string ImputationMessage { get; set; }

        public string ShowMessage { get; set; }

        public string ShowImputationMessage { get; set; }

        public bool GeneralLedgerSuccess { get; set; }
    }

}