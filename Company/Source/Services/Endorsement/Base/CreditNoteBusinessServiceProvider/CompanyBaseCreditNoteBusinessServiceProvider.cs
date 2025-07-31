using Sistran.Company.Application.Base.Endorsement.CreditNoteBusinessService.EEProvider.Business;
using Sistran.Company.Application.Base.Endorsement.CreditNoteBusinessService.EEProvider.Resources;
using Sistran.Company.Application.Endorsement.CreditNoteBusinessService.Models;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Base.Endorsement.CreditNoteBusinessService.EEProvider;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;

namespace Sistran.Company.Application.Base.Endorsement.CreditNoteBusinessService.EEProvider
{
    public class CompanyBaseCreditNoteBusinessServiceProvider : BaseCreditNoteBusinessServiceProvider, ICompanyBaseCreditNoteBusinessService
    {
        public CreditNote Calculate(CreditNote creditNote)
        {
            throw new System.NotImplementedException();
        }

        public List<CompanyEndorsement> GetEndorsementsWithPremiumAmount(int policyId)
        {
            try
            {
                CreditNoteBusiness creditNoteBusiness = new CreditNoteBusiness();
                return creditNoteBusiness.GetEndorsementsWithPremiumAmount(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetEndorsementsWithPremiumAmount), ex);
            }
        }

        public List<CompanyRisk> GetRisksByPolicyIdEndorsementId(int policyId, int endorsementId)
        {
            try
            {
                CreditNoteBusiness creditNoteBusiness = new CreditNoteBusiness();
                return creditNoteBusiness.GetRisksByPolicyIdEndorsementId(policyId, endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetRisksByPolicyIdEndorsementId), ex);
            }
        }

        public decimal GetMaximumPremiumPercetToReturn(int policyId)
        {
            try
            {
                CreditNoteBusiness creditNoteBusiness = new CreditNoteBusiness();
                return creditNoteBusiness.GetMaximumPremiumPercetToReturn(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetMaximumPremiumPercetToReturn), ex);
            }
        }
    }
}