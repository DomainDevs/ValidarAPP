using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim
{
    [DataContract]
    public class ClaimEndorsement
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int Number { get; set; }

        /// <summary>
        /// Identificador poliza
        /// </summary>
        [DataMember]
        public int PolicyId { get; set; }

        /// <summary>
        /// Numero Poliza
        /// </summary>
        [DataMember]
        public string PolicyNumber { get; set; }

        /// <summary>
        /// Riesgo
        /// </summary>
        [DataMember]
        public int RiskId { get; set; }

        [DataMember]
        public Policy Policy { get; set; }
    }
}
