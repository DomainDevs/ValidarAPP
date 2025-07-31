using Sistran.Company.Application.Transports.Endorsement.Adjustment.BusinessService.EEProvider.Business;
using Sistran.Company.Application.Transports.Endorsement.Adjustment.BusinessService.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.Transports.Endorsement.Adjustment.BusinessServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class CompanyTransportAdjustmentBusinessServiceProvider : ICompanyTransportAdjustmentBusinessService
    {
        public CompanyPolicy CreateEndorsementAdjustment(CompanyPolicy companyPolicy, Dictionary<string, object> formValues)
        {
            try
            {
                AdjustmentBusiness adjustmentTransportBusiness = new AdjustmentBusiness();
                return adjustmentTransportBusiness.CreateEndorsementAdjustment(companyPolicy, formValues);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateAdjustmentEndorsement), ex);
            }

        }



            public List<CompanyCoverage> GetCoverageByEndorsementIdPolicyIdriskId(int policyId, int riskId)
        {
            try
            {
                AdjustmentBusiness adjustmentTransportBusiness = new AdjustmentBusiness();
                return adjustmentTransportBusiness.GetCoverageByEndorsementIdPolicyIdriskId(policyId,riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateAdjustmentEndorsement), ex);
            }

        }
  //List<CompanyCoverage> GetCoverageByEndorsementIdPolicyIdriskId(int policyId, int endorsementId, int riskId);

    }
    }