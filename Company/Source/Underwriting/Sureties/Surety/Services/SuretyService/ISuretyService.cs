using Sistran.Company.Application.Sureties.SuretyServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Sureties.SuretyServices;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Sureties.SuretyServices
{
    /// <summary>
    /// Implementaciones Cumplimiento
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.Sureties.SuretyServices.ISuretyServiceCore" />
    [ServiceContract]
    public interface ISuretyService : ISuretyServiceCore
    {
        /// <summary>
        /// Cotiza la poliza especifica lo los riesgos que contenga
        /// </summary>
        /// <param name="suretyPolicy">Poliza a cotizar</param>
        /// <param name="companyPolicy"></param>
        /// <param name="contracts"></param>
        /// <param name="runRulesPre"></param>
        /// <param name="runRulesPost"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyContract> QuotateSureties(CompanyPolicy companyPolicy, List<CompanyContract> contracts, bool runRulesPre, bool runRulesPost);

        /// <summary>
        /// Cotiza el riesgo especifico
        /// </summary>
        /// <param name="companyPolicy"></param>
        /// <param name="contract"></param>
        /// <param name="runRulesPre"></param>
        /// <param name="runRulesPost"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyContract QuotateSurety(CompanyContract contract, bool runRulesPre, bool runRulesPost);

        /// <summary>
        /// Ejecucion de reglas Pre de riesgo
        /// </summary>
        /// <param name="companyPolicy"></param>
        /// <param name="contract">riesgo</param>
        /// <param name="ruleSetId">Id regla</param>
        /// <returns></returns>
        [OperationContract]
        CompanyContract RunRulesRisk(CompanyContract contract, int ruleSetId);

        /// <summary>
        /// Cotiza la cobertura especificada
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="coverage">Cobertura a cotizar</param>
        /// <param name="companyPolicy"></param>
        /// <param name="runRulesPre"></param>
        /// <param name="runRulesPost"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyCoverage QuotateCoverage(CompanyContract contract, CompanyCoverage coverage, bool runRulesPre, bool runRulesPost);

        /// <summary>
        /// Ejecutar Reglas de Cobertura
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="coverage">Cobertura</param>
        /// <param name="ruleSetId">Id Regla</param>
        /// <param name="companyPolicy"></param>
        /// <returns>Cobertura</returns>
        [OperationContract]
        CompanyCoverage RunRulesCoverage(CompanyContract contract, CompanyCoverage coverage, int ruleSetId);

        /// <summary>
        /// Insertar en tablas temporales desde el JSON
        /// </summary>
        /// <param name="contract">Modelo Contract</param>
        [OperationContract]
        CompanyContract CreateSuretyTemporal(CompanyContract contract, bool isMassive);

        /// <summary>
        /// Obtener Poliza de cumplimiento
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <returns>SuretyPolicy</returns>
        [OperationContract]
        List<CompanyContract> GetCompanySuretiesByPolicyId(int policyId);

        /// <summary>
        /// </summary>
        /// <returns>Lista de CompanySurety</returns>
        [OperationContract]
        List<CompanyContract> GetCompanySuretyByEndorsementId(int endorsementId, int riskId = 0);

        /// <summary>
        /// Obtener Riesgos
        /// </summary>
        /// <param name="temporalId">Id Temporal</param>
        /// <returns>Riesgos</returns>
        [OperationContract]
        List<CompanyContract> GetCompanySuretiesByTemporalId(int temporalId);

        /// <summary>
        /// Polizas asociadas a individual
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyContract> GetSuretiesByIndividualId(int individualId);

        /// <summary>
        /// Creates the endorsement.
        /// </summary>
        /// <param name="companyPolicy">The company policy.</param>
        /// <param name="companyContracts">The company contracts.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicy CreateEndorsement(CompanyPolicy companyPolicy, List<CompanyContract> companyContracts);

        /// <summary>
        /// Gets the company surety by risk identifier.
        /// </summary>
        /// <param name="riskId">The risk identifier.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyContract GetCompanySuretyByRiskId(int riskId);

        #region Emision

        /// <summary>
        /// Gets the risk surety by identifier.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyContract GetRiskSuretyById(int temporalId, int id);

        /// <summary>
        /// Gets the risk sureties by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyContract> GetRiskSuretiesById(int id);

        /// <summary>
        /// Validates the available amount by temporal identifier.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <returns></returns>
        [OperationContract]
        List<string> ValidateRiskByTemporalId(int temporalId);

        /// <summary>
        /// Saves the company risk.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="surety">The surety.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyContract SaveCompanyRisk(int temporalId, CompanyContract surety);

        /// <summary>
        /// Deletes the risk.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="riskId">The risk identifier.</param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteRisk(int temporalId, int riskId);

        /// <summary>
        /// Valida si ua existe el Riesgo temporalId=0 si se envian los Contratos
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="riskId">The risk identifier.</param>
        /// <param name="ContractName">Name of the contract.</param>
        /// <returns></returns>
        [OperationContract]
        bool ExistsRisk(int temporalId, int? riskId, string ContractName, List<CompanyContract> contracts = null);

        /// <summary>
        /// Converts the prospect to insured.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="individualId">The individual identifier.</param>
        /// <param name="documentNumber">The document number.</param>
        /// <returns></returns>
        [OperationContract]
        bool ConvertProspectToInsured(int temporalId, int individualId, string documentNumber);

        /// <summary>
        /// Saves the texts.
        /// </summary>
        /// <param name="riskId">The risk identifier.</param>
        /// <param name="companyText">The company text.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyText SaveTexts(int riskId, CompanyText companyText);

        /// <summary>
        /// Saves the contract object.
        /// </summary>
        /// <param name="riskId">The risk identifier.</param>
        /// <param name="companyText">The company text.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyText SaveContractObject(int riskId, CompanyText companyText);

        /// <summary>
        /// Saves the coverages.
        /// </summary>
        /// <param name="tempId">The temporary identifier.</param>
        /// <param name="riskId">The risk identifier.</param>
        /// <param name="coverages">The coverages.</param>
        /// <returns></returns>
        [OperationContract]
        bool SaveCoverages(int tempId, int riskId, List<CompanyCoverage> coverages);

        /// <summary>
        /// Quotations the surety coverage.
        /// </summary>
        /// <param name="tempId">The temporary identifier.</param>
        /// <param name="riskId">The risk identifier.</param>
        /// <param name="coverage">The coverage.</param>
        /// <param name="runRules">if set to <c>true</c> [run rules].</param>
        /// <returns></returns>
        [OperationContract]
        CompanyCoverage QuotationSuretyCoverage(int tempId, int riskId, CompanyCoverage coverage, bool runRules, List<CompanyCoverage> listCompanyCoverage);

        /// <summary>
        /// Gets the coverages by product identifier group coverage identifier.
        /// </summary>
        /// <param name="TemporalId">The temporal identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="GroupCoverageId">The group coverage identifier.</param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyCoverage> GetCoveragesByProductIdGroupCoverageId(int TemporalId, CompanyContract ciaContract);
        #endregion Emision
        #region Contragarantias
        /// <summary>
        /// Gets the insured guarantee by individual identifier.
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        [OperationContract]
        List<IssuanceGuarantee> GetInsuredGuaranteeByIndividualId(int individualId);
        #endregion contragarantias
        /// <summary>
        /// Saves the clauses.
        /// </summary>
        /// <param name="riskId">The risk identifier.</param>
        /// <param name="clauses">The clauses.</param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyClause> SaveClauses(int riskId, List<CompanyClause> clauses);

        /// <summary>
        /// Runs the rules risk surety.
        /// </summary>
        /// <param name="tempId">The temporary identifier.</param>
        /// <param name="ruleSetId">The rule set identifier.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyContract RunRulesRiskSurety(int tempId, int? ruleSetId);

        /// <summary>
        /// Excludes the coverage.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="riskId">The risk identifier.</param>
        /// <param name="riskCoverageId">The risk coverage identifier.</param>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyCoverage ExcludeCoverage(int temporalId, int riskId, int riskCoverageId, string description);

        /// <summary>
        /// Updates the risks.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicy UpdateRisks(int temporalId);

        /// <summary>
        /// Gets the coverage by coverage identifier.
        /// </summary>
        /// <param name="coverageId">The coverage identifier.</param>
        /// <param name="riskId">The risk identifier.</param>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="groupCoverageId">The group coverage identifier.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyCoverage GetCoverageByCoverageId(int coverageId, int riskId, int temporalId, int groupCoverageId);

        /// <summary>
        /// Gets the premium.
        /// </summary>
        /// <param name="ciaContract">TheCompany contract.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyContract GetPremium(CompanyContract ciaContract);

        /// <summary>
        /// Creates the company policy.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="temporalType">Type of the temporal.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicyResult CreateCompanyPolicy(int temporalId, int temporalType, bool clearPolicies, CompanyModification companyModification);

        [OperationContract]
        bool IsConsortiumindividualId(int individualId);

        [OperationContract]
        decimal GetAvailableCumulus(int individualId, int currencyCode, int prefixCode, System.DateTime issueDate);

        [OperationContract]
        bool GetInsuredGuaranteeRelationPolicy(int guaranteeId);

        [OperationContract]
        List<CompanyCoverage> QuotationSuretyCoverages(int tempId, int riskId, CompanyCoverage coverage, bool runRules, List<CompanyCoverage> listCompanyCoverage, int policyId);

        [OperationContract]
        CompanyContract CompanyRiskSuretyQuotation(int RiskId);
        [OperationContract]
        CompanyContract CompanySaveCompanySuretyTemporal(CompanyContract companyVehicle);

        [OperationContract]
        List<CompanyContract> GetCompanySuretiesByEndorsementIdModuleType(int endorsementId, ModuleType moduleType);

        [OperationContract]
        List<CompanyContract> GetCompanyRisksSuretyByInsuredId(int insuredId);
        [OperationContract]
        List<CompanyContract> GetCompanyRisksBySurety(string description);

        [OperationContract]
        List<CompanyContract> GetCompanyRisksSuretyBySuretyId(int suretyId);

        [OperationContract]
        CompanySummary ConvertProspectToInsuredRisk(CompanyPolicy companyPolicy, int individualId);

        [OperationContract]
        CompanyContract GetCompanySuretyByRiskIdModuleType(int riskId, ModuleType moduleType);

        [OperationContract]
        CompanyContract GetCompanyPremium(int policyId, CompanyContract companyContract, int temporalType);
    }
}