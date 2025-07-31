// -----------------------------------------------------------------------
// <copyright file="BusinessQueryServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de servicio de consulta de los negocios.
    /// </summary>
    [DataContract]
    public class BusinessServiceQueryModel : Param.ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece la propiedad de la Lista de negocios.
        /// </summary>
        [DataMember]
        public List<BusinessServiceModel> BusinessServiceModel { get; set; }
    }
}
