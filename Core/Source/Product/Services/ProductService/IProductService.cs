
namespace Sistran.Core.Application.ProductServices
{
    using Sistran.Core.Application.ProductServices.Models;
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using CommonModel = CommonService.Models;
    
    [ServiceContract]
    public interface IProductServiceCore
    {
        #region productos
        /// <summary>
        /// Obtener lista de Productos
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <returns>Lista de Productos</returns>
        [OperationContract]
        List<Product> GetProducts(int prefixCode);

        /// <summary>
        /// Obtener Todos los Produtos
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Product> GetAllProducts();

        /// <summary>
        /// Obtener productos por agente y ramo comercial
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <returns></returns>
        [OperationContract]
        List<Product> GetProductsByAgentIdPrefixId(int agentId, int prefixId);

        /// <summary>
        /// Obtener producto por Identificador
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <returns>Producto</returns>
        [OperationContract]
        Product GetProductById(int id);

        /// <summary>
        /// Consultar productos de Cotizacion por ramo comercial
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <param name="isGreen">Solo productos verdes</param>
        /// <returns>Lista de productos</returns>
        [OperationContract]
        List<Product> GetProductsByPrefixIdIsGreen(int prefixId, bool isGreen);

        /// <summary>
        /// Consultar productos por ramo comercial, agente y con la bandera IsGreen activa
        /// </summary>
        /// <param name="agentId">Id del agente</param>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <param name="isGreen">Solo productos verdes</param>
        /// <returns>Lista de productos</returns>
        [OperationContract]
        List<Product> GetProductsByAgentIdPrefixIdIsGreen(int agentId, int prefixId, bool isGreen);

        /// <summary>
        /// Consultar productos por ramo comercial
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>       
        /// <returns>Lista de productos</returns>
        [OperationContract]
        List<Product> GetProductsByPrefixId(int prefixId);

        /// <summary>
        /// Obtener producto por id y ramo
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <param name="prefixId">Id Ramo Comercial</param>
        /// <returns>Producto</returns>
        [OperationContract]
        Product GetProductByProductIdPrefixId(int productId, int prefixId);

        /// <summary>
        /// Consultar productos por ramo comercial y descripcion
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>      
        /// <param name="description">Descripcion del producto</param>      
        /// <returns>Lista de productos</returns>
        //[OperationContract]
        //List<Product> GetProductsByPrefixIdByDescription(int prefixId, string description);

        /// <summary>
        ///Obtener productos por producto
        /// </summary>
        /// <param name="product">Producto</param>
        /// <returns></returns>
        [OperationContract]
        List<Product> GetProductsByProduct(Product product);

        /// <summary>
        /// Obtienen una lista de productos que utilizan el guion
        /// </summary>
        /// <param name="ScriptId">id del guion</param>
        /// <returns></returns>
        [OperationContract]
        List<Product> GetProductsByScriptId(int ScriptId);

        /// <summary>
        /// Obtiene informacion del producto relacionado con currency
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <returns>Producto</returns>
        //[OperationContract]
        //List<Product> GetMainProductByPrefixIdDescriptionProductId(int prefixId, string description, int productId);


        /// <summary>
        /// Obtiene Datos adicionales relacionados al Id del Producto
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        //[OperationContract]
        //Product GetDataAditionalByProductId(int productId);

        /// <summary>
        /// Obtener comisión de agente por producto
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <param name="agencyId">Id agencia</param>
        /// <param name="productId">Id producto</param>
        /// <returns>Comision agente</returns>
        [OperationContract]
        ProductAgencyCommiss GetCommissByAgentIdAgencyIdProductId(int agentId, int agencyId, int productId);

        /// <summary>
        /// Copiar un producto especificp
        /// </summary>
        /// <param name="copyProduct">Producto.</param>
        /// <returns></returns>
        [OperationContract]
        int CreateCopyProduct(CopyProduct copyProduct);


        /// <summary>
        /// Obtiene los Agentes del producto 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [OperationContract]
        List<ProductAgent> GetProductAgentByProductId(int productId);


        /// <summary>
        /// Consulta si la placa existe producto para agente
        /// </summary>
        /// <param name="agentId">identificador agente</param>
        /// <param name="prefixId">identificador de ramo</param>
        /// <param name="productId">identificador de producto</param>
        /// <returns></returns>
        [OperationContract]
        Boolean ExistProductAgentByAgentIdPrefixIdProductId(int agentId, int prefixId, int productId);

        /// <summary>
        /// generar Archivo del Producto
        /// </summary>
        /// <param name="id">Identificador del producto</param>
        /// <param name="fileName">Nombre Archivo</param>
        /// <returns></returns>
        //[OperationContract]
        //string GenerateFileToProduct(int id, string fileName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CommonModel.Currency> GetCurrenciesByProductId(int productId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [OperationContract]
        List<ProductCurrency> GetProductCurrencies(int productId);

        /// <summary>
        /// Obtiene los agentes que estan asociados al producto de acuerdo al Id del Agent
        /// </summary>
        /// <param name="agentId">Id del Agent</param>
        /// <param name="productId">Id del Producto</param>
        /// <returns>Agencias relacionadas con el producto</returns>  
        [OperationContract]
        List<ProductAgency> GetAgenciesByAgentIdDesciptionProductIdUserId(int agentId, string description, int productId, int userId);

        /// <summary>
        /// Obtiene si se debe calcular prima mínima para el producto
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [OperationContract]
        bool GetCalculateMinPremiumByProductId(int productId);

        #endregion
    }
}