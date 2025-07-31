using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.AuthorizationPolicies.Models
{
    public class AuthorizationRiskListModelView
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
        [Required(ErrorMessage = "Campo es requerido")]
        public string Description { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Campo es requerido")]
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