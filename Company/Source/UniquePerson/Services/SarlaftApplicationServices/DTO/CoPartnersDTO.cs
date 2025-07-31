using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.SarlaftApplicationServices.DTO
{
    [DataContract]
    public class CoPartnersDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public string IdCardNumero { get; set; }
        [DataMember]
        public int DocumentTypeId { get; set; }

        [DataMember]
        public int Participation { get; set; }

        [DataMember]
        public string Occupation { get; set; }

        [DataMember]
        public int IdProfileCd { get; set; }

        [DataMember]
        public string Nationality { get; set; }

        [DataMember]
        public string SocietyHolder { get; set; }


        [DataMember]
        public string SocietyName { get; set; }


        [DataMember]
        public int Constitutionyear { get; set; }


        [DataMember]
        public string Address { get; set; }


        [DataMember]
        public string Phone { get; set; }


        [DataMember]
        public int IdCompanyTypeCd { get; set; }
    }
}
