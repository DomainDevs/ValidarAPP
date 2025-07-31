// -----------------------------------------------------------------------
// <copyright file="ProductQueryServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de producto.
    /// </summary>
    [DataContract]
    public class ProductServiceQueryModel
    {
        /// <summary>
        /// Obtiene o establece el Id del producto.
        /// </summary>
        [DataMember]
        public int ProductId { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción del producto.
        /// </summary>
        [DataMember]
        public string ProductDescription { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción corta del producto.
        /// </summary>
        [DataMember]
        public string ProductSmallDescription { get; set; }

        /// <summary>
        /// Obtiene a establece un valor que indica si el producto está activo.
        /// </summary>
        [DataMember]
        public bool ActiveProduct { get; set; }
    }
}