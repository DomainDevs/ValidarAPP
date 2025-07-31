using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.UndewritingIntegrationServices.DTOs
{
    [DataContract]
    public class PayerPaymentDTO
    {
        [DataMember]
        public int EndorsementId { get; set; }

        [DataMember]
        public int PolicyId { get; set; }

        [DataMember]
        public int PaymentNum { get; set; }

        [DataMember]
        public int PayerId { get; set; }

        [DataMember]
        public DateTime PayExpDate { get; set; }

        [DataMember]
        public decimal PaymentPct { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public DateTime AgtPayExpDate { get; set; }

        [DataMember]
        public int PayerPaymentId { get; set; }

        [DataMember]
        public decimal LocalAmount { get; set; }

        [DataMember]
        public decimal MainAmount { get; set; }

        [DataMember]
        public decimal MainLocalAmount { get; set; }

        [DataMember]
        public int PaymentState { get; set; }


    }
}
