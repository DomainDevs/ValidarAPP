using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    [DataContract]
    public class ErrorBase
    {
        [DataMember]
        public bool StateData { get; set; }

        [DataMember]
        public string Error { get; set; }
    }
}
