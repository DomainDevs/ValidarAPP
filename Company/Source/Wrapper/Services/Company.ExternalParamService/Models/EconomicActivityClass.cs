using System.Runtime.Serialization;

namespace Sistran.Company.ExternalParamService.Models
{
    [DataContract]
    public class EconomicActivityClass
    {
        [DataMember]
        public int EconomicActivityCd { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
