// -----------------------------------------------------------------------
// <copyright file="TextServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo textos precatalogados
    /// </summary>
    [DataContract]
    public class TextServiceModel
    {
        /// <summary>
        /// Obtiene o establece el Codigo del texto
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la Descripcion del texto
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece  cuerpo del texto
        /// </summary>
        [DataMember]
        public string TextBody { get; set; }
    }
}
