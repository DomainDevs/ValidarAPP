using Sistran.Company.Application.Base.Endorsement.CreditNoteApplicationService.EEProvider.Assemblers;
using Sistran.Company.Application.Endorsement.CreditNoteApplicationService.DTOs;
using System.Collections.Generic;

namespace Sistran.Company.Application.Base.Endorsement.CreditNoteApplicationServices.EEProvider.Business
{
    public class BaseCreditNoteBusiness
    {
        public CreditNoteDTO Calculate(CreditNoteDTO creditNote)
        {
            throw new System.NotImplementedException();
        }

        public List<EndorsementDTO> GetEndorsementsWithPremiumAmount(int policyId)
        {
            return DTOAssembler.CreateEndorsements(
                DelegateService.baseCreditNoteBusinessService.GetEndorsementsWithPremiumAmount(policyId));
        }

        public List<RiskDTO> GetRisksByPolicyIdEndorsementId(int policyId, int endorsementId)
        {
            return DTOAssembler.CreateRisks(
                DelegateService.baseCreditNoteBusinessService.GetRisksByPolicyIdEndorsementId(
                    policyId, endorsementId));
        }
    }
}