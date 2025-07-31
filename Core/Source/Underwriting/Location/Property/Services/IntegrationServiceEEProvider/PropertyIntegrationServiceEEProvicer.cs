using Sistran.Core.Integration.PropertyServices;
using Sistran.Core.Integration.PropertyServices.DTOs;
using Sistran.Core.Integration.PropertyServices.EEProvider.Assemblers;
using System;
using System.Collections.Generic;

namespace Sistran.Core.Integration.PropertyServices.EEProvider
{
    public class PropertyIntegrationServiceEEProvicer : IPropertyIntegrationService
    {
        public List<RiskLocationDTO> GetRiskPropertiesByAddress(string adderess)
        {
            return DTOAssembler.CreateRiskLocations(DelegateService.propertyServiceCore.GetRiskPropertiesByAddress(adderess));
        }

        public List<RiskLocationDTO> GetRiskPropertiesByEndorsementId(int endorsementId)
        {
            return DTOAssembler.CreateRiskLocations(DelegateService.propertyServiceCore.GetRiskPropertiesByEndorsementId(endorsementId));
        }

        public List<RiskLocationDTO> GetRiskPropertiesByInsuredId(int insuredId)
        {
            return DTOAssembler.CreateRiskLocations(DelegateService.propertyServiceCore.GetRiskPropertiesByInsuredId(insuredId));
        }

        public RiskLocationDTO GetRiskPropertyByRiskId(int riskId)
        {
            return DTOAssembler.CreateRiskLocation(DelegateService.propertyServiceCore.GetRiskPropertyByRiskId(riskId));
        }
    }
}
