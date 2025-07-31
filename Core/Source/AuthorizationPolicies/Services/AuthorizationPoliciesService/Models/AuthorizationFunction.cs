using Sistran.Core.Application.AuthorizationPoliciesServices.Models.Base;
using Sistran.Core.Application.RulesScriptsServices.Models;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AuthorizationPoliciesServices.Models
{
    [DataContract]
    public class AuthorizationFunction : BaseAuthorizationFunction
    {  
        /// <summary>
        /// Atributo para la propiedad _packageId.
        /// </summary>
        [DataMember]
        public Package Package { set; get; }
       
    }
}
