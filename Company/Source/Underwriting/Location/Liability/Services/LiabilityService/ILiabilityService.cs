using System;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Company.Application.Location.LiabilityServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Location.LiabilityServices;
using Sistran.Core.Application.RulesScriptsServices.Models;

namespace Sistran.Company.Application.Location.LiabilityServices
{
    [ServiceContract]
    public interface ILiabilityService : ILiabilityServiceCore
    {
        /// <summary>
        /// Ejecucion de reglas Pre de riesgo
        /// </summary>
        /// <param name="liabilityRisk">Riesgo</param>
        /// <param name="ruleSetId">Id regla</param>
        /// <returns></returns>
        [OperationContract]
        CompanyLiabilityRisk RunRulesRisk(CompanyLiabilityRisk liabilityRisk, int ruleSetId);
        /// <summary>
        /// Runs the rules Coverage pre.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        CompanyCoverage RunRulesCompanyCoverage(CompanyLiabilityRisk liabilityRisk, CompanyCoverage coverage, int ruleSetId);


        /// <summary>
        /// Cotiza la poliza especifica lo los riesgos que contenga
        /// </summary>
        /// <param name="liabilityPolicy">Poliza de rc a tarifar.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyLiabilityRisk QuotateLiability(CompanyLiabilityRisk companyLiabilityRisk, bool runRulesPre, bool runRulesPost);



        /// <summary>
        /// Cotiza la cobertura especificada
        /// </summary>
        /// <param name="coverage">Cobertura a cotizar</param>
        /// <returns></returns>
        [OperationContract]
        CompanyCoverage QuotationCompanyCoverage(CompanyLiabilityRisk liabilityRisk, CompanyCoverage coverage, bool runRulesPre, bool runRulesPost);



        /// <summary>
        /// Cotiza el riesgo especificado
        /// </summary>
        /// <param name="liabilityRisk">Riesgo a cotizar</param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyLiabilityRisk> QuotateLiabilities(CompanyPolicy companyPolicy, List<CompanyLiabilityRisk> companyLiabilityRisks, bool runRulesPre, bool runRulesPost);

        /// <summary>
        /// Insertar en tablas temporales desde el JSON
        /// </summary>
        /// <param name="liabilityRisk">Modelo liabilityRisk</param>
        [OperationContract]
        CompanyLiabilityRisk CreateLiabilityTemporal(CompanyLiabilityRisk liabilityRisk, bool isMassive);

        /// <summary>
        /// Obtener Poliza de responsabilidad civil
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <param name="temporalId">Id temporal</param>
        /// <returns>LiabilityPolicy</returns>
        [OperationContract]
        List<CompanyLiabilityRisk> GetCompanyLiebilitiesByPolicyId(int policyId);

        [OperationContract]
        List<CompanyAssuranceMode> GetAssuranceMode();

        [OperationContract]
        List<CompanyLiabilityRisk> GetCompanyLiabilitiesByEndorsementId(int endorsementId);

        //[OperationContract]
        //List<CompanyLiabilityRisk> GetCompanyLiabilitiesRiskByEndorsementId(int endorsementId);

        [OperationContract]
        /// <summary>
        /// Obtener Riesgos
        /// </summary>
        /// <param name="temporalId">Id Temporal</param>
        /// <returns>Riesgos</returns>
        List<CompanyLiabilityRisk> GetCompanyLiabilitiesByTemporalId(int temporalId);

        [OperationContract]
        CompanyPolicy CreateEndorsement(CompanyPolicy companyPolicy, List<CompanyLiabilityRisk> liabilityRisks);

        /// <summary>
        /// Obtener Riesgo
        /// </summary>
        /// <param name="riskId">Id Riesgo</param>
        /// <returns>Riesgo</returns>
        [OperationContract]
        CompanyLiabilityRisk GetCompanyLiabilityByRiskId(int riskId);
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
        List<CompanyLiabilityRisk> GetTemporalById(int id);

        /// <summary>
        ///  Obtener Riesgo
        /// </summary>
        /// <param name="temporalId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyLiabilityRisk GetRiskById(int id);

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
        /// <param name="liabilityRisk"></param>
        /// <param name="temporalId"></param>
        /// <param name="riskId"></param>
        /// <param name="policy"></param>
        /// <param name="beneficiary"></param>
        /// <param name="RiskEndorsementType"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyLiabilityRisk SaveRisk(CompanyLiabilityRisk liabilityRisk, int temporalId, int? riskId, int? RiskEndorsementType);

        /// <summary>
        /// obtener 
        /// </summary>
        /// <param name="liabilityRisk"></param>
        /// <param name="coverages"></param>
        /// <param name="dynamicProperties"></param>
        /// <param name="temporalId"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyLiabilityRisk GetPremium(CompanyLiabilityRisk liabilityRisk, List<CompanyCoverage> coverages, List<DynamicConcept> dynamicProperties, int temporalId);

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
        List<CompanyCoverage> GetCoverageByCoverageId(int coverageId, int riskId, int temporalId, int groupCoverageId);

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
        CompanyLiabilityRisk RunRulesRiskPreLiability(int policyId, int? ruleSetId);

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
        CompanyCoverage QuotationCoverage(CompanyCoverage coverage, int riskId, int temporalId, int endorsementType, bool runRulesPost);
        #endregion emision

        /// <summary>
        /// insert Policy
        /// </summary>
        /// <param name="temporalId"></param>
        /// <param name="temporalType"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicyResult CreateCompanyPolicy(int temporalId, int temporalType, bool clearPolicies, CompanyModification companyModification);
        /// <summary>
        /// Get RiskSubActivities
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyRiskSubActivity> GetSubActivities(int riskActivity);


        /// <summary>
        /// Ejecuta reglas pre a nivel de la cobertura
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyCoverage> RunRulesCoveragesPreLiability(int temporalId, int riskId, List<CompanyCoverage> coverages);

        /// <summary>
        /// Metodo utilizado para la ejecución de las reglas POST de coverturas
        /// </summary>
        /// <param name="temporalId">temporal de la poliza</param>
        /// <param name="riskId">Riesgo que se esta validando</param>
        /// <param name="coverages">Coberturas asociadas al riesgo</param>
        [OperationContract]
        List<CompanyCoverage> RunRulesPostCoverages(int temporalId, int riskId, List<CompanyCoverage> coverages);

        /// <summary>
        /// Obtener las coberturas adicionales 
        /// </summary>
        /// <param name="tempId">Temporal</param>
        /// <param name="riskId">Riesgo</param>
        /// <param name="groupCoverageId">Grupo de Cobertura</param>
        /// <param name="companyCoverage">Cobertura Principal</param>
        /// <returns>Lista de coberturas</returns>
        [OperationContract]
        List<CompanyCoverage> GetAddCoveragesByCoverage(int tempId, int riskId, int groupCoverageId, CompanyCoverage companyCoverage);

        /// <summary>
        /// modifica los prospectos por asegurados de la poliza 
        /// </summary>
        /// <param name="companyPolicy">companyPolicy</param>
        /// <param name="individualId">individualId</param>
        /// <returns>CompanySummary</returns>
        [OperationContract]
        CompanySummary ConvertProspectToInsuredRisk(CompanyPolicy companyPolicy, int individualId);

        [OperationContract]
        CompanyLiabilityRisk GetCompanyPremium(int policyId, CompanyLiabilityRisk companyLiabilityRisk);
    }
}