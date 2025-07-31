using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    [DataContract]
    public class AssistanceCover
    {
        [DataMember]
        public int PrefixCode { get; set; }
        [DataMember]
        public string PrefixDescription { get; set; }
        [DataMember]
        public int AssitanceCode { get; set; }
        [DataMember]
        public string AssistanceDescription { get; set; }
        public int TextCode { get; set; }
        [DataMember]
        public string TextDescription { get; set; }

        
    }
}
