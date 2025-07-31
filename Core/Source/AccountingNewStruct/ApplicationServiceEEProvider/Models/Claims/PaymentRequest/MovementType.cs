
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Claims.PaymentRequest
{
    [DataContract]
    public class MovementType
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public ConceptSource ConceptSource { get; set; }
    }
}