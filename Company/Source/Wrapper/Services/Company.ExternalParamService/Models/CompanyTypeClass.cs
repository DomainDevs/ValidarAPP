using System.Runtime.Serialization;

namespace Sistran.Company.ExternalParamService.Models
{
    [DataContract]
    public class CompanyTypeClass
    {
        [DataMember]
        public int CompanyTypeCd { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
