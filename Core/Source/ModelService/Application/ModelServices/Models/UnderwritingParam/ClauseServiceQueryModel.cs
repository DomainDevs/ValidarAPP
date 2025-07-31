// -----------------------------------------------------------------------
// <copyright file="ClauseServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de servicio de Clausula
    /// </summary>
    [DataContract]
    public class ClauseServiceQueryModel
    {
        /// <summary>
        /// Obtiene o establece el id de clausula
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion de la clausula
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece si la clausula es obligatoria
        /// </summary>
        [DataMember]
        public bool IsMandatory { get; set; }
    }
}
