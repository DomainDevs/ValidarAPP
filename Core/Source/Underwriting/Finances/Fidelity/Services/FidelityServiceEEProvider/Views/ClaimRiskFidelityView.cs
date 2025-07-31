using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.Finances.FidelirtyServices.EEProvider.Views
{
    [Serializable()]
    public class ClaimRiskFidelityView : BusinessView
    {
        /// <summary>
        /// Colección de elementos de la entidad
        /// </summary>
        /// /// <value>
        /// Colección de Endosos de riesgo.
        /// </value>
        public BusinessCollection EndorsementRisks
        {
            get
            {
                return this["EndorsementRisk"];
            }
        }

        /// <summary>
        /// Colección de elementos de la entidad
        /// </summary>
        /// /// /// <value>
        /// Colección de pólizas.
        /// </value>
        public BusinessCollection Policies
        {
            get
            {
                return this["Policy"];
            }
        }


        /// <summary>
        /// Colección de elementos de la entidad
        /// </summary>
        /// /// <value>
        /// Colección de riesgos.
        /// </value>
        public BusinessCollection Risks
        {
            get
            {
                return this["Risk"];
            }
        }

        /// <summary>
        /// Colección de elementos de la entidad
        /// </summary>
        /// /// <value>
        /// Colección de riesgo manejo.
        /// </value>
        public BusinessCollection RiskFidelities
        {
            get
            {
                return this["RiskFidelity"];
            }
        }

        /// <summary>
        /// Colección de elementos de la entidad
        /// </summary>
        /// /// <value>
        /// Colección de actividades del riesgo.
        /// </value>
        public BusinessCollection RiskCommercialClasses
        {
            get
            {
                return this["RiskCommercialClass"];
            }
        }
    }
}
