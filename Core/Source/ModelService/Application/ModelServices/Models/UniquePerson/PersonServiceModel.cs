// -----------------------------------------------------------------------
// <copyright file="PersonServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gomez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Contiene las propiedades de la Sucursal
    /// </summary>
    [DataContract]
    public class PersonServiceModel
    {
        /// <summary>
        /// Obtiene o establece el PersonId
        /// </summary>
        [DataMember]
        public int PersonId { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre
        /// </summary>
        [DataMember]
        public string Name { get; set; }
    }
}
