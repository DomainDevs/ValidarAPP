using Sistran.Core.Application.AuthorizationPoliciesServices.Models.Base;
using Sistran.Core.Application.UniqueUserServices.Models;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AuthorizationPoliciesServices.Models
{
    [DataContract]
    public class AuthorizationAnswer : BaseAuthorizationAnswer
    {
        /// <summary>
        /// Atributo para la propiedad AuthorizationRequestId.
        /// </summary>
        [DataMember]
        public AuthorizationRequest AuthorizationRequest { set; get; }

        /// <summary>
        /// Atributo para la propiedad StatusId.
        /// </summary>
        [DataMember]
        public Enums.TypeStatus Status { set; get; }

        /// <summary>
        /// Atributo para la propiedad StatusId.
        /// </summary>
        [DataMember]
        public string StatusDescription { set; get; }

        /// <summary>
        /// Atributo para la propiedad UserAnswerId.
        /// </summary>
        [DataMember]
        public User UserAnswer { set; get; }

        /// <summary>
        /// Atributo para la propiedad HierarchyAnswerId.
        /// </summary>
        [DataMember]
        public CoHierarchyAssociation HierarchyAnswer { set; get; }

        /// <summary>
        /// Atributo para la propiedad HierarchyAnswerId.
        /// </summary>
        [DataMember]
        public int? RejectionCausesId { set; get; }

        [DataMember]
        public string RejectionCausesDescription { set; get; }
    }
}
