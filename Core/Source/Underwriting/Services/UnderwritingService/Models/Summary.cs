using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    /// <summary>
    /// Resumen Póliza
    /// </summary>
    [DataContract]
    public class Summary : BaseSummary
    {
        /// <summary>
        /// Riesgos
        /// </summary>
        [DataMember]
        public List<Risk> Risks { get; set; }
        
    }
}
