using Sistran.Core.Application.UnderwritingServices.Models.Base;
using Sistran.Core.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    [DataContract]
    [Serializable]
    public class ParamTax : BaseParamTax
    {
        [DataMember]
        public RateTypeTax RateType { get; set; }

        [DataMember]
        public List<TaxRole> TaxRoles { get; set; }

        [DataMember]
        public List<TaxAttribute> TaxAttributes { get; set; }

        [DataMember]
        public RetentionTax RetentionTax { get; set; }

        [DataMember]
        public BaseConditionTax BaseConditionTax { get; set; }

        [DataMember]
        public AdditionalRateType AdditionalRateType { get; set; }

        [DataMember]
        public List<ParamTaxRate> TaxRates { get; set; }

        [DataMember]
        public List<ParamTaxCategory> TaxCategories { get; set; }
        [DataMember]
        public List<ParamTaxCondition> TaxConditions { get; set; }
    }
}
