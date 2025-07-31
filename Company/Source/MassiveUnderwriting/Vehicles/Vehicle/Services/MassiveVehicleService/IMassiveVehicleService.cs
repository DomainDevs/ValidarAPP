using System.ServiceModel;
using Sistran.Core.Application.Vehicles.MassiveUnderwritingVehicleServices;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using System.Collections.Generic;

namespace Sistran.Company.Application.Vehicles.MassiveVehicleServices
{
    [ServiceContract]
    [XmlSerializerFormat]
    public interface IMassiveVehicleService : IMassiveVehicleServiceCore
    {
        /// <summary>
        /// Crear Cargue Masivo
        /// </summary>
        /// <param name="MassiveLoad">Cargue Masivo</param>
        /// <returns>Cargue Masivo</returns>
        [OperationContract]
        MassiveEmission CreateMassiveEmission(MassiveEmission massiveEmission);

        /// <summary>
        /// Tarifar Cargue Masivo
        /// </summary>
        /// <param name="massiveLoadId">Id Cargue Masivo</param>
        /// <returns>Cargue Masivo</returns>
        [OperationContract]
        MassiveLoad QuotateMassiveLoad(MassiveLoad massiveLoad);

        /// <summary>
        /// Genera el archivo de reporte del proceso de emisión masiva
        /// </summary>
        /// <param name="massiveLoadProccessId"></param>
        /// <returns></returns>
        [OperationContract]
        string GenerateReportToMassiveLoad(MassiveEmission massiveLoad);


        [OperationContract]
        MassiveRenewal CreateMassiveRenewal(MassiveRenewal massiveRenewal);

        [OperationContract]
        MassiveLoad QuotateMassiveRenewal(MassiveLoad massiveLoad);

        /// <summary>
        /// Obtiene una lista de vigencias extendidas, teniendo en cuenta la lista de placas
        /// recibidas como parámetro.
        /// </summary>
        /// <param name="licensePlateVehicleCollection">Lista de placas consultada</param>
        /// <returns>Lista de polizas asociadas a las placas recibida</returns>
        //[OperationContract]
        //List<VehiclePolicyExtend> GetExtendListByVehiclePlateCollection(List<string> licensePlateVehicleCollection);

        /// <summary>
        /// Genera el archivo de error del proceso de emisión masiva
        /// </summary>
        /// <param name="massiveLoadId"></param>
        /// <returns></returns>
        [OperationContract]
        string GenerateFileErrorsMassiveEmission(int massiveLoadId);

        /// <summary>
        /// Genera el reporte del proceso de renovación masiva
        /// </summary>
        /// <param name="massiveRenewal"></param>
        /// <returns></returns>
        [OperationContract]
        string GenerateReportToMassiveRenewal(MassiveRenewal massiveRenewal);

        [OperationContract]
        MassiveLoad IssuanceMassiveEmission(MassiveLoad massiveLoad);

        //[OperationContract]
        //int FindExternalServicesLoad(int massiveLoadId);

        [OperationContract]
        void ProcessResponseFromScoreService(string response);

        [OperationContract]
        void ProcessResponseFromSimitService(string response);

        [OperationContract]
        void ProcessResponseFromExperienceServiceHistoricPolicies(string response);

        [OperationContract]
        void ProcessResponseFromExperienceServiceHistoricSinister(string response);

        [OperationContract]
        MassiveLoad IssuanceRenewalMassiveEmission(MassiveLoad massiveLoad);

    }
}