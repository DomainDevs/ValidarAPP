using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Transports.TransportBusinessService.Models
{
    [DataContract]
    public class CompanyEndorsementDetail
    {
        [DataMember]
        public decimal PolicyId { get; set; }
        [DataMember]
        public decimal EndorsementType { get; set; }
        [DataMember]
        public int RiskNum { get; set; }
        [DataMember]
        public int InsuredObjectId { get; set; }
        [DataMember]
        public int Version { get; set; }
        [DataMember]
        public DateTime EndorsementDate { get; set; }
        [DataMember]
        public Nullable<decimal> DeclarationValue { get; set; }
        [DataMember]
        public Nullable<decimal> PremiumAmount { get; set; }
        [DataMember]
        public Nullable<decimal> DeductibleAmmount { get; set; }
        [DataMember]

        public Nullable<decimal> Taxes { get; set; }
        [DataMember]
        public Nullable<decimal> Surchanges { get; set; }
        [DataMember]
        public Nullable<decimal> Expenses { get; set; }
    }
}
