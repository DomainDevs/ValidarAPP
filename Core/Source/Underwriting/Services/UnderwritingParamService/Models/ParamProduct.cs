// -----------------------------------------------------------------------
// <copyright file="ParamProduct.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.UnderwritingParamService.Models.Base;
    using Sistran.Core.Application.Utilities.Error;
    /// <summary>
    /// Modelo de producto.
    /// </summary>
    public class ParamProduct: BaseParamProduct
    {

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamProduct"/>.
        /// </summary>
        /// <param name="productId">Identificador del producto.</param>
        /// <param name="productDescription">Descripción del producto.</param>
        /// <param name="productSmallDescription">Descripcion corta del producto.</param>
        /// <param name="activeProduct">Estado del producto.</param>
        private ParamProduct(int productId, string productDescription, string productSmallDescription, bool activeProduct):
            base(productId, productDescription, productSmallDescription, activeProduct)
        {
        }

        /// <summary>
        /// Objeto que crea u obtiene el producto.
        /// </summary>
        /// <param name="productId">Identificador del producto.</param>
        /// <param name="productDescription">Descripcion del producto.</param>
        /// <param name="productSmallDescription">Descripción corta del producto</param>
        /// <param name="activeProduct">Estado del producto.</param>
        /// <returns>Retorna el modelo de producto o un error.</returns>
        public static Result<ParamProduct, ErrorModel> GetParamProduct(int productId, string productDescription, string productSmallDescription, bool activeProduct)
        {
            return new ResultValue<ParamProduct, ErrorModel>(new ParamProduct(productId, productDescription, productSmallDescription, activeProduct));
        }
    }
}
