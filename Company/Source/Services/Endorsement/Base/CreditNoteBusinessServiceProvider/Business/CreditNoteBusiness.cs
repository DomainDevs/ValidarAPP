using Sistran.Company.Application.Base.Endorsement.CreditNoteBusinessService.EEProvider.Resources;
using Sistran.Company.Application.Base.Endorsement.CreditNoteBusinessService.EEProvider.Services;
using Sistran.Company.Application.Base.Endorsement.CreditNoteBusinessService.EEProvider.Views;
using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.Endorsement.CreditNoteBusinessService.Models;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Issuance.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Company.Application.Base.Endorsement.CreditNoteBusinessService.EEProvider.Business
{
    public class CreditNoteBusiness
    {
        BaseBusinessCia provider;
        public CreditNoteBusiness()
        {
            provider = new BaseBusinessCia();
        }


        public CreditNote Calculate(CreditNote creditNote)
        {
            throw new System.NotImplementedException();
        }

        public List<CompanyEndorsement> GetEndorsementsWithPremiumAmount(int policyId)
        {
            return DelegateService.underwritingService.GetCoPolicyEndorsementsWithPremiumByPolicyId(policyId);
        }

        public List<CompanyRisk> GetRisksByPolicyIdEndorsementId(int policyId, int endorsementId)
        {
            return DelegateService.underwritingService.GetRiskByPolicyIdEndorsmentId(policyId, endorsementId);
        }

        public decimal GetMaximumPremiumPercetToReturn(int policyId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Policy.Properties.PolicyId, typeof(Policy).Name);
            filter.Equal();
            filter.Constant(policyId);
            filter.And();
            filter.Property(PrefixBusinessType.Properties.Enabled, typeof(PrefixBusinessType).Name);
            filter.Equal();
            filter.Constant(1);

            PrefixBusinessTypeView prefixBusinessTypeView = new PrefixBusinessTypeView();
            ViewBuilder builder = new ViewBuilder("PrefixBusinessTypeView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, prefixBusinessTypeView);
            DataFacadeManager.Dispose();
            if (prefixBusinessTypeView.PrefixBusinessTypes.Count > 0)
            {
                PrefixBusinessType prefixBusinessType = prefixBusinessTypeView.PrefixBusinessTypes.Cast<PrefixBusinessType>().First();
                return prefixBusinessType.MaxRate;
            }
            throw new BusinessException(Errors.ErrorGetMaximumPremiumPercetToReturnNotFound);
        }
    }
}
