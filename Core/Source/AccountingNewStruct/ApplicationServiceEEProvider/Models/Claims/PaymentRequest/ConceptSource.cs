using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Claims.PaymentRequest
{
    [DataContract]
    public class ConceptSource
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool isChargeRequest { get; set; }
    }
}