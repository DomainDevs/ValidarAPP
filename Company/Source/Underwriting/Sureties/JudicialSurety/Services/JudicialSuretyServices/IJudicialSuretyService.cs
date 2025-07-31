using Sistran.Company.Application.Sureties.JudicialSuretyServices.Models;
using Sistran.Company.Application.Sureties.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Sureties.JudicialSuretyServices;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.Sureties.JudicialSuretyServices
{
    [ServiceContract]
    public interface IJudicialSuretyService : IJudicialSuretyCore
    {

        /// <summary>
        /// Obtener Poliza de Caución Judicial
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <returns>CompanyJudgement</returns>
        [OperationContract]
        List<CompanyJudgement> GetCompanyJudicialSuretyByPolicyId(int policyId);

        /// <summary>
        /// Insertar en tablas temporales desde el JSON
        /// </summary>
        /// <param name="companyJudgement"></param>
        /// <returns>CompanyJudgement</returns>
        [OperationContract]
        CompanyJudgement CreateJudgementTemporal(CompanyJudgement companyJudgement, bool isMassive);

        [OperationContract]
        CompanyJudgement QuotateCompanyJudgement(CompanyJudgement companyJudgement, bool runRulesPre, bool runRulesPost);

        [OperationContract]
        List<CompanyJudgement> QuotateJudgements(CompanyPolicy companyPolicy, List<CompanyJudgement> companyJudgements, bool runRulesPre, bool runRulesPost);

        [OperationContract]
        CompanyCoverage QuotateCoverage(CompanyJudgement companyJudgement, CompanyCoverage coverage, bool runRulesPre, bool runRulesPost);

        [OperationContract]
        Models.CompanyJudgement RunRulesRisk(Models.CompanyJudgement companyJudgement, int ruleId);

        /// <summary>
        /// Obtener Riesgos
        /// </summary>
        /// <param name="temporalId">Id Temporal</param>
        /// <returns>Riesgos</returns>
        [OperationContract]
        List<CompanyJudgement> GetCompanyJudgementsByTemporalId(int temporalId);

        /// <summary>
        /// Obtener Riesgos
        /// </summary>
        /// <param name="endorsementId">Id Endoso</param>
        /// <returns>Riesgos</returns>
        [OperationContract]
        List<CompanyJudgement> GetCompanyJudgementsByEndorsementId(int endorsementId);

        [OperationContract]
        CompanyJudgement GetCompanyJudgementByRiskId(int riskId);

        /// <summary>
        /// Obtener Poliza
        /// </summary>
        /// <param name="liabilityRiskss">riesgos </param>
        /// <returns>Riesgos</returns>
        [OperationContract]
        CompanyPolicy CreateEndorsement(CompanyPolicy companyPolicy, List<CompanyJudgement> liabilityRisks);

        [OperationContract]
        List<CompanyJudgement> GetRisksByTemporalId(int temporalId);

        [OperationContract]
        List<CompanyArticle> GetCompanyArticles();

        [OperationContract]
        CompanyJudgement GetRiskById(EndorsementType endorsementType, int temporalId, int id);

        ///// <summary>
        ///// FUncion para obtener el listado de coberturas por Id
        ///// </summary>
        ///// <param name="policyId"></param>
        ///// <param name="groupCoverageId"></param>
        ///// <returns></returns>
        [OperationContract]
        List<CompanyCoverage> GetCoveragesByProductIdGroupCoverageId(int policyId, int groupCoverageId);

        [OperationContract]
        CompanyCoverage GetCoverageByCoverageId(int coverageId, int groupCoverageId, int policyId);

        /// <summary>
        /// Funcion para guardar el riesgo del Ramo Judicial
        /// </summary>
        /// <param name="CompanyJudgement"></param>
        /// <param name="riskModel"></param>        
        /// <returns></returns>
        [OperationContract]
        CompanyJudgement SaveCompanyRisk(CompanyJudgement companyJudgement, int temporalId);

        [OperationContract]
        List<CiaRiskSuretyGuarantee> SaveGuarantees(int riskId, List<CiaRiskSuretyGuarantee> guarantees);

        [OperationContract]
        bool DeleteRisk(int policyId, int id);

        [OperationContract]
        CompanyJudgement RunRules(CompanyJudgement companyJudgement, int? ruleSetId);

        /// <summary>
        /// Metodo para actualizacion de poliza, se utiliza si el sistema detecta cambios en la pantalla principal de poliza para que
        /// actualice los riesgos y coberturas con los nuevos parametros. El metodo debe estar en todos los ramos haciendo
        /// la adecuacion al modelo correspondiente
        /// </summary>
        /// <param name="tempId">temporal de la poliza</param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicy UpdateRisks(int temporalId);

        [OperationContract]
        void ConvertProspectToInsured(int temporalId, int individualId, string documentNumber);

        [OperationContract]
        List<CompanyBeneficiary> SaveBeneficiaries(int riskId, List<CompanyBeneficiary> beneficiaries);

        [OperationContract]
        CompanyText SaveTexts(int riskId, CompanyText companyText);

        [OperationContract]
        List<CompanyClause> SaveClauses(int riskId, List<CompanyClause> clauses);

        [OperationContract]
        void SaveCoverages(int policyId, int riskId, List<CompanyCoverage> coverages);

        [OperationContract]
        CompanyCoverage ExcludeCoverage(int temporalId, int riskId, int riskCoverageId, string description);

        [OperationContract]
        CompanyJudgement GetPremium(int policyId, CompanyJudgement riskCompanyJudgement);

        [OperationContract]
        CompanyJudgement SaveAdditionalData(CompanyJudgement companyJudgement);

        [OperationContract]
        CompanyPolicyResult CreateCompanyPolicy(int temporalId, int temporalType, CompanyModification companyModification, bool clearPolicies = false);

        [OperationContract]
        List<CompanyJudgement> GetCompanyJudicialSuretiesByEndorsementIdModuleType(int endorsementId, ModuleType moduleType);

        [OperationContract]
        CompanyJudgement GetCompanyJudicialSuretyByRiskIdModuleType(int riskId, ModuleType moduleType);

        [OperationContract]
        CompanySummary ConvertProspectToInsuredRisk(CompanyPolicy companyPolicy, int individualId);

        [OperationContract]
        CompanyJudgement GetCompanyPremium(int policyId, CompanyJudgement companyJudgement);
        [OperationContract]
        List<CompanyProductArticle> GetCompanyProductArticles();
        [OperationContract]
        List<CompanyProductArticle> GetCompanyProductArticlesByDescription(string smallDescription);
        [OperationContract]
        List<CompanyProductArticle> DeleteCompanyProductArticle(List<CompanyProductArticle> productArticleDelete);
        [OperationContract]
        List<CompanyProductArticle> UpdateCompanyProductArticle(List<CompanyProductArticle> productArticleUpdate);
        [OperationContract]
        List<CompanyProductArticle> InsertCompanyProductArticle(List<CompanyProductArticle> productArticleInsert);
        [OperationContract]
        List<CompanyArticleLine> getCompanyArticleLines();
        [OperationContract]
        List<CompanyArticleLine> GetCompanyArticleLineByDescription(string smallDescription);
        [OperationContract]
        List<CompanyArticleLine> CompanyArticleLineDelete(List<CompanyArticleLine> articleLineDelete);
        [OperationContract]
        List<CompanyArticleLine> CompanyArticleLineUpdate(List<CompanyArticleLine> articleLineUpdate);
        [OperationContract]
        List<CompanyArticleLine> CompanyArticleLineInsert(List<CompanyArticleLine> articleLineInsert);
        [OperationContract]
        List<CompanyCourt> GetCompanyCourtsTypeByDescription(string smallDescription);
        [OperationContract]
        List<CompanyCourt> GetCompanyCourtsType();
        [OperationContract]
        List<CompanyCourt> CompanyCourtTypeDelete(List<CompanyCourt> courtTypeDelete);
        [OperationContract]
        List<CompanyCourt> CompanyCourtTypeUpdate(List<CompanyCourt> courtTypeUpdate);
        [OperationContract]
        List<CompanyCourt> CompanyCourtTypeInsert(List<CompanyCourt> courtTypeInsert);
        [OperationContract]
        bool GetInsuredGuaranteeRelationPolicy(int guaranteeId);
    }
}
