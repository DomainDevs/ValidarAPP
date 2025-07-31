// -----------------------------------------------------------------------
// <copyright file="QuotationSearchView.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jeison Amezquita</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.Entities.View
{
    using System;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.Views;

    /// <summary>
    /// Consulta de ramo técnico
    /// </summary>
    [Serializable]
    public class QuotationSearchView : BusinessView
    {
        /// <summary>
        /// Obtiene TempSubscriptions
        /// </summary>
        public BusinessCollection TempSubscriptions
        {
            get
            {
                return this["TempSubscription"];
            }
        }

        /// <summary>
        /// Obtiene TempRiskVehicle
        /// </summary>
        public BusinessCollection TempRiskVehicles
        {
            get
            {
                return this["TempRiskVehicle"];
            }
        }

        /// <summary>
        /// Obtiene Prefixes
        /// </summary>
        public BusinessCollection Prefixes
        {
            get
            {
                return this["Prefix"];
            }
        }

        /// <summary>
        /// Obtiene Branchs
        /// </summary>
        public BusinessCollection Branchs
        {
            get
            {
                return this["Branch"];
            }
        }

        /// <summary>
        /// Obtiene Currency
        /// </summary>
        public BusinessCollection Currencies
        {
            get
            {
                return this["Currency"];
            }
        }
        
    }
}
