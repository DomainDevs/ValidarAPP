using System.Runtime.Serialization;

namespace Sistran.Core.Application.TaxServices.Models.Base
{
    [DataContract]
    public class BaseEconomicActivityTax
    {
        [DataMember]
        public int EconomicActivityTaxId { get; set; }
        [DataMember]
        public int CountryCode { get; set; }
        [DataMember]
        public int StateCode { get; set; }
        [DataMember]
        public int CityCode { get; set; }
        [DataMember]
        public int EconomicActivityCode { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
