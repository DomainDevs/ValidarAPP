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
    public class PaymentScheduleDistributionView : BusinessView
    {
        /// <summary>
        /// Obtiene colección de plan de pagos
        /// </summary>
        public BusinessCollection PaymentSchedules
        {
            get
            {
                return this["PaymentSchedule"];
            }
        }

        /// <summary>
        /// Obtiene colección de distribucción de cuotas
        /// </summary>
        public BusinessCollection PaymentDistributions
        {
            get
            {
                return this["PaymentDistribution"];
            }
        }

        public BusinessCollection PaymentDistributionComponents
        {
            get
            {
                return this["PaymentDistributionComponent"];
            }
        }
    }
}
