using System.Runtime.Serialization;

namespace Sistran.Company.ExternalParamService.Models
{
    [DataContract]
    public class ListRestrictiveClass
    {
        [DataMember]
        public string DocumetType { get; set; }

        [DataMember]
        public string DocumetNum { get; set; }
    }
}
