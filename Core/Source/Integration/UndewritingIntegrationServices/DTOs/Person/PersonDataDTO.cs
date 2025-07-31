using System.Runtime.Serialization;

namespace Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.Person
{
    [DataContract]
    public class PersonDataDTO
    {
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public string FullName { get; set; }
    }
}
