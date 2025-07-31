using System.Runtime.Serialization;

namespace Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.Base
{
    [DataContract]
    public class PersonBaseDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int IdSequential { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
