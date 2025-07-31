using System.ServiceModel;
using Sistran.Core.Application.Location.MassivePropertyServices;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.MassiveServices.Models;

namespace Sistran.Company.Application.Location.MassivePropertyServices
{
    [ServiceContract]
    public interface IMassivePropertyService : IMassivePropertyServiceCore
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
        MassiveLoad QuotateMassiveLoad(MassiveLoad massiveLoad );

        /// <summary>
        /// CreateMassiveRenewal
        /// </summary>
        /// <param name="massiveRenewal">modelo cargue masivo</param>
        /// <returns>MassiveLoad</returns>
        [OperationContract]
        MassiveRenewal CreateMassiveRenewal(MassiveRenewal massiveRenewal);


        /// <summary>
        /// Genera el archivo de reporte del proceso de emisión masiva
        /// </summary>
        /// <param name="massiveLoad">massiveLoad</param>
        /// <returns>string</returns>
        [OperationContract]
        string GenerateReportToMassiveLoad(MassiveEmission massiveEmission);

        /// <summary>
        /// Ejecuta la tarifación de un cargue masivo de renovación del ramo multiriesgo (Hogar)
        /// </summary>
        /// <param name="massiveRenewalId"></param>
        /// <returns></returns>
        [OperationContract]
        MassiveLoad QuotateMassiveRenewal(MassiveLoad massiveLoad);

        /// <summary>
        /// Genera el archivo de reporte del proceso de renovación masiva
        /// </summary>
        /// <param name="massiveRenewal"></param>
        /// <returns></returns>
        [OperationContract]
        string GenerateReportToMassiveRenewal(MassiveRenewal massiveRenewal);

        [OperationContract]
        MassiveLoad IssuanceMassiveEmission(MassiveLoad massiveLoad);

        [OperationContract]
        MassiveLoad IssuanceRenewalMassiveEmission(MassiveLoad massiveLoad);

        [OperationContract]
        int FindExternalServicesLoad(int massiveLoadId);

        [OperationContract]
        void ProcessResponseFromScoreService(string response);
    }
}
