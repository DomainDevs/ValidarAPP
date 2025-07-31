// -----------------------------------------------------------------------
// <copyright file="IdDescriptionQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.Param
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de consultas para combos con Id y Descripción
    /// </summary>
    [DataContract]
    public class IdDescriptionQueryModel
    {
        /// <summary>
        /// Obtiene o establece el Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
