using Sistran.Company.Application.Transports.Endorsement.Adjustment.ApplicationServices.DTOs;
using Sistran.Company.Application.Transports.TransportApplicationService.DTOs;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.Transports.Endorsement.Adjustment.ApplicationServices
{
    /// <summary>
    /// Interfaz de ajuste
    /// </summary>
    [ServiceContract]
    public interface ITransportAdjustmentApplicationService
    {
        /// <summary>
        /// calculo del ajuste
        /// <param name="adjustmentDTO">Modelo declaracion</param>
        /// </summary>
        [OperationContract]
        AdjustmentDTO QuotateAdjustment(AdjustmentDTO adjustmentDTO);
        
        /// <summary>
        /// lista los riespos asociados a la poliza 
        /// </summary>
        /// <param name="temporalId"></param>
        /// <param name="isMasive"></param>
        /// <returns></returns>
        [OperationContract]
        AdjustmentDTO GetTransportsByPolicyId(int policyId, string currentFrom);

        /// <summary>
        /// Caulcula la fecha desde para ennuevo endoso depediendo de periodo de ajuste 
        /// </summary>
        /// <param name="CurrentFrom"></param>
        /// <param name="CurrentTo"></param>
        /// <param name="BillingPeriodId"></param>
        /// <returns></returns>
        [OperationContract]
        AdjustmentDTO CalculateDays(string CurrentFrom, string CurrentTo, int BillingPeriodId);

        /// <summary>
        /// lista los endosos de tipóm declaracion
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        [OperationContract]
        AdjustmentDetailsDTO GetEndorsementByEndorsementTypeDeclarationPolicyId(int policyId, int riskId);
        /// <summary>
        /// crear temporal
        /// </summary>
        /// <param name="adjustmentDTO"></param>
        /// <returns></returns>
        [OperationContract]
        AdjustmentDTO CreateTemporal(AdjustmentDTO adjustmentDTO);

        [OperationContract]
        EndorsementDTO GetTemporalEndorsementByPolicyId(int policyId);
        [OperationContract]
        AdjustmentDTO GetTransportsByTemporalId(int temporalId, bool isMasive);
        /// <summary>
        /// Devuelve los objetos del seguro asociados al riesgo seleccionado
        /// </summary>
        /// <param name="riskId"></param>
        /// <returns></returns>
        [OperationContract]
        List<InsuredObjectDTO> GetInsuredObjectsByRiskId( int riskId);

		/// <summary>
        /// Retorna un objeto de tipo endoso con las vigencias para el endoso de ajuste
        /// </summary>
        /// <param name="policyId">Identificador de la póliza</param>
        /// <returns>Endoso de ajuste</returns>
        [OperationContract]
        AdjustmentDTO GetAdjustmentEndorsementByPolicyId(int policyId);
    }
}