using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.MassiveServices.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.Location.PropertyCollectiveService
{
    [ServiceContract]
    public interface IPropertyCollectiveService
    {
        [OperationContract]
        CollectiveEmission CreateCollectiveEmission(CollectiveEmission collectiveEmission);

        [OperationContract]
        void QuotateCollectiveEmission(List<int> collectiveEmissionIds);

        [OperationContract]
        void QuotateMassiveCollectiveEmission(MassiveLoad massiveLoad);

        /// <summary>
        /// Genera el archivo de reporte del proceso de emisión masiva
        /// </summary>
        /// <param name="massiveLoad">massiveLoad</param>
        /// <returns>string</returns>
        [OperationContract]
        string GenerateReportToCollectiveLoad(CollectiveEmission collectiveEmission, int endorsementType);

        /// <summary>
        /// Emite póliza colectiva de autos desde un cargue masivo de hogar
        /// </summary>
        /// <param name="collectiveEmission"></param>
        /// <returns></returns>
        [OperationContract]
        MassiveLoad IssuanceCollectiveEmission(MassiveLoad collectiveEmission);
    }
}
