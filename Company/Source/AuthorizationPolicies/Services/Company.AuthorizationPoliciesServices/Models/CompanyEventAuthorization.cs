using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.AuthorizationPoliciesServices.Models
{
    [DataContract]
    public class CompanyEventAuthorization
    {
        [DataMember]
        public int AuthorizationId { get; set; }
        [DataMember]
        public int GroupEvendId { get; set; }
        [DataMember]
        public int EventId { get; set; }
        [DataMember]
        public int AccessId { get; set; }
        [DataMember]
        public string UrlAccess { get; set; }
        [DataMember]
        public int HierarchyCode { get; set; }
        [DataMember]
        public int AuthoUserId { get; set; }
        [DataMember]
        public string Operation1Id { get; set; }
        [DataMember]
        public bool AuthorizedInd { get; set; }
        [DataMember]
        public bool RejectInd { get; set; }
        [DataMember]
        public int? RejectId { get; set; }
        [DataMember]
        public DateTime EventDate { get; set; }
        [DataMember]
        public DateTime? AuthorizationDate { get; set; }
        [DataMember]
        public string DescriptionError { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string AuthorizationDescription { get; set; }
        [DataMember]
        public string EntityDescriptionValues { get; set; }
        [DataMember]
        public string AuthorizationReasonCode { get; set; }
        [DataMember]
        public CompanyWorkFlowUser User { get; set; }
        [DataMember]
        public CompanyWorkFlowEndorsement Endorsement { get; set; }
        [DataMember]
        public CompanyWorkFlowPolicy Policy { get; set; }

        [DataMember]
        public int EventWorkFlowModule { get; set; }

        [DataMember]
        public int EventWorkFlowSubModule { get; set; }
    }
}
