// -----------------------------------------------------------------------
// <copyright file="TextsServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo lista de textos precatalogados
    /// </summary>
    [DataContract]
    public class TextsServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece listado de textos
        /// </summary>
        [DataMember]
        public List<TextServiceModel> TextServiceModels { get; set; }
    }
}
