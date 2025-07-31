using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniqueUserServices.Models.Base
{
    [DataContract]
    public class BaseNotificationType : Extension
    {
        /// <summary>
        /// Obtiene o establece el Atributo para la propiedad Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el Atributo para la propiedad Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece el Atributo para la propiedad Title
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Obtiene o establece el Atributo para la propiedad Url
        /// </summary>
        [DataMember]
        public string Url { get; set; }

        /// <summary>
        /// Obtiene o establece el Atributo para la propiedad AccessId
        /// </summary>
        [DataMember]
        public int? AccessId { get; set; }
    }
}
