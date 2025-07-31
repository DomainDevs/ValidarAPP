// -----------------------------------------------------------------------
// <copyright file="CoverageServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------   ------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo coberturas
    /// </summary>
    [DataContract]
    public class CoverageClauseServiceModel
    {
        /// <summary>
        /// Obtiene o establece Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece modelo para objeto del seguro
        /// </summary>
        [DataMember]
        public InsuredObjectServiceQueryModel InsuredObjectServiceQueryModel { get; set; }

        /// <summary>
        /// Obtiene o establece modelo para amparo
        /// </summary>
        [DataMember]
        public PerilServiceQueryModel PerilServiceQueryModel { get; set; }
    }
}
