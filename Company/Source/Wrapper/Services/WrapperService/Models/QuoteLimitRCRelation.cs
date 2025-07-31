using Sistran.Core.Application.CommonService.Models;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.WrapperServices.Models
{
    [DataContract]
    public class QuoteLimitRCRelation
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public bool IsDefault { get; set; }
        [DataMember]
        public PolicyType PolicyType { get; set; }
        [DataMember]
        public Prefix Prefix { get; set; }
    }
}
