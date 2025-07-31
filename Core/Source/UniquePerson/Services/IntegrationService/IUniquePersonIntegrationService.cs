using Sistran.Core.Application.UniquePerson.IntegrationService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Enums;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.UniquePerson.IntegrationService
{
    [ServiceContract]
    public interface IUniquePersonIntegrationService
    {
        [OperationContract]
        IntegrationAgency GetAgencyByAgentIdAgencyId(int agentId, int agencyId);

        [OperationContract]
        IntegrationAgent GetAgentByAgentId(int agentId);

        [OperationContract]
        IntegrationInsured GetInsuredByInsuredCode(int insuredCode);

        [OperationContract]
        IntegrationInsured GetInsuredByIndividualId(int IndividualId);

        [OperationContract]
        List<IntegrationEconomicGroup> GetEconomicGroupByDocument(string groupName, string documentNo);

        [OperationContract]
        IntegrationEconomicGroup GetEconomicGroupById(int Id);

        [OperationContract]
        List<IntegrationInsured> GetInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchTpe, CustomerType custormerType);

        [OperationContract]
        List<IntegrationInsured> GetHoldersByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType);

        [OperationContract]
        CompanyDTO GetCompanyByIndividualId(int individualId);

        [OperationContract]
        ReinsurerDTO GetReinsurerByIndividualId(int individualId);

        [OperationContract]
        List<IntegrationEconomicGroupDetail> GetEconomicGroupDetailByIndividual(int individualId);

        [OperationContract]
        List<IntegrationEconomicGroupDetail> GetEconomicGroupDetailById(int economicGroupId);

    }
}