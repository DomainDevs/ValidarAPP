using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
namespace Sistran.Company.Application.Finances.FidelityServices.EEProvider.Entities.View
{
    [Serializable()]
    public class RiskFidelityView : BusinessView
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
        /// /// <value>
        /// Colección de riesgo hogar.
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
        /// Colección de Beneficiarios.
        /// </value>
        public BusinessCollection RiskBeneficiaries
        {
            get
            {
                return this["RiskBeneficiary"];
            }
        }
        /// <summary>
        /// Colección de elementos de la entidad
        /// </summary>
        /// /// <value>
        /// Colección de Clausulas.
        /// </value>
        public BusinessCollection RiskClauses
        {
            get
            {
                return this["RiskClause"];
            }
        }
      
    }
}
