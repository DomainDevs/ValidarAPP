using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
namespace Sistran.Company.Application.Location.PropertyServices.EEProvider.Entities.View
{
    /// <summary>
    /// Vista de riesgo hogar
    /// </summary>
    [Serializable()]
    public class RiskPropertyView : BusinessView
    {
        /// <summary>
        /// Colección de elementos de la entidad
        /// </summary>
        /// /// <value>
        /// Colección de EndorsementRisk.
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
        /// Colección de RiskLocation.
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
        /// Colección de Risk.
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
        /// Colección de RiskInsuredObject.
        /// </value>
        public BusinessCollection RiskInsuredObjects
        {
            get
            {
                return this["RiskInsuredObject"];
            }
        }
        /// <summary>
        /// Colección de elementos de la entidad
        /// </summary>
        /// /// <value>
        /// Colección de RiskBeneficiary.
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
        /// Colección de RiskClauses.
        /// </value>
        public BusinessCollection RiskClauses
        {
            get
            {
                return this["RiskClause"];
            }
        }

        public BusinessCollection ConstructionCategories
        {
            get
            {
                return this["ConstructionCategory"];
            }
        }

        public BusinessCollection RiskUseEarthquakes
        {
            get {
                return this["RiskUseEarthquake"];
            }
        }
        public BusinessCollection Countries
        {
            get
            {
                return this["Country"];
            }
        }
        public BusinessCollection States
        {
            get
            {
                return this["State"];
            }
        }
        public BusinessCollection Cities
        {
            get
            {
                return this["City"];
            }
        }
        public BusinessCollection Policies
        {
            get
            {
                return this["Policy"];
            }
        }
        public BusinessCollection InsuredObjects
        {
            get
            {
                return this["InsuredObject"];
            }
        }
    }
}
