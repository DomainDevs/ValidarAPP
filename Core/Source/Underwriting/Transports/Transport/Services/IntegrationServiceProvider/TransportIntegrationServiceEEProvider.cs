using Sistran.Core.Integration.TransportServices.DTOs;
using SSistran.Core.Integration.TransportServices.EEProvider.Assemblers;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Collections.Generic;

namespace Sistran.Core.Integration.TransportServices.EEProvider
{
    public class TransportIntegrationServiceEEProvider : ITransportIntegrationService
    {
        public List<TransportDTO> GetTransportByEndorsementIdModuleType(int endorsementId, ModuleType moduleType)
        {
            return DTOAssembler.CreateTransports(DelegateService.transportBusinessService.GetTransportByEndorsementIdModuleType(endorsementId, moduleType));
        }

        public List<TransportDTO> GetTransportsByInsuredId(int insuredId)
        {
            return DTOAssembler.CreateTransports(DelegateService.transportBusinessService.GetTransportsByInsuredId(insuredId));
        }

        public TransportDTO GetRiskTransportByRiskId(int riskId)
        {
            return DTOAssembler.CreateTransport(DelegateService.transportBusinessService.GetTransportByRiskId(riskId));
        }
    }
}