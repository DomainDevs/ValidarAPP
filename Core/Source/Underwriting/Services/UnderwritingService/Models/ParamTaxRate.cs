using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.TaxServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    [DataContract]
    [Serializable]
    public class ParamTaxRate : BaseParamTaxRate
    {
        [DataMember]
        public Branch Branch { get; set; }

        [DataMember]
        public LineBusiness LineBusiness { get; set; }

        [DataMember]
        public TaxCategory TaxCategory { get; set; }

        [DataMember]
        public TaxCondition TaxCondition { get; set; }

        [DataMember]
        public EconomicActivity EconomicActivity { get; set; }

        [DataMember]
        public TaxState TaxState { get; set; }

        [DataMember]
        public TaxPeriodRate TaxPeriodRate { get; set; }
        [DataMember]
        public Coverage Coverage { get; set; }
    }
}
