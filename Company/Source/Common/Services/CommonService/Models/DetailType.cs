using System.Runtime.Serialization;

namespace Sistran.Company.Application.CommonServices.Models
{
    [DataContract]
    public class DetailType
    {
        [DataMember]
        public int DetailTypeCode { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string SmallDescription { get; set; }

        [DataMember]
        public int DetailClassCode { get; set; }
    }
}
