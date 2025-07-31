using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.AuthorizationPolicies.AuthorizationPolicyApplicationService.DTOs
{
    [DataContract]
    public class EventAuthorizationDTO
    {
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
        public int UserId { get; set; }
        [DataMember]
        public string UserAccountName { get; set; }
        [DataMember]
        public int AuthoUserId { get; set; }
        [DataMember]
        public string Operation1Id { get; set; }
        [DataMember]
        public string Operation2Id { get; set; }
        [DataMember]
        public int EndorsementNumber { get; set; }
        [DataMember]
        public bool AuthorizedInd { get; set; }
        [DataMember]
        public bool RejectInd { get; set; }
        [DataMember]
        public DateTime EventDate { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int PolicyId { get; set; }
        [DataMember]
        public int PolicyNumber { get; set; }
        [DataMember]
        public int BranchId { get; set; }
        [DataMember]
        public string BranchDescription { get; set; }
        [DataMember]
        public int PrefixId { get; set; }
        [DataMember]
        public string PrefixDescription{ get; set; }
    }
}
