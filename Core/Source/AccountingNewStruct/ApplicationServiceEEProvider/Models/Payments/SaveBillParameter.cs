namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments
{
    public class SaveBillParameter
    {
        public Collect.Collect Collect { get; set; }

        public int TypeId { get; set; }

        public int UserId { get; set; }

        public int TechnicalTransaction { get; set; }

        public int PaymentCode { get; set; }

        public int BridgeAccoutingId { get; set; }

        public string BridgePackageCode { get; set; }
    }
}
