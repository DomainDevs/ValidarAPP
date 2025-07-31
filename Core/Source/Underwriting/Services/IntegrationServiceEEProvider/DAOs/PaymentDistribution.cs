using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Integration.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using System.Collections.Generic;
using System.Linq;
using PRODEN = Sistran.Core.Application.Product.Entities;
namespace Sistran.Core.Integration.UnderwritingServices.EEProvider.DAOs
{
    public class PaymentDistribution
    {
        public static List<PaymentDistributionCompDTO> GetPaymentDistributionComponents(int paymentScheduleId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PRODEN.CoPaymentDistributionComponent.Properties.PaymentScheduleId, typeof(PRODEN.CoPaymentDistributionComponent).Name);
            filter.Equal().Constant(paymentScheduleId);
            List<PRODEN.CoPaymentDistributionComponent> coPaymentDistributionComponents = DataFacadeManager.Instance.GetDataFacade().List<PRODEN.CoPaymentDistributionComponent>(filter.GetPredicate()).Cast<PRODEN.CoPaymentDistributionComponent>().ToList();
            return DTOAssembler.CreatePaymentDistributionComponents(coPaymentDistributionComponents);
        }
    }
}
