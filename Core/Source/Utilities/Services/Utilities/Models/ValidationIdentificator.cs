using System.Runtime.Serialization;

namespace Sistran.Core.Services.UtilitiesServices.Models
{
    [DataContract]
    public class ValidationIdentificator : Validation
    {
        [DataMember]
        public int ParameterValue1 { get; set; }
        [DataMember]
        public int ParameterValue2 { get; set; }
        [DataMember]
        public int ParameterValue3 { get; set; }
        [DataMember]
        public int ParameterValue4 { get; set; }
        [DataMember]
        public int ParameterValue5 { get; set; }
        [DataMember]
        public int ParameterValue6{ get; set; }
    }
}