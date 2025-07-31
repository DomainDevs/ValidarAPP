using Sistran.Core.Integration.UnderwritingReinsuranceWorkerServices;
using System;

namespace Sistran.Core.Application.UnderwritingReinsuranceWorkerServices.EEProvider.IntegrationProviders
{
    public class UnderwritingReinsuranceWorkerIntegrationServiceProvider : IUnderwritingReinsuranceWorkerIntegrationServices
    {
        public int ReinsuranceIssue(int policyId, int endorsementId, int userId)
        {
            try
            {
                return DelegateService.reinsuranceIntegrationService.ReinsuranceIssue(policyId, endorsementId, userId).ReinsuranceId;
            }
            catch (Exception ex)
            {
                return 0;
            }
            
        }
    }
}
