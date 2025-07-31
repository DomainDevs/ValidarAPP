// -----------------------------------------------------------------------
// <copyright file="PaymentScheduleDistributionView.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
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
    public class ParamPaymentMethodView : BusinessView
    {
      
        /// <summary>
        /// Obtiene colección de metodo de pago
        /// </summary>
        public BusinessCollection PaymentMethod
        {
            get
            {
                return this["PaymentMethod"];
            }
        }


    }
}
