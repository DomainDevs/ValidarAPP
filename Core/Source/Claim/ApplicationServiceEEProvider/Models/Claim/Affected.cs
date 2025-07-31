using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim
{
    /// <summary>
    /// Afectado
    /// </summary>
    [DataContract]
    public class Affected
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Nombre Completo
        /// </summary>
        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public int DocumentTypeId { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }
    }
}
