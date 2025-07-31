using System.ServiceModel;
using Sistran.Core.Application.Location.MassiveLiabilityServices;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveRenewalServices.Models;

namespace Sistran.Company.Application.Location.MassiveLiabilityServices
{
    [ServiceContract]
    public interface IMassiveLiabilityService : IMassiveLiabilityServiceCore
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
        MassiveLoad QuotateMassiveLoad(int massiveLoadId);

        /// <summary>
        /// Crear cargue masivo de renovación
        /// </summary>
        /// <param name="massiveRenewal">Cargue Masivo</param>
        /// <returns>Cargue Masivo</returns>
        [OperationContract]
        MassiveRenewal CreateMassiveRenewal(MassiveRenewal massiveRenewal);

        /// <summary>
        /// Tarifar Cargue Masivo de renovación
        /// </summary>
        /// <param name="massiveLoadId">Id Cargue Masivo de renovación</param>
        /// <returns>Cargue Masivo</returns>
        [OperationContract]
        MassiveLoad QuotateMassiveRenewal(int massiveLoadId);

        [OperationContract]
        MassiveLoad IssuanceMassiveEmission(MassiveLoad massiveLoad);

        [OperationContract]
        MassiveLoad IssuanceRenewalMassiveEmission(MassiveLoad massiveLoad);

        /// <summary>
        /// Generacion de Reportes
        /// </summary>
        /// <param name="massiveEmission"></param>
        /// <returns></returns>
        [OperationContract]
        string GenerateReportToMassiveLoad(MassiveEmission massiveEmission);
    }
}