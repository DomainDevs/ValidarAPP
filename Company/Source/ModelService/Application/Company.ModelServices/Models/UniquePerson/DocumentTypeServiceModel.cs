// -----------------------------------------------------------------------
// <copyright file="DocumentTypeServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ModelServices.Models
{
    using System.Runtime.Serialization;    
    using Sistran.Core.Application.ModelServices.Models.Param;

    /// <summary>
    /// Contiene las propiedades de tipos de documento
    /// </summary>
    [DataContract]
    public class DocumentTypeServiceModel
    {
        /// <summary>
        /// Obtiene o establece el Id de documento.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el Description de documento.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece el Description corta del documento.
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Obtiene o establece del ParametricServiceModel.
        /// </summary>
        [DataMember]
        public ParametricServiceModel ParametricServiceModel { get; set; }
    }
}