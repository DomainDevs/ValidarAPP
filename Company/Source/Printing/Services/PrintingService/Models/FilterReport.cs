using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.PrintingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.PrintingServices.Models
{
    /// <summary>
    /// Filtro de riesgos para la póliza
    /// </summary>
    [DataContract]
    public class FilterReport : FilterPolicy
    {
        /// <summary>
        /// Riesgos
        /// </summary>
        [DataMember]
        public List<Risk> Risks { get; set; }

        /// <summary>
        /// Numero de Riesgos
        /// </summary>
        [DataMember]
        public string RiskNums { get; set; }

        /// <summary>
        /// Adjunto detalle de riesgos
        /// </summary>
        [DataMember]
        public bool AttachRisksDetail { get; set; }
    }
}
