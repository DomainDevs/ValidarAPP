using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.MassiveUnderwritingServices.Models
{
    /// <summary>
    /// Información del excel
    /// </summary>
    [DataContract]
    public class MassiveReport
    {
        /// <summary>
        /// Obtiene o establece el Tipo de reporte
        /// 1. Autos
        /// 2. RC
        /// 3. Exlusion riesgos
        /// </summary>
        [DataMember]
        public int ReportType { get; set; }

        /// <summary>
        /// Obtiene o establece las Cabeceras
        /// </summary>
        [DataMember]
        public List<MassiveLoadHeader> Headers { get; set; }

        /// <summary>
        /// Obtiene o establece los Riesgos
        /// </summary>
        [DataMember]
        public List<MassiveReportRisks> Risks { get; set; }

        

        /// <summary>
        /// Obtiene la descripción del cargue
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
