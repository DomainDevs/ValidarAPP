using Sistran.Core.Application.ProductServices.EEProvider.DAOs;
using Sistran.Core.Application.ProductServices.Models;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.ProductServices.EEProvider
{
    using CommonModel = CommonService.Models;
    using Model = Models;
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class ProductServiceEEProviderCore : IProductServiceCore
    {
        #region productos
        /// <summary>
        /// Copiar un producto especificp
        /// </summary>
        /// <param name="copyProduct">Producto.</param>
        /// <returns></returns>
        public virtual int CreateCopyProduct(CopyProduct copyProduct)
        {
            try
            {
                ProductDAO productDAO = new ProductDAO();
                return productDAO.CreateCopyProduct(copyProduct);
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
        public virtual List<Model.Product> GetProducts(int prefixCode)
        {
            try
            {
                ProductDAO productDAO = new ProductDAO();
                return productDAO.ListProduct(prefixCode);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public virtual List<Model.Product> GetAllProducts()
        {
            try
            {
                ProductDAO productDAO = new ProductDAO();
                return productDAO.GetProducts();
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
        public virtual List<Model.Product> GetProductsByAgentIdPrefixId(int agentId, int prefixId)
        {
            try
            {
                ProductDAO productDAO = new ProductDAO();
                return productDAO.GetProductsByAgentIdPrefixId(agentId, prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener comisión de agente por producto
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <param name="agencyId">Id agencia</param>
        /// <param name="productId">Id producto</param>
        /// <returns>Comision agente</returns>
        public virtual Model.ProductAgencyCommiss GetCommissByAgentIdAgencyIdProductId(int agentId, int agencyId, int productId)
        {
            try
            {
                ProductAgencyCommissDAO productAgencyCommissDAO = new ProductAgencyCommissDAO();
                return productAgencyCommissDAO.GetCommissByAgentIdAgencyIdProductId(agentId, agencyId, productId);
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
        public virtual Model.Product GetProductById(int id)
        {
            try
            {
                ProductDAO productDAO = new ProductDAO();
                return productDAO.GetProductById(id);
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
        /// <param name="isGreen">Solo productos verdes</param>
        /// <returns>Lista de productos</returns>
        public virtual List<Model.Product> GetProductsByPrefixIdIsGreen(int prefixId, bool isGreen)
        {
            try
            {
                ProductDAO productDAO = new ProductDAO();
                return productDAO.GetProductsByPrefixIdIsGreen(prefixId, isGreen);
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
        public virtual List<Model.Product> GetProductsByPrefixId(int prefixId)
        {
            try
            {
                ProductDAO productDAO = new ProductDAO();
                return productDAO.GetProductsByPrefixId(prefixId);
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
        /// <param name="prefixId">Id Ramo Comercial</param>
        /// <returns>Producto</returns>
        public virtual Model.Product GetProductByProductIdPrefixId(int productId, int prefixId)
        {
            try
            {
                ProductDAO productDAO = new ProductDAO();
                return productDAO.GetProductByProductIdPrefixId(productId, prefixId);
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
        public virtual List<Model.Product> GetProductsByPrefixIdByDescription(int prefixId, string description)
        {
            try
            {
                ProductDAO productDAO = new ProductDAO();
                return productDAO.GetProductsByPrefixIdByDescription(prefixId, description);
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
        public virtual List<Model.Product> GetProductsByProduct(Model.Product product)
        {
            try
            {
                ProductDAO productDAO = new ProductDAO();
                return productDAO.GetProductsByProduct(product);
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
        public virtual List<Model.Product> GetProductsByScriptId(int ScriptId)
        {
            try
            {
                ProductDAO productDAO = new ProductDAO();
                return productDAO.GetProductsByScriptId(ScriptId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene los Agentes del producto 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public virtual List<Model.ProductAgent> GetProductAgentByProductId(int productId)
        {
            try
            {
                ProductDAO productDAO = new ProductDAO();
                return productDAO.GetProductAgentByProductId(productId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        /// <summary>
        /// Gets the agentcy by product by indivual identifier.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="individualId">The individual identifier.</param>
        /// <param name="agencyId">The agency identifier.</param>
        /// <returns></returns>
        public virtual Model.ProductAgencyCommiss GetAgentcyByProductByIndivualId(int productId, int individualId, Int16 agencyId)
        {
            ProductDAO productDAO = new ProductDAO();
            return productDAO.GetAgentcyByProductByIndivualId(productId, individualId, agencyId);
        }

        /// <summary>
        /// Obtener lista de monedas asociadas a un producto
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <returns></returns>
        public virtual List<CommonModel.Currency> GetCurrenciesByProductId(int productId)
        {
            try
            {
                ProductCurrencyDAO productCurrencyDAO = new ProductCurrencyDAO();
                return productCurrencyDAO.GetCurrenciesByProductId(productId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Obtener lista de monedas asociadas a un producto
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <returns></returns>
        public virtual List<ProductCurrency> GetProductCurrencies(int productId)
        {
            try
            {
                ProductCurrencyDAO productCurrencyDAO = new ProductCurrencyDAO();
                return productCurrencyDAO.GetProductCurrencies(productId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        /// <summary>
        /// Exists the product agent by agent identifier prefix identifier product identifier.
        /// </summary>
        /// <param name="agentId">The agent identifier.</param>
        /// <param name="prefixId">The prefix identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public virtual Boolean ExistProductAgentByAgentIdPrefixIdProductId(int agentId, int prefixId, int productId)
        {
            try
            {
                ProductDAO productDAO = new ProductDAO();
                return productDAO.ExistProductAgentByAgentIdPrefixIdProductId(agentId, prefixId, productId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        /// <summary>
        /// Obtiene si se debe calcular prima mínima para el producto
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public virtual bool GetCalculateMinPremiumByProductId(int productId)
        {
            try
            {
                ProductDAO dao = new ProductDAO();
                return dao.GetCalculateMinPremiumByProductId(productId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene los agentes que estan asociados al producto de acuerdo al Id del Agent
        /// </summary>
        /// <param name="agentId">Id del Agent</param>
        /// <param name="productId">Id del Producto</param>
        /// <returns>Agencias relacionadas con el producto</returns>  
        public virtual List<ProductAgency> GetAgenciesByAgentIdDesciptionProductIdUserId(int agentId, string description, int productId, int userId)
        {
            try
            {
                ProductDAO productDAO = new ProductDAO();
                return productDAO.GetAgenciesByAgentIdDesciptionProductIdUserId(agentId, description, productId, userId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Consultar productos por ramo comercial, agente y con la bandera IsGreen activa
        /// </summary>
        /// <param name="agentId">Id del agente</param>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <param name="isGreen">Solo productos verdes</param>
        /// <returns>Lista de productos</returns>
        public List<Model.Product> GetProductsByAgentIdPrefixIdIsGreen(int agentId, int prefixId, bool isGreen)
        {
            try
            {
                ProductDAO productDAO = new ProductDAO();
                return productDAO.GetProductsByAgentIdPrefixIdIsGreen(agentId, prefixId, isGreen);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion
    }
}