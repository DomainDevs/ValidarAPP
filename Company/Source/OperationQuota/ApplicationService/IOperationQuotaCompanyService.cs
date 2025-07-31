using Sistran.Company.Application.OperationQuotaServices.DTOs;
using Sistran.Core.Application.RulesScriptsServices.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.OperationQuotaCompanyServices
{
    [ServiceContract]
    public interface IOperationQuotaCompanyService
    {
        [OperationContract]

        List<AgentProgramDTO> GetAgentProgramDTOs();

        [OperationContract]
        List<UtilityDetailsDTO> GetUtilityDTOs();
        [OperationContract]
        List<IndicatorConceptDTO> GetIndicatorConceptsDTOs();

        [OperationContract]
        List<ReportListSisconcDTO> GetReportListSisconcDTOs();

        [OperationContract]
        List<RiskCenterDTO> GetRiskCenterListDTOs();

        [OperationContract]
        List<RestrictiveDTO> GetRestrictiveListDTOs();

        [OperationContract]
        List<PromissoryNoteSignatureDTO> GetPromissoryNoteSignatureDTOs();

        [OperationContract]
        List<AutomaticQuotaOperationDTO> GetAutomaticQuotaOperation(int Id);

        [OperationContract]
        AutomaticQuotaDTO SaveAutomaticQuotaGeneralJSON(AutomaticQuotaDTO automaticQuotaDTO, bool validatePolicies = true);

        [OperationContract]
        ThirdDTO SaveAutomaticQuotaThirdJSON(ThirdDTO thirdDTO, bool validatePolicies = true);

        [OperationContract]
        AutomaticQuotaDTO SaveAutomaticQuotaUtilityJSON(List<UtilityDTO> utilityDTO, AutomaticQuotaDTO automaticDTO, bool validatePolicies = true);

        [OperationContract]
        AutomaticQuotaDTO SaveAutomaticQuotaGeneral(AutomaticQuotaDTO automaticQuotaDto, bool validatePolicies = true);

        [OperationContract]
        AutomaticQuotaDTO UpdateAutomaticQuotaGeneralJSON(AutomaticQuotaDTO automaticQuotaDTO, bool validatePolicies = true);

        [OperationContract]
        ThirdDTO UpdateAutomaticQuotaThirdJSON(ThirdDTO thirdDTO, int id, bool validatePolicies = true);

        [OperationContract]
        AutomaticQuotaDTO UpdateAutomaticQuotaUtilityJSON(List<UtilityDTO> utilityDTO, AutomaticQuotaDTO automaticDTO, int id, bool validatePolicies = true);

        [OperationContract]
        List<AutomaticQuotaOperationDTO> GetAutomaticQuotaOperationByParentId(int ParentId);

        [OperationContract]
        AutomaticQuotaDTO ExecuteCalculate(int id, List<DynamicConcept> dynamicProperties);

        [OperationContract]
        AutomaticQuotaDTO GetAutomaticQuotaDeserealizado(int Id);

        [OperationContract]
        bool SaveProspect(AutomaticQuotaDTO automaticQuotaDTO);

        [OperationContract]
        bool DeleteAutomaticOperationsByParentId(int parentId);

        [OperationContract]
        bool DeleteAutomaticOperation(int id);

        [OperationContract]
        List<AutomaticQuotaDTO> GetAutomaticQuota(int id);
    }
}