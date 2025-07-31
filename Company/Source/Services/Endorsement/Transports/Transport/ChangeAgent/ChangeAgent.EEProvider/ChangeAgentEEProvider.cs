using Sistran.Company.Application.ChangeAgentEndorsement.EEProvider;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.TransportChangeAgentService.EEProvider.Business;
using Sistran.Company.Application.TransportChangeAgentService.EEProvider.Resources;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;

namespace Sistran.Company.Application.TransportChangeAgentService.EEProvider
{
    public class TransportChangeAgentServiceEEProvider : CiaChangeAgentEndorsementEEProvider, ICiaTransportChangeAgentService
    {
        public CompanyPolicy CreateTemporal(CompanyPolicy companyPolicy, bool isMassive = false)
        {
            try
            {
                TransportChangeAgentBusinessCia transportChangeAgentBusinessCia = new TransportChangeAgentBusinessCia();
                return transportChangeAgentBusinessCia.CreateTemporal(companyPolicy, isMassive);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalChangeAgentTransport);
            }
        }

        public List<CompanyPolicy> CreateEndorsementChangeAgent(CompanyEndorsement companyPolicy)
        {
            try
            {
                TransportChangeAgentBusinessCia transportChangeAgentBusinessCia = new TransportChangeAgentBusinessCia();
                return transportChangeAgentBusinessCia.CreateEndorsementChangeAgent(companyPolicy);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalChangeAgentTransport);
            }
        }

    }
}
