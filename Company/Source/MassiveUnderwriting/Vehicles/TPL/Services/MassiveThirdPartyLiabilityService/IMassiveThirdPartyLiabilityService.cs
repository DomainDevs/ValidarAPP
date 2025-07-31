using System.ServiceModel;
using Sistran.Core.Application.Vehicles.MassiveUnderwritingTPLServices;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveRenewalServices.Models;

namespace Sistran.Company.Application.Vehicles.MassiveThirdPartyLiabilityServices
{
    [ServiceContract]
    [XmlSerializerFormat]
    public interface IMassiveThirdPartyLiabilityService : IMassiveTPLServiceCore
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

        [OperationContract]
        MassiveLoad IssuanceRenewalMassiveEmission(MassiveLoad massiveLoad);

    }
}