using Sistran.Core.Application.Transports.CreditNote.BusinessService.EEProvider.Assemblers;
using Sistran.Core.Application.Transports.Endorsement.CreditNote.BusinessServices.Models;
using Sistran.Core.Application.Transports.Endorsement.CreditNote.BusinessServices;

using Sistran.Core.Framework.Queries;
using PARAM = Sistran.Core.Application.Parameters.Entities;
using System.Collections.Generic;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.Utilities.DataFacade;
using System.ServiceModel;

namespace Sistran.Core.Application.Transports.CreditNote.BusinessService.EEProvider
{
    
    public class CreditNoteBusinessServiceProvider : ICreditNoteBusinessService
    {
        public List<EndorsementType> GetEndorsmenteTypesHasQuotation()
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(PARAM.EndorsementType.Properties.HasQuotation, typeof(PARAM.EndorsementType).Name);
            filter.Equal();
            filter.Constant(true);

            BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(PARAM.EndorsementType), filter.GetPredicate());

            if (businessCollection != null)
            {
                return ModelAssembler.GetEndorsmenteTypesHasQuotation(businessCollection);
            }
            else
            {
                return null;
            }
        }

        //public List<EndorsementType> GetEndorsmenteTypesHasQuotation()
    }
}
