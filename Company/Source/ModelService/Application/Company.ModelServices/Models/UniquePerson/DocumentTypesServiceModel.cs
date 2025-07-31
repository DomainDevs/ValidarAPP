// -----------------------------------------------------------------------
// <copyright file="DocumentTypeServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ModelServices.Models
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Contiene las propiedades de tipos de documento
    /// </summary>
    [DataContract]
    public class DocumentTypesServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece una lista de tipos de documento.
        /// </summary>
        [DataMember]
        public List<DocumentTypeServiceModel> DocumentTypeServiceModel { get; set; }
    }
}