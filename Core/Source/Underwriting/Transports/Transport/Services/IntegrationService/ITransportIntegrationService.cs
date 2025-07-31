using Sistran.Core.Integration.TransportServices.DTOs;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Integration.TransportServices
{
    [ServiceContract]
    public interface ITransportIntegrationService
    {
        /// <summary>
        /// Obtiene los riesgos de transporte según el Id del Endoso
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        [OperationContract]
        List<TransportDTO> GetTransportByEndorsementIdModuleType(int endorsementId, ModuleType moduleType);


        /// <summary>
        /// Obtener riesgos de transporte por identificador del asegurado
        /// </summary>
        /// <param name="insuredId"></param>
        /// <returns></returns>
        [OperationContract]
        List<TransportDTO> GetTransportsByInsuredId(int insuredId);

        /// <summary>
        /// Obtener riesgo de transporte por su identificador
        /// </summary>
        /// <param name="riskId"></param>
        /// <returns></returns>
        [OperationContract]
        TransportDTO GetRiskTransportByRiskId(int riskId);
    }
}