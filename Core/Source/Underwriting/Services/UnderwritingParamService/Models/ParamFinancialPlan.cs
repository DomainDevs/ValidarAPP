// -----------------------------------------------------------------------
// <copyright file="ParamFinancialPlan.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.UnderwritingParamService.Models.Base;
    using System.Runtime.Serialization;
    /// <summary>
    /// Modelo plan financiero
    /// </summary>
    [DataContract]
    public class ParamFinancialPlan: BaseParamFinancialPlan
    {
       
        /// <summary>
        /// Obtiene o establece modelo plan de pago
        /// </summary>
        [DataMember]
        public ParametrizationPaymentPlan ParametrizationPaymentPlan { get; set; }

        /// <summary>
        /// Obtiene o establece metodo de pago
        /// </summary>
        [DataMember]
        public ParamPaymentMethod ParamPaymentMethod { get; set; }

        /// <summary>
        /// Obtiene o establece modelo moneda
        /// </summary>
        [DataMember]
        public ParamCurrency ParamCurrency { get; set; }
    }
}
