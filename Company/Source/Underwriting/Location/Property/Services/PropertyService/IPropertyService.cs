using Sistran.Company.Application.Location.PropertyServices.DTO;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.Location.PropertyServices
{
    [ServiceContract]
    public interface IPropertyService : Sistran.Core.Application.Location.PropertyServices.IPropertyServiceCore
    {
        /// <summary>
        /// Ejecucion de reglas Pre de riesgo
        /// </summary>
        /// <param name="propertyRisk">Riesgo</param>
        /// <param name="policy">Poliza</param>
        /// <returns></returns>
        [OperationContract]
        CompanyPropertyRisk RunRulesRisk(CompanyPropertyRisk propertyRisk, int ruleSetId);

        /// <summary>
        /// Runs the rules Coverage pre.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        CompanyCoverage RunRulesCoverage(CompanyPropertyRisk propertyRisk, CompanyCoverage Coverage, int ruleSetId);

        /// <summary>
        /// Cotiza la poliza especifica lo los riesgos que contenga
        /// </summary>
        /// <param name="propertyPolicy">Poliza de Property a tarifar.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyPropertyRisk QuotateProperty(CompanyPropertyRisk companyPropertyRisk, bool runRulesPre, bool runRulesPost);

        /// <summary>
        /// Cotiza el riesgo especificado
        /// </summary>
        /// <param name="propertyRisk">Riesgo a cotizar</param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyPropertyRisk> QuotateProperties(CompanyPolicy companyPolicy, List<CompanyPropertyRisk> companyProperties, bool runRulesPre, bool runRulesPost);

        /// <summary>
        /// Cotiza la cobertura especificada
        /// </summary>
        /// <param name="coverage">Cobertura a cotizar</param>
        /// <returns></returns>
        [OperationContract]
        CompanyCoverage QuotateCoverage(CompanyPropertyRisk propertyRisk, CompanyCoverage Coverage, bool runRulesPre, bool runRulesPost);

        /// <summary>
        /// Insertar en tablas temporales desde el JSON
        /// </summary>
        /// <param name="propertyRisk">Modelo propertyRisk</param>
        [OperationContract]
        CompanyPropertyRisk CreatePropertyTemporal(CompanyPropertyRisk propertyRisk, bool isMassive);

        /// <summary>
        /// Obtener Poliza de de hogar
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <param name="temporalId">Id temporal</param>
        /// <returns>propertyPolicy</returns>
        [OperationContract]
        List<CompanyPropertyRisk> GetCompanyPropertiesByEndorsementId(int endorsementId);
        /// <summary>
        /// Obtener Poliza de de hogar
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <param name="temporalId">Id temporal</param>
        /// <param name="RistIdList">Id temporal</param>
        /// <returns>propertyPolicy</returns>
        [OperationContract]
        List<CompanyPropertyRisk> GetPropertiesByPolicyIdEndorsementIdRiskIdList(int policyId, int endorsementId, List<int> RiskIdList);

        /// <summary>
        /// Metodo para devolver riesgos desde el esquema report
        /// </summary>
        /// <param name="prefixId">ramo</param>
        /// <param name="branchId"> sucursal</param>
        /// <param name="documentNumber"> numero de poliza</param>
        /// <param name="endorsementType"> endorsement</param>
        /// <returns>modelo de vehicles</returns>
        [OperationContract]
        List<CompanyPropertyRisk> GetCompanyPropertyByPrefixBranchDocumentNumberEndorsementType(int prefixId, int branchId, decimal documentNumber, EndorsementType endorsementType);

        //[OperationContract]
        //List<CompanyPropertyRisk> GetCompanyPropertyRiskByEndorsementId(int endorsementId);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        List<CompanyPropertyRisk> GetCompanyPropertiesByPolicyId(int policyId);
        
        [OperationContract]
        /// <summary>
        /// Obtener Riesgos
        /// </summary>
        /// <param name="temporalId">Id Temporal</param>
        /// <returns>Riesgos</returns>
        List<CompanyPropertyRisk> GetCompanyPropertiesByTemporalId(int temporalId);

        [OperationContract]
        /// <summary>
        /// Gets the properties by policy identifier by risk identifier list.
        /// </summary>
        /// <param name="policyId">The policy identifier.</param>
        /// <param name="RiskIdList">The risk identifier list.</param>
        /// <returns></returns>
        List<CompanyPropertyRisk> GetPropertiesByPolicyIdByRiskIdList(int policyId, List<int> RiskIdList, int riskId = 0);

        [OperationContract]
        CompanyPolicy CreateEndorsement(CompanyPolicy companyPolicy, List<CompanyPropertyRisk> companyPropertyRisks);

        /// <summary>
        /// Polizas asociadas a individual
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyPropertyRisk> GetPropertiesByIndividualId(int individualId);

        /// <summary>
        /// Crear el riesgo de hogar con base en el modelo
        /// </summary>
        /// <param name="propertyRisk"></param>
        [OperationContract]
        void CreateRisk(CompanyPropertyRisk propertyRisk);

        /// <summary>
        /// Obtener Riesgo
        /// </summary>
        /// <param name="riskId">Id Riesgo</param>
        /// <returns>Riesgo</returns>
        [OperationContract]
        CompanyPropertyRisk GetCompanyPropertyRiskByRiskId(int riskId);

        /// <summary>
        /// Obtener Riesgo
        /// </summary>
        /// <param name="riskId">Id Riesgo</param>
        /// <returns>Riesgo</returns>
        [OperationContract]
        CompanyRiskLocation GetCompanyPropertyRiskByRiskIdModuleType(int riskId, ModuleType moduleType);

        [OperationContract]
        List<CompanyRiskLocation> GetRiskLocationsByEndorsementIdModuleType(int endorsementId, ModuleType moduleType);

        [OperationContract]
        List<CompanyRiskLocation> GetCompanyRisksLocationByInsuredId(int insuredId);

        /// <summary>
        /// Obtener Tipo de Riesgo por locación
        /// </summary>
        /// <param name="prefixId">
        /// <param name="description">
        /// <returns> Lista por Location </returns>
        [OperationContract]
        List<CompanyRiskLocation> GetRisksLocationByAddress(string adderess);
        #region emision
        /// <summary>
        /// Gets the company risk by risk identifier.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyPropertyRisk GetCompanyRiskByRiskId(int temporalId, int id);

        /// <summary>
        /// Gets the company insured objects by temporal identifier risk identifier.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="riskId">The risk identifier.</param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyInsuredObject> GetCompanyInsuredObjectsByTemporalIdRiskId(int temporalId, int riskId);

        /// <summary>
        /// Gets the company insured object by temporal identifier risk identifier insured object identifier.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="riskId">The risk identifier.</param>
        /// <param name="insuredObjectId">The insured object identifier.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyInsuredObject GetCompanyInsuredObjectByTemporalIdRiskIdInsuredObjectId(int temporalId, int riskId, int insuredObjectId);
       
        /// <summary>
        /// Deletes the company risk.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="riskId">The risk identifier.</param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteCompanyRisk(int temporalId, int riskId);
        
        /// <summary>
        /// Saves the company risk.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="propertyRisk">The property risk.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyPropertyRisk SaveCompanyRisk(int temporalId, CompanyPropertyRisk propertyRisk);

        /// <summary>
        /// Gets the insured objects by product identifier group coverage identifier.
        /// </summary>
        /// <param name="allInsuredObject">if set to <c>true</c> [all insured object].</param>
        /// <param name="companyPropertyRisk">The company property risk.</param>
        /// <param name="isSelected">if set to <c>true</c> [is selected].</param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyInsuredObject> GetInsuredObjectsByProductIdGroupCoverageId(Boolean allInsuredObject, CompanyPropertyRisk companyPropertyRisk, Boolean isSelected = false);

        /// <summary>
        /// Deletes the insured object by risk identifier insured object identifier.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="riskId">The risk identifier.</param>
        /// <param name="insuredObjectId">The insured object identifier.</param>
        /// <returns></returns>
        [OperationContract]
        InsuredObject DeleteInsuredObjectByRiskIdInsuredObjectId(int temporalId, int riskId, int insuredObjectId);

        /// <summary>
        /// Converts the prospect to insured.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="individualId">The individual identifier.</param>
        /// <param name="documentNumber">The document number.</param>
        /// <returns></returns>
        [OperationContract]
        Boolean ConvertProspectToInsured(int temporalId, int individualId, string documentNumber);

        /// <summary>
        /// Saves the texts.
        /// </summary>
        /// <param name="riskId">The risk identifier.</param>
        /// <param name="textModel">The text model.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyText SaveTexts(int riskId, CompanyText textModel);

        /// <summary>
        /// Saves the clauses.
        /// </summary>
        /// <param name="riskId">The risk identifier.</param>
        /// <param name="clauses">The clauses.</param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyClause> SaveClauses(int riskId, List<CompanyClause> clauses);

        /// <summary>
        /// Saves the clauses by coverage identifier.
        /// </summary>
        /// <param name="riskId">The risk identifier.</param>
        /// <param name="coverageId">The coverage identifier.</param>
        /// <param name="clauses">The clauses.</param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyClause> SaveClausesByCoverageId(int riskId, int coverageId, List<CompanyClause> clauses);

        /// <summary>
        /// Updates the risks.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicy UpdateRisks(int temporalId);

        /// <summary>
        /// Saves the insured object.
        /// </summary>
        /// <param name="riskId">The risk identifier.</param>
        /// <param name="objectModel">The object model.</param>
        /// <param name="tempId">The temporary identifier.</param>
        /// <param name="groupCoverageId">The group coverage identifier.</param>
        /// <returns></returns>
        [OperationContract]
        bool SaveInsuredObject(int riskId, CompanyInsuredObject objectModel, int tempId, int groupCoverageId);

        /// <summary>
        /// Gets the temporal by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyPropertyRisk> GetTemporalById(int id);

        /// <summary>
        /// Saves the additional data.
        /// </summary>
        /// <param name="riskId">The risk identifier.</param>
        /// <param name="companyPropertyRisk">The company property risk.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyPropertyRisk SaveAdditionalData(int riskId, CompanyPropertyRisk companyPropertyRisk);

        /// <summary>
        /// Runruleses the specified group coverage identifier.
        /// </summary>
        /// <param name="groupCoverageId">The group coverage identifier.</param>
        /// <param name="companyInsuredObjects">The company insured objects.</param>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="companyPropertyRisk">The company property risk.</param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyInsuredObject> Runrules(int groupCoverageId, List<CompanyInsuredObject> companyInsuredObjects, int temporalId, CompanyPropertyRisk companyPropertyRisk);

        /// <summary>
        /// Creates the company policy.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="temporalType">Type of the temporal.</param>
        /// <returns></returns>
        [OperationContract(Name = "CreateCompanyPolicyProperty")]
        CompanyPolicyResult CreateCompanyPolicy(int temporalId, int temporalType, bool clearPolicies);

        /// <summary>
        /// Get coverage by risk id
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        CompanyCoverage GetCoverageToAddByRiskId(int tempId, int riskId, int coverageId, int insuredObjectId);

        /// <summary>
        /// Get all coverages by coverage
        /// </summary>
        /// <param name="tempId"></param>
        /// <param name="riskId"></param>
        /// <param name="groupCoverageId"></param>
        /// <param name="coverage"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyCoverage> GetAllyCoverageByCoverage(int tempId, int riskId, int groupCoverageId, CompanyCoverage coverage);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        CompanyCoverage ExcludeCoverage(int temporalId, int riskId, int riskCoverageId, string description);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        CompanyPropertyRisk GetPremium(CompanyPropertyRisk riskModel);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        List<CompanyCoverage> GetCoveragesByCoveragesAdd(int productId, int coverageGroupId, int prefixId, string coveragesAdd, int insuredObjectId);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        List<CompanyCoverage> GetTemporalCoveragesByRiskIdInsuredObjectId(int riskId, int insuredObjectId);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        List<CompanyRiskSubActivity> GetSubActivities(int riskActivity);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        List<CompanyAssuranceMode> GetAssuranceMode();
        #endregion

        /// <summary>
        /// Obtener Riesgo
        /// </summary>
        /// <param name="coverageId">Id Riesgo</param>
        /// <returns>Riesgo</returns>
        [OperationContract]
        CompanyCoverage GetCoverageByCoverageId(int coverageId, int riskId, int temporalId, int groupCoverageId);

        /// <summary>
        /// Obtiener la lista de Periodos de Declaración
        /// </summary>
        /// <returns>Lista de periodos de declaracion </returns>
        [OperationContract]
        List<CompanyDeclarationPeriod> GetCompanyDeclarationPeriods();

        /// <summary>
        /// Obtiener la lista de periodos de ajuste
        /// </summary>
        /// <returns>Lista de tipos de periodos de ajuste</returns>
        [OperationContract]
        List<CompanyAdjustPeriod> GetCompayAdjustPeriods();
        
        #region EndosoAjuste
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endorsementTypeId"></param>
        /// <param name="policyId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EndorsementDTO> GetEndorsementByEndorsementTypeIdPolicyId(int endorsementTypeId, int policyId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        [OperationContract]
        EndorsementDTO GetTemporalEndorsementByPolicyId(int policyId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        [OperationContract]
        EndorsementDTO GetNextAdjustmentEndorsementByPolicyId(int policyId);

        //[OperationContract]
        //bool CanMakeAdjustmentEndorsement(int policyId);



        [OperationContract]
        bool CanMakeEndorsement(int policyId, out Dictionary<string, object> validateEndorsement);

        /// <summary>
        /// Devuelve los objetos del seguro asociados al riesgo seleccionado
        /// </summary>
        /// <param name="riskId"></param>
        /// <returns></returns>
        [OperationContract]
        List<InsuredObjectDTO> GetInsuredObjectsByRiskId(int riskId);

        #endregion

        #region EndosoDeclaración
        /// <summary>
        /// 
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        [OperationContract]
        EndorsementDTO GetNextDeclarationEndorsementByPolicyId(int policyId);

        [OperationContract]
        bool CanMakeDeclarationEndorsement(int policyId);
        #endregion


        /// <summary>
        /// Realiza el calculo de las coberturas asoociadas al objeto del seguro
        /// </summary>
        /// <param name="riskId">Riesgo</param>
        /// <param name="insuredObject">Objetos del seguro</param>
        /// <param name="depositPremiumPercent">Porcentaje de prima en deposito</param>
        /// <param name="rate">tasa</param>
        /// <param name="currentFrom">Fecha de inicio póliza</param>
        /// <param name="currentTo">Fecha de fin póliza</param>
        /// <param name="insuredLimit">Suma asegurada del Objeto del seguro</param>
        /// <param name="runRulesPre">Ejecuta Pre</param>
        /// <param name="runRulesPost">Ejecuta Post</param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyCoverage> GetCalculateCoveragesByInsuredObjectId(int riskId, CompanyInsuredObject insuredObject,
            decimal depositPremiumPercent, decimal rate, DateTime currentFrom, DateTime currentTo, decimal insuredLimit, bool runRulesPre, bool runRulesPost);

        [OperationContract]
        bool CanMakeEndorsementByRiskByInsuredObjectId(decimal policyId, decimal riskId, decimal insuredObjectId, EndorsementType endorsementType);
    }
}



