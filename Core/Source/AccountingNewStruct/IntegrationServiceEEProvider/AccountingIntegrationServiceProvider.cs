using Sistran.Core.Integration.AccountingServices.DTOs.Accounting;
using Sistran.Core.Integration.AccountingServices.DTOs.Reversion;
using Sistran.Core.Integration.AccountingServices.EEProvider.DAOs;
using System.Collections.Generic;

namespace Sistran.Core.Integration.AccountingServices.EEProvider
{
    public class AccountingIntegrationServiceProvider : IAccountingIntegrationService
    {
        /// <summary>
        /// Gets the quotas by endorsement identifier.
        /// </summary>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <returns></returns>
        public PaymentAppliedDTO GetQuotasByEndorsementId(int endorsementId)
        {
            return ApplicationPremiumDAO.GetApplicationPremiumByEndorsementFilter(endorsementId);
        }

        /// <summary>
        /// Gets the application component by endorsement identifier.
        /// </summary>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <returns></returns>
        public List<QuotaComponentsDTO> GetApplicationComponentByEndorsementId(int endorsementId)
        {
            return ApplicationPremiumDAO.GetApplicationComponentByEndorsementId(endorsementId);
        }

        /// <summary>
        /// Gets the application premium base by endorsement identifier.
        /// </summary>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <returns></returns>
        public List<PremiumBaseDTO> GetApplicationPremiumBaseByEndorsementId(int endorsementId)
        {
            return ApplicationPremiumDAO.GetApplicationPremiumBaseByEndorsementId(endorsementId);
        }

        /// <summary>
        /// Gets the application premium by endorsement identifier.
        /// </summary>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <returns></returns>
        public List<QuotaPremiumDTO> GetApplicationPremiumByEndorsementId(int endorsementId)
        {
            return ApplicationPremiumDAO.GetApplicationPremiumByEndorsementId(endorsementId);
        }

    }
}