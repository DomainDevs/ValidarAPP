using COMTRAEN = Sistran.Company.Application.Transports.Endorsement.CreditNote.ApplicationServices.DTOs;
using Sistran.Company.Application.Transports.Endorsement.CreditNote.ApplicationServices.EEProvider.Resources;
using Sistran.Company.Application.Transports.Transport.ApplicationServices.EEProvider.Assemblers;
using Sistran.Company.Application.Transports.TransportApplicationService.DTOs;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Company.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.Transports.Endorsement.CreditNote.ApplicationServices.EEProvider.Business
{
    public class BusinessCreditNote
    {
        public COMTRAEN.RiskDTO GetTransportsByPolicyIdByEndorsementId(int policyId, int endorsementId)
        {
            try
            {
                List<TransportDTO> transportDTO = DelegateService.transportsService.GetCompanyTransportsByPolicyIdEndorsementId(policyId, endorsementId);
                return DTOAssembler.CreateCreditNoteTransports(transportDTO);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetAvaibleEndorsementsByPolicyId), ex);
            }
        }

        public COMTRAEN.CreditNoteDTO GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(int policyId, int endorsementId, int riskId)
        {
            List<CoverageDTO> CoveragesDTO = DelegateService.transportsService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, endorsementId, riskId);
            return DTOAssembler.CreateCoverageCreditNote(CoveragesDTO);
        }

        public COMTRAEN.CreditNoteDTO CreateTemporal(COMTRAEN.CreditNoteDTO creditNoteDTO)
        {
            CompanyPolicy companyPolicy = ModelAssembler.CreateCompanyPolicy(creditNoteDTO);
            CompanyRisk companyRisk = ModelAssembler.CreateCompanyRisk(creditNoteDTO.Risk.First());
            CompanyCoverage companyCoverage = ModelAssembler.CreateCompanyCoverage(creditNoteDTO.Coverage.First());
            CompanyPolicy policy = DelegateService.creditNoteService.CreateEndorsementCreditNote
                (companyPolicy, companyRisk, companyCoverage, creditNoteDTO.PremiumToReturn);
            return DTOAssembler.CreateCreditNotesumary(policy);
        }

        public COMTRAEN.CreditNoteDTO GetTemporalById(int temporalId, bool isMasive)
        {

            return DTOAssembler.CreateCreditNotesumary(DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, isMasive));

        }

        public EndorsementDTO GetTemporalEndorsementByPolicyId(int policyId)
        {
            EndorsementDTO endorsement = DelegateService.transportsService.GetTemporalEndorsementByPolicyId(policyId);
            return endorsement;
        }


        public List<COMTRAEN.EndorsementDTO> GetEndorsementsWithPremiumAmount(int policyId)
        {
            return DTOAssembler.CreateEndorsements(
                DelegateService.creditNoteServiceBase.GetEndorsementsWithPremiumAmount(policyId));
        }

        public List<COMTRAEN.EndorsementRiskDTO> GetRisksByPolicyIdEndorsementId(int policyId, int endorsementId)
        {
            return DTOAssembler.CreateRisks(
                DelegateService.creditNoteServiceBase.GetRisksByPolicyIdEndorsementId(
                    policyId, endorsementId));
        }

        public decimal GetMaximumPremiumPercetToReturn(int policyId)
        {
            return DelegateService.creditNoteServiceBase.GetMaximumPremiumPercetToReturn(policyId);
        }
    }
}

