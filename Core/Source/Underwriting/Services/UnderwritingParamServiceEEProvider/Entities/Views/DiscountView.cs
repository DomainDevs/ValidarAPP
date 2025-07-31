// -----------------------------------------------------------------------
// <copyright file="DiscountView.cs" company="SISTRAN">
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
    /// vista para descuentos
    /// </summary>
    [Serializable]
    public class DiscountView : BusinessView
    {
        /// <summary>
        /// Obtiene descuentos
        /// </summary>       
        public BusinessCollection Discount
        {
            get
            {
                return this["Discount"];
            }
        }

        /// <summary>
        /// Obtiene componentes
        /// </summary>    
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
        public BusinessCollection RateType
        {
            get
            {
                return this["RateType"];
            }
        }
    }
}
