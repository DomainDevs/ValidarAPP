// -----------------------------------------------------------------------
// <copyright file="SalePointView.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.CommonParamService.EEProvider.Entities.Views
{
    using System;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.Views;
    
    /// <summary>
    /// Vista de deducible
    /// </summary>
    [Serializable]
    public class SalePointView : BusinessView
    {
        /// <summary>
        /// Obtiene listado deducibles
        /// </summary>
        public BusinessCollection SalePoint
        {
            get
            {
                return this["SalePoint"];
            }
        }

        /// <summary>
        /// Obtiene listado ramos
        /// </summary>
        public BusinessCollection Branch
        {
            get
            {
                return this["Branch"];
            }
        }
    }
}
