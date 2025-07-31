
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Claims.PaymentRequest
{
    [DataContract]
    public class ClaimPersonType
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string SmallDescription { get; set; }
        [DataMember]
        public bool IsEnabled { get; set; }

        [DataMember]
        public bool IsBillEnabled { get; set; }

        [DataMember]
        public bool IsPaymentOrderEnabled { get; set; }

        [DataMember]
        public bool IsPreaplicationEnabled { get; set; }

        [DataMember]
        public bool ChargeRequestEnable { get; set; }

        [DataMember]
        public bool PaymentRequestEnable { get; set; }
    }
}