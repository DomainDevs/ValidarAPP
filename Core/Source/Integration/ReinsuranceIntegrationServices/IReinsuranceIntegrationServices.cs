using Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Sistran.Core.Integration.ReinsuranceIntegrationServices
{
    [ServiceContract]
    public interface IReinsuranceIntegrationServices
    {
        /// <summary>
        /// Reaseguros de emisión
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [OperationContract]
        ReinsuranceDTO ReinsuranceIssue(int policyId, int endorsementId, int userId);

        /// <summary>
        /// Reaseguros de siniestro en línea
        /// </summary>
        /// <param name="claimId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [OperationContract]
        ReinsuranceDTO ReinsuranceClaim(int claimId, int claimModifyId, int userId);

        /// <summary>
        /// Reaseguros de pagos en línea
        /// </summary>
        /// <param name="paymentRequestId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [OperationContract]
        ReinsuranceDTO ReinsurancePayment(int paymentRequestId, int userId);

        /// <summary>
        /// Reaseguros de cancelaciones de recobros y pagos en linea
        /// </summary>
        /// <param name="paymentRequestId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [OperationContract]
        ReinsuranceDTO ReinsuranceCancellationPayment(int paymentRequestId, int userId);

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

        /// <summary>
        /// Lanza la migración de cumulos de reaseguros
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool MigrateReinsuranceCumulus();
    }
}
