using System.Runtime.Serialization;

namespace Sistran.Company.ExternalParamService.Models
{
    [DataContract]
    public class ListProductClass
    {
        [DataMember]
        public int PrefixCd { get; set; }
        [DataMember]
        public string DescriptionPrefix { get; set; }
        [DataMember]
        public int ProductCd { get; set; }
        [DataMember]
        public string DescriptionProduct { get; set; }
        [DataMember]
        public int GroupCoverageCd { get; set; }
        [DataMember]
        public string DescriptionGroup { get; set; }
    }
}
