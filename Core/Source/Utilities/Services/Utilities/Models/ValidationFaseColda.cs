using System.Runtime.Serialization;

namespace Sistran.Core.Services.UtilitiesServices.Models
{
    [DataContract]
    public class ValidationFaseColda : Validation
    {
        [DataMember]
        public string CodeFaseColda { get; set; }

        [DataMember]
        public string Model { get; set; }
    }
}
