using System.ServiceModel;
using System.Collections.Generic;
using Sistran.Core.Application.Vehicles.VehicleCollectiveServices;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.MassiveServices.Models;

namespace Sistran.Company.Application.Vehicles.VehicleCollectiveServices
{
    [ServiceContract]
    public interface IVehicleCollectiveService : IVehicleCollectiveServiceCore
    {
        [OperationContract]
        CollectiveEmission CreateCollectiveLoad(CollectiveEmission collectiveLoad);

        [OperationContract]
        CollectiveEmissionRow GetCollectiveEmissionRowById(int id);
        

        [OperationContract]
        void QuotateCollectiveLoad(List<int> collectiveLoadIds);

        [OperationContract]
        void QuotateMassiveCollectiveEmission(MassiveLoad massiveLoad);

        /// <summary>
        /// Genera el archivo de reporte del proceso de emisión masiva
        /// </summary>
        /// <param name="collectiveEmission">massiveLoad</param>
        /// <param name="endorsementType"></param>
        /// <returns>string</returns>
        [OperationContract]
        string GenerateReportToCollectiveLoad(CollectiveEmission collectiveEmission, int endorsementType);

        /// <summary>
        /// Emite póliza colectiva de autos desde un cargue masivo de autos
        /// </summary>
        /// <param name="collectiveEmission"></param>
        /// <returns></returns>
        [OperationContract]
        MassiveLoad IssuanceCollectiveEmission(MassiveLoad collectiveEmission);

        /// <summary>
        /// Guarda el temporal del riesgo para un cargue masivo de autos
        /// </summary>
        /// <param name="collectiveEmission"></param>
        /// <returns></returns>
        [OperationContract]
        void SaveTemporalVehicle(string[] objectsToSave);
    }
}