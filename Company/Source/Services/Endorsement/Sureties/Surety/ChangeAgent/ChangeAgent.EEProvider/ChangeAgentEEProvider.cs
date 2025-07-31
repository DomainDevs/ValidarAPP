using Sistran.Company.Application.ChangeAgentEndorsement.EEProvider;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.SuretyChangeAgentService.EEProvider.Business;
using Sistran.Company.Application.SuretyChangeAgentService.EEProvider.Resources;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;

namespace Sistran.Company.Application.SuretyChangeAgentService.EEProvider
{
    public class SuretyChangeAgentServiceEEProvider : CiaChangeAgentEndorsementEEProvider, ICiaSuretyChangeAgentService
    {
        public CompanyPolicy CreateTemporal(CompanyPolicy companyPolicy, bool isMassive = false)
        {
            try
            {
                SuretyChangeAgentBusinessCia suretyChangeAgentBusinessCia = new SuretyChangeAgentBusinessCia();
                return suretyChangeAgentBusinessCia.CreateTemporal(companyPolicy, isMassive);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalChangeAgentSurety);
            }
        }

        public List<CompanyPolicy> CreateEndorsementChangeAgent(CompanyEndorsement companyPolicy, bool clearPolicies = false)
        {
            try
            {
                SuretyChangeAgentBusinessCia suretyChangeAgentBusinessCia = new SuretyChangeAgentBusinessCia();
                return suretyChangeAgentBusinessCia.CreateEndorsementChangeAgent(companyPolicy, clearPolicies);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalChangeAgentSurety);
            }
        }

    }
}
