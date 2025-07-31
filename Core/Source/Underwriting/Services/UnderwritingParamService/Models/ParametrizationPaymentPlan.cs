// -----------------------------------------------------------------------
// <copyright file="ParametrizationPaymentPlan.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.UnderwritingParamService.Models.Base;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    /// <summary>
    /// Plan de pago (Modelo del negocio)
    /// </summary>
    [DataContract]
    public class ParametrizationPaymentPlan : BaseParametrizationPaymentPlan
    {
        /// <summary>
        /// Obtiene o establece el tiempo entre cuotas
        /// </summary>
        [DataMember]
        public List<ParametrizationQuota> ParametrizationQuotas { get; set; }
    }
}