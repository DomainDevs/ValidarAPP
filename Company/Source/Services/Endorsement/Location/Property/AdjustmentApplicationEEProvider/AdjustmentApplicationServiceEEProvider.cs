using Sistran.Company.Application.AdjustmentApplicationService;
using Sistran.Company.Application.AdjustmentApplicationService.DTO;
using Sistran.Company.Application.AdjustmentApplicationServiceEEProvider.Business;
using Sistran.Company.Application.AdjustmentApplicationServiceEEProvider.Resources;
using PROP=Sistran.Company.Application.Location.PropertyServices.DTO;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.AdjustmentApplicationServiceEEProvider
{
    public class AdjustmentApplicationServiceEEProvider : IPropertyAdjustmentApplicationService
    {

        public AdjustmentDTO GetRisksByTemporalId(int temporalId, bool isMasive)
        {
            try
            {
                PropertyBusiness propertyBusiness = new PropertyBusiness();
                return propertyBusiness.GetRisksByTemporalId(temporalId, isMasive);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateAdjustmentEndorsement), ex);
            }
        }
        public AdjustmentDTO CalculateDays(string CurrentFrom, string CurrentTo, int BillingPeriodId)
        {
            try
            {
                PropertyBusiness adjustmentBusiness = new PropertyBusiness();
                return adjustmentBusiness.CalculateDays(CurrentFrom, BillingPeriodId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCalculateDays), ex);
            }
        }

        public AdjustmentDTO CreateTemporal(AdjustmentDTO adjustmentDTO)
        {
            try
            {
                PropertyBusiness propertyBusiness = new PropertyBusiness();
                return propertyBusiness.CreateTemporal(adjustmentDTO);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public AdjustmentDetailsDTO GetEndorsementByEndorsementTypeDeclarationPolicyId(int policyId, int riskId)
        {
            try
            {
                PropertyBusiness adjustmentBusiness = new PropertyBusiness();
                return adjustmentBusiness.GetEndorsementByEndorsementTypeDeclarationPolicyId(policyId, riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetEndorsements), ex);

            }
        }

        public AdjustmentDTO GetPropertyRisksByPolicyId(int policyId, string currentFrom)
        {
            try
            {
                PropertyBusiness adjustmentBusiness = new PropertyBusiness();
                return adjustmentBusiness.GetPropertyRiskByPolicyId(policyId, currentFrom);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.GetErrorPropretyRisk), ex);
            }
        }

        public EndorsementDTO GetTemporalEndorsementByPolicyId(int policyId)
        {
            try
            {
                PropertyBusiness adjustmentBusiness = new PropertyBusiness();
                return adjustmentBusiness.GetTemporalEndorsementByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateAdjustmentEndorsement), ex);
            }


        }

        public AdjustmentDTO QuotateAdjustment(AdjustmentDTO adjustmentDTO)
        {
            throw new NotImplementedException();
        }

        public AdjustmentDTO GetPropertyRiskByTemporalId(int temporalId, bool isMasive)
        {
            try
            {
                PropertyBusiness adjustmentBusiness = new PropertyBusiness();
                return adjustmentBusiness.GetPropertyRiskByTemporalId(temporalId, isMasive);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateAdjustmentEndorsement), ex);
            }
        }

        public List<PROP.InsuredObjectDTO> GetInsuredObjectsByRiskId(int riskId)
        {
            try
            {
                PropertyBusiness adjustmentBusiness = new PropertyBusiness();
                return adjustmentBusiness.GetInsuredObjectsByRiskId(riskId);
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
                PropertyBusiness adjustmentBusiness = new PropertyBusiness();
                return adjustmentBusiness.GetAdjustmentEndorsementByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetAdjustmentEndorsementByPolicyId), ex);
            }
        }

    }
}
