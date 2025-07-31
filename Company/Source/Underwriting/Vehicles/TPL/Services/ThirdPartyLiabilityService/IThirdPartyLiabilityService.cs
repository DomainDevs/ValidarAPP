using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Vehicles.Models;
using Sistran.Core.Application.Vehicles.ThirdPartyLiabilityService;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using VEMO = Sistran.Core.Application.Vehicles.Models;
using Sistran.Core.Services.UtilitiesServices.Models;

namespace Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService
{
    [ServiceContract]
    public interface IThirdPartyLiabilityService : IThirdPartyLiabilityServiceCore
    {

        /// <summary>
        /// Ejecucion de reglas Pre de riesgo
        /// </summary>
        /// <param name="companyTplRisk">riesgo</param>
        /// <param name="ruleSetId">Id regla.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyTplRisk RunRulesRisk(CompanyTplRisk companyTplRisk, int ruleSetId);

        /// <summary>
        /// Ejecutar reglas de cobertura
        /// </summary>
        /// <param name="coverage">Cobertura</param>
        /// <param name="ruleSetId">Id regla</param>
        /// <returns>Cobertura</returns>
        [OperationContract]
        CompanyCoverage RunRulesCompanyCoverage(CompanyTplRisk companyTplRisk, CompanyCoverage coverage, int ruleId);

        /// <summary>
        /// Cotiza la poliza especifica lo los riesgos que contenga
        /// </summary>
        /// <param name="companyTplRisk">Poliza a cotizar</param>
        /// <returns></returns>
        [OperationContract]
        CompanyTplRisk QuotateThirdPartyLiability(CompanyTplRisk companyTplRisk, bool runRulesPre, bool runRulesPost);

        /// <summary>
        /// Cotiza el vehiculo especificado
        /// </summary>
        /// <param name="vehicle">Riesgo Vehiculo a cotizar</param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyTplRisk> QuotateThirdPartyLiabilities(CompanyPolicy companyPolicy, List<CompanyTplRisk> companyTplRisks, bool runRulesPre, bool runRulesPost);

        /// <summary>
        /// Cotiza la cobertura especificada
        /// </summary>
        /// <param name="coverage">Cobertura a cotizar</param>
        /// <returns></returns>
        [OperationContract]
        CompanyCoverage QuotateCompanyCoverage(CompanyTplRisk companyTplRisk, CompanyCoverage coverage, bool runRulesPre, bool runRulesPost);

        /// <summary>
        /// Insertar en tablas temporales desde el JSON
        /// </summary>
        /// <param name="temporalId">Id temporal</param>
        /// <param name="thirdPartyLiability">Modelo thirdPartyLiability</param>
        [OperationContract]
        CompanyTplRisk CreateThirdPartyLiabilityTemporal(CompanyTplRisk thirdPartyLiability, bool isMassive);

        /// <summary>
        /// Obtener Poliza de RC pasajeros
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <param name="temporalId">Id temporal</param>
        /// <returns>ThirdPartyLiabilityPolicy</returns>
        [OperationContract]
        List<CompanyTplRisk> GetThirdPartyLiabilityPolicyByPolicyIdEndorsementIdlicensePlate(int policyId, int endorsementId, string licensePlate);

        /// <summary>
        /// Obtener Poliza de RC pasajeros
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <returns>ThirdPartyLiabilityPolicy</returns>
        [OperationContract]
        List<CompanyTplRisk> GetCompanyThirdPartyLiabilitiesByPolicyId(int policyId);

        [OperationContract]
        int GetCountThirdPartyLiabilityPolicyByPolicyId(int policyId);

