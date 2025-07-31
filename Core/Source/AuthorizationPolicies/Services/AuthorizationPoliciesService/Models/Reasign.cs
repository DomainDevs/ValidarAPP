using Sistran.Core.Application.AuthorizationPoliciesServices.Models.Base;
using Sistran.Core.Application.UniqueUserServices.Models;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AuthorizationPoliciesServices.Models
{
    [DataContract]
    public class Reasign : BaseReasign
    {
        /// <summary>
        /// Atributo para la propiedad AuthorizationAnswerId.
        /// </summary>
        [DataMember]
        public AuthorizationAnswer AuthorizationAnswer { set; get; }

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
        /// Atributo para la propiedad UserReasignId.
        /// </summary>
        [DataMember]
        public User UserReasign { set; get; }

        /// <summary>
        /// Atributo para la propiedad HierarchyReasignId.
        /// </summary>
        [DataMember]
        public CoHierarchyAssociation HierarchyReasign { set; get; }

        
    }
}
