using COMTRAMO = Sistran.Company.Application.Transports.Endorsement.CreditNote.ApplicationServices.DTOs;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.Transports.Endorsement.CreditNote.ApplicationServices
{
    [ServiceContract]
    public interface ITransportCreditNoteApplicationService
    {
        /// <summary>
        /// Retorna la lista de endosos que tienen cobro de depósito
        /// </summary>
        /// <param name="policyId">Identificador de la póliza</param>
        /// <returns>Lista de endosos</returns>
        [OperationContract]
        List<DTOs.EndorsementDTO> GetEndorsementsWithPremiumAmount(int policyId);

        /// <summary>
        /// Retorna un listado de riesgos asociados a un endoso
        /// </summary>
        /// <param name="policyId">Identificador de la póliza</param>
        /// <param name="endorsementId">Identificador del endoso</param>
        /// <returns>Listado de riesgos</returns>
        [OperationContract]
        List<COMTRAMO.EndorsementRiskDTO> GetRisksByPolicyIdEndorsementId(int policyId, int endorsementId);

        /// <summary>
        /// calculo de nota de credito
        /// <param name="creditNoteDTO">Modelo declaracion</param>
        /// </summary>
        [OperationContract]
        COMTRAMO.CreditNoteDTO QuotateCreditNote(COMTRAMO.CreditNoteDTO creditNoteDTO);

        /// <summary>
        /// lista los riespos asociados a la poliza 
        /// </summary>
        /// <param name="temporalId"></param>
        /// <param name="isMasive"></param>
        /// <returns></returns>
        [OperationContract]
        COMTRAMO.RiskDTO GetTransportsByPolicyIdByEndorsementId( int policyId,int endorsementId);

        /// <summary>
        /// Listado de coberturas por riesgo
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <param name="riskId"></param>
        /// <returns></returns>
        [OperationContract]
        COMTRAMO.CreditNoteDTO GetCompanyCoveragesByPolicyIdEndorsementIdRiskId (int policyId, int endorsementId, int riskId);

        /// <summary>
        /// Calcula endoso y Crea temporal nota credito 
        /// </summary>
        /// <param name="creditNoteDTO"></param>
        /// <returns></returns>
        [OperationContract]
        COMTRAMO.CreditNoteDTO CreateTemporal(COMTRAMO.CreditNoteDTO creditNoteDTO);

        [OperationContract]
        COMTRAMO.CreditNoteDTO GetTemporalById(int temporalId, bool isMasive);

        /// <summary>
        /// consulata si existe temporal para en el endoso de nota credito
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        [OperationContract]
        TransportApplicationService.DTOs.EndorsementDTO GetTemporalEndorsementByPolicyId(int policyId);

        /// <summary>
        /// Retorna el porcentaje máximo que se va a devolver para una póliza
        /// </summary>
        /// <param name="policyId">Identificador de la póliza</param>
        /// <returns>Porcentaje máximo a devolver</returns>
        [OperationContract]
        decimal GetMaximumPremiumPercetToReturn(int policyId);
    }
}