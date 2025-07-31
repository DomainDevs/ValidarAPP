using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    [DataContract]
    public class CompanyCoverageDeductible
    {
        [DataMember]
        public int Code { get; set; }

        [DataMember]
        public string Description { get; set; }

    }
}
