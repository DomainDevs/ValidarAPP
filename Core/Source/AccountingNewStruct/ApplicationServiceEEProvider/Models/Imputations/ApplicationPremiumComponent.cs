using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    [DataContract]
    public class ApplicationPremiumComponent
    {
        [DataMember]
        public int PremiumId { get; set; }

        [DataMember]
        public int AppComponentId { get; set; }

        [DataMember]
        public int ComponentId { get; set; }

        [DataMember]
        public int CurrencyId { get; set; }

        [DataMember]
        public decimal ExchangeRate { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public decimal LocalAmount { get; set; }

        [DataMember]
        public decimal MainAmount { get; set; }

        [DataMember]
        public decimal MainLocalAmount { get; set; }

        [DataMember]
        public string ComponentTinyDescription { get; set; }
    }
}
