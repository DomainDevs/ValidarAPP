using Sistran.Company.Application.DeclarationApplicationService.DTO;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.Location.PropertyServices.DTO;

namespace Sistran.Company.Application.DeclarationApplicationService
{
    [ServiceContract]
    public interface IDeclarationApplicationService
    {
        /// <summary>
        /// crear la declaration
        /// <param name="declarationDTO">Modelo declaracion</param>
        /// </summary>
        [OperationContract]
        DeclarationDTO CreateTemporal(DeclarationDTO declarationDTO);
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
        DeclarationDTO GetRiskByRiskId(CompanyPropertyRisk Risk);
        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        DeclarationDTO GetTemporalById(int temporalId, bool isMasive);
        /// <summary>
        /// Genera el próximo endoso de ajuste para una póliza
        /// </summary>
        /// <param name="policyId">Identificador de la póliza</param>
        /// <returns>Endoso de declaración</returns>
        [OperationContract]
        DeclarationDTO GetDeclarationEndorsementByPolicyId(int policyId);
        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        List<CompanyPropertyRisk> GetRisksByPolicyId(int policyId);
        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        DeclarationDTO GetRisksByPolicyIdCurrentFrom(int PolicyId, string currentFrom);
        /// <summary>
        /// Valida si algun objeto del seguro de todos los riesgos activos de una poliza es declarativo
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        [OperationContract]
        bool ValidateDeclarativeInsuredObjects(decimal policyId);

    }
}
