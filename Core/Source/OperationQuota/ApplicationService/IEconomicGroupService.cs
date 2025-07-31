using Sistran.Core.Application.OperationQuotaServices.DTOs.EconomicGroup;
using Sistran.Core.Application.OperationQuotaServices.DTOs.OperationQuota;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.OperationQuotaServices
{
    [ServiceContract]
    public interface IEconomicGroupService
    {
        [OperationContract]
        EconomicGroupEventDTO CreateEconomicGroupEvent(EconomicGroupEventDTO economicGroupEventDTO);

        [OperationContract]
        List<EconomicGroupEventDTO> AssigendIndividualToEconomicGroupEvent(List<EconomicGroupEventDTO> economicGroupEventDTOs);

        [OperationContract]
        OperatingQuotaEventDTO GetCumuloCupoEconomicGroupByIndividualIdByLineBusinessId(int individualId, int lineBusinessId);

        [OperationContract]
        List<EconomicGroupEventDTO> GetExistingGroupEconomicEventByeconomicGroupId(int economicGroupId);
    }
}
