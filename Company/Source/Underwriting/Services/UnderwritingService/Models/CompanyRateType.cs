using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    [DataContract]
    public class CompanyRateType
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
