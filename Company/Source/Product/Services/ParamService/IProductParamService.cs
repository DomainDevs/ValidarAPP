namespace Sistran.Company.Application.ProductParamService
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using Sistran.Company.Application.ModelServices.Models.Param;
    using Sistran.Company.Application.ModelServices.Models.Product;
    using Sistran.Core.Application.ProductParamService;

    /// <summary>
    /// Interfaz para ProductParamService
    /// </summary>
    [ServiceContract]
    public interface IProductParamService: IProductParamServiceCore
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefixId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CiaParamProductServiceModel> GetCiaProductsByPrefixId(int prefixId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [OperationContract]
        List<CiaParamProductServiceModel> GetCiaProductsByProduct(CiaParamProductServiceModel product);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefixId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CiaParamProduct2GServiceModel> GetProduct2gByPrefix(int prefixId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefixId"></param>
        /// <returns></returns>
        //[OperationContract]
        //List<CiaParamAssistanceTypeServiceModel> GetCiaAssistanceTypeByPrefix(int prefixId);

        /// <summary>
        /// Copiar un producto especificp
        /// </summary>
        /// <param name="copyProduct">Producto.</param>
        /// <returns></returns>
        [OperationContract]
        int CreateCopyProduct(CiaParamCopyProductServiceModel ciaParamCopyProductServiceModel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currencies"></param>
        /// <returns></returns>
        [OperationContract]
        List<CiaParamFinancialPlanServiceModel> GetPaymentScheduleByCurrencies(List<int> currencies);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<CiaParamBeneficiaryTypeServiceModel> GetBeneficiaryType();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="coverGroupId"></param>
        /// <param name="coverageId"></param>
        /// <param name="beneficiaryTypeCd"></param>
        /// <param name="lineBusinnessCd"></param>
        /// <returns></returns>
        [OperationContract]
        List<CiaParamDeductiblesCoverageServiceModel> GetDeductiblesByProductId(int productId, int coverGroupId, int coverageId, int beneficiaryTypeCd, int lineBusinnessCd);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [OperationContract]
        CiaParamProductServiceModel SaveProduct(CiaParamProductServiceModel product);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [OperationContract]
        string GenerateFileToProducts(string fileName);

        [OperationContract]
        string GenerateFileToProduct(int productId, string fileName);

        [OperationContract]
        List<CiaParamLimitsRCServiceModel> GetLimitsRc(List<int> policyTypeIds, int productId, int prefixCd);

        [OperationContract]
        List<CiaParamCommercialClassServiceModel> GetRiskCommercialClass(int productId);

        [OperationContract]
        List<CiaParamFormServiceModel> GetProductForm(int productId);

        [OperationContract]
        List<CiaParamDeductibleProductServiceModel> GetProductDeductiblesByPrefix(int productId, int prefixCode);

        [OperationContract]
        void SaveAdditionalData(List<CiaParamCommercialClassServiceModel> listRiskCommercialClass, List<CiaParamLimitsRCServiceModel> listCiaParamLimitsRCServiceModel, List<CiaParamDeductibleProductServiceModel> listCiaParamDeductibleProductServiceModel, List<CiaParamFormServiceModel> listCiaParamFormServiceModel, int productId);

		//[OperationContract]
  //      List<CiaParamAgentServiceModel> GetProductAgentByProductId(int productId);
        [OperationContract]
        CiaParamSummaryAgentServiceModel GetProductAgentByProductId(int productId, int prefixId);

        [OperationContract]
        List<CiaParamAgentServiceModel> GetProductAgentByProductIdByIndividualId(int productId, int individualId);		

        [OperationContract]
        CiaParamProductServiceModel SaveAgents(List<CiaParamAgentServiceModel> agents, int productId);

        [OperationContract]
        string SaveAllAgents(int prefixId, int productId, bool assigned);

        [OperationContract]
        bool ValidatePolicyByProductId(int productId, int riskId, int coverId);
		 
		[OperationContract]
        List<CiaParamAgentServiceModel> GetAgent(int agentId, string description, int prefixId, int idagenttye, int idgroupagent);
    }

}
