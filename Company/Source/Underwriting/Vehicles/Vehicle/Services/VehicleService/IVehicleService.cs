using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Vehicles.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.DTOs;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.ModelServices.Models.VehicleParam;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Vehicles.VehicleServices;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System.Collections.Generic;
using System.ServiceModel;
namespace Sistran.Company.Application.Vehicles.VehicleServices
{
    [ServiceContract]
    public interface IVehicleService : IVehicleServiceCore
    {
        /// <summary>
        /// Ejecutar Reglas De Riesgo
        /// </summary>
        /// <param name="vehicle">Vehiculo</param>
        /// <param name="ruleId">Id Regla</param>
        /// <returns>Vehiculo</returns>
        [OperationContract]
        CompanyVehicle RunRulesRisk(CompanyVehicle vehicle, int ruleId);

        /// <summary>
        /// Ejecutar Reglas De Cobertura
        /// </summary>
        /// <param name="companyVehicle">Vehiculo</param>
        /// <param name="coverage">Cobertura</param>
        /// <param name="ruleId">Id Regla</param>
        /// <returns>Cobertura</returns>
        [OperationContract]
        CompanyCoverage RunRulesCompanyCoverage(CompanyVehicle companyVehicle, CompanyCoverage coverage, int ruleId);

        /// <summary>
        /// Tarifar Vehiculo
        /// </summary>
        /// <param name="companyVehicle">Vehiculo</param>
        /// <param name="runRulesPre">Ejecutar Reglas Pre?</param>
        /// <param name="runRulesPost">Ejecutar Reglas Post?</param>
        /// <returns>Vehiculo</returns>
        [OperationContract]
        CompanyVehicle QuotateVehicle(CompanyVehicle companyVehicle, bool runRulesPre, bool runRulesPost, int? valor, bool isEndorsement = false);

        /// <summary>
        /// Tarifar Vehiculos
        /// </summary>
        /// <param name="companyVehicles">Vehiculos</param>
        /// <param name="runRulesPre">Ejecutar Reglas Pre?</param>
        /// <param name="runRulesPost">Ejecutar Reglas Post?</param>
        /// <returns>Vehiculos</returns>
        [OperationContract]
        List<CompanyVehicle> QuotateVehicles(CompanyPolicy companyPolicy, List<CompanyVehicle> companyVehicles, bool runRulesPre, bool runRulesPost, bool isEndorsement = false);

        /// <summary>
        /// Tarifar Cobertura
        /// </summary>
        /// <param name="companyVehicle">Vehiculo</param>
        /// <param name="coverage">Cobertura</param>
        /// <param name="runRulesPre">Ejecutar Reglas Pre?</param>
        /// <param name="runRulesPost">Ejecutar Reglas Post?</param>
        /// <returns>Cobertura</returns>
        [OperationContract]
        CompanyCoverage QuotateCompanyCoverage(CompanyVehicle companyVehicle, CompanyCoverage coverage, bool runRulesPre, bool runRulesPost);

        /// <summary>
        /// Guardar Vehiculo
        /// </summary>
        /// <param name="companyVehicle">Vehiculo</param>
        /// <returns>Vehiculo</returns>
        [OperationContract]
        CompanyVehicle CreateVehicleTemporal(CompanyVehicle companyVehicle, bool isMassive);

        /// <summary>
        /// Obtener Vehiculos De Una Póliza
        /// </summary>
        /// <param name="policyId">Id Póliza</param>
        /// <param name="endorsementId">Id Endoso</param>
        /// <param name="licensePlate">Placa</param>
        /// <returns>Vehiculos</returns>
        [OperationContract]
        List<CompanyVehicle> GetVehiclesByPolicyIdEndorsementIdLicensePlate(int policyId, int endorsementId, string licensePlate, int riskId = 0, bool riskCancelledAndExcluded = true);

        /// <summary>
        /// Obtener Vehiculos De Un Temporal
        /// </summary>
        /// <param name="policyId">Id Temporal</param>
        /// <returns>Vehiculos</returns>
        [OperationContract]
        List<CompanyVehicle> GetCompanyVehiclesByPolicyId(int policyId);

        /// <summary>
        /// Metodo para devolver riesgos
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyVehicle> GetCompanyVehiclesByEndorsementId(int endorsementId);

        /// <summary>
        /// Obtener Vehículo por Placa
        /// </summary>
        /// <param name="licencePlate">Placa</param>
        /// <returns>Vehículo</returns>
        [OperationContract]
        CompanyVehicle GetVehicleByLicensePlate(string licencePlate);
		 /// <summary>
        /// Metodo para devolver riesgos
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyVehicle> GetCompanyVehiclesByEndorsementIdModuleType(int endorsementId, ModuleType moduleType);
        /// <summary>
        /// Validacion masiva de placas
        /// </summary>
        /// <param name="validations">The validations.</param>
        /// <param name="validationsLicensePlate">The validations license plate.</param>
        /// <returns></returns>
        [OperationContract]
        List<Validation> GetVehicleLicensePlate(List<Validation> validations, List<ValidationLicensePlate> validationsLicensePlate);

