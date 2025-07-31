using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim
{
    /// <summary>
    /// Modificaciones de una denuncia
    /// </summary>
    [DataContract]
    public class ClaimModify
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

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
        /// Descripcion del usuario
        /// </summary>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        /// Fecha contable
        /// </summary>
        [DataMember]
        public DateTime AccountingDate { get; set; }

        /// <summary>
        /// Fecha de Registro
        /// </summary>
        [DataMember]
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// Coberturas
        /// </summary>
        [DataMember]
        public List<ClaimCoverage> Coverages { get; set; }
    }
}
