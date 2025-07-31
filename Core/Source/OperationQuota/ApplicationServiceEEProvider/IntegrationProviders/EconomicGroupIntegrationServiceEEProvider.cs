
using Sistran.Core.Integration.OperationQuotaServices.DTOs.OperationQuota;
using System;
using System.Collections.Generic;
using Sistran.Core.Integration.OperationQuotaServices;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Assemblers;
using Sistran.Core.Integration.OperationQuotaServices.DTOs.EconomicGroup;
using System.Linq;

namespace Sistran.Core.Application.OperationQuotaServices.EEProvider.IntegrationProviders
{
    public class EconomicGroupIntegrationServiceEEProvider : IEconomicGroupIntegrationService
    {
        public OperatingQuotaEventDTO GetCumuloCupoEconomicGroupByIndividualIdByLineBusinessId(int individualId, int lineBusinessId)
        {
            OperatingQuotaEventDTO operatingQuotaEventDTO = new OperatingQuotaEventDTO();
            return operatingQuotaEventDTO = DTOAssembler.CreateOperatingQuotaEventDTOs(DelegateService.economicGroupService.GetCumuloCupoEconomicGroupByIndividualIdByLineBusinessId(individualId, lineBusinessId));
        }

        public List<EconomicGroupEventDTO> GetExistingGroupEconomicEventByeconomicGroupId(int economicGroupId)
        {
            return DelegateService.economicGroupService.GetExistingGroupEconomicEventByeconomicGroupId(economicGroupId).ToIntegrationDTOs().ToList();
        }
    }
}