        /// <summary>
        /// Obtener Vehículo por Código Fasecolda
        /// </summary>
        /// <param name="fasecoldaCode">Código Fasecolda</param>
        /// <returns>Vehículo</returns>
        [OperationContract]
        CompanyVehicle GetVehicleByFasecoldaCode(string fasecoldaCode, int year);

        /// <summary>
        /// Obtener Vehículo por Marca, Modelo y Versión
        /// </summary>
        /// <param name="makeId">Id Marca</param>
        /// <param name="modelId">Id Modelo</param>
        /// <param name="versionId">Id Versión</param>
        /// <returns>Vehículo</returns>
        [OperationContract]
        CompanyVehicle GetVehicleByMakeIdModelIdVersionId(int makeId, int modelId, int versionId);
        List<ValidationPlateServiceModel> GetValidationPlate();
        /// <summary>
        /// Obtener lista de Causas
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<CompanyNotInsurableCause> GetNotInsurableCauses();

        [OperationContract]
        int GetCountVehiclePolicyByPolicyId(int policyId);

        /// <summary>
        /// Metodo para devolver riesgos desde el esquema report
        /// </summary>
        /// <param name="prefixId">ramo</param>
        /// <param name="branchId"> sucursal</param>
        /// <param name="documentNumber"> numero de poliza</param>
        /// <param name="endorsementType"> endorsement</param>
        /// <returns>modelo de vehicles</returns>
        [OperationContract]
        List<CompanyVehicle> GetVehiclesByPrefixBranchDocumentNumberEndorsementType(int prefixId, int branchId, decimal documentNumber, EndorsementType endorsementType);

        /// <summary>
        /// Obtener Riesgos
        /// </summary>
        /// <param name="temporalId">Id Temporal</param>
        /// <returns>Riesgos</returns>
        [OperationContract]
        List<CompanyVehicle> GetCompanyVehiclesByTemporalId(int temporalId);

        /// <summary>
        /// Obtener Poliza
        /// </summary>
        /// <param name="companyPolicy">Policy</param>
        ///   /// <param name="vehicles">vehicles</param>
        /// <returns>Riesgos</returns>
        [OperationContract]
        CompanyPolicy CreateEndorsement(CompanyPolicy companyPolicy, List<CompanyVehicle> vehicles);

        /// <summary>
        /// Polizas asociadas a individual
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyVehicle> GetCompanyVehiclesByIndividualId(int individualId);

        /// <summary>
        /// Obtener Riesgo
        /// </summary>
        /// <param name="riskId">Id Riesgo</param>
        /// <returns>Riesgo</returns>
        [OperationContract]
        CompanyVehicle GetCompanyVehicleByRiskId(int riskId);
		[OperationContract]
        List<CompanyVehicle> GetCompanyRisksVehicleByInsuredId(int insuredId);

        [OperationContract]
        CompanyVehicle GetVehicleByRiskId(int riskId);

        #region compañia vehiculo

        /// <summary>
        /// Gets the company risk by identifier.
        /// </summary>
        /// <param name="endorsementType">Type of the endorsement.</param>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyVehicle GetCompanyRiskById(EndorsementType endorsementType, int temporalId, int id);

        /// <summary>
        /// Gets the company premium.
        /// </summary>
        /// <param name="policyId">The policy identifier.</param>
        /// <param name="vehicle">The vehicle.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyVehicle GetCompanyPremium(int policyId, CompanyVehicle vehicle);


        /// <summary>
        /// Saves the company risk.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="vehicle">The vehicle.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyVehicle SaveCompanyRisk(int temporalId, CompanyVehicle vehicle);

        [OperationContract]
        void CreateRisk(CompanyVehicle companyVehicle);

        /// <summary>
        /// Deletes the company risk.
        /// </summary>
        /// <param name="policyId">The policy identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteCompanyRisk(int policyId, int id);

        /// <summary>
        /// Existses the risk.
        /// </summary>
        /// <param name="vehicles">The vehicles.</param>
        /// <param name="riskId">The risk identifier.</param>
        /// <param name="licensePlate">The license plate.</param>
        /// <param name="engineNumber">The engine number.</param>
        /// <param name="chassisNumber">The chassis number.</param>
        /// <param name="endorsementType">Type of the endorsement.</param>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <returns></returns>
        [OperationContract]
        List<string> ExistsRisk(List<CompanyVehicle> vehicles, int riskId, string licensePlate, string engineNumber, string chassisNumber, CompanyPolicy policy, System.Boolean riskIncl = false);

        /// <summary>
        /// Updates the company risks.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicy UpdateCompanyRisks(int temporalId, bool isMassive);

        /// <summary>
        /// Exists the company risk by temporal identifier.
        /// </summary>
        /// <param name="tempId">The temporary identifier.</param>
        /// <returns></returns>
        [OperationContract]
        string ExistCompanyRiskByTemporalId(int tempId);
        #endregion compañia vehiculo
        #region Coberturas
        /// <summary>
        /// Gets the company coverages by product identifier group coverage identifier.
        /// </summary>
        /// <param name="policyId">The policy identifier.</param>
        /// <param name="groupCoverageId">The group coverage identifier.</param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyCoverage> GetCompanyCoveragesByProductIdGroupCoverageId(int policyId, int groupCoverageId);

