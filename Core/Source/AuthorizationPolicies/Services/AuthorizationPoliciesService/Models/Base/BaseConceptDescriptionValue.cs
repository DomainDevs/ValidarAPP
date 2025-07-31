using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.AuthorizationPoliciesServices.Models.Base
{
    [DataContract]
    public class BaseConceptDescriptionValue : Extension
    {
        /// <summary>
        /// Atributo para la propiedad TableName.
        /// </summary>
        [DataMember]
        public string TableName { set; get; }
        /// <summary>
        /// Atributo para la propiedad Fields.
        /// </summary>
        [DataMember]
        public string Fields { set; get; }
        /// <summary>
        /// Atributo para la propiedad Filter.
        /// </summary>
        [DataMember]
        public string Filter { set; get; }
    }
}
