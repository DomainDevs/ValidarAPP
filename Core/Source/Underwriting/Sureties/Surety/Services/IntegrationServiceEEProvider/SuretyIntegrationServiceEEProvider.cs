using Sistran.Core.Integration.SuretyServices.DTOs;
using Sistran.Core.Integration.Sureties.SuretyServices.EEProvider.Assembler;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Integration.SuretyServices.Enums;
using System.Linq;

namespace Sistran.Core.Integration.SuretyServices.EEProvider
{
    public class SuretyIntegrationServiceEEProvider : ISuretyIntegrationService
    {
        public List<SuretyDTO> GetSuretiesByEndorsementIdPrefixIdModuleType(int endorsementId, int prefixId, ModuleType moduleType)
        {
            List<SuretyDTO> sureties = new List<SuretyDTO>();

            if (prefixId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimIntegrationKeys>(ClaimIntegrationKeys.CLM_SURETY_PREFIX)) ||
                prefixId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimIntegrationKeys>(ClaimIntegrationKeys.CLM_LEASING_PREFIX)))
            {
                sureties = DTOAssembler.CreateSureties(DelegateService.suretyService.GetSuretiesByEndorsementIdModuleType(endorsementId, moduleType));
            }
            else if (prefixId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimIntegrationKeys>(ClaimIntegrationKeys.CLM_SURETY_JUDICIAL_PREFIX)))
            {
                sureties = DTOAssembler.CreateJudicialSureties(DelegateService.judicialSuretyService.GetJudicialSuretiesByEndorsementIdModuleType(endorsementId, moduleType));
            }

            return sureties;
        }
        public List<SuretyDTO> GetRisksSuretyByInsuredId(int insuredId)
        {
            List<SuretyDTO> sureties = new List<SuretyDTO>();

            sureties.AddRange(DTOAssembler.CreateSureties(DelegateService.suretyService.GetRisksSuretyByInsuredId(insuredId)));
            sureties.AddRange(DTOAssembler.CreateJudicialSureties(DelegateService.judicialSuretyService.GetJudicialSuretiesBySureryId(insuredId)));

            sureties = sureties.DistinctBy(x => x.DocumentNum).ToList();

            return sureties;
        }

        public List<SuretyDTO> GetRisksSuretyBySuretyIdPrefixId(int suretyId, int prefixId)
        {
            List<SuretyDTO> sureties = new List<SuretyDTO>();

            if (prefixId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimIntegrationKeys>(ClaimIntegrationKeys.CLM_SURETY_PREFIX)) ||
                prefixId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimIntegrationKeys>(ClaimIntegrationKeys.CLM_LEASING_PREFIX)))
            {
                sureties = DTOAssembler.CreateSureties(DelegateService.suretyService.GetRisksSuretyBySuretyId(suretyId));
            }
            else if (prefixId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimIntegrationKeys>(ClaimIntegrationKeys.CLM_SURETY_JUDICIAL_PREFIX)))
            { 
                sureties = DTOAssembler.CreateJudicialSureties(DelegateService.judicialSuretyService.GetJudicialSuretiesBySureryId(suretyId));
            }

            return sureties;
        }

        public SuretyDTO GetSuretyByRiskIdPrefixIdModuleType(int riskId, int prefixId, ModuleType moduleType)
        {
            SuretyDTO surety = new SuretyDTO();

            if (prefixId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimIntegrationKeys>(ClaimIntegrationKeys.CLM_SURETY_PREFIX)) ||
                prefixId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimIntegrationKeys>(ClaimIntegrationKeys.CLM_LEASING_PREFIX)))
            {
                surety = DTOAssembler.CreateSurety(DelegateService.suretyService.GetSuretyByRiskIdModuleType(riskId, moduleType));
            }
            else if (prefixId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimIntegrationKeys>(ClaimIntegrationKeys.CLM_SURETY_JUDICIAL_PREFIX)))
            { 
                surety = DTOAssembler.CreateJudicialSurety(DelegateService.judicialSuretyService.GetJudicialSuretyByRiskIdModuleType(riskId, moduleType));
            }

            return surety;
        }

        public List<SuretyDTO> GetRisksBySurety(string description)
        {
            List<SuretyDTO> sureties = new List<SuretyDTO>();
            sureties.AddRange(DTOAssembler.CreateSureties(DelegateService.suretyService.GetRisksBySurety(description)));
            sureties.AddRange(DTOAssembler.CreateJudicialSureties(DelegateService.judicialSuretyService.GetJudicialSuretiesByDescription(description)));

            sureties = sureties.DistinctBy(x => x.DocumentNum).ToList();

            return sureties;
        }
    }
}
