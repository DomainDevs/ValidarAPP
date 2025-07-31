// -----------------------------------------------------------------------
// <copyright file="PaymentPlansServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Planes de pago
    /// </summary>
    [DataContract]
    public class PaymentPlansServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece los planes de pago (Modelo del servicio)
        /// </summary>
        [DataMember]
        public List<PaymentPlanServiceModel> PaymentPlanServiceModels { get; set; }
    }
}
