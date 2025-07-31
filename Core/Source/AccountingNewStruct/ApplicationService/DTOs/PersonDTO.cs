using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    [DataContract]
    public class PersonDTO
    {
        [DataMember]
        public IdentificationDocumentDTO IdentificationDocument { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public PersonTypeDTO PersonType { get; set; }
        
        [DataMember]
        public int CustomerType { get; set; }

        [DataMember]
        public int IndividualType { get; set; }
        
    }
}
