using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.UndewritingIntegrationServices.DTOs
{
    [DataContract]
    public class CoverageDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public decimal InsuredAmountTotal { get; set; }

        [DataMember]
        public decimal InsurableAmount { get; set; }

        [DataMember]
        public decimal OcurrencyLimit { get; set; }

        [DataMember]
        public decimal PersonLimit { get; set; }

        [DataMember]
        public decimal AggregateLimit { get; set; }

        [DataMember]
        public int CoverageId { get; set; }

        [DataMember]
        public int SubLineBusinessCode { get; set; }

        [DataMember]
        public int LineBusinessCode { get; set; }

        [DataMember]
        public string SubLineBusinessDescription { get; set; }

        [DataMember]
        public int InsuredObjectId { get; set; }

        [DataMember]
        public string InsuredObjectDescription { get; set; }

        [DataMember]
        public int RiskCoverageId { get; set; }

        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public decimal LimitOccurrenceAmount { get; set; }
    }
}
