using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class InsuredProfileDTO
    {
        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
