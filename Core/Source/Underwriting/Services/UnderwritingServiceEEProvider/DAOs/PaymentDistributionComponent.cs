using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.Models.Distribution;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Linq;
using PRODEN = Sistran.Core.Application.Product.Entities;
namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class PaymentDistributionComponent
    {
        public static List<PaymentDistributionComp> GetPaymentDistributionComponents(int paymentScheduleId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PRODEN.CoPaymentDistributionComponent.Properties.PaymentScheduleId, typeof(PRODEN.CoPaymentDistributionComponent).Name);
            filter.Equal().Constant(paymentScheduleId);
            List<PRODEN.CoPaymentDistributionComponent> entityPaymentDistributionComponents = DataFacadeManager.Instance.GetDataFacade().List<PRODEN.CoPaymentDistributionComponent>(filter.GetPredicate()).Cast<PRODEN.CoPaymentDistributionComponent>().ToList();
            return ModelAssembler.CreatePaymentDistributionComponents(entityPaymentDistributionComponents);
        }
    }
}
