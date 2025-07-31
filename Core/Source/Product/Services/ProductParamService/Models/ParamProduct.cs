// -----------------------------------------------------------------------
// <copyright file="ParamProduct.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ProductParamService.Models
{
    using Sistran.Core.Application.ProductParamService.Models.Base;
    using Sistran.Core.Application.ProductServices.Models;
    using Sistran.Core.Application.Utilities.Error;
    using System.Collections.Generic;
    using CommonModel = CommonService.Models;

    /// <summary>
    /// Modelo de producto.
    /// </summary>
    public class ParamProduct: BaseParamProduct
    {
        /// <summary>
        /// Atributo para la propiedad Ramo
        /// </summary> 
        public ParamPrefix Prefix { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ParamCurrency> Currency { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ParamPolicyType> PolicyType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ParamRiskType> RiskType { get; set; }

        public ParamProduct(int id, string description, string smallDescription)
        {
            Id = id;
            Description = description;
            SmallDescription = smallDescription;
        }

        /// <summary>
        /// Objeto que crea u obtiene el producto.
        /// </summary>
        /// <param name="id">Identificador del producto.</param>
        /// <param name="description">Descripcion del producto.</param>
        /// <param name="smallDescription">Descripción corta del producto</param>
        /// <param name="active">Estado del producto.</param>
        /// <returns>Retorna el modelo de producto o un error.</returns>
        public static Result<ParamProduct, ErrorModel> GetParamProduct(int id, string description, string smallDescription)
        {
            return new ResultValue<ParamProduct, ErrorModel>(new ParamProduct(id, description, smallDescription));
        }
    }
}
