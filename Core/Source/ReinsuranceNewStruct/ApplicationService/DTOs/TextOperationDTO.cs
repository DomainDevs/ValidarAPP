using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class TextOperationDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int Description { get; set; }
        [DataMember]
        public string TextOperation { get; set; }
    }
}
