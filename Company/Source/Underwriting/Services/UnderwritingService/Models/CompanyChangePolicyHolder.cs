using Sistran.Core.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    using Core.Application.AuthorizationPoliciesServices.Models;

    [DataContract]
    public class CompanyChangePolicyHolder
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public DateTime CurrentTo { get; set; }
        [DataMember]
        public DateTime CurrentFrom { get; set; }
        [DataMember]
        public CompanyEndorsement Endorsement { get; set; }
        [DataMember]
        public CompanyIssuanceInsured companyContract { get; set; }
        [DataMember]
        public Holder holder { get; set; }
        [DataMember]
        public virtual CompanyText Text { get; set; }
        [DataMember]
        public List<PoliciesAut> InfringementPolicies { get; set; }
    }
}
