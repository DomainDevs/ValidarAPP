using System.Runtime.Serialization;

namespace Sistran.Company.ExternalParamService.Models
{
    [DataContract]
    public class ParamListDetail
    {
        [DataMember]
        public string ProcessMessage { get; set; }

        [DataMember]
        public ListDetailClass[] ListDetailList  { get; set;}
    }
}
