
using System.Runtime.Serialization;

namespace Sistran.Company.Application.WrapperServices.Models
{
    [DataContract]
    public class QuoteVehicleResponse
    {
        [DataMember]
        public int TemporalId { get; set; }
        [DataMember]
        public int QuotationId { get; set; }
        [DataMember]
        public decimal Premium { get; set; }
        [DataMember]
        public decimal Taxes { get; set; }
        [DataMember]
        public decimal Expenses { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
    }
}