using Sistran.Core.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniqueUserServices.Models.Base
{
    [DataContract]
    public class BaseNotificationUser : Extension
    {
        /// <summary>
        /// Obtiene o establece el Atributo para la propiedad Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el Atributo para la propiedad Message
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// Obtiene o establece el Atributo para la propiedad UserId
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si Enabled
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }

        /// <summary>
        /// Obtiene o establece el Atributo para la propiedad Parameters
        /// </summary>
        [DataMember]
        public Dictionary<string, object> Parameters { get; set; }

        /// <summary>
        /// Obtiene o establece el Atributo para la propiedad CreateDate
        /// </summary>
        [DataMember]
        public DateTime CreateDate { get; set; }
    }
}
