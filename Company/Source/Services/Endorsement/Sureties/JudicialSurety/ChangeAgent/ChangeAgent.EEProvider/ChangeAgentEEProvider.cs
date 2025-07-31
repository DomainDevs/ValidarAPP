using Sistran.Company.Application.ChangeAgentEndorsement.EEProvider;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.JudicialSuretyChangeAgentService.EEProvider.Business;
using Sistran.Company.Application.JudicialSuretyChangeAgentService.EEProvider.Resources;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;

namespace Sistran.Company.Application.JudicialSuretyChangeAgentService.EEProvider
{
    public class JudicialSuretyChangeAgentServiceEEProvider : CiaChangeAgentEndorsementEEProvider, IJudicialSuretyChangeAgentServiceCompany
    {
        public CompanyPolicy CreateTemporal(CompanyPolicy companyPolicy, bool isMassive = false)
        {
            try
            {
                SuretyChangeAgentBusinessCompany suretyChangeAgentBusinessCompany = new SuretyChangeAgentBusinessCompany();
                return suretyChangeAgentBusinessCompany.CreateTemporal(companyPolicy, isMassive);
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalChangeAgentSurety);
            }
        }

        public List<CompanyPolicy> CreateEndorsementChangeAgent(CompanyEndorsement companyPolicy, bool clearPolicies = false)
        {
            try
            {
                SuretyChangeAgentBusinessCompany suretyChangeAgentBusinessCompany = new SuretyChangeAgentBusinessCompany();
                return suretyChangeAgentBusinessCompany.CreateEndorsementChangeAgent(companyPolicy, clearPolicies);
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalChangeAgentSurety);
            }
        }

    }
}
