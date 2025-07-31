using System.Runtime.Serialization;
using System.ServiceModel;

namespace Sistran.Company.Application.WrapperServices.Models
{
    [XmlSerializerFormat]   
    [DataContract]
    public class QuoteVehiclePolicyRequest
    {
        [DataMember]
        public int TemporalId { get; set; }        
    }
}