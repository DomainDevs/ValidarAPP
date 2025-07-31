using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.AuthorizationPoliciesServices.Models.Base
{
    [DataContract]
    public class BaseAuthorizationFunction : Extension
    {
        /// <summary>
        /// Atributo para la propiedad functionId.
        /// </summary>
        [DataMember]
        public int FunctionId { set; get; }

        /// <summary>
        /// Atributo para la propiedad Type.
        /// </summary>

        [DataMember]
        public string Type { set; get; }

        /// <summary>
        /// Atributo para la propiedad Method.
        /// </summary>

        [DataMember]
        public string Method { set; get; }
    }
}
