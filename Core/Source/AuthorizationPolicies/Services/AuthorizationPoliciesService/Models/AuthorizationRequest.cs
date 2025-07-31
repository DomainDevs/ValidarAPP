using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models.Base;
using Sistran.Core.Application.UniqueUserServices.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AuthorizationPoliciesServices.Models
{
    [DataContract]
    public class AuthorizationRequest : BaseAuthorizationRequest
    {
        /// <summary>
        /// Atributo para la propiedad PoliciesId.
        /// </summary>
        [DataMember]
        public PoliciesAut Policies { set; get; }

        /// <summary>
        /// Atributo para la propiedad StatusId.
        /// </summary>
        [DataMember]
        public Enums.TypeStatus Status { set; get; }

        /// <summary>
        /// Atributo para la propiedad UserRequestId.
        /// </summary>
        [DataMember]
        public User UserRequest { set; get; }

        /// <summary>
        /// Atributo para la propiedad HierarchyRequestId.
        /// </summary>
        [DataMember]
        public CoHierarchyAssociation HierarchyRequest { set; get; }

        /// <summary>
        /// Atributo para la propiedad AuthorizationAnswers.
        /// </summary>
        [DataMember]
        public List<AuthorizationAnswer> AuthorizationAnswers { set; get; }

        /// <summary>
        /// Atributo para la propiedad AuthorizationAnswers.
        /// </summary>
        [DataMember]
        public List<User> NotificationUsers { set; get; }

        /// <summary>
        /// Atributo para la propiedad FunctionType.
        /// </summary>
        [DataMember]
        public TypeFunction FunctionType { set; get; }
    }
}
