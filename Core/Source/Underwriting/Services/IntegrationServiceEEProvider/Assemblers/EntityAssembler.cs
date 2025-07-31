using Sistran.Core.Integration.UndewritingIntegrationServices.Models;
using System;
using ISSEN = Sistran.Core.Application.Issuance.Entities;

namespace Sistran.Core.Integration.UnderwritingServices.EEProvider.Assemblers
{
    public static class EntityAssembler
    {
        internal static ISSEN.PayerPayment CreatePayerPaymententity(PayerPayment payerPayment)
        {
            return new ISSEN.PayerPayment(payerPayment.EndorsementId, payerPayment.PolicyId, payerPayment.PaymentNum, payerPayment.PayerId)
            {
                AgtPayExpDate = payerPayment.AgtPayExpDate,
                Amount = payerPayment.Amount,
                EndorsementId = payerPayment.EndorsementId,
                LocalAmount = payerPayment.LocalAmount,
                MainAmount = payerPayment.MainAmount,
                MainLocalAmount = payerPayment.MainLocalAmount,
                PayerId = payerPayment.PayerId,
                PayerPaymentId = payerPayment.PayerPaymentId,
                PayExpDate = payerPayment.PayExpDate,
                PaymentNum = payerPayment.PaymentNum,
                PaymentPercentage = payerPayment.PaymentPercentage,
                PaymentState = payerPayment.PaymentState,
                PolicyId = payerPayment.PolicyId
            };
        }
    }
}
