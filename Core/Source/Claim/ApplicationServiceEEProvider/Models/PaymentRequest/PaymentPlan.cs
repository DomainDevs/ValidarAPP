
using Sistran.Core.Application.CommonService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.PaymentRequest
{
    public class PaymentPlan
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public decimal Tax { get; set; }
        [DataMember]
        public Currency Currency { get; set; }

        [DataMember]
        public PaymentClass PaymentClass { get; set; }

        [DataMember]
        public List<PaymentQuota> PaymentQuotas { get; set; }
    }
}
