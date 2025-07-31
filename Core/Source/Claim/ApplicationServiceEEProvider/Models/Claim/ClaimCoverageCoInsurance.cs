using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim
{
    [DataContract]
    public class ClaimCoverageCoInsurance
    {
         [DataMember]
        public int ClaimCoverageId { get; set; }

        [DataMember]
        public int CompanyId { get; set; }

        [DataMember]
        public int EstimationTypeId { get; set; }

        [DataMember]
        public int EstimationTypeStatusId { get; set; }

        [DataMember]
        public int EstimationTypeStatusReasonId { get; set; }

        [DataMember]
        public decimal EstimationAmount { get; set; }

        [DataMember]
        public decimal DeducibleAmount { get; set; }

        [DataMember]
        public int VersionId { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public int CurrencyId { get; set; }

        [DataMember]
        public decimal EstimationAmountAccumulate { get; set; }

        [DataMember]
        public decimal PaticipationCompany { get; set; }

        [DataMember]
        public int CoverageId { get; set; }
    }
}
