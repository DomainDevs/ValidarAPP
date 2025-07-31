using System.Runtime.Serialization;

namespace Sistran.Company.ExternalParamService.Models
{
    [DataContract]
    public class ParamAssistance
    {
        [DataMember]
        public int PrefixCode { get; set; }
        [DataMember]
        public string PrefixDescription { get; set; }
        [DataMember]
        public int AssitanceCode { get; set; }
        [DataMember]
        public string AssistanceDescription { get; set; }
    }
}
