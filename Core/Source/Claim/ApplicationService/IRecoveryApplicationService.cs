using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using Sistran.Core.Application.ClaimServices.DTOs.Recovery;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.ClaimServices
{
    [ServiceContract]
    public interface IRecoveryApplicationService
    {
        /// <summary>
        /// Crear un recobro
        /// </summary>
        /// <param name="recovery"></param>
        /// <returns></returns>
        [OperationContract]
        RecoveryDTO CreateRecovery(RecoveryDTO recovery);

        /// <summary>
        /// Actualizar un recobro
        /// </summary>
        /// <param name="recovery"></param>
        /// <returns></returns>
        [OperationContract]
        RecoveryDTO UpdateRecovery(RecoveryDTO recovery);

        /// <summary>
        /// Obtener los tipos de recobros por identificador
        /// </summary>
        /// <param name="recoveryTypeId"></param>
        /// <returns></returns>
        [OperationContract]
        List<RecoveryTypeDTO> GetRecoveryTypesById(int recoveryTypeId);

        /// <summary>
        /// Obtiene Recobros por denuncia y subreclamo
        /// </summary>
        /// <param name="claimId"></param>
        /// <param name="subClaimId"></param>
        /// <returns></returns>
        [OperationContract]
        List<RecoveryDTO> GetRecoveriesByClaimIdSubClaimId(int claimId, int subClaimId);

        /// <summary>
        /// Obtener los tipos de recobros 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<RecoveryTypeDTO> GetRecoveryTypes();

        /// <summary>
        /// Obtiene los recobros por siniestro
        /// </summary>
        /// <param name="claimId"></param>
        /// <returns></returns>
        [OperationContract]
        List<RecoveryDTO> GetRecoveriesByClaimId(int claimId);

        /// <summary>
        /// Obtiene los recobros de una denuncia por criterios de búsuqeda de la denuncia
        /// </summary>
        /// <param name="prefixId"></param>
        /// <param name="branchId"></param>
        /// <param name="policyDocumentNumber"></param>
        /// <param name="claimNumber"></param>
        /// <returns></returns>
        [OperationContract]
        List<RecoveryDTO> GetRecoveriesByClaim(int? prefixId, int? branchId, string policyDocumentNumber, int claimNumber);

        /// <summary>
        /// Obtener el recobro por su identificador
        /// </summary>
        /// <param name="recoveryId"></param>
        /// <returns></returns>
        [OperationContract]
        RecoveryDTO GetRecoveryByRecoveryId(int recoveryId);

        /// <summary>
        /// Obtener el número de recobros de una denuncia
        /// </summary>
        /// <param name="claimId"></param>
        /// <param name="subClaimId"></param>
        /// <returns></returns>
        [OperationContract]
        int GetRecoveryNumberByClaimIdSubClaimId(int claimId, int subClaimId);
    }
}