using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.AuthorizationPoliciesServices.Models
{
    [DataContract]
    public class CompanySarlaftEventAuthorization
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
        public DateTime EventDate { get; set; }

        [DataMember]
        public string Detail { get; set; }

        [DataMember]
        public string RequestDetail { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]

        public string Assets { get; set; }

        [DataMember]
        public int? AuthorizeReasonId { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public string FormNumber { get; set; }

        [DataMember]
        public int Year { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public string User { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int TypeId { get; set; }

        [DataMember]
        public string DocumentType { get; set; }

        [DataMember]
        public int AuthoUserId { get; set; }

        [DataMember]
        public bool IsRejected { get; set; }

        [DataMember]
        public bool IsAuthorized { get; set; }
    }
}
