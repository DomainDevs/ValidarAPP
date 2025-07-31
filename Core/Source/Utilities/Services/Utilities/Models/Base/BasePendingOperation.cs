using Sistran.Core.Application.Extensions;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UtilitiesServices.Models.Base
{

    [DataContract]
    public class BasePendingOperation : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Id Padre
        /// </summary>
        [DataMember]
        public int ParentId { get; set; }

        /// <summary>
        /// Id Usuario
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Fecha de creacion
        /// </summary>
        [DataMember]
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Fecha de modificacion
        /// </summary>
        [DataMember]
        public DateTime ModificationDate { get; set; }

        /// <summary>
        /// Nombre de usuario
        /// </summary>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        /// JSON
        /// </summary>
        [DataMember]
        public string Operation { get; set; }

        /// <summary>
        /// Informacion adicional
        /// </summary>
        [DataMember]
        public string AdditionalInformation { get; set; }

        /// <summary>
        /// Operacion
        /// </summary>
        [DataMember]
        public string OperationName { get; set; }

        /// <summary>
        /// Operacion
        /// </summary>
        [DataMember]
        public bool IsMassive { get; set; }

    }
}
