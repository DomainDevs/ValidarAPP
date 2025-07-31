using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    [DataContract]
    public class CompanyChangeConsolidation
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
        public virtual CompanyText Text { get; set; }
        [DataMember]
        public List<PoliciesAut> InfringementPolicies { get; set; }
    }
}
