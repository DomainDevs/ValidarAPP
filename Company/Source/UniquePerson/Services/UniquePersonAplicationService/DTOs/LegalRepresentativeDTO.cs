using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class LegalRepresentativeDTO
    {
        [DataMember]
        public int individualId { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string AuthorizationAmount { get; set; }
        [DataMember]
        public int CurrencyId { get; set; }
        [DataMember]
        public decimal Value { get; set; }
        [DataMember]
        public DateTime BirthDate { get; set; }
        [DataMember]
        public string BirthPlace { get; set; }
        [DataMember]
        public string CellPhone { get; set; }
        [DataMember]
        public SelectDTO City { get; set; }
        [DataMember]
        public SelectDTO State { get; set; }
        [DataMember]
        public SelectDTO Country { get; set; }

        [DataMember]
        public String DANECode { get; set; }

        [DataMember]
        public string ContactAdditionalInfo { get; set; }
        [DataMember]
        public string ContactName { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string ExpeditionPlace { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string GeneralManagerName { get; set; }
        [DataMember]
        public int DocumentTypeId { get; set; }
        [DataMember]
        public DateTime ExpeditionDate { get; set; }
        [DataMember]
        public string NumberDocument { get; set; }
        [DataMember]
        public string JobTitle { get; set; }
        [DataMember]
        public string ManagerName { get; set; }
        [DataMember]
        public string Nationality { get; set; }
        [DataMember]
        public string Phone { get; set; }
     

    }
}
