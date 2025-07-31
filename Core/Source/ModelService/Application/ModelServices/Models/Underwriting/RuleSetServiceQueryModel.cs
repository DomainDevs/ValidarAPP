// -----------------------------------------------------------------------
// <copyright file="RuleSetServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Runtime.Serialization;
    /// <summary>
    /// Modelo de Tipo de ejecucion
    /// </summary>
    [DataContract]
    public class RuleSetServiceQueryModel : ErrorServiceModel
    {
        /// <summary>
        /// id modelo tipo de ejecucion
        /// </summary>
        [DataMember]
        public int? Id { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion
        /// </summary>
        [DataMember]
        public string description { get; set; }
    }
}
