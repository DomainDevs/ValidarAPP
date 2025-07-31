// -----------------------------------------------------------------------
// <copyright file="UniqueUsersProduct.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UniqueUserServices.Models
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de negocio para los productos de usuario.
    /// </summary>
    [DataContract]
    public class UniqueUsersProduct
    {
        /// <summary>
        /// Obtiene o establece el identificador del usuario.
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del producto.
        /// </summary>
        [DataMember]
        public int ProductId { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion del producto.
        /// </summary>
        [DataMember]
        public string ProductDescription { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del ramo comercial.
        /// </summary>
        [DataMember]
        public int PrefixCode { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si esta o no habilitado producto para el usuario.
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si esta o no asignado el producto para el usuario.
        /// </summary>
        [DataMember]
        public bool Assign { get; set; }
    }
}
