
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.UndewritingIntegrationServices.DTOs
{
    [DataContract]
    public class PayerBaseDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}