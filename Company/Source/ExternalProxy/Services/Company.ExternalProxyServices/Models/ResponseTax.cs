using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    [DataContract]
    public class ResponseTax
    {
        [DataMember]
        public int CountryId { get; set; }
        [DataMember]
        public int ExtentPercentage { get; set; }
        [DataMember]
        public string ResolutionNumber { get; set; }
        [DataMember]
        public int StateCode { get; set; }
        [DataMember]
        public string StateCodeDescription { get; set; }
        [DataMember]
        public int TaxId { get; set; }
        [DataMember]
        public string TaxDescription { get; set; }
        [DataMember]
        public int TaxCategoryId { get; set; }
        [DataMember]
        public string TaxCategoryDescription { get; set; }
        [DataMember]
        public int TaxCondition { get; set; }
        [DataMember]
        public string TaxConditionDescription { get; set; }
        [DataMember]
        public bool TotalRetention { get; set; }
        [DataMember]
        public int RoleId { get; set; }
        [DataMember]
        public int TaxRateId { get; set; }
    }
}
