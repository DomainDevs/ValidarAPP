using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingParamBusinessService.Model
{
    public class CompanyParamMinPremiunRelation
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public CompanyParamEndorsementType EndorsementType { get; set; }
        [DataMember]
        public CompanyParamCurrency Currency { get; set; }
        [DataMember]
        public CompanyParamBranch Branch { get; set; }
        [DataMember]
        public CompanyParamPrefix Prefix { get; set; }
        [DataMember]
        public CompanyParamProduct Product { get; set; }
        [DataMember]
        public CompanyParamGroupCoverage GroupCoverage { get; set; }
        [DataMember]
        public CompanyParamMinPremiunRange MinPremiunRange { get; set; }
        [DataMember]
        public decimal? SubMinPremiun { get; set; }
        [DataMember]
        public decimal? RiskMinPremiun { get; set; }
    }
}
