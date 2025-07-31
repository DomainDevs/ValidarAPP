using System.Runtime.Serialization;

namespace Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.Person
{
    [DataContract]
    public class PersonRequestDTO
    {
        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public int DocumentTypeId { get; set; }
    }
}
