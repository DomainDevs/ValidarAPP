using Sistran.Core.Integration.OperationQuotaServices.DTOs.Consortium;
using Sistran.Core.Integration.OperationQuotaServices.DTOs.OperationQuota;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Integration.OperationQuotaServices
{
    [ServiceContract]
    public interface IOperationQuotaIntegrationService
    {
        /// <summary>
        /// Inserts the apply endorsement operating quota event.
        /// </summary>
        /// <param name="operatingQuotaEventDTOs">The operating quota event dtos.</param>
        /// <returns></returns>
        [OperationContract]
        List<OperatingQuotaEventDTO> InsertApplyEndorsementOperatingQuotaEvent(List<OperatingQuotaEventDTO> operatingQuotaEventDTOs);

        /// <summary>
        /// Create Operating Quota Events
        /// </summary>
        /// <param name="operatingQuotaEventDTOs"></param>
        /// <returns></returns>
        [OperationContract]
        bool CreateOperatingQuotaEvents(List<OperatingQuotaEventDTO> operatingQuotaEventDTOs);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="lineBusinessId"></param>
        /// <returns></returns>
        [OperationContract]
        OperatingQuotaEventDTO GetOperatingQuotaEventByIndividualIdByLineBusinessId(int individualId, int lineBusinessId);

        /// <summary>
        /// Migra el cúmulo de reaseguros
        /// </summary>
        /// <param name="operatingQuotaEventDTOs"></param>
        /// <returns></returns>
        [OperationContract]
        bool MigrateReinsuranceCumulus(List<OperatingQuotaEventDTO> operatingQuotaEventDTOs);
        [OperationContract]
        bool InsertConsortium(List<RiskConsortiumDTO> consortiums);
        [OperationContract]
        List<RiskConsortiumDTO> GetRiskConsortiumbyPolicy(int endorsementid);

    }
}
