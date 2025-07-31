using Sistran.Core.Integration.AccountingServices.DTOs.Accounting;
using Sistran.Core.Integration.AccountingServices.DTOs.Reversion;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Integration.AccountingServices
{
    [ServiceContract]
    public interface IAccountingIntegrationService
    {

        /// <summary>
        /// Gets the quotas by endorsement identifier.
        /// </summary>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <returns></returns>
        [OperationContract]
        PaymentAppliedDTO GetQuotasByEndorsementId(int endorsementId);

        /// <summary>
        /// Gets the application component by endorsement identifier.
        /// </summary>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <returns></returns>
        [OperationContract]
        List<QuotaComponentsDTO> GetApplicationComponentByEndorsementId(int endorsementId);

        /// <summary>
        /// Gets the application premium base by endorsement identifier.
        /// </summary>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <returns></returns>
        [OperationContract]
        List<PremiumBaseDTO> GetApplicationPremiumBaseByEndorsementId(int endorsementId);

        /// <summary>
        /// Gets the application premium by endorsement identifier.
        /// </summary>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <returns></returns>
        [OperationContract]
        List<QuotaPremiumDTO> GetApplicationPremiumByEndorsementId(int endorsementId);
    }
}