using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.UnderwritingServices.Models.Base;

namespace Sistran.Company.Application.UnderwritingParamBusinessService.Model
{
    [DataContract]
    public class CompanyParamTaxRate : BaseParamTaxRate

    {
        [DataMember]
        public CompanyParamBranch Branch { get; set; }

        [DataMember]
        public CompanyParamLineBusiness LineBusiness { get; set; }

        [DataMember]
        public CompanyParamTaxCategory TaxCategory { get; set; }

        [DataMember]
        public CompanyParamTaxCondition TaxCondition { get; set; }

        [DataMember]
        public CompanyParamEconomicActivity EconomicActivity { get; set; }

        [DataMember]
        public CompanyParamTaxState TaxState { get; set; }

        [DataMember]
        public CompanyParamTaxPeriodRate TaxPeriodRate { get; set; }
        [DataMember]
        public CompanyParamCoverage Coverage { get; set; }
    }
}