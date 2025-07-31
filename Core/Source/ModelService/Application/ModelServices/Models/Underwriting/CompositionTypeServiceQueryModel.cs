// -----------------------------------------------------------------------
// <copyright file="CompositionTypeServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using System.Runtime.Serialization;

    /// <summary>
    /// MOD-S nivel de influencia
    /// </summary>
    [DataContract]
    public class CompositionTypeServiceQueryModel
    {
        /// <summary>
        /// Obtiene o establece el id del nivel de influencia
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Obtiene o establece la descripcion del nivel de influencia
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
