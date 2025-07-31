using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class ThirdAffectedDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public string FullName { get; set; }
    }
}
