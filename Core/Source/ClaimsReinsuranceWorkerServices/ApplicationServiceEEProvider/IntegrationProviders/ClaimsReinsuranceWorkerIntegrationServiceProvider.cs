using Sistran.Core.Integration.ClaimsReinsuranceWorkerServices;
using System;

namespace Sistran.Core.Application.ClaimsReinsuranceWorkerServices.EEProvider.IntegrationProviders
{
    public class ClaimsReinsuranceWorkerIntegrationServiceProvider : IClaimsReinsuranceWorkerIntegrationServices
    {

        public bool ReinsuranceClaim(int claimId, int claimModifyId, int userId)
        {
            try
            {
                return DelegateService.reinsuranceIntegrationService.ReinsuranceClaim(claimId, claimModifyId, userId).IsReinsured;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ReinsurancePayment(int paymentRequestId, int userId)
        {
            try
            {
                return DelegateService.reinsuranceIntegrationService.ReinsurancePayment(paymentRequestId, userId).IsReinsured;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ReinsuranceCancellationPayment(int paymentRequestId, int userId)
        {
            try
            {
                return DelegateService.reinsuranceIntegrationService.ReinsuranceCancellationPayment(paymentRequestId, userId).IsReinsured;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ValidateClaimPaymentRequestReinsurance(int paymentRequestId)
        {
            try
            {
                return DelegateService.reinsuranceIntegrationService.ValidateClaimPaymentRequestReinsurance(paymentRequestId);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ValidateEndorsementReinsurance(int endorsementId)
        {
            try
            {
                return DelegateService.reinsuranceIntegrationService.ValidateEndorsementReinsurance(endorsementId);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
