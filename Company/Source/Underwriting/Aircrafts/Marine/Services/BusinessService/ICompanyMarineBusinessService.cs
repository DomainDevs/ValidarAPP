using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Marines.MarineBusinessService.Models.Base;
using Sistran.Core.Application.Marines.MarineBusinessService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;

namespace Sistran.Company.Application.Marines.MarineBusinessService
{
    [ServiceContract]
    public interface ICompanyMarineBusinessService : IMarineBusinessService
    {
        /// <summary>
        /// Crea un temporal
        /// </summary>
        /// <param name="companyMarine"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyMarine CreateCompanyMarineTemporal(CompanyMarine companyMarine);

        /// <summary>
        /// Editar Temporario - Riesgo que llega de Aplicación
        /// </summary>
        /// <param name="companyMarine"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyMarine UpdateCompanyMarineTemporal(CompanyMarine companyMarine);

        /// <summary>
        /// Eliminar Temporario - Riesgo
        /// </summary>
        /// <param name="riskId">Identificador del riesgo</param>
        /// <returns>Resultado de la operación</returns>
        [OperationContract]
        bool DeleteCompanyMarineTemporal(int riskId);

        /// <summary>
        /// Trae el riego por ID
        /// </summary>
        /// <param name="riskId"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyMarine GetCompanyMarineTemporalByRiskId(int riskId);

        /// <summary>
        /// Consultar un temporario - Riesgo
        /// </summary>
        /// <param name="temporalId">temporalId</param>
        /// <returns>List Marine</returns>
        [OperationContract]
        List<CompanyMarine> GetCompanyMarinesByTemporalId(int temporalId);

        /// <summary>
        /// Consultar los Riesgos de la poliza
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns>List Marines</returns>
        [OperationContract]
        List<CompanyMarine> GetCompanyMarinesByPolicyId(int policyId);

        /// <summary>
        /// Ejecuta reglas del riesgo
        /// </summary>
        /// <param name="companyMarine"></param>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyMarine RunRulesRisk(CompanyMarine companyMarine, int ruleId);

        /// <summary>
        /// Calcular prima Marine
        /// </summary>
        /// <param name="companyMarine">Cobertura</param>
        /// <param name="runRulesPre">Ejecutar Reglas Pre?</param>
        /// <param name="runRulesPost">Ejecutar Reglas Post?</param
        /// <returns>CompanyCoverage</returns>
        [OperationContract]
        CompanyMarine QuotateCompanyMarine(CompanyMarine companyMarine, bool runRulesPre, bool runRulesPost);

        /// <summary>
        /// Cuota de Marinee
        /// </summary>
        /// <param name="companyMarine"></param>
        /// <param name="runRulesPre"></param>
        /// <param name="runRulesPost"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyMarine> QuotateCompanyMarines(List<CompanyMarine> companyMarines, bool runRulesPre, bool runRulesPost);

        /// <summary>
        /// Calcular Cobertura
        /// </summary>
        /// <param name="companyCoverage">Cobertura</param>
        /// <param name="runRulesPre">Ejecutar Reglas Pre?</param>
        /// <param name="runRulesPost">Ejecutar Reglas Post?</param>
        /// <returns>CompanyCoverage</returns>
        [OperationContract]
        CompanyCoverage QuotateCompanyCoverage(CompanyMarine companyMarine, CompanyCoverage companyCoverage, bool runRulesPre, bool runRulesPost);

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
        /// Exclusión de Marinee
        /// </summary>
        /// <param name="companyMarine"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyMarine ExcludeCompanyMarine(int temporalId, int riskId);

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
        CompanyPolicy CreateEndorsement(CompanyPolicy companyPolicy, List<CompanyMarine> companyMarines);

        /// <summary>
        /// Consulta de riesgo por endoso
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyMarine> GetCompanyMarinesByPolicyIdEndorsementId( int policyId, int endorsementId);
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
        /// Consulta un riesgo a partir del identificador del endoso
        /// </summary>
        /// <param name="endorsementId">Identificador del endoso</param>
        /// <returns>Listado de riesgos tipo marine</returns>
        [OperationContract]
        List<CompanyMarine> GetCompanyMarinesByEndorsementId(int endorsementId);

        /// <summary>
        /// Consulta un riesgo a partir del identificador del endoso y el tipo de módulo
        /// </summary>
        /// <param name="endorsementId">Identificador del endoso</param>
        /// <param name="moduleType">Identificador del tipo de módulo</param>
        /// <returns>Listado de riesgos tipo marine</returns>
        [OperationContract]
        List<CompanyMarine> GetCompanyMarinesByEndorsementIdModuleType(int endorsementId, ModuleType moduleType);

        /// <summary>
        /// Consulta los riesgos a partir del identificador del asegurado
        /// </summary>
        /// <param name="insuredId"></param>
        /// <returns>Listado de riesgos tipo marine</returns>
        [OperationContract]
        List<CompanyMarine> GetCompanyMarinesByInsuredId(int insuredId);

        /// <summary>
        /// Consulta los riesgos a partir del identificador del riesgo
        /// </summary>
        /// <param name="riskId"></param>
        /// <returns>Riesgo tipo marine</returns>
        [OperationContract]
        CompanyMarine GetCompanyMarineByRiskId(int riskId);

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