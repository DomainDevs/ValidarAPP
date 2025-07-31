using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class ProspectLegalDTO
    {
        [DataMember]
        public string TradeName { get; set; }

        [DataMember]
        public int ProspectCode { get; set; }

        [DataMember]
        public string AdditionaInformation { get; set; }

        [DataMember]
        public SelectDTO Address { get; set; }

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
        public string Name { get; set; }

        [DataMember]
        public long? PhoneNumber { get; set; }

        [DataMember]
        public int IndividualTypePerson { get; set; }

        [DataMember]
        public string TributaryIdNumber { get; set; }

        [DataMember]
        public int TributaryIdTypeCode { get; set; }

    }
}
