using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UtilitiesServices.Models.Base
{
    [DataContract]
    public class BaseAsynchronousProcess : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { set; get; }

        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember]
        public string Description { set; get; }

        /// <summary>
        /// Hora Inicial
        /// </summary>
        [DataMember]
        public DateTime BeginDate { set; get; }

        /// <summary>
        /// Hora Final
        /// </summary>
        [DataMember]
        public DateTime? EndDate { set; get; }

        /// <summary>
        /// Id Usuario
        /// </summary>
        [DataMember]
        public int UserId { set; get; }

        /// <summary>
        /// Tiene Error?
        /// </summary>
        [DataMember]
        public bool HasError { set; get; }

        /// <summary>
        /// Descripción Error
        /// </summary>
        [DataMember]
        public string ErrorDescription { set; get; }

        /// <summary>
        /// Esta Activo?
        /// </summary>
        [DataMember]
        public bool Active { get; set; }

        /// <summary>
        /// Id Estado
        /// </summary>
        [DataMember]
        public int StatusId { set; get; }
    }
}
