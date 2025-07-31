using System.Runtime.Serialization;

namespace Sistran.Company.Application.WrapperServices.Models
{
    [DataContract]
    public class QuoteDeductibleProduct
    {
        [DataMember]
        public int DeductId { get; set; }
    }
}
