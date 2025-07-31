using Sistran.Company.Application.Transports.Endorsement.Declaration.ApplicationServices.DTOs;
using Sistran.Company.Application.Transports.TransportApplicationService.DTOs;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.Transports.Endorsement.Declaration.ApplicationServices
{
    /// <summary>
    /// Inter
    /// </summary>
    [ServiceContract]
    public interface ITransportDeclarationApplicationService
    {
        /// <summary>
        /// crear la declaration
        /// <param name="declarationDTO">Modelo declaracion</param>
        /// </summary>
        [OperationContract]
        DeclarationDTO CreateDeclaration(DeclarationDTO declarationDTO);
        /// <summary>
        /// calculo de la declaration
        /// <param name="declarationDTO">Modelo declaracion</param>
        /// </summary>
        [OperationContract]
        DeclarationDTO QuotateDeclaration(DeclarationDTO declarationDTO);
        /// <summary>
        /// Consulta información de la póliza
        /// <param name="prefixId" name="branchId" name="policyNumber"></param>
        /// </summary>
        [OperationContract]
        DeclarationDTO GetPolicyByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber);
        /// <summary>
        /// consulta riesgos
        /// <param name="PolicyId"></param>
        /// </summary>
        [OperationContract]
        DeclarationDTO GetTransportsByPolicyId(int PolicyId, string currentFrom);
        /// <summary>
        /// Devuelve los objetos del seguro asociados al riesgo seleccionado
        /// </summary>
        /// <param name="riskId"></param>
        /// <returns></returns>
        [OperationContract]
        List<InsuredObjectDTO> GetInsuredObjectsByRiskId(int riskId);
        /// <summary>
        /// consulta de endosos por tipo y poliza
        /// <param name="endorsementTypeId" name="PolicyId"></param>
        /// </summary>
        [OperationContract]
        DeclarationDTO GetCompanyEndorsementByEndorsementTypeIdPolicyId(int endorsementTypeId, int PolicyId);
        /// <summary>
        /// consulta el riesgo
        /// <param name="RiskId"></param>
        /// </summary>
        [OperationContract]
        DeclarationDTO GetTransportByRiskId(TransportDTO Risk);
        [OperationContract]
        DeclarationDTO GetTemporalById(int temporalId, bool isMasive);

		/// <summary>
        /// Genera el próximo endoso de ajuste para una póliza
        /// </summary>
        /// <param name="policyId">Identificador de la póliza</param>
        /// <returns>Endoso de declaración</returns>
        [OperationContract]
        DeclarationDTO GetDeclarationEndorsementByPolicyId(int policyId);
    }
}