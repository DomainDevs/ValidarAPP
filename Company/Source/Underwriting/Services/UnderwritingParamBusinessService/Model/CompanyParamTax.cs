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
    public class CompanyParamTax : BaseParamTax
    {
        [DataMember]
        public CompanyTaxRate RateType { get; set; }

        [DataMember]
        public List<CompanyTaxRole> TaxRoles { get; set; }

        [DataMember]
        public List<CompanyTaxAttribute> TaxAttributes { get; set; }

        [DataMember]
        public CompanyTax RetentionTax { get; set; }

        [DataMember]
        public CompanyTax BaseConditionTax { get; set; }

        [DataMember]
        public CompanyTaxRate AdditionalRateType { get; set; }

        [DataMember]
        public List<CompanyParamTaxRate> TaxRates { get; set; }

        [DataMember]
        public List<CompanyParamTaxCategory> TaxCategories { get; set; }

        [DataMember]
        public List<CompanyParamTaxCondition> TaxConditions { get; set; }
    }
}
