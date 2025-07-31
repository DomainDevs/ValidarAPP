using System.Runtime.Serialization;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    [DataContract]
    public class ResponseAddress
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int AddressTypeId { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int CityId { get; set; }

        [DataMember]
        public string CityDescription { get; set; }

        [DataMember]
        public int StateId { get; set; }

        [DataMember]
        public string StateDescription { get; set; }

        [DataMember]
        public int CountryId { get; set; }

        [DataMember]
        public string CountryDescription { get; set; }

        [DataMember]
        public bool IsPrincipal { get; set; }

        [DataMember]
        public int StreetTypeId { get; set; }

        [DataMember]
        public string UpdateDate { get; set; }

        [DataMember]
        public string TipoDireccion { get; set; }

    }
}
