using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class CompanyGroupCoverage : BaseGroupCoverage
    {
        [DataMember]
        public CompanyCoverage Coverage { get; set; }
        [DataMember]
        public List<CompanyCoverage> Coverages { get; set; }
        [DataMember]
        public List<CompanyInsuredObject> InsuredObjects { get; set; }
    }
}
