using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.QuotationServices.Models
{
    [DataContract]
    public class CompanyQuotationVehicleSearch
    {
        [DataMember]
        public int BranchId { get; set; }
        [DataMember]
        public string Branch { get; set; }
        [DataMember]
        public int PrefixId { get; set; }
        [DataMember]
        public string Prefix { get; set; }
        [DataMember]
        public int? QuotationId { get; set; }
        [DataMember]
        public DateTime DateQuotation { get; set; }
        [DataMember]
        public int? ProductId { get; set; }
        [DataMember]
        public string Product { get; set; }
        [DataMember]
        public decimal? DeclaredAmount { get; set; }
        [DataMember]
        public decimal PremiumAmount { get; set; }
        [DataMember]
        public bool SentEmail { get; set; }
    }
}
