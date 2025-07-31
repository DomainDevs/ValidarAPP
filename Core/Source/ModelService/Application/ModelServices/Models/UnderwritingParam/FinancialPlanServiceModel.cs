// -----------------------------------------------------------------------
// <copyright file="FinancialPlanServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de servicio plan financiero
    /// </summary>
    [DataContract]
    public class FinancialPlanServiceModel : ParametricServiceModel
    {
        /// <summary>
        /// Obtiene o establece Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece modelo plan de pago
        /// </summary>
        [DataMember]
        public PaymentPlanServiceQueryModel PaymentPlanServiceQueryModel { get; set; }

        /// <summary>
        /// Obtiene o establece modelo metodo de pago
        /// </summary>
        [DataMember]
        public PaymentMethodServiceQueryModel PaymentMethodServiceQueryModel { get; set; }

        /// <summary>
        /// Obtiene o establece modelo moneda
        /// </summary>
        [DataMember]
        public CurrencyServiceQueryModel CurrencyServiceQueryModel { get; set; }

        /// <summary>
        /// Obtiene o establece cuota minima
        /// </summary>
        [DataMember]
        public int MinQuota { get; set; }

        /// <summary>
        /// Obtiene o establece lista de componentes
        /// </summary>
        [DataMember]
        public List<FirstPayComponentServiceModel> FirstPayComponentsServiceModel { get; set; }
    }
}
