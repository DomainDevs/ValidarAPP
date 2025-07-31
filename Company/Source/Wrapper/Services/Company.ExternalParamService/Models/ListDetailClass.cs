using System.Runtime.Serialization;

namespace Sistran.Company.ExternalParamService.Models
{
    [DataContract]
    public class ListDetailClass
    {
        [DataMember]
        public int AccesoryCode { get; set; }

        [DataMember]
        public string Description { get; set; }       
    }
}
