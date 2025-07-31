using Sistran.Company.Application.Finances.FidelityServices.Models;
using Sistran.Company.Application.Location.FidelityServices.DTOs;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Finances.FidelityServices;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.Finances.FidelityServices
{
    [ServiceContract]
    public interface IFidelityService : IFinances
    {
        /// <summary>
        /// Ejecucion de reglas Pre de riesgo
        /// </summary>
        /// <param name="fidelityRisk">Riesgo</param>
        /// <param name="ruleSetId">Id regla</param>
        /// <returns></returns>
        [OperationContract]
        CompanyFidelityRisk RunRulesRisk(CompanyFidelityRisk fidelityRisk, int ruleSetId);
        /// <summary>
        /// Runs the rules Coverage pre.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        CompanyCoverage RunRulesCompanyCoverage(CompanyFidelityRisk fidelityRisk, CompanyCoverage coverage, int ruleSetId);


        /// <summary>
        /// Cotiza la poliza especifica lo los riesgos que contenga
        /// </summary>
        /// <param name="fidelityPolicy">Poliza de rc a tarifar.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyFidelityRisk QuotateFidelity(CompanyFidelityRisk companyFidelityRisk, bool runRulesPre, bool runRulesPost);



        /// <summary>
        /// Cotiza la cobertura especificada
        /// </summary>
        /// <param name="coverage">Cobertura a cotizar</param>
        /// <returns></returns>
        [OperationContract]
        CompanyCoverage QuotationCompanyCoverage(CompanyFidelityRisk fidelityRisk, CompanyCoverage coverage, bool runRulesPre, bool runRulesPost);



        /// <summary>
        /// Cotiza el riesgo especificado
        /// </summary>
        /// <param name="fidelityRisk">Riesgo a cotizar</param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyFidelityRisk> QuotateFidelities(CompanyPolicy companyPolicy, List<CompanyFidelityRisk> companyFidelityRisks, bool runRulesPre, bool runRulesPost);

        /// <summary>
        /// Insertar en tablas temporales desde el JSON
        /// </summary>
        /// <param name="fidelityRisk">Modelo fidelityRisk</param>
        [OperationContract]
        CompanyFidelityRisk CreateFidelityTemporal(CompanyFidelityRisk fidelityRisk, bool isMassive);

        /// <summary>
        /// Obtener Poliza de responsabilidad civil
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <param name="temporalId">Id temporal</param>
        /// <returns>FidelityPolicy</returns>
        [OperationContract]
        List<CompanyFidelityRisk> GetCompanyFidelitiesByPolicyId(int policyId);

        [OperationContract]
        List<CompanyFidelityRisk> GetCompanyFidelitiesByEndorsementId(int endorsementId);

        //[OperationContract]
        //List<CompanyFidelityRisk> GetCompanyFidelitiesRiskByEndorsementId(int endorsementId);

        [OperationContract]
        /// <summary>
        /// Obtener Riesgos
        /// </summary>
        /// <param name="temporalId">Id Temporal</param>
        /// <returns>Riesgos</returns>
        List<CompanyFidelityRisk> GetCompanyFidelitiesByTemporalId(int temporalId);

        [OperationContract]
        CompanyPolicy CreateEndorsement(CompanyPolicy companyPolicy, List<CompanyFidelityRisk> fidelityRisks);

        /// <summary>
        /// Obtener Riesgo
        /// </summary>
        /// <param name="riskId">Id Riesgo</param>
        /// <returns>Riesgo</returns>
        [OperationContract]
        CompanyFidelityRisk GetCompanyFidelityByRiskId(int riskId);
        #region emision


        #region clauses
        /// <summary>
        /// Insert Cláusulas
        /// </summary>
        /// <param name="riskId"></param>
        /// <param name="clauses"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyClause> SaveClauses(int riskId, List<CompanyClause> clauses);
        #endregion clauses

        /// <summary>
        /// obtener temporal
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyFidelityRisk> GetTemporalById(int id);

        /// <summary>
        ///  Obtener Riesgo
        /// </summary>
        /// <param name="temporalId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyFidelityRisk GetRiskById(int id);

        /// <summary>
        /// Eliminar riesgo
        /// </summary>
        /// <param name="temporalId"></param>
        /// <param name="riskId"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean DeleteRisk(int temporalId, int riskId);

        /// <summary>
        /// Insert riesgo
        /// </summary>
        /// <param name="fidelityRisk"></param>
        /// <param name="temporalId"></param>
        /// <param name="riskId"></param>
        /// <param name="policy"></param>
        /// <param name="beneficiary"></param>
        /// <param name="RiskEndorsementType"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyFidelityRisk SaveRisk(CompanyFidelityRisk fidelityRisk, int temporalId, int? riskId, int? RiskEndorsementType);

        /// <summary>
        /// obtener 
        /// </summary>
        /// <param name="fidelityRisk"></param>
        /// <param name="coverages"></param>
        /// <param name="dynamicProperties"></param>
        /// <param name="temporalId"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyFidelityRisk GetPremium(CompanyFidelityRisk fidelityRisk, List<CompanyCoverage> coverages, List<DynamicConcept> dynamicProperties, int temporalId);

        /// <summary>
        /// Insert riesgo
        /// </summary>
        /// <param name="temporalId"></param>
        /// <param name="riskId"></param>
        /// <param name="coverages"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean SaveCoverages(int temporalId, int riskId, List<CompanyCoverage> coverages);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="temporalId"></param>
        /// <param name="riskId"></param>
        /// <param name="riskCoverageId"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyCoverage ExcludeCoverage(int temporalId, int riskId, int riskCoverageId, string description);

        /// <summary>
        /// Insert beneficiarios
        /// </summary>
        /// <param name="riskId"></param>
        /// <param name="beneficiaries"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyBeneficiary> SaveBeneficiaries(int riskId, List<CompanyBeneficiary> beneficiaries);


        /// <summary>
        ///  obtener cobertura 
        /// </summary>
        [OperationContract]
        List<CompanyCoverage> GetCoveragesByProductIdGroupCoverageId(int temporalId, int productId, int groupCoverageId, int prefixId);

        /// <summary>
        /// obtener cobertura 
        /// </summary>
        /// <param name="coverageId"></param>
        /// <param name="riskId"></param>
        /// <param name="temporalId"></param>
        /// <param name="groupCoverageId"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyCoverage GetCoverageByCoverageId(int coverageId, int riskId, int temporalId, int groupCoverageId);

        /// <summary>
        /// obtener cobertura
        /// </summary>
        /// <param name="tempId"></param>
        /// <param name="riskId"></param>
        /// <param name="groupCoverageId"></param>
        /// <param name="coverage"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyCoverage> GetAllyCoverageByCoverage(int tempId, int riskId, int groupCoverageId, CompanyCoverage coverage);

        /// <summary>
        ///  insert clausula 
        /// </summary>
        /// <param name="riskId"></param>
        /// <param name="clauses"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean SaveClause(int riskId, List<CompanyClause> clauses);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="ruleSetId"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyFidelityRisk RunRulesRiskPreFidelity(int policyId, int? ruleSetId);

        /// <summary>
        /// Update riesgo
        /// </summary>
        /// <param name="temporalId"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicy UpdateRisks(int temporalId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="temporalId"></param>
        /// <param name="individualId"></param>
        /// <param name="documentNumber"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean ConvertProspectToInsured(int temporalId, int individualId, string documentNumber);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="coverage"></param>
        /// <param name="riskId"></param>
        /// <param name="temporalId"></param>
        /// <param name="endorsementType"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyCoverage QuotationCoverage(CompanyCoverage coverage, int riskId, int temporalId, int endorsementType);
        #endregion emision

        /// <summary>
        /// insert Policy
        /// </summary>
        /// <param name="temporalId"></param>
        /// <param name="temporalType"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicyResult CreateCompanyPolicy(int temporalId, int temporalType);

        /// <summary>
        /// Retorna un listado de profesiones
        /// </summary>
        /// <returns>Listado de profesiones</returns>
        [OperationContract]
        List<OccupationDTO> GetOccupations();

        /// <summary>
        /// Obtener riesgos de manejo por el identificador del asegurado
        /// </summary>
        /// <param name="insuredId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyFidelityRisk> GetCompanyFidelityRisksByInsuredId(int insuredId);

        /// <summary>
        /// Obtener riesgo de manejo por su identificador
        /// </summary>
        /// <param name="riskId"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyFidelityRisk GetCompanyFidelityRiskByRiskId(int riskId);

        /// <summary>
        /// Obtener riesgos de manejo por identificador del endoso y tipo de módulo
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyFidelityRisk> GetCompanyFidelitiesByEndorsementIdModuleType(int endorsementId, ModuleType moduleType);
    }
}