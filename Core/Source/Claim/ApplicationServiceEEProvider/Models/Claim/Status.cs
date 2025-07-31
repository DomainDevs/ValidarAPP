using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim
{
    /// <summary>
    /// Estado
    /// </summary>
    [DataContract]
    public class Status
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Habilitado
        /// </summary>
        [DataMember]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Codigo Interno
        /// </summary>
        [DataMember]
        public InternalStatus InternalStatus { get; set; }
    }
}
