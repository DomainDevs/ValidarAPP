using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.AuthorizationPoliciesServices.Models.Base
{
    [DataContract]
    public class BaseAuthorizationRequest : Extension
    {
        /// <summary>
        /// Atributo para la propiedad AuthorizationRequestId.
        /// </summary>
        [DataMember]
        public int AuthorizationRequestId { set; get; }

        /// <summary>
        /// Atributo para la propiedad Key.
        /// </summary>
        [DataMember]
        public string Key { set; get; }

        /// <summary>
        /// Atributo para la propiedad Key2.
        /// </summary>
        [DataMember]
        public string Key2 { set; get; }

        /// <summary>
        /// Atributo para la propiedad Description.
        /// </summary>
        [DataMember]
        public string Description { set; get; }

        /// <summary>
        /// Atributo para la propiedad NumberAut.
        /// </summary>
        [DataMember]
        public int NumberAut { set; get; }

        /// <summary>
        /// Atributo para la propiedad DescriptionRequest.
        /// </summary>
        [DataMember]
        public string DescriptionRequest { set; get; }

        /// <summary>
        /// Atributo para la propiedad DateRequest.
        /// </summary>
        [DataMember]
        public DateTime DateRequest { set; get; }


    }
}
