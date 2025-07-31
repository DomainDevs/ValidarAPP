using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Company.AuthorizationPoliciesServices.Models
{
    public class CompanyAuthorizationRiskList
    {
        [DataMember]
        public int AuthorizationId { get; set; }

        [DataMember]
        public int EventGroupId { get; set; }

        [DataMember]
        public int EventId { get; set; }

        [DataMember]
        public int? RejectId { get; set; }

        [DataMember]
        public DateTime? EventDate { get; set; }

        [DataMember]
        public string Detail { get; set; }

        [DataMember]
        public string RequestDetail { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int? AuthorizeReasonId { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public string DocumentType { get; set; }

        [DataMember]
        public bool IsRejected { get; set; }

        [DataMember]
        public bool IsAuthorized { get; set; }
    }
}
