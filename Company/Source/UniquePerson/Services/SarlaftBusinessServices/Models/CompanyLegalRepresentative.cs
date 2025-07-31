using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.SarlaftBusinessServices.Models
{
    [DataContract]
    public class CompanyLegalRepresentative
    {
        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public string LegalRepresentativeName { get; set; }

        [DataMember]
        public DateTime ExpeditionDate { get; set; }

        [DataMember]
        public string ExpeditionPlace { get; set; }

        [DataMember]
        public DateTime BirthDate { get; set; }

        [DataMember]
        public string BirthPlace { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public long? Phone { get; set; }

        [DataMember]
        public string JobTitle { get; set; }

        [DataMember]
        public long? CellPhone { get; set; }

        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public int CountryId { get; set; }
        [DataMember]
        public int StateId { get; set; }

        [DataMember]
        public int CityId { get; set; }

        [DataMember]
        public string Nationality { get; set; }

        [DataMember]
        public string IdCardNo { get; set; }
        [DataMember]
        public decimal? AuthorizationAmount { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int? CurrencyId { get; set; }

        [DataMember]
        public int IdCardTypeCode { get; set; }

        [DataMember]
        public bool IsMain { get; set; }

        [DataMember]
        public int NationalityType { get; set; }
        [DataMember]
        public int NationalityOtherType { get; set; }
        [DataMember]
        public int LegalRepresentType { get; set; }
        [DataMember]
        public int SarlaftId { get; set; }
    }

}
