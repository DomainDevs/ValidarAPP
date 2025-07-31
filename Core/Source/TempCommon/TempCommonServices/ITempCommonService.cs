using Sistran.Core.Application.TempCommonServices.DTOs;
using Sistran.Core.Application.TempCommonServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System.Collections.Generic;
using System.ServiceModel;


namespace Sistran.Core.Application.TempCommonServices
{
    [ServiceContract]
    public interface ITempCommonService
    {
        #region Product

        /// <summary>
        /// GetProducts
        /// </summary>
        /// <param name="prefixId"></param>
        /// <returns>List<Product></returns>
        [OperationContract]
        List<Product> GetProductsByPrefixId(int prefixId);

        #endregion

        #region ModuleDate
        /// <summary>
        /// GetModuleDate
        /// </summary>
        /// <param name="moduleDate"></param>
        /// <returns>ModuleDate</returns>
        [OperationContract]
        ModuleDate GetModuleDate(ModuleDate moduleDate);

        /// <summary>
        /// GetModuleDates
        /// </summary>
        /// <returns>List<ModuleDate></returns>
        [OperationContract]
        List<ModuleDate> GetModuleDates();

        /// <summary>
        /// UpdateModuleDate
        /// </summary>
        /// <param name="moduleDate"></param>
        /// <returns>ModuleDate</returns>
        [OperationContract]
        ModuleDate UpdateModuleDate(ModuleDate moduleDate);

        #endregion

        #region Currency

        /// <summary>
        /// GetCurrencyLocal
        /// </summary>
        /// <returns>int</returns>
        [OperationContract]
        int GetCurrencyLocal();

        #endregion


        #region BankBranch

        /// <summary>
        /// GetBankBranchsByBranchId
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns>List<BankBranch></returns>
        [OperationContract]
        List<BankBranch> GetBankBranchsByBranchId(int branchId);

        #endregion

        #region Person

        /// <summary>
        /// GetReinsurerByName        
        /// </summary>
        /// <param name="name"></param>
        /// <param name="companyTypeCode"></param>
        /// <returns></returns>
        [OperationContract]
        List<IndividualDTO> GetReinsurerByName(string name, int reinsurance, int foreignReinsurance);

        /// <summary>
        /// GetReinsurerByDocumentNumber        
        /// </summary>
        /// <param name="number"></param>
        /// <param name="companyTypeCode"></param>
        /// <returns></returns>
        [OperationContract]
        List<IndividualDTO> GetReinsurerByDocumentNumber(string number, int companyTypeCode);

        /// <summary>
        /// GetSuppliersByDocumentNumber
        /// </summary>
        /// <param name="number"></param>
        /// <returns>List<IndividualDTO></returns>
        [OperationContract]
        List<IndividualDTO> GetSuppliersByDocumentNumber(string number);

        /// <summary>
        /// GetSuppliersByName
        /// </summary>
        /// <param name="name"></param>
        /// <returns>List<IndividualDTO></returns>
        [OperationContract]
        List<IndividualDTO> GetSuppliersByName(string name);

        /// <summary>
        /// GetInsuredByDocumentNumber
        /// </summary>
        /// <param name="number"></param>
        /// <returns>List<IndividualDTO></returns>
        [OperationContract]
        List<IndividualDTO> GetInsuredByDocumentNumber(string number);

        /// <summary>
        /// GetInsuredByName
        /// </summary>
        /// <param name="name"></param>
        /// <returns>List<IndividualDTO></returns>
        [OperationContract]
        List<IndividualDTO> GetInsuredByName(string name);

       

        /// <summary>
        /// GetAgentByDocumentNumber         
        /// </summary>
        /// <param name="number"></param>
        /// <returns>List<AgentDTO></returns>
        [OperationContract]
        List<AgentDTO> GetAgentByDocumentNumber(string number);

        /// <summary>
        /// GetAgentByName
        /// </summary>
        /// <param name="name"></param>
        /// <returns>List<AgentDTO></returns>
        [OperationContract]
        List<AgentDTO> GetAgentByName(string name);

        /// <summary>
        /// GetCommissionDiscountAgreementByAgentId
        /// Verifica si el agente tiene convenio de descuento de comisiones        
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns></returns>
        [OperationContract]
        bool GetCommissionDiscountAgreementByAgentId(int agentId);

        /// <summary>
        /// GetPersonTypeByPaymentOrderEnable
        /// </summary>
        /// <returns>List<PersonType></returns>
        [OperationContract]
        List<PersonType> GetPersonTypeByPaymentOrderEnable();

        /// <summary>
        /// GetPersonTypeByPaymentOrderEnable
        /// </summary>
        /// <returns>List<PersonType></returns>
        [OperationContract]
        List<PersonType> GetPersonTypeByPaymentOrderEnableFilter();

        /// <summary>
        /// GetPersonTypesByBillEnabled
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<PersonType> GetPersonTypesByBillEnabled();

        /// <summary>
        /// GetPersonsByDocumentNumber
        /// </summary>
        /// <param name="documentNumber"></param>
        /// <returns>List<IndividualDTO></returns>
        [OperationContract]
        List<IndividualDTO> GetPersonsByDocumentNumber( string documentNumber, bool typePerson = false);

        /// <summary>
        /// GetPersonsByName
        /// </summary>
        /// <param name="name"></param>
        /// <returns>List<IndividualDTO></returns>
        [OperationContract]
        List<IndividualDTO> GetPersonsByName(string name);

        #endregion

        #region Reinsurance

        /// <summary>
        /// GetEndorsementByPolicyId
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        [OperationContract]
        EndorsementDTO GetEndorsementByPolicyIdEndorsementId(int policyId, int endorsementId);

        [OperationContract]
        List<EndorsementDTO> GetEndorsementByPolicyId(int policyId);

        /// <summary>
		/// Devuelve una poliza Reaseguro
		/// </summary>
		/// <param name="policyId">id de poliza </param>
		/// <param name="endorsementId">id de endoso</param>
		/// <returns>poliza Reaseguro</returns>
		[OperationContract]
        Policy GetPolicyReinsuranceByPolicyIdEndorsementId(int policyId, int endorsementId);

        #endregion

    }
}
