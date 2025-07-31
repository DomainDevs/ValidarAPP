using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    [DataContract]
    public class ApplicationPremiumComponentLBSB
    {
        [DataMember]
        public int ApplicationComponenLSBSId { get; set; }

        [DataMember]
        public int ApplicationComponentId { get; set; }

        [DataMember]
        public int LineBussinesId { get; set; }

        [DataMember]
        public int SubLineBussinesId { get; set; }

        [DataMember]
        public int CurrencyId { get; set; }

        [DataMember]
        public decimal ExchangeRateId { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public decimal LocalAmount { get; set; }

        [DataMember]
        public decimal MainAmount { get; set; }

        [DataMember]
        public decimal MainLocalAmount { get; set; }
    }
}
