using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Integration.TempCommonService.DTOs;

namespace Sistran.Core.Integration.TempCommonService
{
    [ServiceContract]
    public interface ITempCommonIntegrationService
    {
        [OperationContract]
        PolicyDTO GetPolicyReinsuranceByPolicyIdEndorsementId(int policyId, int endorsementId);

        [OperationContract]
        List<ModuleDateDTO> GetModuleDates();

        [OperationContract]
        EndorsementDTO GetEndorsementByPolicyIdEndorsementId(int policyId, int endorsementId);

        /// <summary>
        /// GetReinsurerByName        
        /// </summary>
        /// <param name="name"></param>
        /// <param name="companyTypeCode"></param>
        /// <returns></returns>
        [OperationContract]
        List<IndividualDTO> GetReinsurerByName(string name, int reinsurance, int foreignReinsurance);


        /// <summary>
        /// GetModuleDate
        /// </summary>
        /// <param name="moduleDate"></param>
        /// <returns>ModuleDate</returns>
        [OperationContract]
        ModuleDateDTO GetModuleDate(ModuleDateDTO moduleDateDTO);

        /// <summary>
        /// GetAgentByName
        /// </summary>
        /// <param name="name"></param>
        /// <returns>List<AgentDTO></returns>
        [OperationContract]
        List<AgentDTO> GetAgentByName(string name);

        /// <summary>
        /// GetProducts
        /// </summary>
        /// <param name="prefixId"></param>
        /// <returns>List<Product></returns>
        [OperationContract]
        List<ProductDTO> GetProductsByPrefixId(int prefixId);
    }
}
