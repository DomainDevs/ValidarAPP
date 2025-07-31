using System;
using System.Collections.Generic;
using Sistran.Company.Application.Base.Endorsement.CreditNoteApplicationService.EEProvider.Assemblers;
using Sistran.Company.Application.Base.Endorsement.CreditNoteApplicationService.EEProvider.Resources;
using Sistran.Company.Application.Base.Endorsement.CreditNoteApplicationServices.EEProvider.Business;
using Sistran.Company.Application.Endorsement.CreditNoteApplicationService.DTOs;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using USERV = Sistran.Core.Application.UnderwritingServices.Models;


namespace Sistran.Company.Application.Base.Endorsement.CreditNoteApplicationService.EEProvider
{
    public class BaseCreditNoteApplicationServiceProvider : IBaseCreditNoteApplicationService
    {
        public CreditNoteDTO Calculate(CreditNoteDTO creditNote)
        {
            throw new System.NotImplementedException();
        }

        public List<EndorsementDTO> GetEndorsementsWithPremiumAmount(int policyId)
        {
            try
            {
                BaseCreditNoteBusiness baseCreditNoteBusiness = new BaseCreditNoteBusiness();
                return baseCreditNoteBusiness.GetEndorsementsWithPremiumAmount(policyId);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetEndorsementsWithPremiumAmount), ex);
            }
        }

        public List<RiskDTO> GetRisksByPolicyIdEndorsementId(int policyId, int endorsementId)
        {
            try
            {
                BaseCreditNoteBusiness baseCreditNoteBusiness = new BaseCreditNoteBusiness();
                return baseCreditNoteBusiness.GetRisksByPolicyIdEndorsementId(policyId, endorsementId);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetRisksByPolicyIdEndorsementId), ex);
            }
        }

        public EndorsementDTO GetTemporalEndorsementByPolicyId(int policyId)
        {
            try
            {
                USERV.Endorsement endorsement = DelegateService.endorsementBaseService.GetTemporalEndorsementByPolicyId(policyId);
                return DTOAssembler.CreateEndorsementDTO(endorsement);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetRisksByPolicyIdEndorsementId), ex);
            }
        }

    }
}