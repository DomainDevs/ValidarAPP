// -----------------------------------------------------------------------
// <copyright file="IdDescriptionsQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.Param
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Clase pública IdDescriptionsQueryModel
    /// </summary>
    [DataContract]
    public class IdDescriptionsQueryModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece el Listado de IdDescriptionQueryModel
        /// </summary>
        [DataMember]
        public List<IdDescriptionQueryModel> IdDescriptionModel { get; set; }
    }
}
