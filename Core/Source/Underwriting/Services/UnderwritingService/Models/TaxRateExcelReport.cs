
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    [DataContract]
    public class TaxRateExcelReport
    {
        [DataMember]
        public int TaxCode { get; set; }

        [DataMember]
        public int TaxRateCode { get; set; }

        [DataMember]
        public string CurrentFrom { get; set; }

        [DataMember]
        public string TaxRateCondition { get; set; }

        [DataMember]
        public string TaxRateCategory { get; set; }

        [DataMember]
        public string TaxRateLineBusiness { get; set; }

        [DataMember]
        public string TaxRateState { get; set; }

        [DataMember]
        public string TaxRateCountry { get; set; }

        [DataMember]
        public string TaxRateBranch { get; set; }

        [DataMember]
        public string TaxRateRate { get; set; }

        [DataMember]
        public string TaxRateAdditionalRate { get; set; }

        [DataMember]
        public string TaxRateMinBaseAmount { get; set; }

        [DataMember]
        public string TaxRateMinAdditionalBaseAmount { get; set; }

        [DataMember]
        public string TaxRateMinTaxAmount { get; set; }

        [DataMember]
        public string TaxRateMinAdditionalTaxAmount { get; set; }
        [DataMember]
        public string TaxRateBaseTaxInc { get; set; }
    }
}
