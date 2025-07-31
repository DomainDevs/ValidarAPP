using System.ServiceModel;
using System.Collections.Generic;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.Vehicles.TPLCollectiveServices;

namespace Sistran.Company.Application.Vehicles.TPLCollectiveServices
{
    [ServiceContract]
    public interface IThirdPartyLiabilityCollectiveService : IThirdPartyLiabilityCollectiveServiceCore
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
        /// Envia a guardar temporales y actualizar el cargue de colectivas
        /// </summary>
        /// <param name="collectiveEmission"></param>
        /// <returns></returns>
        [OperationContract]
        void SaveTemporalTpl(string[] objectsToSave);
    }
}