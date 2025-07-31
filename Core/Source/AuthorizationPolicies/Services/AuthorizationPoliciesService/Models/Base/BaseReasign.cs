using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.AuthorizationPoliciesServices.Models.Base
{
    [DataContract]
    public class BaseReasign : Extension
    {
        /// <summary>
        /// Atributo para la propiedad reasignId.
        /// </summary>
        [DataMember]
        public int ReasignId { set; get; }
        /// <summary>
        /// Atributo para la propiedad DescriptionReasign.
        /// </summary>
        [DataMember]
        public string DescriptionReasign { set; get; }

        /// <summary>
        /// Atributo para la propiedad DateReasign.
        /// </summary>
        [DataMember]
        public DateTime DateReasign { set; get; }

    }
}
