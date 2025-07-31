using Sistran.Company.Application.ProductServices.EEProvider.Assemblers;
using Sistran.Company.Application.ProductServices.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using PM = Sistran.Core.Application.ProductServices.Models;
using Sistran.Company.Application.Product.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;

namespace Sistran.Company.Application.ProductServices.EEProvider.Business
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductBusiness
    {
        Core.Application.ProductServices.EEProvider.ProductServiceEEProviderCore coreProvider;

        public ProductBusiness()
        {
            coreProvider = new Core.Application.ProductServices.EEProvider.ProductServiceEEProviderCore();
        }

        /// <summary>
        /// obtiene los productos a partir del ramo comercial
        /// </summary>
        /// <param name="prefixCode">id del ramo comercial</param>
        /// <returns></returns>
        public List<CompanyProduct> GetCompanyProducts(int prefixCode)
        {
            List<PM.Product> coreProducts = coreProvider.GetProducts(prefixCode);
            if (coreProducts != null)
            {
                return ModelAssembler.CreateCompanyProducts(coreProducts);
            }
            else
            {
                return null;
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
            List<PM.Product> coreProducts = coreProvider.GetProductsByAgentIdPrefixId(agentId, prefixId);
            return ModelAssembler.CreateCompanyProducts(coreProducts);

        }

        /// <summary>
        /// obtiene los productos a partir del ramo comercial y el id del agente
        /// </summary>
        /// <param name="agentId">id del agent</param>
        /// <param name="prefixId">id del ramo comercial</param>
        /// <param name="isGreen">Indica si el producto tiene la bandera verde</param>
        /// <returns></returns>
        public List<CompanyProduct> GetCompanyProductsByAgentIdPrefixIdIsGreen(int agentId, int prefixId, bool isGreen)
        {
            List<PM.Product> coreProducts = coreProvider.GetProductsByAgentIdPrefixIdIsGreen(agentId, prefixId, isGreen);
            return ModelAssembler.CreateCompanyProducts(coreProducts);

        }

        /// <summary>
        /// Obtener producto
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <returns>Producto</returns>
        public CompanyProduct GetCompanyProductById(int id)
        {
            PM.Product coreProduct = coreProvider.GetProductById(id);
            return ModelAssembler.CreateCompanyProduct(coreProduct);
        }

        /// <summary>
        /// Consultar productos por ramo comercial
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <param name="isGreen">Solo productos verdes</param>
        /// <returns>Lista de productos</returns>
        public List<CompanyProduct> GetCompanyProductsByPrefixIdIsGreen(int prefixId, bool isGreen)
        {
            List<PM.Product> coreProducts = coreProvider.GetProductsByPrefixIdIsGreen(prefixId, isGreen);
            return coreProducts.Select(ModelAssembler.CreateCompanyProduct).ToList();
        }

        /// <summary>
        /// Consultar productos por ramo comercial
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>        
        /// <returns>Lista de productos</returns>
        public List<CompanyProduct> GetCompanyProductsByPrefixId(int prefixId)
        {
            List<PM.Product> coreProducts = coreProvider.GetProductsByPrefixId(prefixId);
            return coreProducts.Select(ModelAssembler.CreateCompanyProduct).ToList();
        }

        /// <summary>
        /// Consultar todos los productos
        /// </summary>
        /// <returns></returns>
        public List<CompanyProduct> GetCompanyAllProducts()
        {
            List<PM.Product> coreProducts = coreProvider.GetAllProducts();
            return coreProducts.Select(ModelAssembler.CreateCompanyProduct).ToList();
        }

        /// <summary>
        /// Consultar todos los prv payment schedules
        /// </summary>
        /// <returns></returns>
        public List<int> GetPaymentSchedules()
        {
            List<int> paymentSchedules = new List<int>();
            BusinessCollection businessCollection = new BusinessCollection();
            businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CiaPaymentSchedule)));
            var listPrvPaymentSchedule = businessCollection.Cast<CiaPaymentSchedule>();
            foreach (var item in listPrvPaymentSchedule)
            {
                paymentSchedules.Add((int)item.PaymentScheduleId);
            }
           
            return paymentSchedules;
        }

        /// <summary>
        /// Obtener producto
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <param name="prefixId">Id Ramo Comercial</param>
        /// <returns>Producto</returns>
        public CompanyProduct GetCompanyProductByProductIdPrefixId(int productId, int prefixId)
        {
            PM.Product coreProduct = coreProvider.GetProductByProductIdPrefixId(productId, prefixId);
            return ModelAssembler.CreateCompanyProduct(coreProduct);
        }

        /// <summary>
        /// Consultar productos por ramo comercial y descripcion
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>      
        /// <param name="description">Descripcion del producto</param>      
        /// <returns>Lista de productos</returns>
        public List<CompanyProduct> GetCompanyProductsByPrefixIdByDescription(int prefixId, string description)
        {
            List<PM.Product> coreProducts = coreProvider.GetProductsByPrefixIdByDescription(prefixId, description);
            return coreProducts.Select(ModelAssembler.CreateCompanyProduct).ToList();
        }

        /// <summary>
        ///Obtener productos por producto
        /// </summary>
        /// <param name="product">Producto</param>
        /// <returns></returns>
        public List<CompanyProduct> GetCompanyProductsByProduct(CompanyProduct companyProduct)
        {
            PM.Product product = ModelAssembler.CreateProduct(companyProduct);
            List<PM.Product> coreProducts = coreProvider.GetProductsByProduct(product);
            return coreProducts.Select(ModelAssembler.CreateCompanyProduct).ToList();
        }

        /// <summary>
        /// Obtienen una lista de productos que utilizan el guion
        /// </summary>
        /// <param name="ScriptId">id del guion</param>
        /// <returns></returns>
        public List<CompanyProduct> GetCompanyProductsByScriptId(int ScriptId)
        {
            List<PM.Product> coreProducts = coreProvider.GetProductsByScriptId(ScriptId);
            return coreProducts.Select(ModelAssembler.CreateCompanyProduct).ToList();
        }

        ///// <summary>
        ///// Obtiene informacion principal del producto 
        ///// </summary>
        ///// <param name="productId">Id producto</param>
        ///// <returns>Producto</returns>
        //public List<CompanyProduct> GetCompanyMainProductByPrefixIdDescriptionProductId(int prefixId, string description, int productId)
        //{
        //    List<Product> coreProducts = coreProvider.GetMainProductByPrefixIdDescriptionProductId(prefixId, description, productId);
        //    return coreProducts.Select(ModelAssembler.CreateCompanyProduct).ToList();
        //}
        //public List<CompanyProductParametrization> GetCompanyProductParametrizationByPrefixIdDescriptionProductId(int prefixId, string description, int productId)
        //{
        //    List<Product> coreProducts = coreProvider.GetMainProductByPrefixIdDescriptionProductId(prefixId, description, productId);
        //    return coreProducts.Select(ModelAssembler.CreateCompanyProductParametrization).ToList();
        //}
        /// <summary>
        /// Obtiene Riesgo de cobertura de acuerdo al Id del Producto
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        //public CompanyProduct GetCompanyCoveredProductById(int productId)
        //{
        //    Product coreProduct = coreProvider.GetCoveredProductById(productId);
        //    return ModelAssembler.CreateCompanyProduct(coreProduct);
        //}

        ///// <summary>
        ///// Obtiene Datos adicionales relacionados al Id del Producto
        ///// </summary>
        ///// <param name="productId"></param>
        ///// <returns></returns>
        //public CompanyProduct GetCompanyDataAditionalByProductId(int productId)
        //{
        //    Product coreProduct = coreProvider.GetDataAditionalByProductId(productId);
        //    return ModelAssembler.CreateCompanyProduct(coreProduct);
        //}
    }
}
