using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BaseAgency : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

       
        /// <summary>
        /// Código
        /// </summary>
        [DataMember]
        public int Code { get; set; }

        /// <summary>
        /// Nombre
        /// </summary>
        [DataMember]
        public string FullName { get; set; }

        /// <summary>
        /// Fecha de Baja
        /// </summary>
        [DataMember]
        public DateTime? DateDeclined { get; set; }

        /// <summary>
        /// Anotaciones
        /// </summary>
        [DataMember]
        public string Annotations { get; set; }

        /// <summary>
        /// Participación
        /// </summary>
        [DataMember]
        public decimal Participation { get; set; }

        /// <summary>
        /// Es Principal?
        /// </summary>
        [DataMember]
        public bool IsPrincipal { get; set; }
    }
}
