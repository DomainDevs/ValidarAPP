using System;
using Sistran.Core.Framework.BAF;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.JudicialSuretyChangePolicyHolderService.EEProvider.Business;
using Sistran.Company.Application.JudicialSuretyChangePolicyHolderService.EEProvider.Resources;
using Sistran.Company.Application.Sureties.JudicialSuretyServices.Models;
using System.Collections.Generic;

namespace Sistran.Company.Application.JudicialSuretyChangePolicyHolderService.EEProvider
{
    public class JudicialSuretyChangePolicyHolderServiceEEProvider : ICiaJudicialSuretyChangePolicyHolderService
    {
        public CompanyChangePolicyHolder CreateTemporal(CompanyChangePolicyHolder companyChangePolicyHolder, bool isMassive = false)
        {
            try
            {
                JudicialSuretyChangePolicyHolderBusinessCia judicialSuretyChangePolicyHolderBusinessCia = new JudicialSuretyChangePolicyHolderBusinessCia();
                return judicialSuretyChangePolicyHolderBusinessCia.CreateTemporal(companyChangePolicyHolder, isMassive);
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
                JudicialSuretyChangePolicyHolderBusinessCia judicialSuretyChangePolicyHolderBusinessCia = new JudicialSuretyChangePolicyHolderBusinessCia();
                return judicialSuretyChangePolicyHolderBusinessCia.CreateEndorsementChangePolicyHolder(companyChangePolicyHolder, clearPolicies);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalPolicyHolderChangeSurety);
            }
        }
    }
}
