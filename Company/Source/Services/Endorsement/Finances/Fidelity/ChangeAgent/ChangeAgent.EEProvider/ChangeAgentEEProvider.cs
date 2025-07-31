using Sistran.Company.Application.ChangeAgentEndorsement.EEProvider;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.FidelityChangeAgentService.EEProvider.Business;
using Sistran.Company.Application.FidelityChangeAgentService.EEProvider.Resources;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;

namespace Sistran.Company.Application.FidelityChangeAgentService.EEProvider
{
    public class FidelityChangeAgentServiceEEProvider : CiaChangeAgentEndorsementEEProvider, IFidelityChangeAgentServiceCia
    {
        public CompanyPolicy CreateTemporal(CompanyPolicy companyPolicy, bool isMassive = false)
        {
            try
            {
                FidelityChangeAgentBusinessCia fidelityChangeAgentBusinessCia = new FidelityChangeAgentBusinessCia();
                return fidelityChangeAgentBusinessCia.CreateTemporal(companyPolicy, isMassive);
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalChangeAgentFidelity);
            }
        }

        public List<CompanyPolicy> CreateEndorsementChangeAgent(CompanyEndorsement companyPolicy)
        {
            try
            {
                FidelityChangeAgentBusinessCia fidelityChangeAgentBusinessCia = new FidelityChangeAgentBusinessCia();
                return fidelityChangeAgentBusinessCia.CreateEndorsementChangeAgent(companyPolicy);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalChangeAgentFidelity);
            }
        }

    }
}
