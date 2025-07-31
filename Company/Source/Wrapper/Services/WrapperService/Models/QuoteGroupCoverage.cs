using Sistran.Core.Application.CommonService.Enums;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UNMO = Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.WrapperServices.Models
{
    [DataContract]
    public class QuoteGroupCoverage
    {
        [DataMember]
        public QuoteCoverage Coverage { get; set; }
        [DataMember]
        public List<QuoteCoverage> Coverages { get; set; }
        [DataMember]
        public CoveredRiskType? CoveredRiskType { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public List<UNMO.InsuredObject> InsuredObjects { get; set; }
        [DataMember]
        public bool IsMandatory { get; set; }
        [DataMember]
        public bool IsSelected { get; set; }
        [DataMember]
        public int MainCoverageId { get; set; }
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public int? PosRuleSetId { get; set; }
        [DataMember]
        public int? RuleSetId { get; set; }
        [DataMember]
        public int? ScriptId { get; set; }
        [DataMember]
        public decimal SublimitPercentage { get; set; }
    }
}
