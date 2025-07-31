using Sistran.Core.Integration.SuretyServices.DTOs;
using Sistran.Core.Integration.Sureties.SuretyServices.EEProvider.Assembler;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.SuretyServices.EEProvider
{
    public class SuretiesIntegrationServiceEEProvider
    {
        public List<SuretyDTO> GetSuretiesByEndorsementIdPrefixIdModuleType(int endorsementId, int prefixId, ModuleType moduleType)
        {
            List<SuretyDTO> suretyDTO = new List<SuretyDTO>();

            switch (prefixId)
            {
                case 2:
                    suretyDTO = DTOAssembler.CreateSureties(DelegateService.suretyService.GetSuretiesByEndorsementIdModuleType(endorsementId, moduleType));
                    break;
                case 29:
                    suretyDTO = DTOAssembler.CreateJudicialSureties(DelegateService.judicialSuretyService.GetJudicialSuretiesByEndorsementIdModuleType(endorsementId, moduleType));
                    break;
            }

            return suretyDTO;
        }
        public List<SuretyDTO> GetRisksSuretyByInsuredId(int insuredId)
        {
            return DTOAssembler.CreateSureties(DelegateService.suretyService.GetRisksSuretyByInsuredId(insuredId));
        }

        public List<SuretyDTO> GetRisksSuretyBySuretyId(int suretyId)
        {
            return DTOAssembler.CreateSureties(DelegateService.suretyService.GetRisksSuretyBySuretyId(suretyId));
        }
        public SuretyDTO GetSuretyByRiskIdModuleType(int riskId, ModuleType moduleType)
        {
            return DTOAssembler.CreateSurety(DelegateService.suretyService.GetSuretyByRiskIdModuleType(riskId, moduleType));
        }

        public List<SuretyDTO> GetRisksBySurety(string description)
        {
            return DTOAssembler.CreateSureties(DelegateService.suretyService.GetRisksBySurety(description));
        }
    }
}
