// -----------------------------------------------------------------------
// <copyright file="EndorsementTypesServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de servicio de consulta de los Tipos de endoso.
    /// </summary>
    [DataContract]
    public class EndorsementTypesServiceQueryModel : Param.ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece  la lista de los de tipos de endoso.
        /// </summary>
        [DataMember]
        public List<EndorsementTypeServiceQueryModel> EndorsementTypeServiceQueryModel { get; set; }
    }
}
