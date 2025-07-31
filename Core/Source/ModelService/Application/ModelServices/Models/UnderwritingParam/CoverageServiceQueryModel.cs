// -----------------------------------------------------------------------
// <copyright file="CoverageServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Alberto Sánchez Lesmes</author>
// -----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    /// <summary>
    /// Cobertura
    /// </summary>
    [DataContract]
    public class CoverageServiceQueryModel
    {
        /// <summary>
        /// Obtiene o establece el id de la Cobertura
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Obtiene o establece la descripcion de la Cobertura
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
