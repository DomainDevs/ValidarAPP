using System.Runtime.Serialization;

namespace Sistran.Company.ExternalParamService.Models
{
    [DataContract]
    public class ParamAssistanceCover
    {
        [DataMember]
        public int PrefixCode { get; set; }
        [DataMember]
        public string PrefixDescription { get; set; }
        [DataMember]
        public int AssitanceCode { get; set; }
        [DataMember]
        public string AssistanceDescription { get; set; }
        [DataMember]
        public int TextCode { get; set; }
        [DataMember]
        public string TextDescription { get; set; }
    }
}
