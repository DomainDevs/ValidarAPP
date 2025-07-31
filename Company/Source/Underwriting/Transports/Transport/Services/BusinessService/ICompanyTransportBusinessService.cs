using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Transports.TransportBusinessService.Models.Base;
using Sistran.Core.Application.Transports.TransportBusinessService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Transports.TransportBusinessService.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;

namespace Sistran.Company.Application.Transports.TransportBusinessService
{
    [ServiceContract]
    public interface ICompanyTransportBusinessService : ITransportBusinessService
    {
        /// <summary>
        /// Crea un temporal
        /// </summary>
        /// <param name="companyTransport"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyTransport CreateCompanyTransportTemporal(CompanyTransport companyTransport);

        /// <summary>
        /// Editar Temporario - Riesgo que llega de Aplicación
        /// </summary>
        /// <param name="companyTransport"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyTransport UpdateCompanyTransportTemporal(CompanyTransport companyTransport);

        /// <summary>
        /// Eliminar Temporario - Riesgo
        /// </summary>
        /// <param name="riskId">Identificador del riesgo</param>
        /// <returns>Resultado de la operación</returns>
        [OperationContract]
        bool DeleteCompanyTransportTemporal(int riskId);

        /// <summary>
        /// Trae el riego por ID
        /// </summary>
        /// <param name="riskId"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyTransport GetCompanyTransportTemporalByRiskId(int riskId);

        /// <summary>
        /// Consultar un temporario - Riesgo
        /// </summary>
        /// <param name="temporalId">temporalId</param>
        /// <returns>List Transport</returns>
        [OperationContract]
        List<CompanyTransport> GetCompanyTransportsByTemporalId(int temporalId);

        /// <summary>
        /// Consultar los Riesgos de la poliza
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns>List Transports</returns>
        [OperationContract]
        List<CompanyTransport> GetCompanyTransportsByPolicyId(int policyId);

        /// <summary>
        /// Ejecuta reglas del riesgo
        /// </summary>
        /// <param name="companyTransport"></param>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyTransport RunRulesRisk(CompanyTransport companyTransport, int ruleId);

        /// <summary>
        /// Calcular prima transport
        /// </summary>
        /// <param name="companyTransport">Cobertura</param>
        /// <param name="runRulesPre">Ejecutar Reglas Pre?</param>
        /// <param name="runRulesPost">Ejecutar Reglas Post?</param
        /// <returns>CompanyCoverage</returns>
        [OperationContract]
        CompanyTransport QuotateCompanyTransport(CompanyTransport companyTransport, bool runRulesPre, bool runRulesPost);

        /// <summary>
        /// Cuota de Transporte
        /// </summary>
        /// <param name="companyTransport"></param>
        /// <param name="runRulesPre"></param>
        /// <param name="runRulesPost"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyTransport> QuotateCompanyTransports(List<CompanyTransport> companyTransports, bool runRulesPre, bool runRulesPost);

        /// <summary>
        /// Calcular Cobertura
        /// </summary>
        /// <param name="companyCoverage">Cobertura</param>
        /// <param name="runRulesPre">Ejecutar Reglas Pre?</param>
        /// <param name="runRulesPost">Ejecutar Reglas Post?</param>
        /// <returns>CompanyCoverage</returns>
        [OperationContract]
        CompanyCoverage QuotateCompanyCoverage(CompanyTransport companyTransport, CompanyCoverage companyCoverage, bool runRulesPre, bool runRulesPost);

        /// <summary>
        /// Cuota de cobertura
        /// </summary>
        /// <param name="companyCoverage"></param>
        /// <param name="runRulesPre"></param>
        /// <param name="runRulesPost"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyCoverage> QuotateCompanyCoverages(List<CompanyCoverage> companyCoverages, bool runRulesPre, bool runRulesPost);

        /// <summary>
        /// Exclusión de transporte
        /// </summary>
        /// <param name="companyTransport"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyTransport ExcludeCompanyTransport(int temporalId, int riskId);

        /// <summary>
        /// Exclusion de covertura
        /// </summary>
        /// <param name="temporalId"></param>
        /// <param name="riskId"></param>
        /// <param name="riskCoverageId"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyCoverage ExcludeCompanyCoverage(int temporalId, int riskId, int coverageId);

        [OperationContract]
        CompanyPolicy CreateEndorsement(CompanyPolicy companyPolicy, List<CompanyTransport> companyTransports);

