using Sistran.Core.Application.Finances.FidelityServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Collections.Generic;
using System.ServiceModel;
namespace Sistran.Core.Application.Finances.FidelityServices
{
    [ServiceContract]
    public interface IFidelityServiceCore : Sistran.Core.Application.Finances.IFinancesCore
    {
        #region Claims

        /// <summary>
        /// Obtener riesgos de manejo por identificador del endoso y tipo de módulo
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        [OperationContract]
        List<FidelityRisk> GetRiskFidelitiesByEndorsementId(int endorsementId);

        /// <summary>
        /// Obtener riesgos de manejo por el identificador del asegurado
        /// </summary>
        /// <param name="insuredId"></param>
        /// <returns></returns>
        [OperationContract]
        List<FidelityRisk> GetRiskFidelitiesByInsuredId(int insuredId);

        /// <summary>
        /// Obtener riesgo de manejo por su identificador
        /// </summary>
        /// <param name="riskId"></param>
        /// <returns></returns>
        [OperationContract]
        FidelityRisk GetRiskFidelityByRiskId(int riskId);

        #endregion
    }

}
