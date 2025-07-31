// -----------------------------------------------------------------------
// <copyright file="DetailTypeView.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views
{
    using System;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.Views;

    /// <summary>
    /// Vista de plan de pago y distribuccion de cuotas
    /// </summary>
    [Serializable]    
    public class DetailTypeView : BusinessView
    {
        /// <summary>
        /// Obtiene colección de plan de pagos
        /// </summary>
        public BusinessCollection Details
        {
            get
            {
                return this["Detail"];
            }
        }

        /// <summary>
        /// Obtiene colección de distribucción de cuotas
        /// </summary>
        public BusinessCollection DetailTypes
        {
            get
            {
                return this["DetailType"];
            }
        }
    }
}
