using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim
{
    [DataContract]
    public class ClaimAirCraft
    {
        /// <summary>
        /// Reclamación
        /// </summary>
        [DataMember]
        public Claim Claim { get; set; }
    }
}
