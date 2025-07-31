// -----------------------------------------------------------------------
// <copyright file="ListaComboDTO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camilo Jimenéz </author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ParametrizationAplicationServices.DTO
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Sistran.Company.Application.Utilities.DTO;

    /// <summary>
    /// ListaComboDTO. Modelo DTO con origen de datos asociados.
    /// </summary>
    [DataContract]
    public class ListaComboDTO
    {
        /// <summary>
        /// Gets or sets. Nombre del campo de llave primaria de la tabla(Value Member).
        /// </summary>
        [DataMember]
        public string PkColumn { get; set; }

        /// <summary>
        /// Gets or sets. Nombre del campo mostrado para el usuario (Display Member).
        /// </summary>
        [DataMember]
        public string DescriptionColumn { get; set; }

        /// <summary>
        /// Gets or sets.  Tabla asociada.
        /// </summary>
        [DataMember]
        public string Table { get; set; }

        /// <summary>
        /// Gets or sets. Proyeccción asociada  a la consulta.
        /// </summary>
        [DataMember]
        public string Filter { get; set; }

        /// <summary>
        /// Gets or sets. Orden asociada  a la consulta.
        /// </summary>
        [DataMember]
        public string Order { get; set; }

        /// <summary>
        /// Gets or sets. Lista de Elementos asociados. 
        /// </summary>
        [DataMember]
        public List<ElementDTO> Elements { get; set; }

        /// <summary>
        /// Gets or sets. Objeto que contiene lista de errores asociados y tipo de respuesta
        /// </summary>
        [DataMember]
        public ErrorDTO Error { get; set; }
     }  
}
