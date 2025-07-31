using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.DTOs
{
    [DataContract]
    public class ComboDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
