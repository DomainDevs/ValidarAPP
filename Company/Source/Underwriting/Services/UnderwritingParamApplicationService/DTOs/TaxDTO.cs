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
    public class TaxDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string TinyDescription { get; set; }

        [DataMember]
        public DateTime CurrentFrom { get; set; }

        [DataMember]
        public bool IsSurPlus { get; set; }

        [DataMember]
        public bool IsAdditionalSurPlus { get; set; }

        [DataMember]
        public bool Enabled { get; set; }

        [DataMember]
        public bool IsEarned { get; set; }

        [DataMember]
        public bool IsRetention { get; set; }

        [DataMember]
        public RateTypeDTO RateType { get; set; }

        [DataMember]
        public RateTypeDTO AdditionalRateType { get; set; }

        [DataMember]
        public BaseTaxDTO RetentionTax { get; set; }

        [DataMember]
        public BaseTaxDTO BaseConditionTax { get; set; }

        [DataMember]
        public List<TaxRoleDTO> TaxRoles { get; set; }

        [DataMember]
        public List<TaxAttributeDTO> TaxAttributes { get; set; }

        [DataMember]
        public List<TaxRateDTO> TaxRates { get; set; }

        [DataMember]
        public List<TaxCategoryDTO> TaxCategories { get; set; }

        [DataMember]
        public List<TaxConditionDTO> TaxConditions { get; set; }

        [DataMember]
        public ErrorDTO errorDTO { get; set; }
    }
}
