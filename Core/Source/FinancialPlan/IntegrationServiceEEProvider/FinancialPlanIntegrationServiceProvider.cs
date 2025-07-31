
using Sistran.Core.Integration.AccountingServices;
using Sistran.Core.Integration.AccountingServices.DTOs.Accounting;
using Sistran.Core.Integration.AccountingServices.EEProvider.DAOs;

namespace Sistran.Core.Integration.UnderwritingServices.EEProvider
{
    public class AccountingIntegrationServiceProvider : IAccountingIntegrationService
    {
        public PaymentPlanDTO GetQuotasByEndorsementId(int endorsementId)
        {
            return ApplicationPremiumDAO.GetApplicationPremiumByEndorsementFilter(endorsementId);
        }
    }
}