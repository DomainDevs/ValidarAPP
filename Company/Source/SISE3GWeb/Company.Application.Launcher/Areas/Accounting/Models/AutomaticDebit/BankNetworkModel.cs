namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.AutomaticDebit
{
    public class BankNetworkModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool HasTax { get; set; }
        public int TaxTypeId { get; set; }
        public decimal CommissionValue { get; set; }
        public int RetriesNumber { get; set; }
        public bool RequiresNotification { get; set; }
    }
}