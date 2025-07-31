
using System.Runtime.Serialization;

namespace Sistran.Company.Application.WrapperServices.Models
{
    [DataContract]
    public class QuotePolicy
    {
        [DataMember]
        public QuoteProduct QuoteProduct { get; set; }
    }
}
