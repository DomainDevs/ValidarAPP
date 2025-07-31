// -----------------------------------------------------------------------
// <copyright file="ParamProduct.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// Modelo de producto.
    /// </summary>
    public class BaseParamProduct: Extension
    {
        /// <summary>
        /// Id del producto.
        /// </summary>
        private readonly int productId;

        /// <summary>
        /// Descripción del producto.
        /// </summary>
        private readonly string productDescription;

        /// <summary>
        /// Descripcion corta del producto.
        /// </summary>
        private readonly string productSmallDescription;

        /// <summary>
        /// Estado del producto.
        /// </summary>
        private readonly bool activeProduct;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamProduct"/>.
        /// </summary>
        /// <param name="productId">Identificador del producto.</param>
        /// <param name="productDescription">Descripción del producto.</param>
        /// <param name="productSmallDescription">Descripcion corta del producto.</param>
        /// <param name="activeProduct">Estado del producto.</param>
        protected BaseParamProduct(int productId, string productDescription, string productSmallDescription, bool activeProduct)
        {
            this.productId = productId;
            this.productDescription = productDescription;
            this.productSmallDescription = productSmallDescription;
            this.activeProduct = activeProduct;
        }

        /// <summary>
        /// Obtiene el Id del producto.
        /// </summary>
        public int ProductId
        {
            get
            {
                return this.productId;
            }
        }

        /// <summary>
        /// Obtiene la Descripción del producto.
        /// </summary>
        public string ProductDescription
        {
            get
            {
                return this.productDescription;
            }
        }

        /// <summary>
        /// Obtiene la descripción corta del producto.
        /// </summary>
        public string ProductSmallDescription
        {
            get
            {
                return this.productSmallDescription;
            }
        }

        /// <summary>
        /// Obtiene un valor que indica si el producto está activo.
        /// </summary>
        public bool ActiveProduct
        {
            get
            {
                return this.activeProduct;
            }
        }

        
    }
}
