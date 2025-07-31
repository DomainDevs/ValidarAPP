using System.Collections.Generic;
using System;
using Sistran.Company.Application.Transports.Endorsement.CreditNote.ApplicationServices.DTOs;
using Sistran.Company.Application.Transports.Endorsement.CreditNote.ApplicationServices.EEProvider.Business;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Company.Application.Transports.Endorsement.CreditNote.ApplicationServices.EEProvider.Resources;
using Sistran.Company.Application.Transports.TransportApplicationService.DTOs;

namespace Sistran.Company.Application.Transports.Endorsement.CreditNote.ApplicationServices.EEProvider
{
    public class CompanyCreditNoteApplicationServiceProvider : ITransportCreditNoteApplicationService
    {
        public CreditNoteDTO GetEndorsementInformationByPolicyIdEndorsementId(int policyId, int endorsementId)
        {
            throw new NotImplementedException();
        }
        public CreditNoteDTO QuotateCreditNote(CreditNoteDTO creditNoteDTO)
        {
            throw new NotImplementedException();
        }


        public RiskDTO GetTransportsByPolicyIdByEndorsementId(int policyId, int endorsementId)
        {
            try
            {
                BusinessCreditNote businessCreditNote = new BusinessCreditNote();
                return businessCreditNote.GetTransportsByPolicyIdByEndorsementId(policyId, endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetAvaibleEndorsementsByPolicyId), ex); 
            }
        }

        public CreditNoteDTO GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(int policyId, int endorsementId, int riskId)
        {
            try
            {
                BusinessCreditNote businessCreditNote = new BusinessCreditNote();
                return businessCreditNote.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, endorsementId, riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCoverageByRisk), ex);
            }

        }

        public CreditNoteDTO CreateTemporal(CreditNoteDTO creditNoteDTO)
        {
            try
            {
                BusinessCreditNote businessCreditNote = new BusinessCreditNote();
                return businessCreditNote.CreateTemporal(creditNoteDTO);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreatingTemporal), ex);
            }
        }

        public CreditNoteDTO GetTemporalById(int temporalId, bool isMasive)
        {
            try
            {
                BusinessCreditNote businessCreditNote = new BusinessCreditNote();
                return businessCreditNote.GetTemporalById(temporalId, isMasive);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateCreditNoteEndorsement), ex);
            }
        }


        public TransportApplicationService.DTOs.EndorsementDTO GetTemporalEndorsementByPolicyId(int policyId)
        {
            try
            {
                BusinessCreditNote businessCreditNote = new BusinessCreditNote();
                return businessCreditNote.GetTemporalEndorsementByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetTemporal), ex);
            }
        }

        public List<EndorsementRiskDTO> GetRisksByPolicyIdEndorsementId(int policyId, int endorsementId)
        {
            try
            {
                BusinessCreditNote businessCreditNote = new BusinessCreditNote();
                return businessCreditNote.GetRisksByPolicyIdEndorsementId(policyId, endorsementId);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetRisksByPolicyIdEndorsementId), ex);
            }
        }

        public List<DTOs.EndorsementDTO> GetEndorsementsWithPremiumAmount(int policyId)
        {
            try
            {
                BusinessCreditNote businessCreditNote = new BusinessCreditNote();
                return businessCreditNote.GetEndorsementsWithPremiumAmount(policyId);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetEndorsementsWithPremiumAmount), ex);
            }
        }

        public decimal GetMaximumPremiumPercetToReturn(int policyId)
        {
            try
            {
                BusinessCreditNote creditNoteBusiness = new BusinessCreditNote();
                return creditNoteBusiness.GetMaximumPremiumPercetToReturn(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetMaximumPremiumPercetToReturn), ex);
            }
        }
    }
}