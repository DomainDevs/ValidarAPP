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
    public class ParamFinancialPlanView : BusinessView
    {
        /// <summary>
        /// Obtiene colección de plan financiero
        /// </summary>
        public BusinessCollection FinancialPlan
        {
            get
            {
                return this["FinancialPlan"];
            }
        }

        /// <summary>
        /// Obtiene colección de componentes asociacion
        /// </summary>
        public BusinessCollection PayComponents
        {
            get
            {
                return this["PayComponents"];
            }
        }

        /// <summary>
        /// Obtiene colección de componentes
        /// </summary>
        public BusinessCollection Components
        {
            get
            {
                return this["Components"];
            }
        }

        /// <summary>
        /// Obtiene colección de plan de pago
        /// </summary>
        public BusinessCollection PaymentPlan
        {
            get
            {
                return this["PaymentPlan"];
            }
        }

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

        /// <summary>
        /// Obtiene colección de moneda
        /// </summary>
        public BusinessCollection Currency
        {
            get
            {
                return this["Currency"];
            }
        }


    }
}
