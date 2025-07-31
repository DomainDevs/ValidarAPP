using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BaseSarlaftExoneration : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// identificador usuario
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Es Email principal?
        /// </summary>
        [DataMember]
        public bool IsExonerated { get; set; }

        /// <summary>
        /// fecha de creacion
        /// </summary>
        [DataMember]
        public DateTime? EnteredDate { get; set; }

        /// <summary>
        /// fecha de creacion
        /// </summary>
        [DataMember]
        public int RolId { get; set; }

        public BaseSarlaftExoneration() { }
    }
}
