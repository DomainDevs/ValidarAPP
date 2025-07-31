using Sistran.Core.Integration.VehicleService.EEProvider.Assemblers;
using Sistran.Core.Integration.VehicleServices;
using Sistran.Core.Integration.VehicleServices.DTOs;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.VehicleService.EEProvider
{
    public class VehicleIntegrationServiceEEProvicer : IVehicleIntegrationService
    {
        public List<VehicleDTO> GetRiskVehiclesByEndorsementId(int endorsementId)
        {
            return DTOAssembler.CreateVehicles(DelegateService.vehicleService.GetRiskVehiclesByEndorsementId(endorsementId));
        }

        public List<VehicleDTO> GetRiskVehiclesByEndorsementIdModuleType(int endorsementId, ModuleType moduleType)
        {
            return DTOAssembler.CreateVehicles(DelegateService.vehicleService.GetRiskVehiclesByEndorsementIdModuleType(endorsementId, moduleType));
        }

        public List<MakeDTO> GetVehicleMakes()
        {
            return DTOAssembler.CreateVehicleMakes(DelegateService.vehicleService.GetMakes());
        }

        public List<MakeDTO> GetVehicleMakesByDescription(string description)
        {
            return DTOAssembler.CreateVehicleMakes(DelegateService.vehicleService.GetVehicleMakesByDescription(description));
        }

        public List<YearDTO> GetVehicleYearsByMakeIdModelIdVersionId(int MakeId, int ModelId, int VersionId)
        {
            return DTOAssembler.CreateVehicleYears(DelegateService.vehicleService.GetYearsByMakeIdModelIdVersionId(MakeId, ModelId, VersionId));
        }

        public List<ModelDTO> GetVehicleModelsByMakeId(int makeId)
        {
            return DTOAssembler.CreateVehicleModels(DelegateService.vehicleService.GetModelsByMakeId(makeId));
        }

        public List<VersionDTO> GetVehicleVersionsByMakeIdModelId(int makeId, int modelId)
        {
            return DTOAssembler.CreateVersions(DelegateService.vehicleService.GetVersionsByMakeIdModelId(makeId, modelId));

        }
        
        public List<VehicleDTO> GetRisksVehicleByInsuredId(int insuredId)
        {
            return DTOAssembler.CreateVehicles(DelegateService.vehicleService.GetRisksVehicleByInsuredId(insuredId));
        }

        public List<VehicleDTO> GetRisksVehicleByLicensePlate(string licensePlate)
        {
            return DTOAssembler.CreateVehicles(DelegateService.vehicleService.GetRisksVehicleByLicensePlate(licensePlate));
        }

        public VehicleDTO GetRiskVehicleByRiskId(int riskId)
        {
            return DTOAssembler.CreateVehicle(DelegateService.vehicleService.GetRiskVehicleByRiskId(riskId));
        }

        public List<ModelDTO> GetVehicleModelsByDescription(string description)
        {
            return DTOAssembler.CreateVehicleModels(DelegateService.vehicleService.GetVehicleModelsByDescription(description));
        }

        public List<ColorDTO> GetVehicleColors()
        {
            return DTOAssembler.CreateVehicleColors(DelegateService.vehicleService.GetColors());
        }

        public List<VehicleDTO> GetSelectRisksVehicleByLicensePlate(string licencePlate)
        {
            return DTOAssembler.CreateVehicles(DelegateService.vehicleService.GetSelectRisksVehicleByLicensePlate(licencePlate));
        }
    }
}