        /// <summary>
        /// Saves the company coverages.
        /// </summary>
        /// <param name="policyId">The policy identifier.</param>
        /// <param name="riskId">The risk identifier.</param>
        /// <param name="coverages">The coverages.</param>
        /// <returns></returns>
        [OperationContract]
        bool SaveCompanyCoverages(int policyId, int riskId, List<CompanyCoverage> coverages);

        /// <summary>
        /// Excludes the company coverage.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="riskId">The risk identifier.</param>
        /// <param name="riskCoverageId">The risk coverage identifier.</param>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyCoverage ExcludeCompanyCoverage(int temporalId, int riskId, int riskCoverageId, string description);
        #endregion
        #region prospectos
        [OperationContract]
        bool ConvertProspectToInsured(int temporalId, int individualId, string documentNumber);
        #endregion
        #region textos
        [OperationContract]
        CompanyText SaveCompanyTexts(int riskId, CompanyText companyText);
        #endregion
        #region poliza
        [OperationContract(Name = "CreateCompanyPolicyVehicle")]
        CompanyPolicyResult CreateCompanyPolicy(int temporalId, int temporalType, bool clearPolicies);
        #endregion
        #region clausulas
        /// <summary>
        /// Saves the company clauses.
        /// </summary>
        /// <param name="riskId">The risk identifier.</param>
        /// <param name="clauses">The clauses.</param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyClause> SaveCompanyClauses(int riskId, List<CompanyClause> clauses);
        #endregion

        #region vehicleVersion

        [OperationContract]
        Vehicles.Models.CompanyVersion CreateCompanyVersion(Vehicles.Models.CompanyVersion companyVersion);

        /// <summary>
        /// Actualiza vehicleVersion
        /// </summary>
        /// <param name="vehicleVersion"></param>
        /// <returns></returns>
        [OperationContract]
        Vehicles.Models.CompanyVersion UpdateCompanyVersion(Vehicles.Models.CompanyVersion vehicleVersion);


        /// <summary>
        /// retorna una lista de vehicleVersions filtrada por descripción
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        [OperationContract]
        List<Vehicles.Models.CompanyVersion> GetCompanyVersionsByDescription(string description);

        /// <summary>
        /// Retorna una lista de vehicleVersions de acuerdo a los filtros proporcionados
        /// </summary>
        /// <param name="makeCode"></param>
        /// <param name="modelCode"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        [OperationContract]
        List<Vehicles.Models.CompanyVersion> GetCompanyVersionsByMakeModelVersion(int? makeCode, int? modelCode, string description);

        /// <summary>
        /// Elimina una vehicleVersion 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        [OperationContract]
        void DeleteCompanytVehicle(int id, int makeId, int modelId);
        #endregion

        #region clausulas

        //[OperationContract]
        //CompanyVehicle CompanyGetCompanyPremium(int policyId, CompanyVehicle vehicle);
        //[OperationContract]
        //CompanyVehicle CompanyQuotateVehicle(CompanyVehicle companyVehicle, bool runRulesPre, bool runRulesPost);
        [OperationContract]
        CompanyVehicle CompanySaveCompanyVehicleTemporal(CompanyVehicle companyVehicle);
        #endregion

        [OperationContract]
        CompanyVehicle CalculateVehicleMinimumPremium(CompanyVehicle companyVehicle);

        [OperationContract]
        List<CompanyServiceType> GetListCompanyServiceType();

        /// <summary>
        /// Obtiene los Beneficiarios 
        /// </summary>
        /// <param name="individualId">beneficiario</param>
        /// <param name="customerType">Tipo</param>
        /// <returns></returns>
        [OperationContract]
        IssuanceIdentificationDocument GetIdentificationDocumentByIndividualIdCustomerType(int individualId, int customerType);

        [OperationContract]
        List<PoliciesAut> ValidateAuthorizationPolicies(CompanyVehicle companyVehicle);

        [OperationContract]
        List<PoliciesAut> ValidateAuthorizationPoliciesMassive(CompanyVehicle companyVehicle, int hierarchy, List<int> ruleToValidateRisk, List<int> ruleToValidateCoverage);


        [OperationContract]
        bool GetCityExempt(int Branch);

        [OperationContract]
        CompanyPolicy UpdateQuotationRisk(int temporalId, bool isMassive);

        [OperationContract]
        List<string> ExistsRiskAuthorization(int temporalId);
        #region accesorios
        /// <summary>
        /// Accesorios Acomulado
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="riskNumber"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        [OperationContract]
        List<AccessoryDTO> GetPremiumAccesory(int policyId, int riskNumber, int days, bool isCancelation = false);

        [OperationContract]
        List<CompanyVehicle> GetCompanyRisksVehicleByLicensePlate(string licensePlate);
        #endregion

        [OperationContract]
        int GetSummaryRisk(CompanyEndorsement endorsement);

    }
}