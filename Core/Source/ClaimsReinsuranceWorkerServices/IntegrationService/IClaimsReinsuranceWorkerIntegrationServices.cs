using System.ServiceModel;

namespace Sistran.Core.Integration.ClaimsReinsuranceWorkerServices
{
    [ServiceContract]
    public interface IClaimsReinsuranceWorkerIntegrationServices
    {
        [OperationContract]
        bool ReinsuranceClaim(int claimId, int claimModifyId, int userId);

        [OperationContract]
        bool ReinsurancePayment(int paymentRequestId, int userId);

        [OperationContract]
        bool ReinsuranceCancellationPayment(int paymentRequestId, int userId);

        /// <summary>
        /// Valida si la solicitud de pago de siniestros está reasegurada
        /// </summary>
        /// <param name="paymentRequestId"></param>
        /// <returns></returns>
        [OperationContract]
        bool ValidateClaimPaymentRequestReinsurance(int paymentRequestId);

        /// <summary>
        /// Valida si el endoso está reasegurado
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <returns></returns>
        [OperationContract]
        bool ValidateEndorsementReinsurance(int endorsementId);
    }
}