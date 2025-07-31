using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Aircrafts.AircraftBusinessService.Models.Base;
using Sistran.Core.Application.Aircrafts.AircraftBusinessService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;

namespace Sistran.Company.Application.Aircrafts.AircraftBusinessService
{
    [ServiceContract]
    public interface ICompanyAircraftBusinessService : IAircraftBusinessService
    {
        /// <summary>
        /// Crea un temporal
        /// </summary>
        /// <param name="companyAircraft"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyAircraft CreateCompanyAircraftTemporal(CompanyAircraft companyAircraft);

        /// <summary>
        /// Editar Temporario - Riesgo que llega de Aplicación
        /// </summary>
        /// <param name="companyAircraft"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyAircraft UpdateCompanyAircraftTemporal(CompanyAircraft companyAircraft);

        /// <summary>
        /// Eliminar Temporario - Riesgo
        /// </summary>
        /// <param name="riskId">Identificador del riesgo</param>
        /// <returns>Resultado de la operación</returns>
        [OperationContract]
        bool DeleteCompanyAircraftTemporal(int riskId);

        /// <summary>
        /// Trae el riego por ID
        /// </summary>
        /// <param name="riskId"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyAircraft GetCompanyAircraftTemporalByRiskId(int riskId);

        /// <summary>
        /// Consultar un temporario - Riesgo
        /// </summary>
        /// <param name="temporalId">temporalId</param>
        /// <returns>List Aircraft</returns>
        [OperationContract]
        List<CompanyAircraft> GetCompanyAircraftsByTemporalId(int temporalId);

        /// <summary>
        /// Consultar los Riesgos de la poliza
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns>List Aircrafts</returns>
        [OperationContract]
        List<CompanyAircraft> GetCompanyAircraftsByPolicyId(int policyId);

        /// <summary>
        /// Ejecuta reglas del riesgo
        /// </summary>
        /// <param name="companyAircraft"></param>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyAircraft RunRulesRisk(CompanyAircraft companyAircraft, int ruleId);

        /// <summary>
        /// Calcular prima Aircraft
        /// </summary>
        /// <param name="companyAircraft">Cobertura</param>
        /// <param name="runRulesPre">Ejecutar Reglas Pre?</param>
        /// <param name="runRulesPost">Ejecutar Reglas Post?</param
        /// <returns>CompanyCoverage</returns>
        [OperationContract]
        CompanyAircraft QuotateCompanyAircraft(CompanyAircraft companyAircraft, bool runRulesPre, bool runRulesPost);

        /// <summary>
        /// Cuota de Aircrafte
        /// </summary>
        /// <param name="companyAircraft"></param>
        /// <param name="runRulesPre"></param>
        /// <param name="runRulesPost"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyAircraft> QuotateCompanyAircrafts(List<CompanyAircraft> companyAircrafts, bool runRulesPre, bool runRulesPost);

        /// <summary>
        /// Calcular Cobertura
        /// </summary>
        /// <param name="companyCoverage">Cobertura</param>
        /// <param name="runRulesPre">Ejecutar Reglas Pre?</param>
        /// <param name="runRulesPost">Ejecutar Reglas Post?</param>
        /// <returns>CompanyCoverage</returns>
        [OperationContract]
        CompanyCoverage QuotateCompanyCoverage(CompanyAircraft companyAircraft, CompanyCoverage companyCoverage, bool runRulesPre, bool runRulesPost);

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
        /// Exclusión de Aircrafte
        /// </summary>
        /// <param name="companyAircraft"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyAircraft ExcludeCompanyAircraft(int temporalId, int riskId);

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
        CompanyPolicy CreateEndorsement(CompanyPolicy companyPolicy, List<CompanyAircraft> companyAircrafts);

        /// <summary>
        /// Consulta de riesgo por endoso
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyAircraft> GetCompanyAircraftsByPolicyIdEndorsementId( int policyId, int endorsementId);
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
        [OperationContract]
        List<CompanyCoverage> GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(int policyId, int endorsementId, int riskId);
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
        bool saveInsuredObject(int riskId, InsuredObject insuredObject, int tempId, int groupCoverageId);

        /// <summary>
        /// Consulta los riesgos a partir del identificador del endoso
        /// </summary>
        /// <param name="endorsementId">Identificador del endoso</param>
        /// <returns>Listado de reisgos</returns>
        [OperationContract]
        List<CompanyAircraft> GetCompanyAircraftsByEndorsementId(int endorsementId);

        /// <summary>
        /// Consulta los riesgos por identificador del asegurado
        /// </summary>
        /// <param name="insuredId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyAircraft> GetCompanyAircraftByInsuredId(int insuredId);

        /// <summary>
        /// Consulta los riesgos por identificador del riesgo
        /// </summary>
        /// <param name="insuredId"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyAircraft GetCompanyAircraftByRiskId(int riskId);

        /// <summary>
        /// Consulta los riesgos de casco aviación por endoso y tipo de módulo
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyAircraft> GetCompanyAircraftsByEndorsementIdModuleType(int endorsementId, ModuleType moduleType);

        /// <summary>
        /// Creacion de poliza
        /// </summary>
        /// <param name="temporalId"></param>
        /// <param name="temporalType"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicyResult CreateCompanyPolicy(int temporalId, int temporalType);
    }
}