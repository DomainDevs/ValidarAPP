// -----------------------------------------------------------------------
// <copyright file="PaymentPlanBusiness.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Business
{
    using System;
    using System.Linq;
    using Sistran.Core.Application.UnderwritingParamService.Models;

    /// <summary>
    /// Validacion de plan de pago
    /// </summary>
    public class PaymentPlanBusiness
    {
        /// <summary>
        /// Valida si las cuotas suman 100
        /// </summary>
        /// <param name="parametrizationPaymentPlan">Plan de pago MOD-B</param>
        /// <returns>True en caso de validacion correcta</returns>
        public bool ValidateQuotaDistribution(ParametrizationPaymentPlan parametrizationPaymentPlan)
        {
            if (parametrizationPaymentPlan.ParametrizationQuotas != null)
            {
                if (Convert.ToInt32(parametrizationPaymentPlan.ParametrizationQuotas.AsParallel().Sum(x => x.Percentage)) == 100)
                {
                    return true;
                }
            }

            return false;           
        }
    }
}