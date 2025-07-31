using Sistran.Core.Application.UniquePerson.IntegrationService.Models;
using Sistran.Core.Application.UniquePerson.IntegrationService.Provider.Assemblers;
using Sistran.Core.Application.UniquePerson.IntegrationService.Provider.Resources;
using Sistran.Core.Application.UniquePersonService.V1.Enums;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Sistran.Core.Application.UniquePerson.IntegrationService.Provider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class UniquePersonIntegrationServiceProvider : IUniquePersonIntegrationService
    {
        public IntegrationAgency GetAgencyByAgentIdAgencyId(int agentId, int agencyId)
        {
            try
            {
                return IntegrationAssembler.CreateIntegrationAgency(DelegateService.uniquePersonService.GetAgencyByAgentIdAgentAgencyId(agentId, agencyId));
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorGetAgency, ex);
            }
        }

        public IntegrationAgent GetAgentByAgentId(int agentId)
        {
            try
            {
                return IntegrationAssembler.CreateIntegrationAgent(DelegateService.uniquePersonService.GetAgentByIndividualId(agentId));
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorGetAgency, ex);
            }
        }

        public IntegrationInsured GetInsuredByInsuredCode(int insuredCode)
        {
            try
            {
                return null;
                //return IntegrationAssembler.CreateIntegrationInsured(DelegateService.uniquePersonService.GetInsuredByInsuredCode(insuredCode));
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorGettingInsuredbyCode, ex);
            }
        }

        public List<IntegrationInsured> GetInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchTpe, CustomerType custormerType)
        {
            try
            {
                return null;
                //return IntegrationAssembler.CreateIntegrationInsuredList(DelegateService.uniquePersonService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchTpe, custormerType));
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorGettingInsuredList, ex);
            }
        }

        public IntegrationInsured GetInsuredByIndividualId(int individualId)
        {
            try
            {
                return IntegrationAssembler.CreateIntegrationInsured(DelegateService.uniquePersonService.GetInsuredByIndividualId(individualId));
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorGetInsured, ex);

            }
        }

        public List<IntegrationInsured> GetHoldersByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType)
        {
            try
            {
                return null;
                // return IntegrationAssembler.CreateIntegrationInsuredList(DelegateService.uniquePersonService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType));
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorGettingInsuredList, ex);
            }
        }

        public CompanyDTO GetCompanyByIndividualId(int individualId)
        {
            try
            {
                return IntegrationAssembler.CreateCompanyByIndividualId(DelegateService.uniquePersonService.GetCompanyByIndividualId(individualId));
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorGettingInsuredList, ex);
            }
        }

        public List<IntegrationEconomicGroup> GetEconomicGroupByDocument(string groupName, string documentNo)
        {
            try
            {
                return DelegateService.uniquePersonService.GetEconomicGroupByDocument(groupName, documentNo).ToIntegrationDTOs().ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public IntegrationEconomicGroup GetEconomicGroupById(int id)
        {
            IntegrationEconomicGroup economicGroup = new IntegrationEconomicGroup();
            economicGroup = DelegateService.uniquePersonService.GetGroupEconomicById(id).ToIntegrationDTOs().ToList().FirstOrDefault();
            
            if(economicGroup == null)
            {
                economicGroup = new IntegrationEconomicGroup();
            }

            return economicGroup;
        }

        public List<IntegrationEconomicGroupDetail> GetEconomicGroupDetailByIndividual(int individualId)
        {
            return DelegateService.uniquePersonService.GetEconomicGroupDetailByIndividual(individualId).ToIntegrationDTOs().ToList();
        }

        public List<IntegrationEconomicGroupDetail> GetEconomicGroupDetailById(int economicGroupId)
        {
            return DelegateService.uniquePersonService.GetEconomicGroupDetailById(economicGroupId).ToIntegrationDTOs().ToList();
        }

        public ReinsurerDTO GetReinsurerByIndividualId(int individualId)
        {
            try
            {
                return IntegrationAssembler.CreateIntegrationReinsurerDTO(DelegateService.uniquePersonService.GetReInsurerByIndividualId(individualId));
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}