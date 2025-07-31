using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    [DataContract]
    public class GoodExperienceYear
    {
        [DataMember]
        public int IdCardTypeCode { get; set; }
        [DataMember]
        public string IdCardNo { get; set; }
        [DataMember]
        public int GoodExperienceNum { get; set; }
        [DataMember]
        public string GoodExpNumRate { get; set; }
        [DataMember]
        public int GoodExpNumPrinter { get; set; }
        [DataMember]
        public DateTime? MaximumVigency { get; set; }
        [DataMember]
        public string IsEffectiveDate { get; set; }
    }
}
