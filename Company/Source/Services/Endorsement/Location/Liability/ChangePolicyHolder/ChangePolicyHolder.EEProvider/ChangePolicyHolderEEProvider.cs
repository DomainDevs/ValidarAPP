using System;
using Sistran.Core.Framework.BAF;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.LiabilityChangePolicyHolderService.EEProvider.Business;
using Sistran.Company.Application.LiabilityChangePolicyHolderService.EEProvider.Resources;
using Sistran.Company.Application.Location.LiabilityServices.Models;
using System.Collections.Generic;

namespace Sistran.Company.Application.LiabilityChangePolicyHolderService.EEProvider
{
    public class LiabilityChangePolicyHolderServiceEEProvider : ICiaLiabilityChangePolicyHolderService
    {
        public CompanyChangePolicyHolder CreateTemporal(CompanyChangePolicyHolder companyChangePolicyHolder, bool isMassive = false)
        {
            try
            {
                LiabilityChangePolicyHolderBusinessCia LiabilityPolicyHolderChangeBusinessCia = new LiabilityChangePolicyHolderBusinessCia();
                return LiabilityPolicyHolderChangeBusinessCia.CreateTemporal(companyChangePolicyHolder, isMassive);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalPolicyHolderChangeLiability);
            }
        }

        public List<CompanyPolicy> CreateEndorsementChangePolicyHolder(CompanyChangePolicyHolder companyChangePolicyHolder, bool clearPolicies = false)
        {
            try
            {
                LiabilityChangePolicyHolderBusinessCia LiabilityPolicyHolderChangeBusinessCia = new LiabilityChangePolicyHolderBusinessCia();
                return LiabilityPolicyHolderChangeBusinessCia.CreateEndorsementChangePolicyHolder(companyChangePolicyHolder, clearPolicies);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalPolicyHolderChangeLiability);
            }
        }
    }
}
