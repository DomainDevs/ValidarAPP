using Sistran.Core.Integration.VehicleServices.DTOs;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.VehicleServices
{
    [ServiceContract]
    public interface IVehicleIntegrationService
    {
        [OperationContract]
        List<VehicleDTO> GetRiskVehiclesByEndorsementId(int endorsementId);

        [OperationContract]
        List<VehicleDTO> GetRiskVehiclesByEndorsementIdModuleType(int endorsementId, ModuleType moduleType);

        [OperationContract]
        List<MakeDTO> GetVehicleMakes();

        [OperationContract]
        List<MakeDTO> GetVehicleMakesByDescription(string description);

        [OperationContract]
        List<YearDTO> GetVehicleYearsByMakeIdModelIdVersionId(int makeId, int modelId, int versionId);

        [OperationContract]
        List<ModelDTO> GetVehicleModelsByMakeId(int makeId);

        [OperationContract]
        List<ModelDTO> GetVehicleModelsByDescription(string description);

        [OperationContract]
        List<ColorDTO> GetVehicleColors();

        [OperationContract]
        List<VersionDTO> GetVehicleVersionsByMakeIdModelId(int makeId, int modelId);
        
        [OperationContract]
        List<VehicleDTO> GetRisksVehicleByInsuredId(int insuredId);

        [OperationContract]
        List<VehicleDTO> GetRisksVehicleByLicensePlate(string licensePlate);

        [OperationContract]
        VehicleDTO GetRiskVehicleByRiskId(int riskId);

        [OperationContract]
        List<VehicleDTO> GetSelectRisksVehicleByLicensePlate(string licencePlate);
    }
}
