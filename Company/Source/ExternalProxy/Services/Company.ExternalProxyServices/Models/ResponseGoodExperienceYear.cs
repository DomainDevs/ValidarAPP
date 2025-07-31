using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    [DataContract]
    public class ResponseGoodExperienceYear
    {
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
        [DataMember]
        public Error Error { get; set; }
        [DataMember]
        public List<ResponsePoliciesHistorical> PoliciesHistorical { get; set; }
        [DataMember]
        public List<ResponseHistoricalSinister> HistoricalSinister { get; set; }
    }
}
