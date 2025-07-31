using Sistran.Core.Application.AccountingServices.Enums;
using System.Collections.Generic;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    public class AnalysisCode
    {
        /// <summary>
        /// Identificador único del modelo
        /// </summary>
        public int AnalysisCodeId { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Naturaleza Contable
        /// </summary>
        public AccountingNature AccountingNature { get; set; }

        /// <summary>
        /// Controla Saldo
        /// </summary>
        public bool CheckBalance { get; set; }

        /// <summary>
        /// Controla Origen: Emision, etc
        /// </summary>
        public bool CheckModuleType { get; set; }

        /// <summary>
        /// Conceptos
        /// </summary>
        public List<AnalysisConcept> AnalisisConcepts { get; set; }
    }
}
