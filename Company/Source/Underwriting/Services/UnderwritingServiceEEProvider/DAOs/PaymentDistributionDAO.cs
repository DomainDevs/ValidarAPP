using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.DAOs
{
    public class PaymentDistributionDAO
    {
        /// <summary>
        /// Obtener datos de las cuotas
        /// Se expone aca para la inicialización de cache del proceso de cotizacion
        /// </summary> 
        /// <param name="paymentPlanId">Identificador del plan de pago</param>
        /// <returns></returns>
        public List<PaymentDistribution> GetPaymentDistributionByPaymentPlanId(int paymentPlanId)
        {
            List<PaymentDistribution> paymentDistributions = new List<PaymentDistribution>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Core.Application.Product.Entities.PaymentDistribution.Properties.PaymentScheduleId);
            filter.Equal();
            filter.Constant(paymentPlanId);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Core.Application.Product.Entities.PaymentDistribution), filter.GetPredicate()));

            if (businessCollection != null)
            {
                paymentDistributions = ModelAssembler.CreatePaymentDistributions(businessCollection);
                return paymentDistributions;
            }
            return null;
        }
    }
}
