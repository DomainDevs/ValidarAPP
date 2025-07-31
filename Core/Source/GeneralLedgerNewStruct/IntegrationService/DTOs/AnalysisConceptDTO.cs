using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.GeneralLedgerServices.DTOs
{
    /// <summary>
    ///     Modelo utilizado para el Concepto de Análisis
    ///     Implementación inicial aún no definida hasta que se verifique el uso del ABM.
    /// </summary>
    [DataContract]
    public class AnalysisConceptDTO
    {
        /// <summary>
        ///     Identificador único del modelo
        /// </summary>
        [DataMember]
        public int AnalysisConceptId { get; set; }

        /// <summary>
        ///     Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     Codigo de Concepto
        /// </summary>
        [DataMember]
        public int AnalysisCode { get; set; }

        /// <summary>
        ///     Tratamiento de Analisis
        /// </summary>
        [DataMember]
        public int AnalysisTreatment { get; set; }
    }
}
