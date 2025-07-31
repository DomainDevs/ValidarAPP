using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    [DataContract]
    public class BaseBillingQuota
    {
        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public DateTime Date { get; set; }
    }
}