        /// <summary>
        /// Consulta de riesgo por endoso
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyTransport> GetCompanyTransportsByPolicyIdEndorsementId(int policyId, int endorsementId);
        /// <summary>
        ///listado de endosos por tipo de endoso
        /// </summary>
        /// <param name="endorsementTypeId"></param>
        /// <param name="policyId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyEndorsement> GetCompanyEndorsementByEndorsementTypeIdPolicyId(int endorsementTypeId, int policyId);

        [OperationContract]
        List<CompanyCoverage> GetCoveragesByRiskId(int riskId, int temporalId);

        /// <summary>
        /// Verifica si es año bisiesto
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool GetLeapYear();
        /// <summary>
        ///  Eliminar el riesgo en la emision
        /// </summary>
        /// <param name="temporalId"></param>
        /// <param name="riskId"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteCompanyRisk(int temporalId, int riskId);
        /// <summary>
        /// Coberturas asociadas a un objeto del seguro perteneciente a un riesgo
        /// </summary>
        /// <param name="riskId"></param>
        /// <param name="insuredObjectId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyCoverage> GetTemporalCoveragesByRiskIdInsuredObjectId(int riskId, int insuredObjectId);
        /// <summary>
        /// Se guarda un objeto del seguro nuevo, asociado a un riesgo
        /// </summary>
        /// <param name="riskId"></param>
        /// <param name="insuredObject"></param>
        /// <param name="tempId"></param>
        /// <param name="groupCoverageId"></param>
        /// <returns></returns>
        [OperationContract]
        bool saveInsuredObject(int riskId, CompanyInsuredObject insuredObject, int tempId, int groupCoverageId);

        [OperationContract]
        List<CompanyRiskCommercialClass> GetRiskCommercialClasses(string description);

        [OperationContract]
        List<CompanyHolderType> GetHolderTypes();

        [OperationContract]
        List<CompanyTransport> GetCompanyTransportByEndorsementIdModuleType(int endorsementId, ModuleType moduleType);

        [OperationContract]
        List<CompanyTransport> GetCompanyTransportsByInsuredId(int insuredId);

        [OperationContract]
        CompanyTransport GetCompanyTransportByRiskId(int riskId);

        /// <summary>
        /// Obtiene la vigencia para el próximo endoso de ajuste para una póliza
        /// </summary>
        /// <param name="policyId">Identificador de la póliza</param>
        /// <returns>Endoso configurado con la vigencia para el próximo endoso de ajuste</returns>
        [OperationContract]
        CompanyEndorsement GetNextAdjustmentEndorsementByPolicyId(int policyId);

        /// <summary>
        /// Obtiene la vigencia para el próximo endoso de declaración´para una póliza
        /// </summary>
        /// <param name="policyId">Identificador de la póliza</param>
        /// <returns>Endoso configurado con la vigencia para el próximo endoso de declaración</returns>
        [OperationContract]
        CompanyEndorsement GetNextDeclarationEndorsementByPolicyId(int policyId);

        /// <summary>
        /// Indica si es posible hacer un endoso de declaración
        /// </summary>
        /// <param name="policyId">Identificador de la póliza</param>
        /// <returns>True, si la póliza puede generar endoso de declaración</returns>
        [OperationContract]
        bool CanMakeDeclarationEndorsement(int policyId);

        /// <summary>
        /// Indica si es posible hacer un endoso de Ajuste
        /// </summary>
        /// <param name="policyId">Identificador de la póliza</param>
        /// <returns>True, si la póliza puede generar endoso de ajuste</returns>
        [OperationContract]
        bool CanMakeAdjustmentEndorsement(int policyId);

        /// <summary>
        /// Creates the company policy.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="temporalType">Type of the temporal.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="BusinessException"></exception>
        [OperationContract]
        CompanyPolicyResult CreateCompanyPolicy(int temporalId, int temporalType, bool clearPolicies);

        [OperationContract]
        bool CanMakeEndorsement(int policyId, out Dictionary<string, object> endorsementValidate);

        [OperationContract]
        List<CompanyCoverage> GetCoveragesByCoveragesAdd(int productId, int coverageGroupId, int prefixId, string coveragesAdd, int insuredObjectId);

        [OperationContract]
        CompanyPolicy UpdateCompanyRisks(int temporalId);
        [OperationContract]
        CompanyEndorsementPeriod SaveCompanyEndorsementPeriod(CompanyEndorsementPeriod companyEndorsementPeriod);

        [OperationContract]
        bool CanMakeEndorsementByRiskByInsuredObjectId(decimal policyId, decimal riskId, decimal insuredObjectId, EndorsementType endorsementType);

        [OperationContract]
        CompanyCoverage GetCoverageByCoverageId(int coverageId, int temporalId, int groupCoverageId);
    }
}