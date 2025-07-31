// -----------------------------------------------------------------------
// <copyright file="PaymentPlanServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo para consultar plan de pago
    /// </summary>
    [DataContract]
    public class PaymentPlanServiceQueryModel
    {
        /// <summary>
        /// Obtiene o establece el Id de Plan de Pago
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la Descripcion de Plan de Pago
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
