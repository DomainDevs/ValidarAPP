using Sistran.Company.Application.Utilities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingParamApplicationService.DTOs
{
    [DataContract]
    public class TaxRateDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int IdTax { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public TaxConditionDTO TaxCondition { get; set; }

        [DataMember]
        public TaxCategoryDTO TaxCategory { get; set; }

        [DataMember]
        public TaxStateDTO TaxState { get; set; }

        [DataMember]
        public BranchDTO Branch { get; set; }

        [DataMember]
        public EconomicActivityDTO EconomicActivity { get; set; }

        [DataMember]
        public LineBusinnessDTO LineBusiness { get; set; }

        [DataMember]
        public TaxPeriodRateDTO TaxPeriodRate { get; set; }

        [DataMember]
        public ErrorDTO errorDTO { get; set; }
        [DataMember]
        public CoverageDTO Coverage { get; set; }
    }
}
