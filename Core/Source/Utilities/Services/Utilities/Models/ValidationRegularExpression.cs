using System.Runtime.Serialization;

namespace Sistran.Core.Services.UtilitiesServices.Models
{
    [DataContract]
    public class ValidationRegularExpression : Validation
    {
        [DataMember]
        public string ParameterValue { get; set; }
    }
}
