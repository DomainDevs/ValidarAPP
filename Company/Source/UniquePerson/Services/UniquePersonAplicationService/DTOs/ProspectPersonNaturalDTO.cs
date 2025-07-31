using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    public class ProspectPersonNaturalDTO
    {
        [DataMember]
        public int ProspectCode { get; set; }

        [DataMember]
        public string AdditionaInformation { get; set; }

        [DataMember]
        public SelectDTO Address { get; set; }

        [DataMember]
        public DateTime BirthDate { get; set; }

        [DataMember]
        public SelectDTO City { get; set; }

        [DataMember]
        public SelectDTO State { get; set; }

        [DataMember]
        public SelectDTO Country { get; set; }

        [DataMember]
        public String DANECode { get; set; }

        [DataMember]
        public string EmailAddres { get; set; }

        [DataMember]
        public string Gender { get; set; }

        [DataMember]
        public SelectDTO Card { get; set; }

        public int MartialStatus { get; set; }

        [DataMember]
        public string MotherLastName { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public long? PhoneNumber { get; set; }

        [DataMember]
        public string SurName { get; set; }

        [DataMember]
        public int IndividualTypePerson { get; set; }

        [DataMember]
        public string IdCardNo { get; set; }

        [DataMember]
        public int? IdCardTypeCode { get; set; }
    }
}
