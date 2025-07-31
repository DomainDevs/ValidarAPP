// -----------------------------------------------------------------------
// <copyright file="ElementDTO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camilo Jimenéz </author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ParametrizationAplicationServices.DTO
{
    using System.Runtime.Serialization;

    /// <summary>
    /// ElementDTO.Elemento asociado a Combo Box.
    /// </summary>
    [DataContract]
    public class ElementDTO
    {
        /// <summary>
        /// Gets or sets Identificador del elemento(llave primaria).
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Valor Desplegable al usuario.
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
