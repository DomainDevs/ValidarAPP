using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.SarlaftApplicationServices.DTO
{
    [DataContract]
    public class PersonDTO
    {
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int DocumentTypeId { get; set; }
        [DataMember]
        public string DocumentNumber { get; set; }
        [DataMember]
        public int? EconomicActivityId { get; set; }
        [DataMember]
        public string EconomicActivityDesc { get; set; }
        [DataMember]
        public int PersonType { get; set; }
        [DataMember]
        public int AssociationType { get; set; }

    }
}
