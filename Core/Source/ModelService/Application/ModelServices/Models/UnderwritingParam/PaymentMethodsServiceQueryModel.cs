// -----------------------------------------------------------------------
// <copyright file="PaymentMethodsServiceQueryModel.cs" company="SISTRAN">
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
    /// Clase pública PaymentMethodsServiceQueryModel
    /// </summary>
    [DataContract]
    public class PaymentMethodsServiceQueryModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece el modelo PaymentMethodServiceQueryModel
        /// </summary>
        [DataMember]
        public List<PaymentMethodServiceQueryModel> PaymentMethodServiceModels { get; set; }
    }
}
