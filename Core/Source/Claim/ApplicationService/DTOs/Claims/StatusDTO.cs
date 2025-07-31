using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class StatusDTO
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripcion 
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Estado (Habilitado/Deshabilitado) de ClaimStatus
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        [DataMember]
        public EstimationTypeDTO EstimationType { get; set; }

        /// <summary>
        /// Codigo interno
        /// </summary>
        [DataMember]
        public InternalStatusDTO InternalStatus { get; set; }
    }
}
