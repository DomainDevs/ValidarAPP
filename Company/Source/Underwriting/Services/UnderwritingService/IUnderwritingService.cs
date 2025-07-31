using Sistran.Company.Application.ProductServices.Models;
using Sistran.Company.Application.UnderwritingServices.DTOs;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.ModelServices.Models.Param;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.TaxServices.DTOs;
using Sistran.Core.Application.UnderwritingServices;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using MODSM = Sistran.Core.Application.ModelServices.Models.Param;


namespace Sistran.Company.Application.UnderwritingServices
{
    /// <summary>
    /// Interfaz Emision
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.UnderwritingServices.IUnderwritingServiceCore" />
    [ServiceContract]
    public interface IUnderwritingService : IUnderwritingServiceCore
    {
        /// <summary>
        /// Insertar en tablas temporales desde el JSON
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        [OperationContract]
        CompanyPolicy CreatePolicyTemporal(CompanyPolicy policy, bool isMasive);

        /// <summary>
        /// Eliminar riesgo
        /// </summary>
        /// <param name="operationId">Id operacion</param>
        /// <returns>Resultado</returns>
        [OperationContract]
        bool DeleteRisk(int operationId);

        /// <summary>
        /// Eliminar riesgo
        /// </summary>
        /// <param name="operationId">Id operacion</param>
        /// <returns>Resultado</returns>
        [OperationContract]
        bool DeleteCompanyRisksByRiskId(int riskId, bool isMasive);

        /// <summary>
        /// Obtener Póliza
        /// </summary>
        /// <param name="temporalId">Id Temporal</param>
        /// <returns>Póliza</returns>
        [OperationContract]
        CompanyPolicy GetCompanyPolicyByTemporalId(int temporalId, bool isMasive);

        /// <summary>
        /// Gets the company risks by temporal identifier.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="isMasive">if set to <c>true</c> [is masive].</param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyRisk> GetCompanyRisksByTemporalId(int temporalId, bool isMasive);


        /// <summary>
        /// Gets the cia risk by temporal identifier.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="isMasive">if set to <c>true</c> [is masive].</param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyRisk> GetCiaRiskByTemporalId(int temporalId, bool isMasive);

        [OperationContract]
        List<CompanyClause> GetClausesByCoverageId(int CoverageId);


        /// <summary>
        /// Gets the cover detail types by coverage identifier.
        /// </summary>
        /// <param name="CoverageId">The coverage identifier.</param>
        /// <returns></returns>
        [OperationContract]
        List<CoverDetailType> GetCoverDetailTypesByCoverageId(int CoverageId);

        /// <summary>
        /// Genera archivo excel objetos del seguro
        /// </summary>
        /// <param name="CompanyInsuredObject"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [OperationContract]
        string GenerateFileToCompanyInsuredObject(List<CompanyInsuredObject> CompanyInsuredObject, string fileName);



        [OperationContract]
        CompanyPolicy RunRulesCompanyPolicy(CompanyPolicy companyPolicy, int ruleId);

        /// <summary>
        /// Recupera el valor de la prima mínima, si lo tiene, de los conceptos de un modelo
        /// </summary>
        /// <param name="modelDynamicConcepts">conceptos dinámicos del modelo</param>
        /// <returns></returns>
        [OperationContract]
        decimal GetMinimumPremiumAmountByModelDynamicConcepts(List<DynamicConcept> modelDynamicConcepts);

        /// <summary>
        /// Recupera el valor de si se debe prorratear la prima mínima, si lo tiene, de los conceptos de un modelo
        /// </summary>
        /// <param name="modelDynamicConcepts">conceptos dinámicos del modelo</param>
        /// <returns></returns>
        [OperationContract]
        bool GetProrateMinimumPremiumByModelDynamicConcepts(List<DynamicConcept> modelDynamicConcepts);

        /// <summary>
        /// </summary>
        /// <param name="CompanyInsuredObjectsIds"></param>
        /// <param name="groupCoverageId"></param>
        /// <param name="productId"></param>
        /// <param name="filterSelected"></param>
        /// <returns>Modelos de coberturas company</returns>
        [OperationContract]
        List<CompanyCoverage> GetCompanyCoveragesByInsuredObjectIdsGroupCoverageIdProductId(List<int> insuredObjectsIds, int groupCoverageId, int productId, bool filterSelected);

