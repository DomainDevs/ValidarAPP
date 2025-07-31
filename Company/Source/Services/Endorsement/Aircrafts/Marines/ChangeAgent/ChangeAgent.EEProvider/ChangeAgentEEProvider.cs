using Sistran.Company.Application.ChangeAgentEndorsement.EEProvider;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.MarineChangeAgentService.EEProvider.Business;
using Sistran.Company.Application.MarineChangeAgentService.EEProvider.Resources;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;

namespace Sistran.Company.Application.MarineChangeAgentService.EEProvider
{
    public class MarineChangeAgentServiceEEProvider : CiaChangeAgentEndorsementEEProvider, ICiaMarineChangeAgentService
    {
        public CompanyPolicy CreateTemporal(CompanyPolicy companyPolicy, bool isMassive = false)
        {
            try
            {
                MarineChangeAgentBusinessCia marineChangeAgentBusinessCia = new MarineChangeAgentBusinessCia();
                return marineChangeAgentBusinessCia.CreateTemporal(companyPolicy, isMassive);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalChangeAgentMarine);
            }
        }

        public List<CompanyPolicy> CreateEndorsementChangeAgent(CompanyEndorsement companyPolicy)
        {
            try
            {
                MarineChangeAgentBusinessCia marineChangeAgentBusinessCia = new MarineChangeAgentBusinessCia();
                return marineChangeAgentBusinessCia.CreateEndorsementChangeAgent(companyPolicy);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalChangeAgentMarine);
            }
        }

    }
}