        /// <summary>
        /// Obtiene el tipo de servicio de un vehículo por el código del tipo de servicio
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ServiceType GetServiceTypeByServiceTypeId(int serviceTypeId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefixId"></param>
        /// <param name="branchId"></param>
        /// <param name="correlativePolicyNumber"></param>
        /// <param name="productId"></param>
        /// <param name="licensePlate"></param>
        /// <returns></returns>
        [OperationContract]
        bool ValidateThirdPartyLiabilityCorrelativePolicy(int prefixId, int branchId, decimal correlativePolicyNumber, int productId, string licensePlate);

        /// <summary>
        /// Obtener Riesgos
        /// </summary>
        /// <param name="endorsementId">Id Endoso</param>
        /// <returns>Riesgos</returns>
        [OperationContract]
        List<CompanyTplRisk> GetCompanyThirdPartyLiabilitiesByEndorsementId(int endorsementId);

        /// <summary>
        /// Obtener Riesgos
        /// </summary>
        /// <param name="temporalId">Id Temporal</param>
        /// <returns>Riesgos</returns>
        [OperationContract]
        List<CompanyTplRisk> GetThirdPartyLiabilitiesByTemporalId(int temporalId);

        [OperationContract]
        CompanyPolicy CreateEndorsement(CompanyPolicy companyPolicy, List<CompanyTplRisk> companyVehicles);

        /// <summary>
        /// Obtener Riesgo
        /// </summary>
        /// <param name="riskId">Id Riesgo</param>
        /// <returns>Riesgo</returns>
        [OperationContract]
        CompanyTplRisk GetCompanyTplRiskByRiskId(int riskId);

        #region emision
        /// <summary>
        /// Exclusion Riesgo
       
        [OperationContract]
        CompanyCoverage ExcludeCompanyCoverage(int temporalId, int riskId, int riskCoverageId, string description);

        /// <summary>
        /// Guardar Cobertura

        [OperationContract]
        Boolean SaveCompanyCoverages(int temporalId, int riskId, List<CompanyCoverage> coverages);

        /// <summary>
        /// Actualizar Riesgo

        [OperationContract]
        CompanyPolicy UpdateCompanyRisks(int temporalId);

        /// <summary>
        /// Valida la existencia del Riesgo 

        [OperationContract]
        string ExistCompanyRiskByTemporalId(int tempId);

        /// <summary>
        /// Obtiene el Riesgo por el id 

        [OperationContract]
        CompanyTplRisk GetCompanyRiskById(EndorsementType endorsementType, int temporalId, int id);


        [OperationContract]
        CompanyTplRisk GetCompanyPremium(int policyId, CompanyTplRisk ThirdPartyLiability);

        /// <summary>
        /// Guardar el Riesgo
        /// </summary>
        /// <param name="thirdPartyLiability"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyTplRisk SaveCompanyRisk(int temporalId,CompanyTplRisk thirdPartyLiability);

        /// <summary>
        /// Eliminar el Riesgo
        /// </summary>
        /// <param name="temporalId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteCompanyRisk(int temporalId, int id);

        /// <summary>
        /// Convertir
        /// </summary>
        /// <param name="temporalId"></param>
        /// <param name="individualId"></param>
        /// <param name="documentNumber"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean ConvertProspectToInsured(int temporalId, int individualId, string documentNumber);

        /// <summary>
        /// Guardar el texto
        /// </summary>
        /// <param name="riskId"></param>
        /// <param name="companyText"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyText SaveCompanyTexts(int riskId, CompanyText companyText);

        /// <summary>
        /// Guardar Clausulas
        /// </summary>
        /// <param name="riskId"></param>
        /// <param name="clauses"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyClause> SaveCompanyClauses(int riskId, List<CompanyClause> clauses);
        #endregion

        #region poliza
        [OperationContract(Name = "CreateCompanyPolicyTPL")]
        CompanyPolicyResult CreateCompanyPolicy(int temporalId, int temporalType, bool clearPolicies);
        #endregion

        /// <summary>
        /// tipo de carga transpotada
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<CargoType> GetCargoTypes();



        /// <summary>
        /// Crea el riesgo para colectivas
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        void CreateRisk(CompanyTplRisk companyTplRisk);

        [OperationContract]
        List<PoliciesAut> ValidateAuthorizationPoliciesMassive(CompanyTplRisk companyVehicle, int hierarchy, List<int> ruleToValidateRisk, List<int> ruleToValidateCoverage);

        /// <summary>
        /// Obtener lista de versiones por marca y modelo
        /// </summary>
        /// <param name="makeId">Id marca</param>
        /// <param name="year">aÑO</param>
        /// <returns>version</returns>
        [OperationContract]
        VEMO.Version GetVersionsByMakeIdYear(int makeId, int year);
        
        /// <summary>
        /// Valida la autorizacion de politicas
        /// </summary>
        /// <param name="companyTplRisk"></param>
        /// <returns></returns>
        [OperationContract]
        List<PoliciesAut> ValidateAuthorizationPolicies(CompanyTplRisk companyTplRisk);


        /// <summary>
        /// Validacion masiva de placas
        /// </summary>
        /// <param name="validations">The validations.</param>
        /// <param name="validationsLicensePlate">The validations license plate.</param>
        /// <returns></returns>
        [OperationContract]
        List<Validation> GetVehicleLicensePlate(List<Validation> validations, List<ValidationLicensePlate> validationsLicensePlate);

    }
}
