using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.TaxServices.DTOs
{

    [DataContract]
    public class IndividualTaxCategoryConditionDTO
    {        
        [DataMember]
        public int TaxId { get; set; }

        [DataMember]
        public string TaxDescription { get; set; }

        [DataMember]
        public int TaxCategoryId { get; set; }

        [DataMember]
        public string TaxCategoryDescription { get; set; }

        [DataMember]
        public int TaxConditionId { get; set; }

        [DataMember]
        public string TaxConditionDescription { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public int? BaseConditionTaxId { get; set; }

        [DataMember]
        public bool IsRetention { get; set; }

        [DataMember]
        public bool Enabled { get; set; }

        [DataMember]
        public decimal Rate { get; set; }

        [DataMember]
        public decimal MinBaseAmount { get; set; }

        [DataMember]
        public DateTime CurrentFrom { get; set; }

        [DataMember]
        public DateTime CurrentTo { get; set; }

        [DataMember]
        public int? BranchId { get; set; }

        [DataMember]
        public string BranchDescription { get; set; }

        [DataMember]
        public int CountryId { get; set; }

        [DataMember]
        public int CoverageId { get; set; }

        [DataMember]
        public int EconomicActivityTaxId { get; set; }

        [DataMember]
        public int StateId { get; set; }

        [DataMember]
        public int LineBusinessId { get; set; }

        [DataMember]
        public int RateTypeId { get; set; }

        [DataMember]
        public string RateTypeDescription { get; set; }

        [DataMember]
        public int? RetentionTaxId { get; set; }
    }
}

