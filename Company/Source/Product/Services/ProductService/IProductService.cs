namespace Sistran.Company.Application.ProductServices
{
    using Sistran.Company.Application.ProductServices.DTOs;
    using Sistran.Company.Application.ProductServices.Models;
    using Sistran.Core.Application.ProductServices;
    using System.Collections.Generic;
    using System.ServiceModel;

    /// <summary>
    /// Implemetacion de productos
    /// </summary>
    [ServiceContract]
    public interface IProductService : IProductServiceCore
    {
        #region productos
        /// <summary>
        /// Gets the company product by product identifier prefix identifier.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="prefixId">The prefix identifier.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyProduct GetCompanyProductByProductIdPrefixId(int productId, int prefixId);

        /// <summary>
        /// Gets the company product by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyProduct GetCompanyProductById(int id);


        /// <summary>
        /// Gets the company products by agent identifier prefix identifier.
        /// </summary>
        /// <param name="agentId">The agent identifier.</param>
        /// <param name="prefixId">The prefix identifier.</param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyProduct> GetCompanyProductsByAgentIdPrefixId(int agentId, int prefixId);

        /// <summary>
        /// Gets the company products by agent identifier prefix identifier.
        /// </summary>
        /// <param name="agentId">The agent identifier.</param>
        /// <param name="prefixId">The prefix identifier.</param>
        /// <param name="isGreen">Indicates if its green product.</param>
        /// <returns>List of products</returns>
        [OperationContract]
        List<CompanyProduct> GetCompanyProductsByAgentIdPrefixIdIsGreen(int agentId, int prefixId, bool isGreen);

        /// <summary>
        /// Gets the company products by prefix identifier.
        /// </summary>
        /// <param name="prefixId">The prefix identifier.</param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyProduct> GetCompanyProductsByPrefixId(int prefixId);

        /// <summary>
        /// Gets the company products by script identifier.
        /// </summary>
        /// <param name="ScriptId">The script identifier.</param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyProduct> GetCompanyProductsByScriptId(int ScriptId);

        /// <summary>
        /// Gets the company products
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<CompanyProduct> GetCompanyAllProducts();

        /// <summary>
        /// Gets the prv company products
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<int> GetPaymentSchedules();

        /// <summary>
        /// Gets the company sub coverage risk type by product identifier prefix identifier.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="prefixId">The prefix identifier.</param>
        /// <returns></returns>
        [OperationContract]
        SubCoverageDTO GetCompanySubCoverageRiskTypeByProductIdPrefixId(int productId, int prefixId);
        #endregion
        //#region cpt
        //[OperationContract]
        //CompanyProduct GetCompanyProductByCoreProduct(Product coreProduct);
        //[OperationContract]
        //CompanyProduct GetValidDaysByIdProduct(int idProduct);
        //#endregion cpt
    }
}