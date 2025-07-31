using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    [DataContract]
    public class OthersDeclinedTypes
    {
        [DataMember]
        public decimal Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string SmalDescription { get; set; }

        [DataMember]
        public decimal RoleCd { get; set; }
    }
}