        /// <summary>
        /// Obtiene una lista de coberturas company por número de poliza, id de endoso y id de riesgo (Compatibilidad R1)
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <param name="riskId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyCoverage> GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(int policyId, int endorsementId, int riskId);

        /// <summary>
        /// Obtener Póliza Por Identificador
        /// </summary>
        /// <param name="endorsementId">Id Endoso</param>
        /// <returns>Póliza</returns>
        [OperationContract]
        CompanyPolicy GetCompanyPolicyByEndorsementId(int endorsementId);

        /// <summary>
        /// Gets the policy by endorsement document number.
        /// </summary>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <param name="documentNumber">The document number.</param>
        /// <returns></returns>
        [OperationContract]
        String GetPolicyByEndorsementDocumentNumber(int endorsementId, decimal documentNumber);

        [OperationContract]
        List<CompanyCoverage> GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(int productId, int groupCoverageId, int prefixId);

        [OperationContract]
        List<CompanyCoverage> GetDeductiblesByCompanyCoverages(List<CompanyCoverage> companyCoverage);

        [OperationContract]
        void CalculateCompanyPremiumDeductible(CompanyCoverage companyCoverage);

        [OperationContract]
        CompanyCoverage GetCompanyCoverageByCoverageIdProductIdGroupCoverageId(int coverageId, int productId, int groupCoverageId);

        [OperationContract]
        List<CompanyCoverage> GetCompanyCoveragesAccessoriesByProductIdGroupCoverageIdPrefixId(int productId, int groupCoverageId, int prefixId);

        [OperationContract]
        CompanyCoverage QuotateCompanyCoverage(CompanyCoverage companyCoverage, int policyId, int riskId, int decimalQuantity, int? CoveredRiskType = 0, int? prefixId = 0);

        [OperationContract]
        List<CompanyCoverage> GetCompanyCoveragesByInsuredObjectIdGroupCoverageIdProductId(int insuredObjectId, int groupCoverageId, int productId);

        [OperationContract]
        List<CompanyCoverage> GetCompanyCoverageByCoverageIdsProductIdGroupCoverageId(List<int> coverageIds, int productId, int groupCoverageId);

        [OperationContract]
        CompanyCoverage GetCompanyCoverageByRiskCoverageId(int riskCoverageId);

        [OperationContract]
        List<CompanyInsuredObject> GetCompanyInsuredObjects();

        [OperationContract]
        List<CompanyInsuredObject> GetCompanyInsuredObjectsByProductIdGroupCoverageId(int productId, int groupCoverageId, int prefixId);

        [OperationContract]
        List<CompanyCoverage> GetAllyCompanyCoveragesByCoverageIdProductIdGroupCoverageId(int coverageId, int productId, int groupCoverageId);

        [OperationContract]
        List<CompanyInsuredObject> GetCompanyInsuredObjectByPrefixIdList(int prefixId);

        [OperationContract]
        List<CompanyCoverage> GetCompanyCoveragesByTechnicalPlanId(int technicalPlanId);

        [OperationContract]
        List<CompanyCoverage> GetCompanyCoveragesPrincipalByInsuredObjectId(int insuredObjectId);

        [OperationContract]
        CompanyCoverage GetCompanyCoverageProductByCoverageId(int coverageId);

        [OperationContract]
        List<CompanyInsuredObject> GetCompanyInsuredObjectsByRiskId(int riskId);

        [OperationContract]
        List<CompanyCoverage> GetCompanyCoveragesByInsuredObjectId(int insuredObjectId);

        [OperationContract]
        List<CompanyCoverage> CalculateMinimumPremiumRatePerCoverage(List<CompanyCoverage> companyCoverages, decimal minimumPremiumAmount, bool prorate, bool assistance);

        [OperationContract]
        List<CompanyInsuredObject> GetCompanyInsuredObjectsByLineBusinessId(int lineBusinessId);

        [OperationContract]
        List<CompanyInsuredObject> GetCompanyInsuredObjectsByDescription(string description);

        [OperationContract]
        List<CompanyInsuredObject> CreateCompanyInsuredObjects(List<CompanyInsuredObject> companyInsuredObjects);

        #region CompanyPolicy Extendidos


        [OperationContract]
        CompanyPolicy CreateCompanyPolicy(CompanyPolicy companyPolicy);

