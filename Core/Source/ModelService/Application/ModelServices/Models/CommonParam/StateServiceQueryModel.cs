// -----------------------------------------------------------------------
// <copyright file="StateServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.CommonParam
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de estado
    /// </summary>
    [DataContract]
    public class StateServiceQueryModel
    {
        /// <summary>
        /// Obtiene o establece identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece el Pais
        /// </summary>
        [DataMember]
        public CountryServiceQueryModel Country { get; set; }
    }
}