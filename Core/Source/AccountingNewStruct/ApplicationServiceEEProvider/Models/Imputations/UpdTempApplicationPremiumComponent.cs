namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    public class UpdTempApplicationPremiumComponent
    {

        public UpdTempApplicationPremiumComponent()
        {

        }

        public int TempApplicationPremiumCode { get; set; }

        public int ComponentCurrencyCode { get; set; }
        
        public decimal ExchangeRate { get; set; }
        
        public decimal PremiumAmount { get; set; }
        
        public decimal ExpensesLocalAmount { get; set; }
        
        public decimal TaxLocalAmount { get; set; }
    }
}
