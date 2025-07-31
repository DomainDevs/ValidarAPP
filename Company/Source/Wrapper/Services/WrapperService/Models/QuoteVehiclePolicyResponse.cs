using System.Runtime.Serialization;
namespace Sistran.Company.Application.WrapperServices.Models
{
    [DataContract]
    public class QuoteVehiclePolicyResponse
    {        

        [DataMember]
        public string Message { get; set; }
    }
}