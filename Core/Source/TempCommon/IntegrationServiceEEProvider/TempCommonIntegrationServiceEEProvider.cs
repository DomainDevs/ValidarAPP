using Sistran.Core.Integration.TempCommon.EEProvider.Assemblers;
using Sistran.Core.Integration.TempCommonService;
using Sistran.Core.Integration.TempCommonService.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Core.Integration.TempCommon.EEProvider
{
    public class TempCommonIntegrationServiceEEProvider : ITempCommonIntegrationService
    {
        public PolicyDTO GetPolicyReinsuranceByPolicyIdEndorsementId(int policyId, int endorsementId)
        {
            return DelegateService.tempCommonService.GetPolicyReinsuranceByPolicyIdEndorsementId(policyId, endorsementId).ToDTO();
        }

        public List<ModuleDateDTO> GetModuleDates()
        {
            return DelegateService.tempCommonService.GetModuleDates().ToDTOs().ToList();
        }

        public EndorsementDTO GetEndorsementByPolicyIdEndorsementId(int policyId, int endorsementId)
        {
            return DelegateService.tempCommonService.GetEndorsementByPolicyIdEndorsementId(policyId, endorsementId).ToDTO();
        }

        public List<IndividualDTO> GetReinsurerByName(string name, int reinsurance, int foreignReinsurance)
        {
            return DelegateService.tempCommonService.GetReinsurerByName(name, reinsurance , foreignReinsurance).ToDTOs().ToList();
        }

        public ModuleDateDTO GetModuleDate(ModuleDateDTO moduleDateDTO)
        {
            return DelegateService.tempCommonService.GetModuleDate(moduleDateDTO.ToModel()).ToDTO();
        }

        public List<AgentDTO> GetAgentByName(string name)
        {
            return DelegateService.tempCommonService.GetAgentByName(name).ToDTOs().ToList();
        }

        public List<ProductDTO> GetProductsByPrefixId(int prefixId)
        {
            return DelegateService.tempCommonService.GetProductsByPrefixId(prefixId).ToDTOs().ToList();
        }
    }
}
