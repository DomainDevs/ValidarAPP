using Sistran.Core.Integration.ReinsuranceOperatingQuotaServices.DTOs;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Integration.ReinsuranceOperatingQuotaServices
{
    [ServiceContract]
    public interface IReinsuranceOperatingQuotaIntegrationServices
    {
        /// <summary>
        /// Get Cumulus By Individual
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="lineBusiness"></param>
        /// <param name="dateCumulus"></param>
        /// <param name="IsFuture"></param>
        /// <returns></returns>
        [OperationContract]
        List<OperatingQuotaEventDTO> GetCumulusCoveragesByIndividual(int individualId, int lineBusiness, DateTime dateCumulus, bool IsFuture, int subLineBusiness, int prefixCd, bool validatePriorityRetention = false);

        /// <summary>
        /// Create Operating Quota Events
        /// </summary>
        /// <param name="operatingQuotaEventDTOs"></param>
        /// <returns></returns>
        [OperationContract]
        bool CreateOperatingQuotaEvents(List<OperatingQuotaEventDTO> operatingQuotaEventDTOs);

        /// <summary>
        /// Create Operating Quota Events
        /// </summary>
        /// <param name="operatingQuotaEventDTOs"></param>
        /// <returns></returns>
        [OperationContract]
        bool MigrateReinsuranceCumulus(List<OperatingQuotaEventDTO> operatingQuotaEventDTOs);

        /// <summary>
        /// GetExistingGroupEconomicEventByeconomicGroupId
        /// </summary>
        /// <param name="economicGroupId"></param>
        /// <returns></returns>
        List<EconomicGroupEventDTO> GetExistingGroupEconomicEventByeconomicGroupId(int economicGroupId);
    }
}
