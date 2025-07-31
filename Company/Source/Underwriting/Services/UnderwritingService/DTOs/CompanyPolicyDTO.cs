using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.DTOs
{
    [DataContract]
    public class CompanyPolicyDTO
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public int EndorsmentId { get; set; }

        [DataMember]
        public decimal DocumentNumber { get; set; }
    }
}
