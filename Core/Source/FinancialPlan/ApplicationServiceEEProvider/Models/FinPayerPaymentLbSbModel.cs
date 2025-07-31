namespace Sistran.Core.Application.FinancialPlanServices.EEProvider.Models
{
    public class FinPayerPaymentLbSbModel
    {
        public int Id { get; set; }
        public int PaymentId { get; set; }
        public int ComponentId { get; set; }
        public int LineBussinesId { get; set; }
        public int SubLineBussinesId { get; set; }
        public decimal Amount { get; set; }
        public decimal LocalAmount { get; set; }
        public decimal MainAmount { get; set; }
        public decimal MainLocalAmount { get; set; }
    }
}
