// -----------------------------------------------------------------------
// <copyright file="PaymentMethodServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo metodo de pago
    /// </summary>
    [DataContract]
    public class PaymentMethodServiceQueryModel
    {
        /// <summary>
        /// Obtiene o establece Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
