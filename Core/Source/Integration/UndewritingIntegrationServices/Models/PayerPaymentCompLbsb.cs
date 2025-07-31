using System.Runtime.Serialization;

namespace Sistran.Core.Integration.UndewritingIntegrationServices.Models
{
    [DataContract]
    public class PayerPaymentCompLbsb
    {
        [DataMember]
        public int PayerPaymentCompLbsbId { get; set; }

        [DataMember]
        public int PayerPaymentId { get; set; }

        [DataMember]
        public int ComponentCode { get; set; }

        [DataMember]
        public int LineBusinessCode { get; set; }

        [DataMember]
        public int SubLineBusinessCode { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public decimal LocalAmount { get; set; }

        [DataMember]
        public decimal MainAmount { get; set; }

        [DataMember]
        public decimal MainLocalAmount { get; set; }
       
    }
}
