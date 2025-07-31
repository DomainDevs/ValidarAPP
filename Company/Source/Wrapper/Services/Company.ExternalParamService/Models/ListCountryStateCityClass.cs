using System.Runtime.Serialization;

namespace Sistran.Company.ExternalParamService.Models
{
    [DataContract]
    public class ListCountryStateCityClass
    {
        [DataMember]
        public int CountryCd { get; set; }
        [DataMember]
        public string CountryDescription { get; set; }
        [DataMember]
        public int StateCd { get; set; }
        [DataMember]
        public string StateDescription { get; set; }
        [DataMember]
        public int CityCd { get; set; }
        [DataMember]
        public string CityDescription { get; set; }
    }
}