        [OperationContract]
        CompanyPolicy GetCurrentCompanyPolicyByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber);

        [OperationContract]
        CompanyRisk RunRulesCompanyRisk(CompanyPolicy policy, CompanyRisk risk, int rulsetId);

        [OperationContract]
        List<CompanyPolicy> GetCompanyTemporalPoliciesByCompanyPolicy(CompanyPolicy policy);

        [OperationContract]
        List<CompanyPolicy> GetCompanyPoliciesByCompanyPolicy(CompanyPolicy policy);

        [OperationContract]
        CompanySummary CalculateSummaryByCompanyPolicy(CompanyPolicy policy, List<CompanyRisk> risks);

        [OperationContract]
        List<IssuanceAgency> CalculateCommissionsByCompanyPolicy(CompanyPolicy policy, List<CompanyRisk> risks);

        //[OperationContract]
        //List<Quota> CalculateQuotasByCompanyPolicy(CompanyPolicy policy);

        /// <summary>
        /// Calcular Componentes
        /// </summary>
        /// <param name="companyPolicy">Póliza</param>
        /// <param name="risks">Riesgos</param>
        /// <returns>Componentes</returns>
        [OperationContract]
        List<CompanyPayerComponent> CalculatePayerComponentsByCompanyPolicy(CompanyPolicy policy, List<CompanyRisk> risks);
        #endregion

        [OperationContract]
        CompanyInsuredObject GetCompanyInsuredObjectByProductIdGroupCoverageIdInsuredObjectId(int productId, int groupCoverageId, int insuredObjectId);

        [OperationContract]
        List<String> GetRiskByEndorsementDocumentNumber(int endorsementId);

        [OperationContract]
        List<PoliciesAut> ValidateAuthorizationPolicies(CompanyPolicy policy);

        [OperationContract]
        bool GetAssistanceInPremiumMin(List<DynamicConcept> modelDynamicProperties);

        [OperationContract]
        CompanyPolicy GetTemporalPolicyByPolicyIdEndorsementId(int policyId, int endorsementId);

        [OperationContract]
        CompanyLimitRc GetCompanyLimitRcById(int id);

        [OperationContract]
        CompanyRatingZone GetCompanyRatingZoneByRatingZoneId(int ratingZoneId);


        /// <summary>
        /// Gets the company beneficiary types.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<CompanyBeneficiaryType> GetCompanyBeneficiaryTypes();
        #region Comisiones
        /// <summary>
        /// Gets the company commissions by temporary identifier.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="agency">The agency.</param>
        /// <param name="agencies">The agencies.</param>
        /// <returns></returns>
        [OperationContract]
        List<IssuanceAgency> GetCompanyCommissionsByTempId(int temporalId, IssuanceAgency agency, List<IssuanceAgency> agencies);

        /// <summary>
        /// Saves the company commissions.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="agencies">The agencies.</param>
        /// <returns></returns>
        [OperationContract]
        List<IssuanceAgency> SaveCompanyCommissions(int temporalId, List<IssuanceAgency> agencies);
        #endregion Comisiones
        #region Beneficiarios
        /// <summary>
        /// Saves the company beneficiary.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="beneficiaries">The beneficiaries.</param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyBeneficiary> SaveCompanyBeneficiary(int temporalId, List<CompanyBeneficiary> beneficiaries);

        /// <summary>
        /// Gets the beneficiary by prefix Id.
        /// </summary>
        /// <param name="prefixId"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyBeneficiary GetBeneficiaryByPrefixId(int prefixId);
        #endregion
        #region poliza
        /// <summary>
        /// Gets the type of the company temporal by identifier temporal.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="temporalType">Type of the temporal.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicy GetCompanyTemporalByIdTemporalType(int id, TemporalType temporalType);

        /// <summary>
        /// Saves the company temporal.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <param name="dynamicProperties">The dynamic properties.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicy SaveCompanyTemporal(CompanyPolicy policy, bool isMasive);
        #endregion

        #region pagadores componentes

        /// <summary>
        /// Calculates the policy amounts.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <param name="risks">The risks.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicy UpdatePolicyComponents(int policyId);
        #endregion pagadores

        #region personas

