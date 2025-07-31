using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.ThirdPartyLiabilityChangeAgentService.EEProvider.Business;
using Sistran.Company.Application.ThirdPartyLiabilityChangeAgentService.EEProvider.Resources;
using Sistran.Core.Framework.BAF;
using System;
using Sistran.Company.Application.ChangeAgentEndorsement.EEProvider;
using System.Collections.Generic;

namespace Sistran.Company.Application.ThirdPartyLiabilityChangeAgentService.EEProvider
{
    public class ThirdPartyLiabilityChangeAgentServiceEEProvider : CiaChangeAgentEndorsementEEProvider, IThirdPartyLiabilityChangeAgentServiceCia
    {
        public CompanyPolicy CreateTemporal(CompanyPolicy companyPolicy, bool isMassive = false)
        {
            try
            {
                ThirdPartyLiabilityChangeAgentBusinessCia tplChangeAgentBusinessCia = new ThirdPartyLiabilityChangeAgentBusinessCia();
                return tplChangeAgentBusinessCia.CreateTemporal(companyPolicy, isMassive);
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalChangeAgentThirdPartyLiability);
            }
        }

        public List<CompanyPolicy> CreateEndorsementChangeAgent(CompanyEndorsement companyPolicy)
        {
            try
            {
                ThirdPartyLiabilityChangeAgentBusinessCia tplChangeAgentBusinessCia = new ThirdPartyLiabilityChangeAgentBusinessCia();
                return tplChangeAgentBusinessCia.CreateEndorsementChangeAgent(companyPolicy);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalChangeAgentThirdPartyLiability);
            }
        }

    }
}
