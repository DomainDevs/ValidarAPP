namespace Sistran.Core.Application.UnderwritingServices.Models.Distribution
{
    public class ComponentValue
    {
        public decimal Premium { get; set; }
        public decimal Tax { get; set; }
        public decimal Expenses { get; set; }
        public decimal Surcharges { get; set; }
        public decimal Discounts { get; set; }
        public decimal Total { get; set; }
        
    }
}
