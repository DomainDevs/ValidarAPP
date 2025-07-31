using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPEN = Sistran.Core.Application.UniquePerson.Entities;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class PaymentMethodDAO
    {
        public IssuancePaymentMethod GetPaymentMethodByIndividualId(int individualId)
        {
            BusinessCollection businessCollection = null;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UPEN.IndividualPaymentMethod.Properties.IndividualId, typeof(UPEN.IndividualPaymentMethod).Name);
            filter.Equal();
            filter.Constant(individualId);
            filter.And();
            filter.Property(UPEN.IndividualPaymentMethod.Properties.Enabled, typeof(UPEN.IndividualPaymentMethod).Name);
            filter.Equal();
            filter.Constant(1);
            businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UPEN.IndividualPaymentMethod), filter.GetPredicate()));

            if (businessCollection.Count > 0)
            {
                return ModelAssembler.CreatePaymentMethod((UPEN.IndividualPaymentMethod)businessCollection[0]);
            }
            else
            {
                return null;
            }
        }
    }
}