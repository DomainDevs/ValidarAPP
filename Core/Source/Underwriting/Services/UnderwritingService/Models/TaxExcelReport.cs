
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    [DataContract]
    public class TaxExcelReport
    {
        [DataMember]
        public int TaxCode { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string SmallDescription { get; set; }

        [DataMember]
        public string CurrentFrom { get; set; }

        [DataMember]
        public string RateType { get; set; }

        [DataMember]
        public string AditionalRateType { get; set; }

        [DataMember]
        public string IsSurplus { get; set; }

        [DataMember]
        public string IsAditionalSurplus { get; set; }

        [DataMember]
        public string Enabled { get; set; }

        [DataMember]
        public string IsEarned { get; set; }

        [DataMember]
        public string IsRetention { get; set; }

        [DataMember]
        public string Retention { get; set; }

        [DataMember]
        public string BaseContitionTax { get; set; }

        [DataMember]
        public string TaxAttributes { get; set; }

        [DataMember]
        public string TaxRoles { get; set; }
    }
}
