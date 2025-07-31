using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models.Base
{
    [DataContract]
    public class BaseAddress : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Dirección
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Es la direción principal
        /// </summary>
        [DataMember]
        public bool IsMailAddress { get; set; }

        /// <summary>
        /// Usuario que actualiza el registro
        /// </summary>
        [DataMember]
        public string UpdateUser { get; set; }

        /// <summary>
        /// Fecha de actualizacion del registro
        /// </summary>
        [DataMember]
        public string UpdateDate { get; set; }


    }
}
