using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim
{
    [DataContract]
    public class ClaimSurety
    {
        /// <summary>
        /// Número de Contrato
        /// </summary>
        [DataMember]
        public string BidNumber { get; set; }

        /// <summary>
        /// Número de Tribunal
        /// </summary>
        [DataMember]
        public string CourtNumber { get; set; }

        /// <summary>
        /// Reclamación
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Reclamación
        /// </summary>
        [DataMember]
        public Claim Claim { get; set; }
    }
}
