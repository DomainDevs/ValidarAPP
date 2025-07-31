using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Error;
using Sistran.Company.Application.Vehicles.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.DTOs;
using Sistran.Company.Application.Vehicles.VehicleServices.EEProvider.Assemblers;
using Sistran.Company.Application.Vehicles.VehicleServices.EEProvider.Business;
using Sistran.Company.Application.Vehicles.VehicleServices.EEProvider.DAOs;
using Sistran.Company.Application.Vehicles.VehicleServices.EEProvider.Resources;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.ModelServices.Models.VehicleParam;
using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Application.Vehicles.VehicleServices.EEProvider;
using Sistran.Core.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Vehicles.VehicleServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class VehicleServiceEEProvider : VehicleServiceEEProviderCore, IVehicleService
    {

        public VehicleServiceEEProvider()
        {
            Errors.Culture = KeySettings.ServiceResourceCulture;
        }
        /// <summary>
        /// Ejecutar Reglas De Riesgo
        /// </summary>
        /// <param name="vehicle">Vehiculo</param>
        /// <param name="ruleId">Id Regla</param>
        /// <returns>Vehiculo</returns>
        public CompanyVehicle RunRulesRisk(CompanyVehicle companyVehicle, int ruleId)
        {
            try
            {
                VehicleBusiness businessVehicle = new VehicleBusiness();
                return businessVehicle.RunRulesRisk(companyVehicle, ruleId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Ejecutar Reglas De Cobertura
        /// </summary>
        /// <param name="companyVehicle">Vehiculo</param>
        /// <param name="coverage">Cobertura</param>
        /// <param name="ruleId">Id Regla</param>
        /// <returns>Cobertura</returns>
        public CompanyCoverage RunRulesCompanyCoverage(CompanyVehicle companyVehicle, CompanyCoverage coverage, int ruleId)
        {
            try
            {
                CoverageBusiness businessCoverage = new CoverageBusiness();
                return businessCoverage.RunRulesCoverage(companyVehicle, coverage, ruleId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Tarifar Vehiculo
        /// </summary>
        /// <param name="companyVehicle">Vehiculo</param>
        /// <param name="runRulesPre">Ejecutar Reglas Pre?</param>
        /// <param name="runRulesPost">Ejecutar Reglas Post?</param>
        /// <returns>Vehiculo</returns>
        public CompanyVehicle QuotateVehicle(CompanyVehicle companyVehicle, bool runRulesPre, bool runRulesPost, int? valor, bool isEndorsement = false)
        {
            try
            {
                VehicleBusiness vehicleBusiness = new VehicleBusiness();
                return vehicleBusiness.QuotateVehicle(companyVehicle, runRulesPre, runRulesPost, valor, isEndorsement);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorQuotate), ex);
            }
        }

        /// <summary>
        /// Tarifar Vehiculos
        /// </summary>
        /// <param name="companyVehicles">Vehiculos</param>
        /// <param name="runRulesPre">Ejecutar Reglas Pre?</param>
        /// <param name="runRulesPost">Ejecutar Reglas Post?</param>
        /// <returns>Vehiculos</returns>
        public List<CompanyVehicle> QuotateVehicles(CompanyPolicy companyPolicy, List<CompanyVehicle> companyVehicles, bool runRulesPre, bool runRulesPost, bool isEndorsement = false)
        {
            try
            {
                VehicleBusiness businessVehicle = new VehicleBusiness();
                return businessVehicle.QuotateVehicles(companyPolicy, companyVehicles, runRulesPre, runRulesPost, isEndorsement);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Tarifar Cobertura
        /// </summary>
        /// <param name="companyVehicle">Vehiculo</param>
        /// <param name="coverage">Cobertura</param>
        /// <param name="runRulesPre">Ejecutar Reglas Pre?</param>
        /// <param name="runRulesPost">Ejecutar Reglas Post?</param>
        /// <returns>Cobertura</returns>
        public CompanyCoverage QuotateCompanyCoverage(CompanyVehicle companyVehicle, CompanyCoverage coverage, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                CoverageBusiness businessCoverage = new CoverageBusiness();
                return businessCoverage.Quotate(companyVehicle, coverage, runRulesPre, runRulesPost);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Guardar Vehiculo
        /// </summary>
        /// <param name="companyVehicle">Vehiculo</param>
        /// <returns>Vehiculo</returns>
        public CompanyVehicle CreateVehicleTemporal(CompanyVehicle companyVehicle, bool isMassive)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                CompanyVehicle vehicle = vehicleDAO.CreateVehicleTemporal(companyVehicle, isMassive);
                return vehicle;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<PoliciesAut> ValidateAuthorizationPolicies(CompanyVehicle companyVehicle)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.ValidateAuthorizationPolicies(companyVehicle);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<PoliciesAut> ValidateAuthorizationPoliciesMassive(CompanyVehicle companyVehicle, int hierarchy, List<int> ruleToValidateRisk, List<int> ruleToValidateCoverage)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.ValidateAuthorizationPoliciesMassive(companyVehicle, hierarchy, ruleToValidateRisk, ruleToValidateCoverage);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }



        /// <summary>
        /// Obtener Vehiculos De Una Póliza
        /// </summary>
        /// <param name="policyId">Id Póliza</param>
        /// <param name="endorsementId">Id Endoso</param>
        /// <param name="licensePlate">Placa</param>
        /// <param name="riskId">Id del riesgo si lo tiene</param>
        /// <param name="riskCancelledAndExcluded">si requiere filtrar los riesgos cancelados y excluidos</param>
        /// <returns>Vehiculos</returns>
        public List<CompanyVehicle> GetVehiclesByPolicyIdEndorsementIdLicensePlate(int policyId, int endorsementId, string licensePlate, int riskId = 0, bool riskCancelledAndExcluded = true)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();

                List<CompanyVehicle> list = vehicleDAO.GetVehiclesByPolicyIdEndorsementIdLicensePlate(policyId, endorsementId, licensePlate, riskId, riskCancelledAndExcluded);
                return list;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Vehiculos De Un Temporal
        /// </summary>
        /// <param name="policyId">Id Temporal</param>
        /// <returns>Vehiculos</returns>
        public List<CompanyVehicle> GetCompanyVehiclesByPolicyId(int policyId)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetCompanyVehiclesByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Vehículo por Placa
        /// </summary>
        /// <param name="licencePlate">Placa</param>
        /// <returns>Vehículo</returns>
        public CompanyVehicle GetVehicleByLicensePlate(string licencePlate)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetVehicleByLicensePlate(licencePlate);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Obtener Vehículo por Placa
        /// </summary>
        /// <param name="licencePlate">Placa</param>
        /// <returns>Vehículo</returns>
        public List<Validation> GetVehicleLicensePlate(List<Validation> validations, List<ValidationLicensePlate> validationsLicensePlate)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetVehicleLicensePlate(validations, validationsLicensePlate);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Obtener Vehículo por Código Fasecolda
        /// </summary>
        /// <param name="fasecoldaCode">Código Fasecolda</param>
        /// <returns>Vehículo</returns>
        public CompanyVehicle GetVehicleByFasecoldaCode(string fasecoldaCode, int year)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetVehicleByFasecoldaCode(fasecoldaCode, year);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Vehículo por Marca, Modelo y Versión
        /// </summary>
        /// <param name="makeId">Id Marca</param>
        /// <param name="modelId">Id Modelo</param>
        /// <param name="versionId">Id Versión</param>
        /// <returns>Vehículo</returns>
        public CompanyVehicle GetVehicleByMakeIdModelIdVersionId(int makeId, int modelId, int versionId)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetVehicleByMakeIdModelIdVersionId(makeId, modelId, versionId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<ValidationPlateServiceModel> GetValidationPlate()
        {
            ValidationPlatesServiceModel ValidationPlateModel = new ValidationPlatesServiceModel();
            VehicleDAO VehicleDAO = new VehicleDAO();
            Result<List<CompanyValidationPlate>, ErrorModel> result = VehicleDAO.GetValidationPlate();
            if (result is ResultError<List<CompanyValidationPlate>, ErrorModel>)
            {
                ErrorModel errorModelResult = (result as ResultError<List<CompanyValidationPlate>, ErrorModel>).Message;
                ValidationPlateModel.ErrorTypeService = (Core.Application.ModelServices.Enums.ErrorTypeService)errorModelResult.ErrorType;
                ValidationPlateModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is ResultValue<List<CompanyValidationPlate>, ErrorModel>)
            {
                List<CompanyValidationPlate> paramValidationPlatec = (result as ResultValue<List<CompanyValidationPlate>, ErrorModel>).Value;
                ValidationPlateModel.ValidationPlateModel = ModelAssembler.CreateValidationPlatesService(paramValidationPlatec);
            }

            return ValidationPlateModel.ValidationPlateModel;
        }

        /// <summary>
        /// Obtener lista de Causas
        /// </summary>
        /// <returns></returns>
        public List<Models.CompanyNotInsurableCause> GetNotInsurableCauses()
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetNotInsurableCauses();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Riesgos
        /// </summary>
        /// <returns></returns>
        public int GetCountVehiclePolicyByPolicyId(int policyId)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetCountVehiclePolicyByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Poliza de vehiculos(sin riesgos)
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <param name="endorsementId">Id endoso</param>
        /// <returns>Vehiclepolicy</returns>
        public CompanyPolicy GetVehiclePolicyWithOutRiskByPolicyId(int policyId, int endorsementId)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetVehiclePolicyWithOutRiskByPolicyId(policyId, endorsementId);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        public List<CompanyVehicle> GetCompanyVehiclesByEndorsementId(int endorsementId)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetCompanyVehiclesByEndorsementId(endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }
        public List<CompanyVehicle> GetCompanyVehiclesByEndorsementIdModuleType(int endorsementId, ModuleType moduleType)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();

                List<CompanyVehicle> companyVehicles = vehicleDAO.GetCompanyVehiclesByEndorsementIdModuleType(endorsementId, moduleType);

                return companyVehicles;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        public List<CompanyVehicle> GetVehiclesByPrefixBranchDocumentNumberEndorsementType(int prefixId, int branchId, decimal documentNumber, EndorsementType endorsementType)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetVehiclesByPrefixBranchDocumentNumberEndorsementType(prefixId, branchId, documentNumber, endorsementType);
            }
            catch (Exception ex)
            {

                throw new BusinessException(Errors.ErrorGetVehicles, ex);
            }
        }

        /// <summary>
        /// Obtener Riesgos
        /// </summary>
        /// <param name="temporalId">Id Temporal</param>
        /// <returns>Riesgos</returns>
        public List<CompanyVehicle> GetCompanyVehiclesByTemporalId(int temporalId)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetCompanyVehiclesByTemporalId(temporalId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, "Error al consultar riesgos"));
            }
        }

        public CompanyPolicy CreateEndorsement(CompanyPolicy companyPolicy, List<CompanyVehicle> vehicles)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.CreateEndorsement(companyPolicy, vehicles);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error creando endoso", ex);
            }
        }

        /// <summary>
        /// Polizas asociadas a individual
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public List<CompanyVehicle> GetCompanyVehiclesByIndividualId(int individualId)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetVehiclesByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public void CreateRisk(CompanyVehicle companyVehicle)
        {
            try
            {
                new VehicleDAO().CreateRisk(companyVehicle);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, "ErrorCreateCompanyVehicle"), ex);
            }
        }

        /// <summary>
        /// Obtener Riesgo
        /// </summary>
        /// <param name="riskId">Id Riesgo</param>
        /// <returns>Riesgo</returns>
        public CompanyVehicle GetCompanyVehicleByRiskId(int riskId)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetCompanyVehicleByRiskId(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyVehicle GetVehicleByRiskId(int riskId)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetCompanyRiskVehicleByRiskId(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyVehicle> GetCompanyRisksVehicleByInsuredId(int insuredId)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetCompanyRisksVehicleByInsuredId(insuredId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #region Metodos Migrados
        #region Riesgo Company
        /// <summary>
        /// Gets the company risk by identifier.
        /// </summary>
        /// <param name="endorsementType">Type of the endorsement.</param>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public CompanyVehicle GetCompanyRiskById(EndorsementType endorsementType, int temporalId, int id)
        {
            try
            {
                CompanyVehicle vehicle = this.GetCompanyVehicleByRiskId(id);

                if (vehicle != null)
                {
                    if (vehicle.Accesories != null && vehicle.Accesories.Any())
                    {
                        vehicle.Accesories.AsParallel().ForAll(x => x.OriginalAmount = x.Amount);
                    }
                    vehicle = this.GetRiskDescriptions(endorsementType, vehicle);

                    return vehicle;
                }
                else
                {
                    throw new BusinessException(Errors.NoRiskWasFound);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the risk descriptions.
        /// </summary>
        /// <param name="endorsementType">Type of the endorsement.</param>
        /// <param name="vehicle">The vehicle.</param>
        /// <returns></returns>
        private CompanyVehicle GetRiskDescriptions(EndorsementType endorsementType, CompanyVehicle vehicle)
        {
            try
            {
                switch (endorsementType)
                {
                    case EndorsementType.Emission:
                        vehicle = this.GetDataEmission(vehicle);
                        break;
                    default:
                        break;
                }

                return vehicle;
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message);
            }

        }


        /// <summary>
        /// Gets the data emission.
        /// </summary>
        /// <param name="vehicle">The vehicle.</param>
        /// <returns></returns>
        private CompanyVehicle GetDataEmission(CompanyVehicle vehicle)
        {
            try
            {
                if (vehicle.Risk.MainInsured != null && vehicle.Risk.MainInsured.IdentificationDocument == null)
                {
                    if (vehicle.Risk.MainInsured.IndividualId != 0)
                    {
                        vehicle.Risk.MainInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(vehicle.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, vehicle.Risk.MainInsured.CustomerType);
                        vehicle.Risk.MainInsured.Name = vehicle.Risk.MainInsured.Surname + " " + (string.IsNullOrEmpty(vehicle.Risk.MainInsured.SecondSurname) ? "" : vehicle.Risk.MainInsured.SecondSurname + " ") + vehicle.Risk.MainInsured.Name;
                    }
                    else
                    {
                        throw new Exception(Errors.ErrorInsuredMain);
                    }
                }

                if (vehicle.Risk.Beneficiaries == null && vehicle.Risk.Beneficiaries?[0].IdentificationDocument == null)
                {
                    TP.Parallel.ForEach(vehicle.Risk?.Beneficiaries, item =>
                    {
                        Beneficiary beneficiary = new Beneficiary();
                        beneficiary = DelegateService.underwritingService.GetBeneficiariesByDescription(item.IndividualId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
                        item.IdentificationDocument = beneficiary.IdentificationDocument;
                        item.Name = beneficiary.Name;
                    });
                }
                vehicle?.Risk?.Coverages?.AsParallel().ForAll(x =>
                {
                    if (Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(x.CoverStatus.Value)) == null)
                    {

                        x.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(x.CoverStatus.Value);
                    }
                    else
                    {
                        x.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(x.CoverStatus.Value));
                    }

                }
                );
                if (vehicle.Accesories != null && vehicle.Accesories.Count > 0)
                {
                    List<Accessory> accessories = new List<Accessory>();
                    accessories = this.GetAccessories();
                    vehicle.Accesories.AsParallel().ForAll(x => x.Description = accessories.FirstOrDefault(z => z.Id == x.Id).Description);
                }

                return vehicle;
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message);
            }

        }


        /// <summary>
        /// Gets the company premium.
        /// </summary>
        /// <param name="policyId">The policy identifier.</param>
        /// <param name="vehicle">The vehicle.</param>
        /// <returns></returns>
        public CompanyVehicle GetCompanyPremium(int policyId, CompanyVehicle vehicle)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(policyId, false);
                policy.IsPersisted = true;
                vehicle.Risk.Policy = policy;
                vehicle.ActualDateMovement = DateTime.Now;
                vehicle = this.QuotateVehicle(vehicle, true, true, 0);
                vehicle?.Risk?.Coverages.AsParallel().ForAll(x =>
                {
                    if (Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(x?.CoverStatus.Value)) == null)
                    {

                        x.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(x?.CoverStatus.Value);
                    }
                    else
                    {
                        x.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(x?.CoverStatus.Value));
                    }

                });
                return vehicle;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.GetBaseException().Message);
            }

        }

        /// <summary>
        /// Saves the company risk.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="vehicle">The vehicle.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// </exception>
        public CompanyVehicle SaveCompanyRisk(int temporalId, CompanyVehicle vehicle)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                if (policy != null)
                {
                    List<CompanyVehicle> vehicles = this.GetCompanyVehiclesByTemporalId(temporalId);
                    if (policy != null && policy.PolicyOrigin == PolicyOrigin.Collective && policy.Endorsement.EndorsementType == EndorsementType.Modification && vehicles?.FirstOrDefault(x => x.Risk.Id == vehicle.Risk.Id)?.Risk.Status == RiskStatusType.Included)
                        vehicle.Alerts = this.ExistsRisk(vehicles, vehicle.Risk.Id, vehicle.LicensePlate, vehicle.EngineSerial, vehicle.ChassisSerial, policy, true);
                    else
                        vehicle.Alerts = this.ExistsRisk(vehicles, vehicle.Risk.Id, vehicle.LicensePlate, vehicle.EngineSerial, vehicle.ChassisSerial, policy);

                    if (!vehicle.Alerts.Any() || (policy.TemporalType == TemporalType.TempQuotation))
                    {
                        if (vehicle != null && vehicle.Risk != null)
                        {
                            vehicle.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                            vehicle.Risk.LimitRc = DelegateService.underwritingService.GetCompanyLimitRcById(vehicle.Risk.LimitRc.Id);

                            vehicle.Risk.CoveredRiskType = policy.Product.CoveredRisk.CoveredRiskType;
                            vehicle.Risk.Policy = policy;

                            if (vehicle.Risk?.Id == 0)
                            {
                                int countRiskNumber = DelegateService.underwritingService.GetEndorsementRiskCount(policy.Endorsement.PolicyId, (EndorsementType)policy.Endorsement.EndorsementType);//agregar el metodo creado GetEndorsementRiskCount
                                if (countRiskNumber != 0)
                                {
                                    vehicle.Risk.Number = countRiskNumber + 1;
                                }
                                else
                                {
                                    if (vehicles?.Count < 1)
                                    {
                                        vehicle.Risk.Number = 1;
                                    }
                                    else
                                    {
                                        vehicle.Risk.Number = vehicles.OrderByDescending(x => x.Risk.Number).FirstOrDefault().Risk.Number + 1;
                                    }
                                }

                                if (policy.Endorsement.EndorsementType == EndorsementType.Modification)
                                {
                                    vehicle.Risk.Status = RiskStatusType.Included;
                                }
                                if (vehicles.Count < policy.Product.CoveredRisk.MaxRiskQuantity)
                                {
                                    if (policy.DefaultBeneficiaries != null && policy.DefaultBeneficiaries != null && policy.DefaultBeneficiaries.Count > 0)
                                    {
                                        vehicle.Risk.Beneficiaries = policy.DefaultBeneficiaries;
                                    }
                                    else
                                    {
                                        ModelAssembler.CreateMapCompanyInsured();
                                        vehicle.Risk.Beneficiaries.Add(ModelAssembler.CreateBeneficiaryFromInsured(vehicle.Risk.MainInsured));
                                    }
                                }
                                else
                                {
                                    throw new BusinessException(Errors.ProductNotAddingMoreRisks);
                                }
                            }
                            else
                            {
                                switch (policy.Endorsement.EndorsementType.Value)
                                {
                                    case EndorsementType.Emission:
                                    case EndorsementType.Renewal:
                                        vehicle = SetDataEmission(vehicle);
                                        break;
                                    case EndorsementType.Modification:
                                        vehicle = SetDataModification(vehicle);
                                        break;
                                    default:
                                        break;
                                }
                            }

                            vehicle = CreateVehicleTemporal(vehicle, false);

                            return vehicle;
                        }
                        else
                        {
                            throw new BusinessException(Errors.ErrorSaveRiskVehicle);
                        }
                    }
                    else
                    {
                        throw new BusinessException(String.Join(" : ", vehicle.Alerts));
                    }
                }
                else
                {
                    throw new BusinessException(Errors.ErrorTemporalNotFound);
                }

            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "BusinessException")
                {
                    throw new BusinessException(ex.Message);
                }
                else
                {
                    throw new Exception(Errors.ErrorSaveRiskVehicle);
                }
            }
        }


        public bool DeleteCompanyRisk(int policyId, int id)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(policyId, false);
                bool result = false;

                if (policy != null)
                {
                    CompanyPolicy vehiclePolicy = new CompanyPolicy();
                    CompanyVehicle vehicle = GetCompanyVehicleByRiskId(id);
                    if (vehicle != null)
                    {
                        if (vehicle.Risk?.Status == RiskStatusType.Original || vehicle.Risk?.Status == RiskStatusType.Included)
                        {
                            result = DelegateService.utilitiesServiceCore.DeletePendingOperation(vehicle.Risk.Id);
                            DelegateService.underwritingService.DeleteRisk(vehicle.Risk.Id);
                            ReindexingRisks(policyId, id);
                        }
                        else
                        {
                            vehicle.Risk.Status = RiskStatusType.Excluded;
                            vehicle.Accesories?.AsParallel().ForAll(x => x.Status = (int)CoverageStatusType.Excluded);
                            vehicle.Risk.Description = string.Format("{0} ({1})", vehicle.LicensePlate, Errors.ResourceManager.GetString(EnumHelper.GetItemName<RiskStatusType>(vehicle.Risk.Status)));
                            vehicle.Risk.IsPersisted = true;
                            vehicle.Risk.Policy = vehiclePolicy;
                            vehicle.Risk.Policy.Id = policy.Id;
                            vehicle = QuotateVehicle(vehicle, false, false, policy.Endorsement.AppRelation);
                            vehicle?.Risk?.Coverages.AsParallel().ForAll(x =>
                            {
                                if (Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(x?.CoverStatus.Value)) == null)
                                {

                                    x.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(x?.CoverStatus.Value);
                                }
                                else
                                {
                                    x.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(x?.CoverStatus.Value));
                                }
                                x.CurrentFrom = policy.CurrentFrom;
                                x.CurrentTo = policy.CurrentTo;
                            });
                            vehicle = CreateVehicleTemporal(vehicle, false);
                            result = true;
                        }
                    }
                    else
                    {
                        throw new BusinessException(Errors.ErrorRiskEmpty);
                    }

                    if (result)
                    {
                        return true;
                    }
                    else
                    {
                        throw new BusinessException(Errors.ErrorDeleteRisk);
                    }
                }
                else
                {
                    throw new BusinessException(Errors.ErrorDeleteRisk);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Reindexings the risks.
        /// </summary>
        /// <param name="policyId">The policy identifier.</param>
        /// <param name="id">The identifier.</param>
        private void ReindexingRisks(int policyId, int id)
        {
            List<CompanyVehicle> vehicles = this.GetCompanyVehiclesByTemporalId(policyId);//.Where(x => x.Risk.Status == RiskStatusType.Included || x.Risk.Status == RiskStatusType.Original).ToList();
            if (vehicles?.Count() > 0)
            {
                List<CompanyVehicle> vehiclesInclude = vehicles.Where(x => x.Risk.Status == RiskStatusType.Included || x.Risk.Status == RiskStatusType.Original).ToList();
                if (vehiclesInclude?.Count() > 0)
                {
                    List<CompanyVehicle> riskVehicles = vehicles.Where(x => x.Risk.Status != RiskStatusType.Included && x.Risk.Status != RiskStatusType.Original).ToList();
                    int CountInitial = riskVehicles != null && riskVehicles.Any() ? riskVehicles.Max(x => x.Risk.Number) : 1;
                    List<object> z = new List<object>();
                    foreach (CompanyVehicle vehicle in vehiclesInclude)
                    {
                        z.Add(new { Id = vehicle.Risk.Id, Number = CountInitial });
                        vehicle.Risk.Number = CountInitial;
                        CreateVehicleTemporal(vehicle, false);
                        CountInitial += CountInitial;
                    }
                }
            }
        }

        private CompanyVehicle SetDataEmission(CompanyVehicle vehicle)
        {
            try
            {
                CompanyVehicle vehicleOld = GetCompanyVehicleByRiskId(vehicle.Risk.Id);

                vehicle.Risk.Beneficiaries = vehicleOld.Risk.Beneficiaries;
                vehicle.Risk.Text = vehicleOld.Risk.Text;
                vehicle.Risk.Number = vehicleOld.Risk.Number;
                vehicle.Risk.SecondInsured = vehicleOld.Risk.SecondInsured;
                vehicle.Risk.Clauses = vehicleOld.Risk.Clauses;

                return vehicle;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

        }

        private CompanyVehicle SetDataModification(CompanyVehicle vehicle)
        {
            try
            {
                CompanyVehicle vehicleOld = GetCompanyVehicleByRiskId(vehicle.Risk.Id);

                vehicle.Risk.RiskId = vehicleOld.Risk.RiskId;
                vehicle.Risk.Number = vehicleOld.Risk.Number;
                vehicle.Risk.Description = vehicleOld.Risk.Description;
                //Placa TL tiene formato 2 letras y 4 numeros
                if (!vehicleOld.LicensePlate.StartsWith("TL") && Regex.Match(vehicleOld.LicensePlate, @"\d+").Value.Length != 4)
                {
                    vehicle.LicensePlate = vehicleOld.LicensePlate;
                }
                //vehicle.EngineSerial = vehicleOld.EngineSerial;
                //vehicle.ChassisSerial = vehicleOld.ChassisSerial;
                vehicle.Risk.Beneficiaries = vehicleOld.Risk.Beneficiaries;
                vehicle.Risk.Text = vehicleOld.Risk.Text;
                vehicle.Risk.Clauses = vehicleOld.Risk.Clauses;
                vehicle.Fasecolda = vehicleOld.Fasecolda;
                vehicle.Version.Body = vehicleOld.Version.Body;
                vehicle.NewPrice = vehicleOld.NewPrice;
                vehicle.OriginalPrice = vehicleOld.OriginalPrice;
                vehicle.Risk.Status = vehicleOld.Risk.Status;
                vehicle.Risk.OriginalStatus = vehicleOld.Risk.OriginalStatus;

                if (vehicle.Risk.Status != RiskStatusType.Included && vehicle.Risk.Status != RiskStatusType.Excluded)
                {
                    vehicle.Risk.Status = RiskStatusType.Modified;
                }

                return vehicle;
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message);
            }

        }


        /// <summary>
        /// Exists  the risk of Authorization.
        /// </summary>
        /// <param name="temporalId">Id Temporal.</param>
        /// <returns></returns>
        public List<string> ExistsRiskAuthorization(int temporalId)
        {
            try
            {
                List<string> message = null;

                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);

                if (policy != null)
                {
                    List<CompanyVehicle> vehicles = GetCompanyVehiclesByTemporalId(temporalId);
                    if (vehicles?.Count() > 0)
                    {
                        foreach (CompanyVehicle vehicle in vehicles)
                        {
                            vehicle.Alerts = ExistsRisk(vehicles, vehicle.Risk.Id, vehicle.LicensePlate, vehicle.EngineSerial, vehicle.ChassisSerial, policy);
                            if (vehicle.Alerts.Any())
                            {
                                message = message ?? new List<string>();
                                message.AddRange(vehicle.Alerts);
                            }
                        }
                    }
                }

                return message;
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "BusinessException")
                {
                    throw new BusinessException(ex.Message);
                }
                else
                {
                    throw new Exception(Errors.ErrorSaveRiskVehicle);
                }
            }
        }

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
        public List<string> ExistsRisk(List<CompanyVehicle> vehicles, int riskId, string licensePlate, string engineNumber, string chassisNumber, CompanyPolicy policy, Boolean riskIncl= false)
        {
            try
            {
                var endorsementType = policy.Endorsement.EndorsementType.Value;
                ConcurrentBag<string> messages = new ConcurrentBag<string>();

                if (endorsementType == EndorsementType.Emission || endorsementType == EndorsementType.Renewal || endorsementType == EndorsementType.Modification)
                {
                    VehicleDAO CompanyVehicle = new VehicleDAO();
                    string message = "";

                    if (policy != null && policy.PolicyOrigin == PolicyOrigin.Collective && policy.Endorsement.EndorsementType == EndorsementType.Modification   && riskIncl)
                        message = CompanyVehicle.ExistsRiskByLicensePlateEngineNumberChassisNumberCompany(licensePlate, engineNumber, chassisNumber, 0, policy.CurrentFrom);
                    else
                        message = CompanyVehicle.ExistsRiskByLicensePlateEngineNumberChassisNumberCompany(licensePlate, engineNumber, chassisNumber, policy.Endorsement.PolicyId, policy.CurrentFrom);

                    if (message != "")
                    {
                        messages.Add(message);

                    }
                }
                var vehicleEngine = (riskId > 0) ? vehicles.SingleOrDefault(x => x.EngineSerial == engineNumber && x.Risk.Id != riskId) : vehicles.SingleOrDefault(x => x.EngineSerial == engineNumber);
                if (vehicleEngine != null)
                {
                    messages.Add(CreateMessageExist(vehicleEngine, policy));
                    return messages.ToList();
                }

                var vehiclePlate = (riskId > 0) ? vehicles.SingleOrDefault(x => x.LicensePlate == licensePlate && x.Risk.Id != riskId) : vehicles.SingleOrDefault(x => x.LicensePlate == licensePlate);
                if (vehiclePlate != null)
                {
                    messages.Add(CreateMessageExist(vehiclePlate, policy));
                    return messages.ToList();
                }

                var vehicleChasis = (riskId > 0) ? vehicles.SingleOrDefault(x => x.ChassisSerial == chassisNumber && x.Risk.Id != riskId) : vehicles.SingleOrDefault(x => x.ChassisSerial == chassisNumber);
                if (vehicleChasis != null)
                {
                    messages.Add(CreateMessageExist(vehicleChasis, policy));
                    return messages.ToList();
                }
                return messages.ToList();
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message);
            }

        }

        public string CreateMessageExist(CompanyVehicle vehicle, CompanyPolicy policy)
        {
            return $@"**El vehiculo ya posee una poliza vigente. 

                       Vehiculo:
                       Placa = {vehicle.LicensePlate} 
                       Número de chasis = {vehicle.ChassisSerial} 
                       Número de motor = {vehicle.EngineSerial}
                       Sucursal = {policy.Branch.Id}
                       Producto = {policy.Product.Description}
                       Nro Poliza = {policy.DocumentNumber} 
                       Fecha de inicio = {policy.CurrentFrom } 
                       Fecha de fin = {policy.CurrentTo}.";
        }

        /// <summary>
        /// Updates the company risks.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <returns></returns>
        public CompanyPolicy UpdateQuotationRisk(int temporalId, bool isMassive)
        {
            try
            {
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                if (companyPolicy != null)
                {
                    List<CompanyVehicle> companyVehicles = GetCompanyVehiclesByTemporalId(temporalId);

                    if (companyVehicles != null && companyVehicles.Any())
                    {
                        ConcurrentBag<string> log = new ConcurrentBag<string>();
                        Parallel.ForEach(companyVehicles, ParallelHelper.DebugParallelFor(), companyVehicle =>
                        {
                            try
                            {
                                companyVehicle.Risk.Policy = companyPolicy;
                                companyVehicle?.Risk?.Coverages.AsParallel().ForAll(x =>
                                {
                                    x.CurrentTo = companyPolicy.CurrentTo;
                                    x.CurrentFrom = companyPolicy.CurrentFrom;
                                });
                                CreateVehicleTemporal(companyVehicle, false);
                            }
                            catch (Exception ex)
                            {
                                log.Add(ex.Message);
                            }

                        });
                        if (log?.Count() > 0)
                        {
                            throw new Exception(Errors.ErrorQuotate);
                        }
                        companyPolicy = DelegateService.underwritingService.UpdatePolicyComponents(companyPolicy.Id);
                        List<CompanyRiskInsured> risks = new List<CompanyRiskInsured>();
                        foreach (CompanyVehicle item in companyVehicles)
                        {
                            CompanyRiskInsured risk = new CompanyRiskInsured();
                            risk.Beneficiaries = new List<CompanyBeneficiary>();
                            foreach (var beneficiary in item.Risk.Beneficiaries)
                            {
                                if (companyPolicy.TemporalType == TemporalType.Policy && beneficiary.CustomerType == CustomerType.Prospect)
                                {
                                    CompanyBeneficiary companyBeneficiary = new CompanyBeneficiary()
                                    {
                                        BeneficiaryType = beneficiary.BeneficiaryType,
                                        CompanyName = companyPolicy.Holder.CompanyName,
                                        DeclinedDate = companyPolicy.Holder.DeclinedDate,
                                        ExtendedProperties = companyPolicy.Holder.ExtendedProperties,
                                        IdentificationDocument = companyPolicy.Holder.IdentificationDocument,
                                        CodeBeneficiary = beneficiary.CodeBeneficiary,
                                        CustomerType = companyPolicy.Holder.CustomerType,
                                        IndividualId = companyPolicy.Holder.IndividualId,
                                        IndividualType = companyPolicy.Holder.IndividualType,
                                        Name = companyPolicy.Holder.Name,
                                        BeneficiaryTypeDescription = beneficiary.BeneficiaryTypeDescription,
                                        Participation = beneficiary.Participation
                                    };
                                    risk.Beneficiaries.Add(companyBeneficiary);
                                    item.Risk.Beneficiaries = risk.Beneficiaries;
                                }
                                else
                                {
                                    risk.Beneficiaries.Add(beneficiary);
                                }
                            }

                            if (companyPolicy.TemporalType == TemporalType.Policy && item.Risk.MainInsured.CustomerType == CustomerType.Prospect)
                            {
                                CompanyIssuanceInsured
                                      Insured = new CompanyIssuanceInsured()
                                      {
                                          BirthDate = companyPolicy.Holder.BirthDate,
                                          CompanyName = companyPolicy.Holder.CompanyName,
                                          CustomerType = companyPolicy.Holder.CustomerType,
                                          CustomerTypeDescription = companyPolicy.Holder.CustomerTypeDescription,
                                          DeclinedDate = companyPolicy.Holder.DeclinedDate,
                                          EconomicActivity = companyPolicy.Holder.EconomicActivity,
                                          ExtendedProperties = companyPolicy.ExtendedProperties,
                                          IdentificationDocument = companyPolicy.Holder.IdentificationDocument,
                                          PaymentMethod = companyPolicy.Holder.PaymentMethod,
                                          Name = companyPolicy.Holder.Name,
                                          IndividualId = companyPolicy.Holder.IndividualId,
                                          IndividualType = companyPolicy.Holder.IndividualType,
                                          InsuredId = companyPolicy.Holder.InsuredId,
                                          Gender = companyPolicy.Holder.Gender,
                                          SecondSurname = companyPolicy.Holder.SecondSurname,
                                          OwnerRoleCode = companyPolicy.Holder.OwnerRoleCode,
                                          Surname = companyPolicy.Holder.Surname
                                      };
                                risk.Insured = Insured;
                                risks.Add(risk);
                                item.Risk.MainInsured = Insured;
                            }
                            else
                            {
                                risk.Insured = item.Risk.MainInsured;
                                risks.Add(risk);
                            }
                            CreateVehicleTemporal(item, false);
                        }
                        companyPolicy.Summary.RisksInsured = risks;
                        return companyPolicy;
                    }
                    else
                    {
                        return companyPolicy;
                    }
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorUpdatePolicy);
            }
        }

        /// <summary>
        /// Updates the company risks.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <returns></returns>
        public CompanyPolicy UpdateCompanyRisks(int temporalId, bool isMassive)
        {
            try
            {
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                if (companyPolicy != null)
                {
                    List<CompanyVehicle> companyVehicles = GetCompanyVehiclesByTemporalId(temporalId);

                    if (companyVehicles != null && companyVehicles.Any())
                    {
                        ConcurrentBag<string> log = new ConcurrentBag<string>();
                        Parallel.ForEach(companyVehicles, ParallelHelper.DebugParallelFor(), companyVehicle =>
                        {
                            try
                            {
                                companyVehicle.Risk.Policy = companyPolicy;
                                companyVehicle?.Risk?.Coverages.AsParallel().ForAll(x =>
                                {
                                    x.CurrentTo = companyPolicy.CurrentTo;
                                    x.CurrentFrom = companyPolicy.CurrentFrom;
                                });
                                companyVehicle.Risk.DynamicProperties = companyVehicle.Risk?.DynamicProperties?.Where(y => y.QuestionId != null)?.ToList();
                            }
                            catch (Exception ex)
                            {
                                log.Add(ex.Message);
                            }

                        });
                        if (log?.Count() > 0)
                        {
                            throw new Exception(Errors.ErrorQuotate);
                        }
                        companyPolicy = DelegateService.underwritingService.UpdatePolicyComponents(companyPolicy.Id);
						List<CompanyRiskInsured> risks = new List<CompanyRiskInsured>();
                        foreach (CompanyVehicle item in companyVehicles)
                        {
                            CompanyRiskInsured risk = new CompanyRiskInsured();
                            risk.Beneficiaries = new List<CompanyBeneficiary>();
                            foreach (var beneficiary in item.Risk.Beneficiaries)
                            {
                                if (companyPolicy.TemporalType == TemporalType.Policy && beneficiary.CustomerType == CustomerType.Prospect)
                                {
                                    CompanyBeneficiary companyBeneficiary = new CompanyBeneficiary()
                                    {
                                        BeneficiaryType = beneficiary.BeneficiaryType,
                                        CompanyName = companyPolicy.Holder.CompanyName,
                                        DeclinedDate = companyPolicy.Holder.DeclinedDate,
                                        ExtendedProperties = companyPolicy.Holder.ExtendedProperties,
                                        IdentificationDocument = companyPolicy.Holder.IdentificationDocument,
                                        CodeBeneficiary = beneficiary.CodeBeneficiary,
                                        CustomerType = companyPolicy.Holder.CustomerType,
                                        IndividualId = companyPolicy.Holder.IndividualId,
                                        IndividualType = companyPolicy.Holder.IndividualType,
                                        Name = companyPolicy.Holder.Name,
                                        BeneficiaryTypeDescription = beneficiary.BeneficiaryTypeDescription,
                                        Participation = beneficiary.Participation
                                    };
                                    risk.Beneficiaries.Add(companyBeneficiary);
                                }
                                else
                                {
                                    risk.Beneficiaries.Add(beneficiary);
                                }
                            }

                            if (companyPolicy.TemporalType == TemporalType.Policy && item.Risk.MainInsured.CustomerType == CustomerType.Prospect)
                            {
                                CompanyIssuanceInsured
                                      Insured = new CompanyIssuanceInsured()
                                      {
                                          BirthDate = companyPolicy.Holder.BirthDate,
                                          CompanyName = companyPolicy.Holder.CompanyName,
                                          CustomerType = companyPolicy.Holder.CustomerType,
                                          CustomerTypeDescription = companyPolicy.Holder.CustomerTypeDescription,
                                          DeclinedDate = companyPolicy.Holder.DeclinedDate,
                                          EconomicActivity = companyPolicy.Holder.EconomicActivity,
                                          ExtendedProperties = companyPolicy.ExtendedProperties,
                                          IdentificationDocument = companyPolicy.Holder.IdentificationDocument,
                                          PaymentMethod = companyPolicy.Holder.PaymentMethod,
                                          Name = companyPolicy.Holder.Name,
                                          IndividualId = companyPolicy.Holder.IndividualId,
                                          IndividualType = companyPolicy.Holder.IndividualType,
                                          InsuredId = companyPolicy.Holder.InsuredId,
                                          Gender = companyPolicy.Holder.Gender,
                                          SecondSurname = companyPolicy.Holder.SecondSurname,
                                          OwnerRoleCode = companyPolicy.Holder.OwnerRoleCode,
                                          Surname = companyPolicy.Holder.Surname
                                      };
                                risk.Insured = Insured;
                                risks.Add(risk);
                            }
                            else
                            {
                                risk.Insured = item.Risk.MainInsured;
                                risks.Add(risk);
                            }
                        }
                        companyPolicy.Summary.RisksInsured = risks;
                        return companyPolicy;
                    }
                    else
                    {
                        return companyPolicy;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorUpdatePolicy);
            }
        }

        public string ExistCompanyRiskByTemporalId(int tempId)
        {
            var message = "";
            CompanyPolicy companyVehiclePolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(tempId, false);
            if (companyVehiclePolicy != null)
            {
                List<CompanyVehicle> companyVehicles = GetCompanyVehiclesByTemporalId(tempId);
                if (companyVehicles != null && companyVehicles.Count > 0)
                {
                    companyVehicles.ForEach(risk =>
                    {
                        if (!string.IsNullOrEmpty(risk.EngineSerial) && !string.IsNullOrEmpty(risk?.ChassisSerial))
                        {
                            message = ExistsRiskByLicensePlateEngineNumberChassisNumberProductId(risk.LicensePlate, risk.EngineSerial, risk.ChassisSerial, companyVehiclePolicy.Product.Id, companyVehiclePolicy.Endorsement.Id, companyVehiclePolicy.CurrentFrom);
                            if (message != "")
                            {
                                throw new BusinessException(message);
                            }
                        }
                        else
                        {
                            throw new BusinessException(Errors.WithoutEngineChassis);
                        }
                    });
                }
                return message;
            }
            else
            {
                throw new BusinessException(Errors.ErrorTemporalNotFound);
            }
        }
        #endregion Riesgo Company
        #region Coberturas
        /// <summary>
        /// Gets the company coverages by product identifier group coverage identifier.
        /// </summary>
        /// <param name="policyId">The policy identifier.</param>
        /// <param name="groupCoverageId">The group coverage identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// </exception>
        public List<CompanyCoverage> GetCompanyCoveragesByProductIdGroupCoverageId(int policyId, int groupCoverageId)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(policyId, false);

                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(policy.Product.Id, groupCoverageId, policy.Prefix.Id);
                if (coverages != null && coverages.Any())
                {
                    coverages.AsParallel().ForAll(x =>
                    {
                        x.EndorsementType = policy.Endorsement.EndorsementType;
                        x.CurrentFrom = Convert.ToDateTime(policy.CurrentFrom);
                        x.CurrentTo = Convert.ToDateTime(policy.CurrentTo);
                        if (policy.Endorsement.EndorsementType == EndorsementType.Modification)
                        {
                            if (Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Included)) == null)
                            {

                                x.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Included);
                            }
                            else
                            {
                                x.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Included));
                            }

                            x.CoverStatus = CoverageStatusType.Included;
                        }
                        else
                        {
                            if (Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Original)) == null)
                            {

                                x.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Original);
                            }
                            else
                            {
                                x.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Original));
                            }

                        }
                    });

                    coverages = coverages?.Where(x => x.IsSelected == true).ToList();

                    return coverages;
                }
                else
                {
                    throw new BusinessException(Errors.ErrorSearchCoverages);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Saves the company coverages.
        /// </summary>
        /// <param name="policyId">The policy identifier.</param>
        /// <param name="riskId">The risk identifier.</param>
        /// <param name="coverages">The coverages.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Boolean SaveCompanyCoverages(int policyId, int riskId, List<CompanyCoverage> coverages)
        {
            CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(policyId, false);
            CompanyVehicle vehicle = GetCompanyVehicleByRiskId(riskId);
            if (policy != null && vehicle != null && coverages != null && coverages.Any())
            {
                vehicle.Risk.IsPersisted = true;
                vehicle.Risk.Coverages = coverages;
                vehicle.Risk.Policy = policy;
                vehicle = QuotateVehicle(vehicle, false, true, policy.Endorsement.AppRelation, false);
                vehicle = CreateVehicleTemporal(vehicle, false);
                return true;
            }

            else
            {
                throw new BusinessException(Errors.NoExistTemporaryNoHaveCoverages);
            }
        }

        /// <summary>
        /// Excludes the coverage.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="riskId">The risk identifier.</param>
        /// <param name="riskCoverageId">The risk coverage identifier.</param>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        public CompanyCoverage ExcludeCompanyCoverage(int temporalId, int riskId, int riskCoverageId, string description)
        {
            CompanyPolicy vehiclePolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
            CompanyCoverage coverage;

            if (vehiclePolicy != null)
            {
                CompanyVehicle vehicle = GetCompanyVehicleByRiskId(riskId);
                if (vehicle != null)
                {
                    String coverStatusName = String.Empty;
                    if (Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Excluded)) == null)
                    {
                        coverStatusName = EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Excluded);
                    }
                    else
                    {
                        coverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Excluded));
                    }
                    vehicle.Risk.Policy = vehiclePolicy;
                    coverage = DelegateService.underwritingService.GetCompanyCoverageByRiskCoverageId(riskCoverageId);
                    coverage.Description = description;
                    coverage.SubLineBusiness = vehicle.Risk.Coverages.First(x => x.RiskCoverageId == riskCoverageId).SubLineBusiness;
                    coverage.Rate = coverage.Rate * -1;
                    coverage.AccumulatedPremiumAmount = 0;
                    coverage.EndorsementType = vehiclePolicy.Endorsement.EndorsementType;
                    coverage.CurrentFrom = vehiclePolicy.CurrentFrom;
                    coverage.CoverStatus = CoverageStatusType.Excluded;
                    coverage = QuotateCompanyCoverage(vehicle, coverage, false, false);
                    coverage.CoverStatusName = coverStatusName;
                    coverage.LimitAmount = 0;
                    coverage.SubLimitAmount = 0;
                    coverage.EndorsementLimitAmount = coverage.EndorsementLimitAmount * -1;
                    coverage.EndorsementSublimitAmount = coverage.EndorsementSublimitAmount * -1;
                    return coverage;
                }
                else
                {
                    throw new BusinessException(Errors.NoExistRisk);
                }
            }
            else
            {
                throw new BusinessException(Errors.ErrorTemporalNotFound);
            }
        }
        #endregion
        #region personas
        /// <summary>
        /// Converts the prospect to insured.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="individualId">The individual identifier.</param>
        /// <param name="documentNumber">The document number.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public Boolean ConvertProspectToInsured(int temporalId, int individualId, string documentNumber)
        {
            try
            {
                DelegateService.underwritingService.ConvertProspectToHolder(temporalId, individualId, documentNumber);

                List<CompanyVehicle> companyVehicles = GetCompanyVehiclesByTemporalId(temporalId);

                if (companyVehicles.Count > 0)
                {
                    foreach (CompanyVehicle vehicle in companyVehicles)
                    {

                        CompanyRisk risk = DelegateService.underwritingService.ConvertProspectToInsured(vehicle.Risk, individualId, documentNumber);

                        vehicle.Risk.Beneficiaries = risk.Beneficiaries;

                        CreateVehicleTemporal(vehicle, false);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorConvertingProspectIntoIndividual);
            }
        }
        #endregion personas
        #region texto riesgo

        /// <summary>
        /// Saves the company texts.
        /// </summary>
        /// <param name="riskId">The risk identifier.</param>
        /// <param name="companyText">The company text.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException">
        /// </exception>
        public CompanyText SaveCompanyTexts(int riskId, CompanyText companyText)
        {
            try
            {
                CompanyVehicle risk = GetCompanyVehicleByRiskId(riskId);

                if (risk?.Risk?.Id > 0)
                {
                    risk.Risk.Text = companyText;
                    risk = CreateVehicleTemporal(risk, false);
                    if (risk != null)
                    {
                        return risk.Risk.Text;
                    }
                    else
                    {
                        throw new BusinessException(Errors.ErrorSaveText);
                    }
                }
                else
                {
                    throw new BusinessException(Errors.NoExistRisk);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        #endregion
        #region poliza riego
        /// <summary>
        /// Creates the company policy.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="temporalType">Type of the temporal.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public CompanyPolicyResult CreateCompanyPolicy(int temporalId, int temporalType, bool clearPolicies)
        {

            try
            {
                CompanyPolicyResult companyPolicyResult = new CompanyPolicyResult
                {
                    IsError = false,
                    Errors = new List<ErrorBase>()
                };
                string message = string.Empty;
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                if (policy.PaymentPlan.PremiumFinance != null && (policy.Endorsement.EndorsementType ==  EndorsementType.Modification || policy.Endorsement.EndorsementType == EndorsementType.EffectiveExtension ) )
                {
                    policy.PaymentPlan = DelegateService.underwritingService.GetDefaultPaymentPlan(policy.Product.Id);
                    CompanyPolicy policyQuotas = new CompanyPolicy
                    {
                        PaymentPlan = new CompanyPaymentPlan { Id = 1 },
                        Summary = policy.Summary,
                        CurrentFrom = policy.CurrentFrom,
                        CurrentTo = policy.CurrentTo,
                        IssueDate = policy.IssueDate
                    };
                    ComponentValueDTO componentValue = ModelAssembler.CreateCompanyComponentValueDTO(policy.Summary);
                    List<Quota> quotas = DelegateService.underwritingService.CalculateQuotas(new QuotaFilterDTO { PlanId = policy.PaymentPlan.Id, CurrentFrom = policy.CurrentFrom, IssueDate = policy.IssueDate, ComponentValueDTO= componentValue });                   
                    policy.PaymentPlan.Quotas = quotas;
                }
                policy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Today);
                policy.Errors = new List<ErrorBase>();
                if (policy == null)
                {
                    companyPolicyResult.IsError = true;
                    companyPolicyResult.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorTemporalNotFound });

                }
                else
                {

                    if (temporalType != TempType.Quotation)
                    {
                        ValidateHolder(ref policy);
                    }
                    if (policy.Errors != null && !policy.Errors.Any() && policy.Product.CoveredRisk != null)
                    {
                        List<CompanyVehicle> vehicles = GetCompanyVehiclesByTemporalId(policy.Id);

                        if (vehicles != null && vehicles.Any())
                        {
                            if (clearPolicies)
                            {
                                policy.InfringementPolicies.Clear();
                                vehicles.ForEach(x => x.Risk.InfringementPolicies.Clear());
                            }



                            policy = CreateEndorsement(policy, vehicles);
                            //se agrega la validación para el caso en que tenga un evento de autorzación
                            if (policy?.InfringementPolicies?.Count == 0)
                            {
                                //if (policy.Endorsement.PolicyId != 0 && policy.Endorsement.Id != 0)

                                DelegateService.underwritingService.SaveTextLarge(policy.Endorsement.PolicyId, policy.Endorsement.Id);
                            }
                        }
                        else
                        {
                            throw new ArgumentException(Errors.NoExistRisk);
                        }

                        if (temporalType != TempType.Quotation)
                        {
                            if (policy.InfringementPolicies.Any())
                            {
                                companyPolicyResult.TemporalId = policy.Id;
                                companyPolicyResult.InfringementPolicies = policy.InfringementPolicies;
                            }
                            else
                            {
                                if (policy.Endorsement.EndorsementType == EndorsementType.Emission)
                                {
                                    if (policy.PaymentPlan.PremiumFinance == null)
                                    {
                                        companyPolicyResult.Message = string.Format(Errors.PolicyNumber, policy.DocumentNumber);
                                    }
                                    else
                                    {
                                        companyPolicyResult.Message = string.Format(Errors.PolicyNumber, policy.DocumentNumber +
                                       " \n\r " + Errors.labelPay + policy.PaymentPlan.PremiumFinance.PromissoryNoteNumCode +
                                       " \n\r " + Errors.labelUser + policy.UserId.ToString());

                                        companyPolicyResult.PromissoryNoteNumCode = policy.PaymentPlan.PremiumFinance.PromissoryNoteNumCode;
                                    }
                                }
                                else
                                {
                                    string additionalFinancing = "";

                                    if (policy.PaymentPlan.PremiumFinance != null)
                                    {
                                        companyPolicyResult.PromissoryNoteNumCode = policy.PaymentPlan.PremiumFinance.PromissoryNoteNumCode;
                                        additionalFinancing = string.Format(Errors.PromissoryNote, policy.PaymentPlan.PremiumFinance.PromissoryNoteNumCode, policy.User.UserId);
                                    }

                                    companyPolicyResult.Message = string.Format(Errors.EndorsementNumber, policy.DocumentNumber, policy.Endorsement.Number, policy.Endorsement.Id, additionalFinancing);
                                }

                                companyPolicyResult.DocumentNumber = policy.DocumentNumber;
                                companyPolicyResult.EndorsementId = policy.Endorsement.Id;
                                companyPolicyResult.EndorsementNumber = policy.Endorsement.Number;
                            }
                        }
                        else
                        {
                            companyPolicyResult.Message = string.Format(Errors.QuotationNumber, policy.Endorsement.QuotationId.ToString());
                            companyPolicyResult.DocumentNumber = Convert.ToDecimal(policy.Endorsement.QuotationId);
                        }
                    }
                    else
                    {
                        companyPolicyResult.IsError = true;
                        companyPolicyResult.Errors.Add(new ErrorBase { StateData = false, Error = string.Join(" - ", policy.Errors[0].Error) });

                    }

                }
                return companyPolicyResult;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message != string.Empty && ex.InnerException.Message.Contains("PK_RSK_BENEF"))
                {
                    throw new BusinessException(Errors.ErrorBeneficiary);
                }
                else if (ex.InnerException != null && ex.InnerException.Message != String.Empty)
                    throw new BusinessException(ex.InnerException.Message);
                else
                {
                    throw new BusinessException(Errors.ErrorCreatePolicy);
                }
            }

        }
        /// <summary>
        /// Validates the holder.
        /// </summary>
        /// <param name="policy">The policy.</param>
        public void ValidateHolder(ref CompanyPolicy policy)
        {
            if (policy.Holder != null)
            {
                if (policy.Holder.CustomerType == CustomerType.Prospect)
                {
                    policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorHolderNoInsuredRole });
                }
                else
                {
                    List<Holder> holders = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(policy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, policy.Holder.CustomerType);

                    if (holders != null && holders.Count == 1)
                    {
                        if (holders[0].InsuredId == 0)
                        {
                            policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorPolicyholderWithoutRol });
                        }
                        else if (holders[0]?.DeclinedDate > DateTime.MinValue)
                        {
                            policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorPolicyholderDisabled });
                        }
                    }
                    else
                    {
                        policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorConsultPolicyholder });
                    }

                    if (policy.Holder.PaymentMethod != null)
                    {
                        if (policy.Holder.PaymentMethod.Id == 0)
                        {
                            policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorPolicyholderDefaultPaymentPlan });
                        }
                    }

                    //Validación asegurado principal como prospecto
                    switch (policy.Product.CoveredRisk.CoveredRiskType)
                    {
                        case CoveredRiskType.Vehicle:
                            List<CompanyVehicle> vehicles = GetCompanyVehiclesByTemporalId(policy.Id);

                            int result = vehicles.Select(x => x.Risk).Where(z => z.MainInsured?.CustomerType == CustomerType.Prospect).Count();
                            if (result > 0)
                            {
                                policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorInsuredNoInsuredRole });
                            }
                            break;
                    }
                }
            }

        }

        #endregion
        #region Clausulas
        public List<CompanyClause> SaveCompanyClauses(int riskId, List<CompanyClause> clauses)
        {
            try
            {
                CompanyVehicle risk = GetCompanyVehicleByRiskId(riskId);

                if (risk.Risk?.Id > 0)
                {

                    risk.Risk.Clauses = clauses;
                    risk = CreateVehicleTemporal(risk, false);

                    if (risk != null)
                    {
                        return clauses;
                    }
                    else
                    {
                        throw new Exception(Errors.ErrorSaveClauses);
                    }
                }
                else
                {
                    throw new Exception(Errors.NoExistRisk);
                }
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message);
            }

        }
        #endregion
        #endregion metodos migrados

        #region vehicleVersion


        public Vehicles.Models.CompanyVersion CreateCompanyVersion(Vehicles.Models.CompanyVersion companyVersion)
        {
            return ModelAssembler.CreateCompanyVersion(CreateVehicleVersion(ModelAssembler.CreateVersion(companyVersion)));
        }

        /// <summary>
        /// Actualiza vehicleVersion
        /// </summary>
        /// <param name="vehicleVersion"></param>
        /// <returns></returns>

        public Vehicles.Models.CompanyVersion UpdateCompanyVersion(Vehicles.Models.CompanyVersion companyVersion)
        {
            return ModelAssembler.CreateCompanyVersion(UpdateVehicleVersion(ModelAssembler.CreateVersion(companyVersion)));
        }


        /// <summary>
        /// retorna una lista de vehicleVersions filtrada por descripción
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>

        public List<Vehicles.Models.CompanyVersion> GetCompanyVersionsByDescription(string description)
        {
            return ModelAssembler.CreateCompanyVersions(GetVehicleVersionsByDescription(description));
        }

        /// <summary>
        /// Retorna una lista de vehicleVersions de acuerdo a los filtros proporcionados
        /// </summary>
        /// <param name="makeCode"></param>
        /// <param name="modelCode"></param>
        /// <param name="description"></param>
        /// <returns></returns>

        public List<Vehicles.Models.CompanyVersion> GetCompanyVersionsByMakeModelVersion(int? makeCode, int? modelCode, string description)
        {
            return ModelAssembler.CreateCompanyVersions(GetVehicleVersionsByMakeModelVersion(makeCode, modelCode, description));
        }

        /// <summary>
        /// Elimina una vehicleVersion 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>

        public void DeleteCompanytVehicle(int id, int makeId, int modelId)
        {
            DeleteVehicleVersion(id, makeId, modelId);
        }
        #endregion

        public IssuanceIdentificationDocument GetIdentificationDocumentByIndividualIdCustomerType(int individualId, int customerType)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetIdentificationDocumentByInsured(individualId, customerType);

            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorCreateCompanyVehicle, ex);
            }
        }

        /// <summary>
        /// Grabar Tablas Temporal de CompanyVehicle
        /// </summary>
        /// <param name="companyVehicle"> Modelo CompanyVehicle</param>
        /// <returns></returns>
        public CompanyVehicle CompanySaveCompanyVehicleTemporal(CompanyVehicle companyVehicle)
        {
            try
            {
                return CreateVehicleTemporal(companyVehicle, false);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyVehicle CalculateVehicleMinimumPremium(CompanyVehicle companyVehicle)
        {
            try
            {
                VehicleMinimumPremium vehicleBusiness = new VehicleMinimumPremium();
                return vehicleBusiness.CalculateVehicleMinimumPremium(companyVehicle);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.GetBaseException().Message);
            }
        }

        public List<CompanyServiceType> GetListCompanyServiceType()
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetServiceType();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.GetBaseException().Message);
            }
        }


        



		public List<CompanyVehicle> GetCompanyRisksVehicleByLicensePlate(string licensePlate)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetCompanyRisksVehicleByLicensePlate(licensePlate);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public CompanyVehicle CompanySaveCompanyVehicleTableTemporal(CompanyVehicle companyVehicle)
        {
            try
            {

                VehicleDAO vehicleDAO = new VehicleDAO();
                companyVehicle.Risk.RiskId = vehicleDAO.SaveCompanyVehicleTemporalTables(companyVehicle);
                return companyVehicle;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public bool GetCityExempt(int Branch)
        {

            return new VehicleDAO().GetCityExempt(Branch);

        }
        public List<AccessoryDTO> GetPremiumAccesory(int policyId, int riskNumber, int days, bool isCancelation = false)
        {
            VehicleDAO vehicleDAO = new VehicleDAO();
            return vehicleDAO.GetPremiumAccesory(policyId, riskNumber, days, isCancelation);
        }

        public int GetSummaryRisk(CompanyEndorsement endorsement)
        {
            VehicleDAO vehicleDAO = new VehicleDAO();
            return vehicleDAO.GetSummaryRisk(endorsement);
        }
    }

}