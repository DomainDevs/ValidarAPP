using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{

    [DataContract]
    public class CompanyAcceptCoInsuranceAgent
    {
        [DataMember]
        public CompanyPolicyAgent Agent { get; set; }

        [DataMember]
        public decimal ParticipationPercentage { get; set; }
    }
}
