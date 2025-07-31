using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim
{
    [DataContract]
    public class PendingOperation
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }
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
        /// JSON
        /// </summary>
        [DataMember]
        public string Operation { get; set; }
    }
}
