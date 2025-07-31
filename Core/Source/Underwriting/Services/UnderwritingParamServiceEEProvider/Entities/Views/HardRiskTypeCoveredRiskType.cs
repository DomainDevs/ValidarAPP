// -----------------------------------------------------------------------
// <copyright file="HardRiskTypeCoveredRiskType.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Desconocido</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamService.EEProviderWeb.Views
{
    using System;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.Views;

    /// <summary>
    /// Tipo de riesgo cubierto
    /// </summary>
    [Serializable]
    public class HardRiskTypeCoveredRiskType : BusinessView
    {
        /// <summary>
        /// Obtiene HardRiskType
        /// </summary>
        public BusinessCollection HardRiskTypes
        {
            get
            {
                return this["HardRiskType"];
            }
        }

        /// <summary>
        /// Obtiene LineBusinessCoveredRiskType
        /// </summary>
        public BusinessCollection LineBusinessByCoveredRiskType
        {
            get
            {
                return this["LineBusinessCoveredRiskType"];
            }
        }
    }
}
