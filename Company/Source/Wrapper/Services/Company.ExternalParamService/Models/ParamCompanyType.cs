using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.ExternalParamService.Models
{
    [DataContract]
    public class ParamCompanyType
    {
        [DataMember]
        public List<CompanyTypeClass> CompanyTypeClass { get; set; }

        [DataMember]
        public string ProcessMessage { get; set; }
    }
}
