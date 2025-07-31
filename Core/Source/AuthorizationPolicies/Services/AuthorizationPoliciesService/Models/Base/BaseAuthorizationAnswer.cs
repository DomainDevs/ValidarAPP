using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.AuthorizationPoliciesServices.Models.Base
{
    [DataContract]
    public class BaseAuthorizationAnswer : Extension
    {

        /// <summary>
        /// Atributo para la propiedad AuthorizationAnswerId.
        /// </summary>
        [DataMember]
        public int AuthorizationAnswerId { set; get; }

        /// <summary>
        /// Atributo para la propiedad DescriptionAnswer.
        /// </summary>
        [DataMember]
        public string DescriptionAnswer { set; get; }

        /// <summary>
        /// Atributo para la propiedad Required.
        /// </summary>
        [DataMember]
        public bool Required { set; get; }

        /// <summary>
        /// Atributo para la propiedad Enabled.
        /// </summary>
        [DataMember]
        public bool Enabled { set; get; }

        /// <summary>
        /// Atributo para la propiedad DateAnswer.
        /// </summary>
        [DataMember]
        public DateTime? DateAnswer { set; get; }
    }
}
