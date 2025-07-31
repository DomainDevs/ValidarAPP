using Sistran.Company.Application.Endorsement.CreditNoteApplicationService.DTOs;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.Base.Endorsement.CreditNoteApplicationService
{
    [ServiceContract]
    public interface IBaseCreditNoteApplicationService 
    {
        /// <summary>
        /// Retorna la lista de endosos que tienen cobro de depósito
        /// </summary>
        /// <param name="policyId">Identificador de la póliza</param>
        /// <returns>Lista de endosos</returns>
        [OperationContract]
        List<EndorsementDTO> GetEndorsementsWithPremiumAmount(int policyId);

        /// <summary>
        /// Retorna un listado de riesgos asociados a un endoso
        /// </summary>
        /// <param name="policyId">Identificador de la póliza</param>
        /// <param name="endorsementId">Identificador del endoso</param>
        /// <returns>Listado de riesgos</returns>
        [OperationContract]
        List<RiskDTO> GetRisksByPolicyIdEndorsementId(int policyId, int endorsementId);

        /// <summary>
        /// Cálculo del endoso
        /// </summary>
        /// <param name="creditNote">Endoso de nota crédito</param>
        /// <returns>Resúmen</returns>
        [OperationContract]
        CreditNoteDTO Calculate(CreditNoteDTO creditNote);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        EndorsementDTO GetTemporalEndorsementByPolicyId(int policyId);

    }
}