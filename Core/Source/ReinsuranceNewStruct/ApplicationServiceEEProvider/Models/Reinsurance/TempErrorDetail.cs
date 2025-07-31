using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class TempErrorDetail
    {
        /// <summary>
        /// Atributo para la propiedad TempReinsuranceProcessCode .
        /// </summary>
        [DataMember] 
        public int? ProcessId { get; set; }
        /// <summary>
        /// Atributo para la propiedad ModuleCode.
        /// </summary>
        [DataMember] 
        public int? ModuleId { get; set; }
        /// <summary>
        /// Atributo para la propiedad TemporalCode.
        /// </summary>
        [DataMember] 
        public int? TemporalId { get; set; }
        /// <summary>
        /// Atributo para la propiedad ReinsuranceErrorCode.
        /// </summary>
        [DataMember] 
        public int? ReinsuranceErrorId { get; set; }
    }
}
