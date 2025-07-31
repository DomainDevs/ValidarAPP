using Sistran.Core.Application.ClaimServices.DTOs.Salvage;
using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using System;
using System.Collections.Generic;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Business.Salvage
{
    public class SaleBusiness
    {
        public List<PaymentQuotaDTO> CalculateSaleAgreedPlan(DateTime dateStart, decimal totalSale, int payments, string currencyDescription)
        {
            List<PaymentQuotaDTO> paymentQuotas = new List<PaymentQuotaDTO>();

            decimal paymentValue = Decimal.Round(totalSale, 2) / payments;
            paymentValue = Decimal.Round(paymentValue, 2);

            for (int i = 0; i < payments; i++)
            {
                paymentQuotas.Add(DTOAssembler.CreatePaymentQuota(i + 1, dateStart, currencyDescription, paymentValue));

                dateStart = dateStart.AddMonths(1);
            }

            return paymentQuotas;
        }
    }
}
