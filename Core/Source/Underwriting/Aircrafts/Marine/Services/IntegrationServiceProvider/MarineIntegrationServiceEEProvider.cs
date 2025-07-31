using AIR = Sistran.Core.Application.Aircrafts.AircraftBusinessService.Models;
using Sistran.Core.Application.Marines.MarineBusinessService.Models;
using Sistran.Core.Integration.MarineServices.DTOs;
using Sistran.Core.Integration.MarineServices.EEProvider.Assembler;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Integration.MarineServices;

namespace Sistran.Core.Integration.MarineServices.EEProvider
{
    public class MarineIntegrationServiceEEProvider : IMarineIntegrationService
    {
        public List<AirCraftDTO> GetMarinesByEndorsementIdModuleType(int endorsementId)
        {
            return DTOAssembler.CreateCompanyMarines(DelegateService.marineBusinessService.GetMarinesByEndorsementIdModuleType(endorsementId));
        }
    }
}