using Sistran.Core.Application.Sureties.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.ServiceModel;
using UNDTO = Sistran.Core.Application.UnderwritingServices.DTOs;
using UniquePersonModel = Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Core.Application.Sureties
{
    [ServiceContract]
    public interface ISuretiesCore
    {
        /// <summary>
        /// Obtener  las contragarantias que han sido asignadas
        /// </summary>
        /// <param name="guarantees">Listado de contragarantias</param>
        /// <returns>Lista de contragarantías cumplimiento</returns>
        [OperationContract]
        List<RiskSuretyGuarantee> GetRiskSuretyGuaranteesByGuarantees(List<IssuanceGuarantee> guarantees);
        /// <summary>
        /// Consulta los datos basicos de las polizas que esten asociadas al afianzado
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns>poliza con datos basicos</returns>
        [OperationContract]
        List<UNDTO.PolicyRiskDTO> GetPolicyRiskDTOsByIndividualId(int individualId);

        [OperationContract]
        List<IssuanceInsuredGuarantee> GetRiskSuretyGuaranteesByRiskId(int riskId);
    }
}
