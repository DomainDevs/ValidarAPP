using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class ClaimModifyDTO
    {
        /// <summary>
        /// Identificador de la modificación de la denuncia
        /// </summary>
        [DataMember]
        public int ClaimModifyId { get; set; }

        /// <summary>
        /// Identificador de la denuncia
        /// </summary>
        [DataMember]
        public int ClaimId { get; set; }

        /// <summary>
        /// Fecha de registro
        /// </summary>
        [DataMember]
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// Fecha contable
        /// </summary>
        [DataMember]
        public DateTime AccountingDate { get; set; }

        /// <summary>
        /// Identificador del usuario
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Perfil del usuario que realiza la modificación
        /// </summary>
        [DataMember]
        public int UserProfileId { get; set; }

        /// <summary>
        /// Lista de subsiniestros
        /// </summary>
        [DataMember]
        public List<ClaimCoverageDTO> ClaimCoverages { get; set; }
    }
}