        #region prospecto
        /// <summary>
        /// Converts the prospect to insured.
        /// </summary>
        /// <param name="risk">The risk.</param>
        /// <param name="individualId">The individual identifier.</param>
        /// <param name="documentNumber">The document number.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyRisk ConvertProspectToInsured(CompanyRisk risk, int individualId, string documentNumber);

        /// <summary>
        /// Converts the prospect to holder.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="individualId">The individual identifier.</param>
        /// <param name="documentNumber">The document number.</param>
        /// <returns></returns>
        Boolean ConvertProspectToHolder(int temporalId, int individualId, string documentNumber);
        #endregion
        #endregion

        /// <summary>
        /// Save Clauses
        /// </summary>
        /// <param name="temporalId"></param>
        /// <param name="clauses"></param>
        [OperationContract]
        List<CompanyClause> SaveCompanyClauses(int temporalId, List<CompanyClause> clauses);

        [OperationContract]
        CompanyPolicy SaveCompanyCoinsurance(CompanyIssuanceCoInsuranceCompany coInsuranceCompany, List<CompanyIssuanceCoInsuranceCompany> assignedCompanies, BusinessType businessType, int temporalId);

        [OperationContract]
        CompanyPaymentPlan SaveCompanyPaymentPlan(int temporalId, CompanyPaymentPlan paymentPlan, List<Quota> quotas);

        [OperationContract]
        CompanyText SaveCompanyTexts(int temporalId, CompanyText companyText);

