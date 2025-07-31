using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.DTOs
{
    [DataContract]
    public class EndorsementDTO
    {
        /// <summary>
        /// Obtiene o establece Id del endos0
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece Descripcion estado Endoso
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece Estado del registro de parametrizacion
        /// </summary>
        [DataMember]
        public int State { get; set; }
    }
}
