using Sistran.Company.Application.ProductServices.DTOs;
using Sistran.Company.Application.ProductServices.EEProvider.Business;
using Sistran.Company.Application.ProductServices.EEProvider.DAOs;
using Sistran.Company.Application.ProductServices.Models;
using Sistran.Core.Application.ProductServices.EEProvider;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.ProductServices.EEProvider
{

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class ProductServiceEEProvider : ProductServiceEEProviderCore, IProductService
    {

        /// <summary>
        /// Obtener producto
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <param name="prefixId">Id Ramo Comercial</param>
        /// <returns>Producto</returns>
        public CompanyProduct GetCompanyProductByProductIdPrefixId(int productId, int prefixId)
        {
            try
            {
                ProductBusiness productBIS = new ProductBusiness();
                return productBIS.GetCompanyProductByProductIdPrefixId(productId, prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// obtiene los productos a partir del ramo comercial y el id del agente
        /// </summary>
        /// <param name="agentId">id del agent</param>
        /// <param name="prefixId">id del ramo comercial</param>
        /// <returns></returns>
        public List<CompanyProduct> GetCompanyProductsByAgentIdPrefixId(int agentId, int prefixId)
        {
            try
            {
                ProductBusiness productBIS = new ProductBusiness();
                return productBIS.GetCompanyProductsByAgentIdPrefixId(agentId, prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// obtiene los productos a partir del ramo comercial y el id del agente
        /// </summary>
        /// <param name="agentId">id del agent</param>
        /// <param name="prefixId">id del ramo comercial</param>
        /// <returns></returns>
        public List<CompanyProduct> GetCompanyProductsByAgentIdPrefixIdIsGreen(int agentId, int prefixId, bool isGreen)
        {
            try
            {
                ProductBusiness productBIS = new ProductBusiness();
                return productBIS.GetCompanyProductsByAgentIdPrefixIdIsGreen(agentId, prefixId, isGreen);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener producto
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <returns>Producto</returns>
        public CompanyProduct GetCompanyProductById(int id)
        {
            try
            {
                ProductBusiness productBIS = new ProductBusiness();
                return productBIS.GetCompanyProductById(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtienen una lista de productos que utilizan el guion
        /// </summary>
        /// <param name="ScriptId">id del guion</param>
        /// <returns></returns>
        public List<CompanyProduct> GetCompanyProductsByScriptId(int ScriptId)
        {
            try
            {
                ProductBusiness productBIS = new ProductBusiness();
                return productBIS.GetCompanyProductsByScriptId(ScriptId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// obtiene los productos a partir del ramo comercial
        /// </summary>
        /// <param name="prefixCode">id del ramo comercial</param>
        /// <returns></returns>
        public List<CompanyProduct> GetCompanyProducts(int prefixCode)
        {
            try
            {
                ProductBusiness productBIS = new ProductBusiness();
                return productBIS.GetCompanyProducts(prefixCode);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Consultar todos los productos
        /// </summary>
        /// <returns></returns>
        public List<CompanyProduct> GetCompanyAllProducts()
        {
            try
            {
                ProductBusiness productBIS = new ProductBusiness();
                return productBIS.GetCompanyAllProducts();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

      

        /// <summary>
        /// Consultar productos por ramo comercial
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>        
        /// <returns>Lista de productos</returns>
        public List<CompanyProduct> GetCompanyProductsByPrefixId(int prefixId)
        {
            try
            {
                ProductBusiness productBIS = new ProductBusiness();
                return productBIS.GetCompanyProductsByPrefixId(prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }



        /// <summary>
        /// Consultar productos por ramo comercial y descripcion
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>      
        /// <param name="description">Descripcion del producto</param>      
        /// <returns>Lista de productos</returns>
        public List<CompanyProduct> GetCompanyProductsByPrefixIdByDescription(int prefixId, string description)
        {
            try
            {
                ProductBusiness productBIS = new ProductBusiness();
                return productBIS.GetCompanyProductsByPrefixIdByDescription(prefixId, description);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        ///Obtener productos por producto
        /// </summary>
        /// <param name="product">Producto</param>
        /// <returns></returns>
        //public List<CompanyProduct> GetCompanyProductsByProduct(CompanyProduct product)
        //{
        //    try
        //    {
        //        ProductBusiness productBIS = new ProductBusiness();
        //        return productBIS.GetCompanyProductsByProduct(product);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException(ex.Message, ex);
        //    }
        //}

        /// <summary>
        /// Obtiene informacion principal del producto 
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <returns>Producto</returns>
        //public List<CompanyProduct> GetCompanyMainProductByPrefixIdDescriptionProductId(int prefixId, string description, int productId)
        //{
        //    try
        //    {
        //        ProductBusiness productBIS = new ProductBusiness();
        //        return productBIS.GetCompanyMainProductByPrefixIdDescriptionProductId(prefixId, description, productId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException(ex.Message, ex);
        //    }
        //}
        //public List<CompanyProductParametrization> GetCompanyProductParametrizationByPrefixIdDescriptionProductId(int prefixId, string description, int productId)
        //{
        //    try
        //    {
        //        ProductBusiness productBIS = new ProductBusiness();
        //        return productBIS.GetCompanyProductParametrizationByPrefixIdDescriptionProductId(prefixId, description, productId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException(ex.Message, ex);
        //    }
        //}
        /// <summary>
        /// Obtiene Riesgo de cobertura de acuerdo al Id del Producto
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        //public CompanyProduct GetCompanyCoveredProductById(int productId)
        //{
        //    try
        //    {
        //        ProductBusiness productBIS = new ProductBusiness();
        //        return productBIS.GetCompanyCoveredProductById(productId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException(ex.Message, ex);
        //    }
        //}

        ///// <summary>
        ///// Obtiene Datos adicionales relacionados al Id del Producto
        ///// </summary>
        ///// <param name="productId"></param>
        ///// <returns></returns>
        //public CompanyProduct GetCompanyDataAditionalByProductId(int productId)
        //{
        //    try
        //    {
        //        ProductBusiness productBIS = new ProductBusiness();
        //        return productBIS.GetCompanyDataAditionalByProductId(productId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException(ex.Message, ex);
        //    }

        //}

        /// <summary>
        /// Consultar productos por ramo comercial
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <param name="isGreen">Solo productos verdes</param>
        /// <returns>Lista de productos</returns>
        public List<CompanyProduct> GetCompanyProductsByPrefixIdIsGreen(int prefixId, bool isGreen)
        {
            try
            {
                ProductBusiness productBIS = new ProductBusiness();
                return productBIS.GetCompanyProductsByPrefixIdIsGreen(prefixId, isGreen);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<int> GetPaymentSchedules()
        {
                try
                {
                    ProductBusiness productBIS = new ProductBusiness();
                    return productBIS.GetPaymentSchedules();
                }
                catch (Exception ex)
                {
                    throw new BusinessException(ex.Message, ex);
                }
        }

        //public CompanyProduct GetCompanyProductByCoreProduct(Core.Application.ProductServices.Models.Product coreProduct)
        //{
        //    try
        //    {
        //        CompanyProductDAO companyProductDAO = new CompanyProductDAO();
        //        return companyProductDAO.GetCompanyProductByCoreProduct(coreProduct);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException(ex.Message, ex);
        //    }


        //}

        //public CompanyProduct GetValidDaysByIdProduct(int idProduct)
        //{
        //    try
        //    {
        //        CompanyProductDAO companyProductDAO = new CompanyProductDAO();
        //        return companyProductDAO.GetValidDaysByIdProduct(idProduct);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException(ex.Message, ex);
        //    }


        //}

        public SubCoverageDTO GetCompanySubCoverageRiskTypeByProductIdPrefixId(int productId, int prefixId)
        {
            try
            {
                CompanyProductDAO companyProductDAO = new CompanyProductDAO();
                return companyProductDAO.GetCompanySubCoverageRiskTypeByProductIdPrefixId(productId, prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}