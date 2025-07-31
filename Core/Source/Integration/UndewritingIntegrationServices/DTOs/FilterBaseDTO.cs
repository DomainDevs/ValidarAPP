
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.UndewritingIntegrationServices.DTOs
{
    [DataContract]
    public class FilterBaseDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int Quota { get; set; }
    }
}