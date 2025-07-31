using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UtilitiesServices.Models.Base
{
    [DataContract]
    public class BaseTemplate : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int TemplateId { get; set; }

        /// <summary>
        /// PropertyName
        /// </summary>
        [DataMember]
        public string PropertyName { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Es Obligatoria?
        /// </summary>
        [DataMember]
        public bool IsMandatory { get; set; }

        /// <summary>
        /// Esta Habilitado?
        /// </summary>
        [DataMember]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Esta Habilitado?
        /// </summary>
        [DataMember]
        public bool IsPrincipal { get; set; }

        [DataMember]
        public int Order { get; set; }

        [DataMember]
        public bool HasError { get; set; }

        [DataMember]
        public string ErrorDescription { get; set; }

    }
}
