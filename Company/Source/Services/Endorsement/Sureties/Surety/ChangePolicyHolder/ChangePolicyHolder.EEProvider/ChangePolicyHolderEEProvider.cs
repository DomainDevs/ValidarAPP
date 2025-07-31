using System;
using Sistran.Core.Framework.BAF;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.SuretyChangePolicyHolderService.EEProvider.Business;
using Sistran.Company.Application.SuretyChangePolicyHolderService.EEProvider.Resources;
using Sistran.Company.Application.Sureties.SuretyServices.Models;
using System.Collections.Generic;

namespace Sistran.Company.Application.SuretyChangePolicyHolderService.EEProvider
{
    public class SuretyChangePolicyHolderServiceEEProvider : ICiaSuretyChangePolicyHolderService
    {
        public CompanyChangePolicyHolder CreateTemporal(CompanyChangePolicyHolder companyChangePolicyHolder, bool isMassive = false)
        {
            try
            {
                SuretyChangePolicyHolderBusinessCia suretyPolicyHolderChangeBusinessCia = new SuretyChangePolicyHolderBusinessCia();
                return suretyPolicyHolderChangeBusinessCia.CreateTemporal(companyChangePolicyHolder, isMassive);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalPolicyHolderChangeSurety);
            }
        }

        public List<CompanyPolicy> CreateEndorsementChangePolicyHolder(CompanyChangePolicyHolder companyChangePolicyHolder, bool clearPolicies = false)
        {
            try
            {
                SuretyChangePolicyHolderBusinessCia suretyPolicyHolderChangeBusinessCia = new SuretyChangePolicyHolderBusinessCia();
                return suretyPolicyHolderChangeBusinessCia.CreateEndorsementChangePolicyHolder(companyChangePolicyHolder, clearPolicies);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalPolicyHolderChangeSurety);
            }
        }

        public CompanyContract GetCompanyContractByTemporalId(int temporalId, bool isMasive)
        {
            try
            {
                SuretyChangePolicyHolderBusinessCia suretyPolicyHolderChangeBusinessCia = new SuretyChangePolicyHolderBusinessCia();
                return suretyPolicyHolderChangeBusinessCia.GetCompanyContractByTemporalId(temporalId,isMasive);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalPolicyHolderChangeSurety);
            }
        }

    }
}
