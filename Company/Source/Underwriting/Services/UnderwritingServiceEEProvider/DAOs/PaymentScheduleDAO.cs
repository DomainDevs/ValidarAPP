using Sistran.Company.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Product.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.DAOs
{
    public class PaymentScheduleDAO
    {
        /// <summary>
        /// Obtener el calendario de pagos según el Id del plan de pago
        /// </summary>
        /// <param name="paymentPlanId"></param>
        /// <returns></returns>
        public CompanyPaymentSchedule GetPaymentScheduleByPaymentPlanId(int paymentPlanId)
        {
            PrimaryKey key = PaymentSchedule.CreatePrimaryKey(paymentPlanId);
            PaymentSchedule paymentSchedule = (PaymentSchedule)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            return ModelAssembler.CreatePaymentSchedule(paymentSchedule);
        }
    }
}