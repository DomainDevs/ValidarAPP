// -----------------------------------------------------------------------
// <copyright file="PrefixsServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de servicio de consulta de los racoms comerciales.
    /// </summary>
    [DataContract]
    public class PrefixsServiceQueryModel : Param.ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece la lista de los de tipos de endoso.
        /// </summary>
        [DataMember]
        public List<PrefixServiceQueryModel> PrefixServiceQueryModel { get; set; }
    }
}
