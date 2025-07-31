using Sistran.Core.Application.CommonService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Claims
{
    [DataContract]
    public class Estimation
    {
        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public decimal DeductibleAmount { get; set; }

        [DataMember]
        public decimal AmountAccumulated { get; set; }

        [DataMember]
        public int Version { get; set; }

        [DataMember]
        public DateTime CreationDate { get; set; }

        [DataMember]
        public decimal? PaymentAmount { get; set; }

        [DataMember]
        public EstimationType Type { get; set; }

        [DataMember]
        public Reason Reason { get; set; }

        [DataMember]
        public Currency Currency { get; set; }
    }
}
