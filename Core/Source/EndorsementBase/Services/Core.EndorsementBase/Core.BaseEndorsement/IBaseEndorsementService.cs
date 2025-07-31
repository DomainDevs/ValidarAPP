using Sistran.Core.Application.BaseEndorsementService.DTOs;
using Sistran.Core.Application.EndorsementBaseService.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.BaseEndorsementService
{
    [ServiceContract]
    public interface IBaseEndorsementService
    {
        /// <summary>
        /// Obtener lista de motivos del endoso
        /// </summary>
        /// <param name="EndorsementType">Tipo de endoso</param>
        /// <returns>Lista de motivos del endoso</returns>
        [OperationContract]
        List<EndorsementReason> GetEndorsementReasonsByEndorsementType(EndorsementType endorsementType);

        /// <summary>
        /// Obtener temporal de una póliza
        /// </summary>
        /// <param name="policyId">Id póliza</param>
        /// <returns>modelo Endorsement</returns>
        [OperationContract]
        Endorsement GetTemporalEndorsementByPolicyId(int policyId);

        /// <summary>
        /// Grabar endoso de una póliza
        /// </summary>
        /// <param name="temporalId">Id temporal</param>
        /// <returns>Número de endoso</returns>
        [OperationContract]
        Policy CreateEndorsement(int temporalId);

        /// <summary>
        /// Gets the type of the modification.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EndorsementTypeDTO> GetModificationType();
    }
}
