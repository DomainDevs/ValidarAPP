// -----------------------------------------------------------------------
// <copyright file="ErrorServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño Gutierrez</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ModelServices.Models.Param
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    /// <summary>
    /// Modelo generico para retornar listado de model
    /// </summary>
    [DataContract]
    public class GenericModelsServicesQueryModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece listado de model
        /// </summary>
        [DataMember]
        public List<GenericModelServicesQueryModel> GenericModelServicesQueryModel { get; set; }
    }
}
