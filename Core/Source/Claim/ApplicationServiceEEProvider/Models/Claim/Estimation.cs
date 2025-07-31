using Sistran.Core.Application.CommonService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim
{
    [DataContract]
    public class Estimation
    {
        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public decimal DeductibleAmount { get; set; }

        [DataMember]
        public decimal AmountAccumulate { get; set; }

        [DataMember]
        public decimal ExchangeRate { get; set; }

        [DataMember]
        public decimal CoverageInsuredAmountEquivalent { get; set; }

        [DataMember]
        public int Version { get; set; }

        [DataMember]
        public DateTime CreationDate { get; set; }

        [DataMember]
        public decimal? PaymentAmount { get; set; }

        [DataMember]
        public EstimationType Type { get; set; }

        [DataMember]
        public EstimationTypeStatus EstimationTypeStatus { get; set; }

        [DataMember]
        public Reason Reason { get; set; }

        [DataMember]
        public Currency Currency { get; set; }

        [DataMember]
        public decimal MinimumSalariesNumber { get; set; }

        [DataMember]
        public bool IsMinimumSalary { get; set; }

        [DataMember]
        public decimal? MinimumSalaryValue { get; set; }

    }
}
