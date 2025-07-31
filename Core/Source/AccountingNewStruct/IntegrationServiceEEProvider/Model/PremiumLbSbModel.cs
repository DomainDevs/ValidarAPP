namespace Sistran.Core.Integration.AccountingServices.EEProvider.Model
{
    public class PremiumLbSbModel
    {
        public int PayerId { get; set; }
        public int QuotaNumber { get; set; }      
        public int ComponentId { get; set; }
        public int Lb { get; set; }
        public int Sb { get; set; }
        public decimal Amount { get; set; }
     

    }
}
