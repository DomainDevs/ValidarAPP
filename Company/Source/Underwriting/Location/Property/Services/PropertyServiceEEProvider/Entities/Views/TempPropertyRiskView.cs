using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
namespace Sistran.Company.Application.Location.PropertyServices.EEProvider.Entities.views
{
    /// <summary>
    /// Vista de riesgo hogar
    /// </summary>
    [Serializable()]
    public class TempPropertyRiskView : BusinessView
    {
        /// <summary>
        /// Colección de elementos de la entidad
        /// </summary>
        /// /// <value>
        /// Colección de TempRisk.
        /// </value>
        public BusinessCollection TempRisks
        {
            get
            {
                return this["TempRisk"];
            }
        }
        /// <summary>
        /// Colección de elementos de la entidad
        /// </summary>
        /// /// <value>
        /// Colección de CoTempRisk.
        /// </value>
        public BusinessCollection CoTempRisks
        {
            get
            {
                return this["CoTempRisk"];
            }
        }
        /// <summary>
        /// Colección de elementos de la entidad
        /// </summary>
        /// /// <value>
        /// Colección de TempRiskLocation.
        /// </value>
        public BusinessCollection TempRiskLocations
        {
            get
            {
                return this["TempRiskLocation"];
            }
        }
        /// <summary>
        /// Colección de elementos de la entidad
        /// </summary>
        /// /// <value>
        /// Colección de TempRiskInsuredObject.
        /// </value>
        public BusinessCollection TempRiskInsuredObjects
        {
            get
            {
                return this["TempRiskInsuredObject"];
            }
        }
        /// <summary>
        /// Colección de elementos de la entidad
        /// </summary>
        /// /// <value>
        /// Colección de TempRiskBeneficiary.
        /// </value>
        public BusinessCollection TempRiskBeneficiaries
        {
            get
            {
                return this["TempRiskBeneficiary"];
            }
        }
        /// <summary>
        /// Colección de elementos de la entidad
        /// </summary>
        /// /// <value>
        /// Colección de TempRiskClause.
        /// </value>
        public BusinessCollection TempRiskClauses
        {
            get
            {
                return this["TempRiskClause"];
            }
        }
    }
}
