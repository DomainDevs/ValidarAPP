using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class TempRiskCoverage
    {
        [DataMember]
        public int TempRiskCoverageId { get; set; }
        [DataMember]
        public int TempReinsLayerIssuanceCode { get; set; }
        [DataMember]
        public int LineBusinessCode { get; set; }
        [DataMember]
        public int SubLineBusinessCode { get; set; }
        [DataMember]
        public int RiskCode { get; set; }
        [DataMember]
        public int RiskNumber { get; set; }
        [DataMember]
        public int InsuredObjectId { get; set; }
        [DataMember]
        public int CoverageNumber { get; set; }
        [DataMember]
        public bool IsFacultative { get; set; }
        [DataMember]
        public decimal DeclaredAmount { get; set; }
        [DataMember]
        public decimal PremiumAmount { get; set; }
        [DataMember]
        public decimal DeclaredAmtIssue { get; set; }
        [DataMember]
        public decimal PremiumAmtIssue { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public string CumulusAux { get; set; }
        [DataMember]
        public int? InsuredCode { get; set; }
        [DataMember]
        public int? IndividualCode { get; set; }
        [DataMember]
        public int?  CoverageId { get; set; }
        [DataMember]
        public string LongitudeEarthquake { get; set; }
        [DataMember]
        public string LatitudeEarthquake { get; set; }
        [DataMember]
        public int? LineCode { get; set; }
        [DataMember]
        public string  CumulusKey { get; set; }
        [DataMember]
        public DateTime?  CurrentTo { get; set; }
        [DataMember]
        public DateTime? CurrentFrom { get; set; }
        [DataMember]
        public bool IsRetention { get; set; }
    }
}
