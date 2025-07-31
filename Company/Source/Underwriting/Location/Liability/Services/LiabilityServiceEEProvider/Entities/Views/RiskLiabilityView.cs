using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
namespace Sistran.Company.Application.Location.LiabilityServices.EEProvider.Entities.View
{
    [Serializable()]
    public class RiskLiabilityView : BusinessView
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
        public BusinessCollection RiskLocations
        {
            get
            {
                return this["RiskLocation"];
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
