// -----------------------------------------------------------------------
// <copyright file="PrefixServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo ramo comercial
    /// </summary>
    [DataContract]
    public class PrefixServiceModel
    {
        /// <summary>
        /// Obtiene o establece descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece pequeña descripcion
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
