using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    [DataContract]
    public class ParamApplicationPremiumComponent
    {
        [DataMember]
        public int PolicyId { get; set; }
        [DataMember]
        public int EndorsementId { get; set; }
        [DataMember]
        public int QuotaNumber { get; set; }
        [DataMember]
        public int CurrencyId { get; set; }
        [DataMember]
        public decimal ExchangeRate { get; set; }
        [DataMember]
        public int PremiumId { get; set; }

        [DataMember]
        public int TempApplicationPremiumCode { get; set; }

        [DataMember]
        public decimal ApplicationAmount { get; set; }


    }
}
