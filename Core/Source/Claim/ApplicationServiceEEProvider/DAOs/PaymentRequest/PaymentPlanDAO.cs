using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.PaymentRequest;
using Sistran.Core.Application.Utilities.DataFacade;
using CLMEN = Sistran.Core.Application.Claims.Entities;
using PAYMEN = Sistran.Core.Application.Claims.Entities;
using System.Collections.Generic;


namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.PaymentRequest
{
    public class PaymentPlanDAO
    {
        public PaymentPlan CreatePaymentPlan(PaymentPlan paymentPlan)
        {
            if (paymentPlan != null)
            {
                PAYMEN.PaymentPlan entityPaymentPlan = EntityAssembler.CreatePaymentPlan(paymentPlan);

                entityPaymentPlan = (PAYMEN.PaymentPlan)DataFacadeManager.Insert(entityPaymentPlan);

                paymentPlan.Id = entityPaymentPlan.PaymentPlanCode;

                paymentPlan.PaymentQuotas = CreatePaymentQuotas(paymentPlan.PaymentQuotas, paymentPlan.Id);
            }

            return paymentPlan;
        }

        public List<PaymentQuota> CreatePaymentQuotas(List<PaymentQuota> paymentQuotas, int paymentPlanId)
        {
            foreach (PaymentQuota paymentQuota in paymentQuotas)
            {
                PAYMEN.PaymentSchedule entityPaymentSchedule = EntityAssembler.CreatePaymentSchedule(paymentQuota, paymentPlanId);

                entityPaymentSchedule = (PAYMEN.PaymentSchedule)DataFacadeManager.Insert(entityPaymentSchedule);

                paymentQuota.Id = entityPaymentSchedule.PaymentScheduleCode;
            }

            return paymentQuotas;
        }
    }
}
