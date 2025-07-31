using Sistran.Core.Application.CommonService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim
{
    /// <summary>
    /// Denuncia
    /// </summary>
    [DataContract]
    public class ClaimLocation
    {
        /// <summary>
        /// Direccion
        /// </summary>
        [DataMember]
        public string Address { get; set; }

        /// <summary>
        /// Ciudad
        /// </summary>
        [DataMember]
        public City City { get; set; }

        /// <summary>
        /// Denuncia
        /// </summary>
        [DataMember]
        public Claim Claim { get; set; }
    }
}
