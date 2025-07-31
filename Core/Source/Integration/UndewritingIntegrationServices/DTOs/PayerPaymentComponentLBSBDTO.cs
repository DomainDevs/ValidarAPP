using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.UndewritingIntegrationServices.DTOs
{
    [DataContract]
    public class PayerPaymentComponentLBSBDTO
    {

        [DataMember]
        public int PayerPaymentComponentLBSBId { get; set; }

        [DataMember]
        public int PayerPaymentId { get; set; }

        [DataMember]
        public int ComponentId { get; set; }

        [DataMember]
        public int LineBusiness { get; set; }

        [DataMember]
        public int SubLineBusiness { get; set; }

        [DataMember]
        public int CurrencyId { get; set; }

        [DataMember]
        public decimal ExchangeRate { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public decimal LocalAmount { get; set; }

        [DataMember]
        public decimal MainAmount { get; set; }

        [DataMember]
        public decimal MainLocalAmount { get; set; }
        [DataMember]
        public decimal Percentage { get; set; }

    }
}
