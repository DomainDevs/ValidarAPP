using Sistran.Company.Application.ChangeAgentEndorsement.EEProvider;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.AircraftChangeAgentService.EEProvider.Business;
using Sistran.Company.Application.AircraftChangeAgentService.EEProvider.Resources;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;

namespace Sistran.Company.Application.AircraftChangeAgentService.EEProvider
{
    public class AircraftChangeAgentServiceEEProvider : CiaChangeAgentEndorsementEEProvider, ICiaAircraftChangeAgentService
    {
        public CompanyPolicy CreateTemporal(CompanyPolicy companyPolicy, bool isMassive = false)
        {
            try
            {
                AircraftChangeAgentBusinessCia aircraftChangeAgentBusinessCia = new AircraftChangeAgentBusinessCia();
                return aircraftChangeAgentBusinessCia.CreateTemporal(companyPolicy, isMassive);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalChangeAgentAircraft);
            }
        }

        public List<CompanyPolicy> CreateEndorsementChangeAgent(CompanyEndorsement companyPolicy)
        {
            try
            {
                AircraftChangeAgentBusinessCia aircraftChangeAgentBusinessCia = new AircraftChangeAgentBusinessCia();
                return aircraftChangeAgentBusinessCia.CreateEndorsementChangeAgent(companyPolicy);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalChangeAgentAircraft);
            }
        }

    }
}