        [OperationContract]
        List<Holder> GetCompanyHoldersByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType, TemporalType temporalType);

        [OperationContract]
        List<CompanyProduct> GetCompanyProductsByAgentIdPrefixId(int agentId, int prefixId, bool isCollective);

        [OperationContract]
        bool? SaveCompanyAdditionalDAta(int temporalId, bool calculateMinimumPremium);
        [OperationContract]
        int? SaveCompanyCorrelativePolicy(int temporalId, int? correlativePolicyNumber);

        [OperationContract]
        string ValidateCompanySurety(int temporalId, CompanyPolicy policy);

        /// <summary>
        /// Gets the status risk policy by temporal identifier.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="coveredRiskType">Type of the covered risk.</param>
        /// <returns></returns>
        [OperationContract]
        bool GetStatusRiskPolicyByTemporalId(int temporalId, CoveredRiskType coveredRiskType);
        #region coberturas
        /// <summary>
        /// Gets the company covered risk by temporal identifier.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="isMasive">if set to <c>true</c> [is masive].</param>
        /// <returns></returns>
        [OperationContract]
        CompanyCoveredRisk GetCompanyCoveredRiskByTemporalId(int temporalId, bool isMasive);

        /// <summary>
        /// Gets the coverage by coverage identifier.
        /// </summary>
        /// <param name="coverageId">The coverage identifier.</param>
        /// <param name="groupCoverageId">The group coverage identifier.</param>
        /// <param name="policyId">The policy identifier.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyCoverage GetCompanyCoverageByCoverageId(int coverageId, int groupCoverageId, int policyId);

        /// <summary>
        /// Gets the prv_coverage by coverage identifier and Number.
        /// </summary>
        /// <param name="coverageId">The coverage identifier.</param>
        /// <param name="coverageNum">The co coverage number.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyPrvCoverage GetPrvCoverageByIdAndNum(int coverageId, int coverageNum);

        /// <summary>
        /// Create Prv_coverage.
        /// </summary>
        /// <param name="coverageId">The coverage identifier.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyPrvCoverage CreatePrvCoverage(CompanyPrvCoverage prvCoverage);

        /// <summary>
        /// Update Prv_coverage.
        /// </summary>
        /// <param name="coverageId">The coverage identifier.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyPrvCoverage UpdatePrvCoverage(CompanyPrvCoverage prvCoverage);
        #endregion
        #region reglas
        /// <summary>
        /// Runs the rules company policy pre.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicy RunRulesCompanyPolicyPre(CompanyPolicy policy);
        #endregion reglas
        #region endosos
        /// <summary>
        /// Gets the cia endorsements by filter policy.
        /// </summary>
        /// <param name="branchId">The branch identifier.</param>
        /// <param name="prefixId">The prefix identifier.</param>
        /// <param name="policyNumber">The policy number.</param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyEndorsement> GetCiaEndorsementsByFilterPolicy(int branchId, int prefixId, decimal policyNumber, bool isCurrent = false, bool? isExchange = false);

        [OperationContract]
        CompanyPolicy GetCiaCurrentStatusPolicyByEndorsementIdIsCurrent(int endorsementId, bool isCurrent);


        [OperationContract]
        CompanyPolicy GetEndorsementInformation(int endorsementId, bool isCurrent);

        [OperationContract]
        void CreateCompanyPolicyPayer(CompanyPolicy companyPolicy);

        [OperationContract]
        bool DeleteEndorsementByPolicyIdEndorsementIdEndorsementType(int policyId, int endorsementId, EndorsementType endorsementType);
        #endregion
        #region findig polices
        [OperationContract]
        List<CompanyPolicy> GetCiaPoliciesByPolicy(CompanyPolicy companyPolicy);
        #endregion

        [OperationContract]
        List<CompanyCoverage> GetCompanyCoveragesByLineBusinessIdSubLineBusinessId(int lineBusinessId, int subLineBusinessId);

        [OperationContract]
        List<CompanyClause> RemoveClauses(List<CompanyClause> companyClauses, List<int> clauseIds);

        [OperationContract]
        List<CompanyClause> AddClauses(List<CompanyClause> companyClauses, List<int> clauseIds);

        [OperationContract]
        List<ConditionLevel> GetConditionLevels();

        [OperationContract]
        List<CompanyCoverage> RemoveCoverages(List<CompanyCoverage> companyCoverages, List<int> coverageIds);

        [OperationContract]
        Models.CompanyIssuanceInsured GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType);

        [OperationContract(Name = "GetCompanyInsuredsByDescriptionInsuredSearchType")]
        List<CompanyIssuanceInsured> GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType, TemporalType temporalType);

        [OperationContract]
        List<CompanyIssuanceInsured> GetListCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType);

        #region VehicleType

        [OperationContract]
        List<CompanyVehicleType> ExecuteOperationsCompanyVehicleType(List<CompanyVehicleType> vehicleTypes);

        [OperationContract]
        List<CompanyVehicleType> GetCompanyVehicleTypes();

        [OperationContract]
        string GenerateFileToCompanyVehicleType(string fileName);

        [OperationContract]
        string GenerateFileToCompanyVehicleBody(CompanyVehicleType vehicleTypeDTO, string fileName);

        #endregion


        #region Previsora
        [OperationContract]
        CompanyPolicy GetPolicyByPendingOperation(int operationId);

        [OperationContract]
        CompanyPolicy CompanySavePolicyTemporal(CompanyPolicy policy, bool isMasive,bool polities=false);

        [OperationContract]
        List<CompanyPolicy> GetCompanyPoliciesByQuotationIdVersionPrefixId(int quotationId, int version, int prefixId, int branchId);

        [OperationContract]
        List<CompanyPolicy> GetPoliciesByQuotationIdVersionPrefixId(int quotationId, int version, int prefixId, int branchId);

        [OperationContract]
        CompanyPolicy CompanyGetTemporalByIdTemporalType(int id, TemporalType temporalType);

        [OperationContract]
        CompanyPolicy CompanyGetPolicyByTemporalId(int temporalId, bool isMasive);

        [OperationContract]
        Core.Framework.Rules.Facade CreateFacadeGeneral(CompanyPolicy companyPolicy);

        [OperationContract]
        CompanySummaryComponent CompanySaveDiscounts(int temporalId, CompanySummaryComponent discounts);

        [OperationContract]
        CompanySummaryComponent CompanySaveSurcharge(int temporalId, CompanySummaryComponent surcharge);

        [OperationContract]
        List<CompanySurchargeComponent> GetCompanySurcharges();

        [OperationContract]
        CompanyPaymentPlan CompanySavePremiumFinance(int temporalId, CompanyPaymentPlan companyPaymentPlan);
        /// <summary>
        /// Gets CompanyJustificationSarlaft.
        /// </summary>
        /// <returns>List<CompanyJustificationSarlaft> </returns>
        [OperationContract]
        List<CompanyJustificationSarlaft> GetJustificationSarlaft();
        /// <summary>
        /// SaveCompanyPremiumFinance.
        /// </summary>
        [OperationContract]
        CompanyPaymentPlan SaveCompanyPremiumFinance(int temporalId, CompanyPaymentPlan premiumFinance);

        /// <summary>
        /// SaveCompanyPremiumFinance.
        /// </summary>
        [OperationContract]
        CompanyPremiumFinance GetCompanyNumberFinalcialPremium(int policyId);

        [OperationContract]
        void SaveTextLarge(int PolicyId, int EndorsementId);

        


        #endregion
        [OperationContract]
        List<CompanyPolicyAgent> GetAgenciesByDesciption(string agentId = "", string description = "", string productId = "", string userId = "");

        [OperationContract]
        CompanyPolicy CreateNewVersionQuotation(int quotationId);

        [OperationContract]
        List<CompanyPolicy> GetQuotationById(int quotationId, int IndividualId, int UserId, DateTime CurrentFrom, DateTime CurrentTo);
        [OperationContract]
        List<CompanyCoverage> GetCompanyCoveragesByRiskId(int riskId);

        [OperationContract]
        ExcelFileServiceModel GeneratePoductionReportServiceModel(CompanyProductionReport CompanyProductionReport);
        #region SubscriptionSearch

        [OperationContract]
        List<CompanyRiskVehicle> GetCompanyRiskByPlate(string description);

        [OperationContract]
        List<CompanyQuotationSearch> SearchQuotations(CompanySubscriptionSearch companySubscriptionSearch);

        [OperationContract]
        CompanyCoverageDeductible GetCompanyCoverageDeductibleByCoverageId(int CoverageId);

        [OperationContract]
        List<CompanyPolicySearch> SearchPolicies(CompanySubscriptionSearch companySubscriptionSearch);

        [OperationContract]
        List<CompanyTemporalSearch> SearchTemporals(CompanySubscriptionSearch companySubscriptionSearch);

        /// <summary>
        /// Genera el reporte al buscar cotizaciones
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>Modelo de archivo excel</returns>
        [OperationContract]
        MODSM.ExcelFileServiceModel GenerateQuotations(string fileName, CompanySubscriptionSearch companySubscriptionSearch);

        /// <summary>
        /// Genera el reporte al buscar polizas
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>Modelo de archivo excel</returns>
        [OperationContract]
        MODSM.ExcelFileServiceModel GeneratePolicies(string fileName, CompanySubscriptionSearch companySubscriptionSearch);

        #endregion
        /// <summary>
        /// Validar Coverturas Con Post Contractuales
        /// </summary>
        /// <param name="Policyid"></param>
        /// <returns>ArrayList</returns>
        [OperationContract]
        System.Collections.ArrayList ValidateCoveragePostContractual(int Policyid);

        [OperationContract]
        int GetCurrentRiskNumByPolicyId(int policyId);

        [OperationContract]
        void RecordEndorsementOperation(int endorsementId, int pendingOperationId);

        /// <summary>
        /// Obtener Plan De Pago Por Identificador
        /// </summary>
        /// <param name="paymentPlanId">Identificador</param>
        /// <returns>Plan De Pago</returns>
        [OperationContract]
        CompanyPaymentPlan GetPaymentPlanByPaymentPlanId(int paymentPlanId);

        [OperationContract]
        CompanyPaymentPlan GetPaymentPlanByPolicyId(int policyId);

        [OperationContract]
        DateTime GetQuotationDate(int moduleCode, DateTime issueDate);

        [OperationContract]
        List<PayerPayment> CalculatePayerPayment(CompanyPolicy companyPolicy, bool RequestIsOpen, DateTime RequestFrom, DateTime RequestTo);

        [OperationContract]
        List<Quota> CalculateQuotasWithrequestGroupig(List<PayerPayment> payerPayments);

        /// <summary>
        /// Obtener un riesgo especifico
        /// </summary>
        /// <param name="policyId">The policy identifier.</param>
        /// <param name="riskId">The risk identifier.</param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyCoverage> GetCompanyCoveragesByPolicyIdByRiskId(int policyId, int riskId);

        /// <summary>
        /// Obtener un riesgo especifico
        /// </summary>
        /// <param name="policyId">The policy identifier.</param>
        /// <param name="riskId">The risk identifier.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicy SaveTableTemporal(CompanyPolicy companyPolicy);

        /// <summary>
        /// Calculo de componentes
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="risks"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicy CalculatePolicyAmounts(CompanyPolicy policy, List<CompanyRisk> risks);

        /// <summary>
        /// Obtener Endosos de una Póliza
        /// </summary>
        /// <param name="policyNumber">Número de Póliza</param>
        /// <returns>Endosos</returns>        
        [OperationContract]
        List<Endorsement> GetEndorsementsContainByPolicyId(int policyId);

        #region CiaRatingZoneBranch

        [OperationContract]
        CiaRatingZoneBranch CreateCiaRatingZoneBranch(CiaRatingZoneBranch ciaRatingZoneBranch);

        [OperationContract]
        void DeleteCiaBranchRatingZone(int ratingZoneCode, int branchCode);

        [OperationContract]
        CiaRatingZoneBranch GetRatingZoneBranch(int ratingZoneCode, int branchCode);

        [OperationContract]
        List<CiaRatingZoneBranch> GetRatingZonesBranchs();

        [OperationContract]
        List<CompanyRatingZone> GetRatingZonesByPrefixIdAndBranchId(int prefixId, int branchId);

        [OperationContract]
        List<CompanyRatingZone> GetCompanyRatingZonesByPrefixId(int prefixId);

        [OperationContract]
        List<CiaRatingZoneBranch> SaveCiaRatingZoneBranch(List<CompanyRatingZone> companyRatingZones, int branchId);

        [OperationContract]
        List<CompanyRatingZone> GetRatingZonesAndPrefixAndBranch();

        [OperationContract]
        string GenerateFileToCiaRatingZone(List<CompanyRatingZone> companyRatingZones, string fileName);
        #endregion
        /// <summary>
        /// Retorna una listado de productos filtrado por ciertos criterios
        /// </summary>
        /// <param name="agentId">Identificador del agente</param>
        /// <param name="prefixId">Identificador del ramo</param>
        /// <param name="isGreen">Indica si retorna los productos con etiqueta IsGreen</param>
        /// <returns>Listado de productos</returns>
        [OperationContract]
        List<CompanyProduct> GetCompanyProductsByAgentIdPrefixIdIsGreen(int agentId, int prefixId, bool isGreen);

        /// <summary>
        /// Consulta los endosos que se han ejecutad contra una póliza
        /// </summary>
        /// <param name="prefixId">Identificador del ramo</param>
        /// <param name="branchId">Identificador de la sucursal</param>
        /// <param name="policyNumber">Número de póliza</param>
        /// <returns>Listado de endosos</returns>
        [OperationContract]
        List<CompanyEndorsement> GetCoPolicyEndorsementsByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber);

        /// <summary>
        /// Consulta un riesgo a partir de la información del endoso
        /// </summary>
        /// <param name="policyId">Identificador de póliza</param>
        /// <param name="endorsementId">Identificador del endoso</param>
        /// <returns>Riesgo</returns>
        [OperationContract]
        List<CompanyRisk> GetRiskByPolicyIdEndorsmentId(int policyId, int endorsementId);

        /// <summary>
        /// Consulta los endosos que han generado prima para una póliza
        /// </summary>
        /// <param name="policyId">Identificador de la póliza</param>
        /// <returns>Listado de endosos</returns>
        [OperationContract]
        List<CompanyEndorsement> GetCoPolicyEndorsementsWithPremiumByPolicyId(int policyId);

        /// <summary>
        /// Obtener las coberturas adicionales 
        /// </summary>
        /// <param name="coverageId">Id QUOEN.Coverage</param>
        /// <param name="productId">Id QUOEN.Group</param>
        /// <param name="groupCoverageId">Id QUOEN.Coverage</param>
        /// <returns>Lista de coberturas</returns>
        [OperationContract]
        List<CompanyCoverage> GetAddCompanyCoveragesByCoverageIdProductIdGroupCoverageId(int coverageId, int productId, int groupCoverageId);


        /// <summary>
        /// Actualiza el numero de documento de la poliza al final de emitir
        /// </summary>
        /// <param name="companyPolicy"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicy UpdateCompanyPolicyDocumentNumber(CompanyPolicy companyPolicy);

        [OperationContract]
        int? GetOperationIdTemSubscription(int temporalId);

        /// <summary>
        /// Devuelve información de la poliza.
        /// </summary>
        /// <param name="Policyid"></param>
        /// <returns>ArrayList</returns>
        [OperationContract]
        CompanyPolicy GetCiaCurrentStatusPolicyByEndorsementIdIsCurrentCompany(int endorsementId, bool isCurrent, bool fromPrinting = false);

        [OperationContract]
        int GetEndorsementRiskCount(int policyId, EndorsementType endorsementType);

        [OperationContract]
        TemporalDTO GetTemporalByDocumentNumberPrefixIdBrachId(decimal documentNumber, int prefixId, int branchId);

        /// <summary>
        /// Gets the company endorsements by filter policy.
        /// </summary>
        /// <param name="branchId">The branch identifier.</param>
        /// <param name="prefixId">The prefix identifier.</param>
        /// <param name="policyNumber">The policy number.</param>
        /// <param name="isCurrent">if set to <c>true</c> [is current].</param>
        /// <returns></returns>
        [OperationContract]
        List<EndorsementDTO> GetCompanyEndorsementsByFilterPolicy(int branchId, int prefixId, decimal policyNumber, bool isCurrent = true);

        /// <summary>
        /// Gets the current policy by endorsement identifier.
        /// </summary>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <param name="isCurrent">if set to <c>true</c> [is current].</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// </exception>
        [OperationContract]
        CompanyPolicy GetCurrentPolicyByEndorsementId(int endorsementId, bool isCurrent = true);

        /// <summary>
        /// Gets the temporal by policy identifier endorsement identifier.
        /// </summary>
        /// <param name="policyId">The policy identifier.</param>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <returns></returns>
        [OperationContract]
        TemporalDTO GetTemporalByPolicyIdEndorsementId(int policyId, int endorsementId);

        /// <summary>
        /// Obtiene los endosos por sucursal, numero de poliza y ramo
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="policyNumber"></param>
        /// <param name="prefixId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EndorsementCompanyDTO> GetEndorsementsByPrefixIdBranchIdPolicyNumberCompany(int branchId, int prefixId, decimal policyNumber);

        [OperationContract]
        int GetPaymentPlanScheduleByPolicyId(int policyId);

        [OperationContract]
        Tuple<Holder, List<IssuanceCompanyName>> GetHolderByIndividualId(string individualId, CustomerType? customerType);

        [OperationContract]
        List<Holder> GetHoldersByDocument(string document, CustomerType? customerType);

        [OperationContract]
        CompanyBeneficiary ConvertProspectToBeneficiary(CompanyBeneficiary beneficiary, int individualId);

        [OperationContract]
        CompanyRisk ConvertProspectToInsuredRisk(CompanyRisk risk, int individualId);

        [OperationContract]
        CompanyPaymentPlan GetDefaultPaymentPlan(int productId);

        [OperationContract]
        int GetPaymentPlanScheduleByPolicyEndorsementId(int policyId, int endorsementId);


        [OperationContract]
        List<Holder> GetPersonOrCompanyByDescription(string description, CustomerType? customerType);

        /// <summary>
        /// Validacion de pagare
        /// </summary>
        /// <param name="companyPolicy"></param>
        /// <param name="companyIssuanceInsured"></param>
        /// <returns></returns>

        [OperationContract]
        CompanyPolicy ValidateApplyPremiumFinance(CompanyPolicy companyPolicy, CompanyIssuanceInsured companyIssuanceInsured);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <param name="riskId"></param>
        /// <param name="riskNum"></param>
        /// <returns></returns>
        [OperationContract]
        List<DynamicConcept> LoadDynamicPropertiesRiskConcepts(int policyId, int endorsementId, int riskId, int riskNum);

        /// <summary>
        /// Actualiza la informacion del texto en emision
        /// </summary>
        /// <param name="companyPolicy"></param>
        /// <returns></returns>
        [OperationContract]
        Endorsement SaveContractObjectPolicyId(int endorsementId, int riskId, string textRisk, string textPolicy);

        /// <summary>
        /// Guarda Log de Cambios de Texto a nivel de riego y Cobertura
        /// </summary>
        /// <param name="endoChangeText"></param>
        /// <returns></returns>
        [OperationContract]
        EndoChangeText SaveLog(EndoChangeText endoChangeText);

        [OperationContract]
        long GetRateCoveragesByCoverageIdPolicyId(int policyId, int coverageId);

        [OperationContract]
        decimal GetCumulusQSise(int individualId);
    }
}