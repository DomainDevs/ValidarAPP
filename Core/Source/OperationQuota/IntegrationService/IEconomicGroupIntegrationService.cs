using Sistran.Core.Integration.OperationQuotaServices.DTOs.EconomicGroup;
using Sistran.Core.Integration.OperationQuotaServices.DTOs.OperationQuota;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.OperationQuotaServices
{
    [ServiceContract]
    public interface IEconomicGroupIntegrationService
    {
        [OperationContract]
        OperatingQuotaEventDTO GetCumuloCupoEconomicGroupByIndividualIdByLineBusinessId(int individualId, int lineBusinessId);

        [OperationContract]
        List<EconomicGroupEventDTO> GetExistingGroupEconomicEventByeconomicGroupId(int economicGroupId);
    }
}
