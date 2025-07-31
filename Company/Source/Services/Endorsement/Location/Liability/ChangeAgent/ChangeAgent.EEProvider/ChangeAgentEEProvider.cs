using Sistran.Company.Application.ChangeAgentEndorsement.EEProvider;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.LiabilityChangeAgentService.EEProvider.Business;
using Sistran.Company.Application.LiabilityChangeAgentService.EEProvider.Resources;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;

namespace Sistran.Company.Application.LiabilityChangeAgentService.EEProvider
{
    public class LiabilityChangeAgentServiceEEProvider : CiaChangeAgentEndorsementEEProvider, ILiabilityChangeAgentServiceCia
    {
        public CompanyPolicy CreateTemporal(CompanyPolicy companyPolicy, bool isMassive = false)
        {
            try
            {
                LiabilityChangeAgentBusinessCia libialityChangeAgentBusinessCia = new LiabilityChangeAgentBusinessCia();
                return libialityChangeAgentBusinessCia.CreateTemporal(companyPolicy, isMassive);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalChangeAgentLibiality);
            }
        }

        public List<CompanyPolicy> CreateEndorsementChangeAgent(CompanyEndorsement companyPolicy, bool clearPolicies = false)
        {
            try
            {
                LiabilityChangeAgentBusinessCia libialityChangeAgentBusinessCia = new LiabilityChangeAgentBusinessCia();
                return libialityChangeAgentBusinessCia.CreateEndorsementChangeAgent(companyPolicy, clearPolicies);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalChangeAgentLibiality);
            }
        }

    }
}
