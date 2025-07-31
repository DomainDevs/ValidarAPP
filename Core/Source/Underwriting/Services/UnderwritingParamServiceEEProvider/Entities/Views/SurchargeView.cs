// -----------------------------------------------------------------------
// <copyright file="SurchargeView.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views
{
    using System;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.Views;

    /// <summary>
    /// vista para recargos
    /// </summary>
    [Serializable]
    public class SurchargeView : BusinessView
    {
        /// <summary>
        /// Obtiene recargos
        /// </summary>
        /// <returns> retorna el modelo de componentes </returns>
        public BusinessCollection Surcharge
        {
            get
            {
                return this["Surcharge"];
            }
        }

        /// <summary>
        /// Obtiene componentes 
        /// </summary>
        /// <returns> retorna el modelo de componentes </returns>
        public BusinessCollection Component
        {
            get
            {
                return this["Component"];
            }
        }

        /// <summary>
        /// Obtiene tipo de tasa
        /// </summary>
        /// <returns> retorna el modelo de componentes </returns>
        public BusinessCollection RateType
        {
            get
            {
                return this["RateType"];
            }
        }
    }
}
