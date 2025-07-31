// -----------------------------------------------------------------------
// <copyright file="LineBusinessServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using System.Runtime.Serialization;
    /// <summary>
    /// Clase pública Lineas de negocio
    /// </summary>
    [DataContract]
    public class SubLineBusinessServiceQueryModel
    {
        /// <summary>
        /// Identificador 
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Ramo tecnico
        /// </summary>
        [DataMember]
        public string Description { get; set; }

    }
}
