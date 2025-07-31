using System;
using Sistran.Company.Application.Transports.Endorsement.Adjustment.ApplicationServices;
using Sistran.Company.Application.Transports.Endorsement.Adjustment.ApplicationServices.DTOs;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Transports.Endorsement.Adjustment.ApplicationServices.EEProvider.Business;
using Sistran.Core.Framework.BAF;
using Sistran.Company.Application.Transports.Endorsement.Adjustment.ApplicationServices.EEProvider.Resources;
using Sistran.Core.Application.Utilities.Managers;
using System.Collections.Generic;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Company.Application.Transports.TransportApplicationService.DTOs;

namespace Sistran.Company.Application.Transports.Transport.ApplicationServices.EEProvider
{
    public class TransportAdjustmentApplicationServiceProvider : ITransportAdjustmentApplicationService
    {
        public AdjustmentDTO CreateTemporal(AdjustmentDTO adjustmentDTO)
        {
            try
            {
                AdjustmentTransportBusiness adjustmentTransportBusiness = new AdjustmentTransportBusiness();
                return adjustmentTransportBusiness.CreateTemporal(adjustmentDTO);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateAdjustmentEndorsement), ex);
            }
            
        }
        
        public AdjustmentDTO GetTransportsByPolicyId(int policyId, string currentFrom)
        { 
            try
            {
                AdjustmentTransportBusiness adjustmentTransportBusiness = new AdjustmentTransportBusiness();
                return adjustmentTransportBusiness.GetTransportsByPolicyId(policyId, currentFrom);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetTransports), ex);
            }
        }
        
        public AdjustmentDTO CalculateDays(string CurrentFrom, string CurrentTo, int BillingPeriodId)
        {
            try
            {
                AdjustmentTransportBusiness adjustmentTransportBusiness = new AdjustmentTransportBusiness();
                return adjustmentTransportBusiness.CalculateDays(CurrentFrom,BillingPeriodId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCalculateDays), ex);
            }

        }
        public AdjustmentDTO QuotateAdjustment(AdjustmentDTO adjustmentDTO)
        {
            throw new NotImplementedException();
        }

        public AdjustmentDetailsDTO GetEndorsementByEndorsementTypeDeclarationPolicyId(int policyId, int riskId)
        {
            try
            {
                AdjustmentTransportBusiness adjustmentTransportBusiness = new AdjustmentTransportBusiness();
                return adjustmentTransportBusiness.GetEndorsementByEndorsementTypeDeclarationPolicyId(policyId,riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetEndorsements), ex);

            }
        }
      

        public EndorsementDTO GetTemporalEndorsementByPolicyId(int policyId)
        {
            try
            {
                AdjustmentTransportBusiness adjustmentTransportBusiness = new AdjustmentTransportBusiness();
                return adjustmentTransportBusiness.GetTemporalEndorsementByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateAdjustmentEndorsement), ex);
            }
        }
        
        public AdjustmentDTO GetTransportsByTemporalId(int temporalId, bool isMasive)
        {
            try
            {
                AdjustmentTransportBusiness adjustmentTransportBusiness = new AdjustmentTransportBusiness();
                return adjustmentTransportBusiness.GetTransportsByTemporalId(temporalId,isMasive);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateAdjustmentEndorsement), ex);
            }
        }

        public List<InsuredObjectDTO> GetInsuredObjectsByRiskId(int riskId)
        {
            try
            {
                AdjustmentTransportBusiness adjustmentTransportBusiness = new AdjustmentTransportBusiness();
                return adjustmentTransportBusiness.GetInsuredObjectsByRiskId(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetInsuredObjects), ex);
            }
        }
		
		public AdjustmentDTO GetAdjustmentEndorsementByPolicyId(int policyId)
        {
            try
            {
                AdjustmentTransportBusiness adjustmentTransportBusiness = new AdjustmentTransportBusiness();
                return adjustmentTransportBusiness.GetAdjustmentEndorsementByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetAdjustmentEndorsementByPolicyId), ex);
            }
        }
    }
}