// -----------------------------------------------------------------------
// <copyright file="ParametrizationLineBusinessView.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Manuel Méndez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamService.EEProviderWeb.Views
{
    using System;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.Views;

    /// <summary>
    /// Consulta de ramo técnico
    /// </summary>
    [Serializable]
    public class ParametrizationLineBusinessView : BusinessView
    {
        /// <summary>
        /// Obtiene LineBusiness
        /// </summary>
        public BusinessCollection LineBusiness
        {
            get
            {
                return this["LineBusiness"];
            }
        }

        /// <summary>
        /// Obtiene LineBusinessCoveredRiskTypes
        /// </summary>
        public BusinessCollection LineBusinessCoveredRiskTypes
        {
            get
            {
                return this["LineBusinessCoveredRiskType"];
            }
        }

        /// <summary>
        /// Obtiene CoveredRiskTypes
        /// </summary>
        public BusinessCollection CoveredRiskTypes
        {
            get
            {
                return this["CoveredRiskType"];
            }
        }

        /// <summary>
        /// Obtiene InsObjLineBusinesses
        /// </summary>
        public BusinessCollection InsObjLineBusinesses
        {
            get
            {
                return this["InsObjLineBusiness"];
            }
        }

        /// <summary>
        /// Obtiene InsuredObjects
        /// </summary>
        public BusinessCollection InsuredObjects
        {
            get
            {
                return this["InsuredObject"];
            }
        }

        /// <summary>
        /// Obtiene PerilLineBusinesses
        /// </summary>
        public BusinessCollection PerilLineBusinesses
        {
            get
            {
                return this["PerilLineBusiness"];
            }
        }

        /// <summary>
        /// Obtiene Perils
        /// </summary>
        public BusinessCollection Perils
        {
            get
            {
                return this["Peril"];
            }
        }

        /// <summary>
        /// Obtiene ClauseLevels
        /// </summary>
        public BusinessCollection ClauseLevels
        {
            get
            {
                return this["ClauseLevel"];
            }
        }

        /// <summary>
        /// Obtiene Clauses
        /// </summary>
        public BusinessCollection Clauses
        {
            get
            {
                return this["Clause"];
            }
        }
    }
}
