using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.UndewritingIntegrationServices.DTOs
{
    [DataContract]
    public class PayerPaymentComponentDTO
    {

        [DataMember]
        public int PayerPaymentComponentId { get; set; }

        [DataMember]
        public int PayerPaymentId { get; set; }

        [DataMember]
        public int ComponentId { get; set; }

        [DataMember]
        public decimal PaymentPct { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public decimal LocalAmount { get; set; }

        [DataMember]
        public decimal MainAmount { get; set; }

        [DataMember]
        public decimal MainLocalAmount { get; set; }

        [DataMember]
        public string TinyDescription { get; set; }

        [DataMember]
        public decimal ExchangeRate { get; set; }
    }
}
