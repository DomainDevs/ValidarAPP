using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim
{
    [DataContract]
    public class SubCause
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Requiere Fecha de Inspección
        /// </summary>
        [DataMember]
        public Cause Cause { get; set; }
    }
}
