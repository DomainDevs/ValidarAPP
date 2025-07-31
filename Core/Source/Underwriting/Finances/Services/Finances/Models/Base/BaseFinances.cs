using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Finances.Models.Base
{
    [DataContract]
    public class BaseFinances: Extension
    {
        /// <summary>
        /// Indica si es declarativo
        /// </summary>
        [DataMember]
        public bool IsDeclarative { get; set; }

        /// <summary>
        /// Fecha de descubrimiento
        /// </summary>
        [DataMember]
        public DateTime DiscoveryDate { get; set; }

        /// <summary>
        /// Identificador de la profesión
        /// </summary>
        [DataMember]
        public int IdOccupation { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
