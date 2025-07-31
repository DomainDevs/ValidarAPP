using System.Runtime.Serialization;

namespace Sistran.Company.ExternalParamService.Models
{
    [DataContract]
    public class ParamAssistanceCoverClass
    {
        [DataMember]
        public ParamAssistanceCover ParamAssistanceCover { get; set;}

        [DataMember]
        public string ProcessMessage { get; set; }
    }
}
