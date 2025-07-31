// -----------------------------------------------------------------------
// <copyright file="ProductDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Queries;
    using Sistran.Core.Framework.Transactions;
    using Sistran.Core.Application.Parameters.Entities;
    using Sistran.Core.Application.Product.Entities;
    using Sistran.Core.Application.Common.Entities;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Application.Utilities.Enums;

    /// <summary>
    /// Clase DAO del objeto Product.
    /// </summary>
    public class ProductDAO
    {
        /// <summary>
        /// Obtiene la lista de productos.
        /// </summary>
        /// <returns>Lista de productos</returns>
        public Result<List<ParamProduct>, ErrorModel> GetProducts()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Product)));
                Result<List<ParamProduct>, ErrorModel> product = ModelAssembler.CreateProduct(businessCollection);
                if (product is ResultError<List<ParamProduct>, ErrorModel>)
                {
                    return product;
                }
                else
                {
                    List<ParamProduct> resultValue = (product as ResultValue<List<ParamProduct>, ErrorModel>).Value;
                    if (resultValue.Count == 0)
                    {
                        errorModelListDescription.Add("No se encontraron productos.");
                        return new ResultError<List<ParamProduct>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));

                    }
                    else
                    {
                        return product;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add("Ocurrio un error inesperado en la consulta de productos. Comuniquese con el administrador");
                return new ResultError<List<ParamProduct>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs");
            }
        }

        /// <summary>
        /// Obtiene productos por id del producto
        /// </summary>
        /// <param name="productCode">Id del producto</param>
        /// <returns></returns>
        public Result<List<ParamProduct>, ErrorModel> GetProductByProductCode(int productCode)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(Product.Properties.ProductId, productCode);
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Product), filter.GetPredicate()));
                Result<List<ParamProduct>, ErrorModel> product = ModelAssembler.CreateProduct(businessCollection);
                if (product is ResultError<List<ParamProduct>, ErrorModel>)
                {
                    return product;
                }
                else
                {
                    List<ParamProduct> resultValue = (product as ResultValue<List<ParamProduct>, ErrorModel>).Value;
                    if (resultValue.Count == 0)
                    {
                        errorModelListDescription.Add("No se encontraron productos.");
                        return new ResultError<List<ParamProduct>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));

                    }
                    else
                    {
                        return product;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add("Ocurrio un error inesperado en la consulta de productos. Comuniquese con el administrador");
                return new ResultError<List<ParamProduct>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs");
            }
        }

        /// <summary>
        /// Obtiene productos por el ramo comercial del producto
        /// </summary>
        /// <param name="productCode">Id del producto</param>
        /// <returns></returns>
        public Result<List<ParamProduct>, ErrorModel> GetProductsByPrefixCode(int prefixCode)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(Product.Properties.PrefixCode, prefixCode);
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Product), filter.GetPredicate()));
                Result<List<ParamProduct>, ErrorModel> product = ModelAssembler.CreateProduct(businessCollection);
                if (product is ResultError<List<ParamProduct>, ErrorModel>)
                {
                    return product;
                }
                else
                {
                    List<ParamProduct> resultValue = (product as ResultValue<List<ParamProduct>, ErrorModel>).Value;
                    if (resultValue.Count == 0)
                    {
                        errorModelListDescription.Add("No se encontraron productos.");
                        return new ResultError<List<ParamProduct>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));

                    }
                    else
                    {
                        return product;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add("Ocurrio un error inesperado en la consulta de productos. Comuniquese con el administrador");
                return new ResultError<List<ParamProduct>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs");
            }
        }
    }
}
