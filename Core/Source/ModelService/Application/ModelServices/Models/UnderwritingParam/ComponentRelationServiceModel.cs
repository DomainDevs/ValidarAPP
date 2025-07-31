// -----------------------------------------------------------------------
// <copyright file="ComponentRelationServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using System.Runtime.Serialization;
    using Sistran.Core.Application.ModelServices.Models.Param;

    /// <summary>
    /// Modelo componentes
    /// </summary>
    [DataContract]
    public class ComponentRelationServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
