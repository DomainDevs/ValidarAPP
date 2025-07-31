using Sistran.Core.Application.UniquePersonService.V1.Models;
using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    /// <summary>
    /// Exoneraciones Sarlaft
    /// </summary>
    [DataContract]
    public class CompanySarlaftExoneration
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
        /// Tipo exoneracion
        /// </summary>
        [DataMember]
        public CompanyExonerationType ExonerationType { get; set; }


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
    }
}
