

using System.Runtime.Serialization;

namespace Sistran.Company.Application.Transports.TransportApplicationService.DTOs
{
    [DataContract]
    public class CurrencyDTO
    {
        [DataMember]
        public int Id { get; set; }
        
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string SmallDescription { get; set; }

        [DataMember]
        public string TinyDescription { get; set; }
    }
}
