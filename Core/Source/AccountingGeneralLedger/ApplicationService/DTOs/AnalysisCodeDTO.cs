#region Using

using System.Collections.Generic;
using System.Runtime.Serialization;


#endregion

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs
{
    /// <summary>
    ///     Modelo utilizado para el Código de Análisis
    ///     Implementación inicial aún no definida hasta que se verifique el uso del ABM.
    /// </summary>
    [DataContract]
    public class AnalysisCodeDTO
    {
        /// <summary>
        ///     Identificador único del modelo
        /// </summary>
        [DataMember]
        public int AnalysisCodeId { get; set; }

        /// <summary>
        ///     Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     Naturaleza Contable
        /// </summary>
        [DataMember]
        public int AccountingNature { get; set; }

        /// <summary>
        ///     Controla Saldo
        /// </summary>
        [DataMember]
        public bool CheckBalance { get; set; }

        /// <summary>
        ///     Controla Origen: Emision, etc
        /// </summary>
        [DataMember]
        public bool CheckModuleType { get; set; }

        /// <summary>
        ///     Conceptos
        /// </summary>
        [DataMember]
        public List<AnalysisConceptDTO> AnalisisConcepts { get; set; }
    }
}