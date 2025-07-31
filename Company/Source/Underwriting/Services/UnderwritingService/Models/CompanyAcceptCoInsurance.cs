using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{

    [DataContract]
    public class CompanyAcceptCoInsurance
    {
        [DataMember]
        public decimal Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public decimal ParticipationPercentage { get; set; }
        [DataMember]
        public decimal ExpensesPercentage { get; set; }
        [DataMember]
        public decimal ParticipationPercentageOwn { get; set; }
        [DataMember]
        public string PolicyNumber { get; set; }
        [DataMember]
        public string EndorsementNumber { get; set; }
        [DataMember]
        List<CompanyAcceptCoInsuranceAgent> acceptCoInsuranceAgent { get; set; }

    }
}
