using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Application.ReinsuranceOperatingQuotaServices.EEProvider.Assemblers;
using Sistran.Core.Integration.ReinsuranceOperatingQuotaServices;
using Sistran.Core.Integration.ReinsuranceOperatingQuotaServices.DTOs;
using Sistran.Core.Integration.ReinsuranceOperatingQuotaServices.Enums;

namespace Sistran.Core.Application.ReinsuranceOperatingQuotaServices.EEProvider.IntegrationProviders
{
    public class ReinsuranceOperatingQuotaIntegrationServiceEEProvider : IReinsuranceOperatingQuotaIntegrationServices
    {
        public List<OperatingQuotaEventDTO> GetCumulusCoveragesByIndividual(int individualId, int lineBusiness, DateTime dateCumulus, bool IsFuture, int subLineBusiness, int prefixCd, bool validatePriorityRetention = false)
        {
            FilterOperationQuotaDTO filterOperationQuotaDTO = new FilterOperationQuotaDTO();
            filterOperationQuotaDTO.IndividualId = individualId;
            filterOperationQuotaDTO.LineBusiness = lineBusiness;
            filterOperationQuotaDTO.DateCumulus = dateCumulus;
            filterOperationQuotaDTO.IsFuture = IsFuture;
            filterOperationQuotaDTO.SubLineBusiness = subLineBusiness;
            filterOperationQuotaDTO.PrefixCd = prefixCd;
            return DTOAssembler.CreateOperatingQuotaEventsIntegrationDTO(DelegateService.operationQuotaService.GetCumulusCoveragesByIndividual(DTOAssembler.CreateFilterOperationQuotaIntegrationDTO(filterOperationQuotaDTO), validatePriorityRetention));
        }

        public bool CreateOperatingQuotaEvents(List<OperatingQuotaEventDTO> operatingQuotaEventDTOs)
        {   
            return DelegateService.operationQuotaIntegrationService.CreateOperatingQuotaEvents(operatingQuotaEventDTOs.CreateOQINTDTOOperatingQuotaByOperatingQuotaEventDTOs());
        }

        public bool MigrateReinsuranceCumulus(List<OperatingQuotaEventDTO> operatingQuotaEventDTOs)
        {
            return DelegateService.operationQuotaIntegrationService.MigrateReinsuranceCumulus(operatingQuotaEventDTOs.CreateOQINTDTOOperatingQuotaByOperatingQuotaEventDTOs());
        }

        public List<EconomicGroupEventDTO> GetExistingGroupEconomicEventByeconomicGroupId(int economicGroupId)
        {
            return DelegateService.economicGroupIntegrationService.GetExistingGroupEconomicEventByeconomicGroupId(economicGroupId).ToIntegrationDTOs().ToList();
        }
    }
}