using Sistran.Core.Integration.FidelityServices.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.FidelityServices
{
    [ServiceContract]
    public interface IFidelityIntegrationService
    {
        /// <summary>
        /// Obtener riesgos de manejo por identificador del endoso y tipo de módulo
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        [OperationContract]
        List<FidelityDTO> GetRiskFidelitiesByEndorsementId(int endorsementId);

        /// <summary>
        /// Obtener riesgos de manejo por el identificador del asegurado
        /// </summary>
        /// <param name="insuredId"></param>
        /// <returns></returns>
        [OperationContract]
        List<FidelityDTO> GetRiskFidelitiesByInsuredId(int insuredId);

        /// <summary>
        /// Obtener riesgo de manejo por su identificador
        /// </summary>
        /// <param name="riskId"></param>
        /// <returns></returns>
        [OperationContract]
        FidelityDTO GetRiskFidelityByRiskId(int riskId);

        /// <summary>
        /// Obtiene las ocupaciones
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<OccupationDTO> GetOccupations();
    }
}
