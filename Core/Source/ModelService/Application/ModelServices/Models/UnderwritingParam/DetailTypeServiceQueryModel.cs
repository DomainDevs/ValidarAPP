// -----------------------------------------------------------------------
// <copyright file="DetailTypeServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Clase pública tipo detalle
    /// </summary>
    [DataContract]
    public class DetailTypeServiceQueryModel
    {
        /// <summary>
        /// Obtiene o establece el Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece si el tipo de detalle es obligatoria
        /// </summary>
        [DataMember]
        public bool IsMandatory { get; set; }
    }
}
