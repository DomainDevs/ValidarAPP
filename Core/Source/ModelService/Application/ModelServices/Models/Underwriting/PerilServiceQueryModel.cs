// -----------------------------------------------------------------------
// <copyright file="PaymentPlanServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo amparo
    /// </summary>
    [DataContract]
    public class PerilServiceQueryModel
    {
        /// <summary>
        /// Obtiene o establece el Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
