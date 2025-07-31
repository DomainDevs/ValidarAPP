using System.Runtime.Serialization;

namespace Sistran.Core.Application.NombreServices.DTOs.ConceptsB
{
    [DataContract]
    public class ConceptBDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
