using System.Runtime.Serialization;

namespace Sistran.Company.ExternalParamService.Models
{
    [DataContract]
    public class AgentModel
    {
        [DataMember]
        public string Surname { get; set; }
        [DataMember]
        public string MotherLastName { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string TradeName { get; set; }
        [DataMember]
        public int IndividualTypeCd { get; set; }
        [DataMember]
        public string Message { get; set; }
    }
}
