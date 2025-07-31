// -----------------------------------------------------------------------
// <copyright file="DetailTypesServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Clase pública tipo detalle
    /// </summary>
    [DataContract]
    public class DetailTypesServiceQueryModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece el modelo DetailTypeServiceQueryModel
        /// </summary>
        [DataMember]
        public List<DetailTypeServiceQueryModel> DetailTypeServiceQueryModel { get; set; }
    }
}
