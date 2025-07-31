using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Vehicles.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Vehicles.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CiaUnderwritingModel = Sistran.Company.Application.UnderwritingServices.Models;
using UNMO = Sistran.Core.Application.UnderwritingServices.Models;
using VECO = Sistran.Core.Application.Vehicles.VehicleServices.Models;
using Sistran.Company.Application.ExternalProxyServices.Models;
using Sistran.Company.Application.Utilities.Enums;
using System.Text.RegularExpressions;
using Sistran.Core.Services.UtilitiesServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Controllers
{
    public class RiskVehicleController : Controller
    {
        UnderwritingController underwritingController = new UnderwritingController();
        private static List<Make> makes = new List<Make>();
        private static List<Sistran.Core.Application.Vehicles.Models.Type> types = new List<Sistran.Core.Application.Vehicles.Models.Type>();
        private static List<VECO.Use> uses = new List<VECO.Use>();
        private static List<Color> colors = new List<Color>();
        private static List<Body> bodies = new List<Body>();
        private static List<Fuel> fuels = new List<Fuel>();
        private static List<VECO.Accessory> accessoryDescriptions = new List<VECO.Accessory>();
        private static List<RateType> rateTypes = new List<RateType>();
        private static List<CompanyClause> Riskclauses = new List<CompanyClause>();
        private static List<CompanyBeneficiary> companyBeneficiary = new List<CompanyBeneficiary>();
        private static CompanyText companyText = new CompanyText();

        #region CompanyVehicle

        [NoDirectAccess]
        public ActionResult Vehicle()
        {
            return View();
        }

        public ActionResult GetRisksByTemporalId(int temporalId)
        {
            try
            {
                List<CompanyVehicle> risks = DelegateService.vehicleService.GetCompanyVehiclesByTemporalId(temporalId);

                if (risks != null)
                {

                    return new UifJsonResult(true, risks);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorTempNoExist);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchRisk);
            }
        }
        public ActionResult GetCiaRiskByTemporalId(int temporalId)
        {
            try
            {
                List<CompanyRisk> risks = DelegateService.underwritingService.GetCiaRiskByTemporalId(temporalId, false);

                if (risks != null)
                {
                    return new UifJsonResult(true, risks);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorTempNoExist);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchRisk);
            }
        }
        public ActionResult GetRiskById(EndorsementType endorsementType, int temporalId, int id)
        {
            try
            {
                var risks = DelegateService.vehicleService.GetCompanyRiskById(endorsementType, temporalId, id);
                return new UifJsonResult(true, risks);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchRisk);
            }
        }

        [IgnoreValidation]
        public UifJsonResult GetFasecoldaByCode(string code, int year)
        {
            try
            {
                CompanyVehicle vehicle = DelegateService.vehicleService.GetVehicleByFasecoldaCode(code, year);

                if (vehicle != null)
                {
                    return new UifJsonResult(true, vehicle);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.NoExistFascoldaCode);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchFasecolda);
            }
        }

        [IgnoreValidation]
        public ActionResult GetFasecoldaCodeByMakeIdModelIdVersionId(int makeId, int modelId, int versionId)
        {
            try
            {
                CompanyVehicle vehicle = DelegateService.vehicleService.GetVehicleByMakeIdModelIdVersionId(makeId, modelId, versionId);

                if (vehicle != null)
                {
                    return new UifJsonResult(true, vehicle);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.NoExistFascoldaCode);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchFasecolda);
            }
        }

        public ActionResult GetAllLicencePlates()
        {
            try
            {
                List<string> lincencePlates = DelegateService.vehicleService.GetAllLicencePlates();

                return new UifSelectResult(lincencePlates.OrderBy(x => x).ToList());
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorLicensePlate);
            }

        }

        public ActionResult GetMakes()
        {
            try
            {
                if (makes.Count == 0)
                {
                    makes = DelegateService.vehicleService.GetMakes();
                }
                return new UifJsonResult(true, makes.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetMakes);
            }

        }

        public ActionResult GetModelsByMakeId(int makeId)
        {
            try
            {
                List<Model> models = DelegateService.vehicleService.GetModelsByMakeId(makeId);
                return new UifJsonResult(true, models.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetMakes);
            }

        }

        public ActionResult GetVersionsByMakeIdModelId(int makeId, int modelId)
        {
            try
            {
                var versions = DelegateService.vehicleService.GetVersionsByMakeIdModelId(makeId, modelId);
                return new UifJsonResult(true, versions.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetMakes);
            }

        }

        public ActionResult GetTypesByTypeId(int typeId)
        {
            try
            {
                if (types.Count == 0)
                {
                    types = DelegateService.vehicleService.GetTypes();
                }
                return new UifJsonResult(true, types.Where(x => x.Id == typeId).OrderBy(y => y.Description).ToList());
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetTypes);
            }

        }

        public ActionResult GetUses()
        {
            try
            {
                if (uses.Count == 0)
                {
                    uses = DelegateService.vehicleService.GetUses();
                }
                return new UifJsonResult(true, uses.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetUses);
            }

        }

        public UifJsonResult GetYearsByMakeIdModelIdVersionId(int makeId, int modelId, int versionId)
        {
            try
            {
                List<Year> years = DelegateService.vehicleService.GetYearsByMakeIdModelIdVersionId(makeId, modelId, versionId);
                //Se omite el filtro debido a que existen modelos sin precio, a los que no permite seleccionar el año
                //return new UifJsonResult(true, years.Where(x => x.Price != 0).OrderByDescending(x => x.Description).ToList());
                return new UifJsonResult(true, years.OrderByDescending(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetYears);
            }

        }

        public UifJsonResult GetPriceByMakeIdModelIdVersionId(int makeId, int modelId, int versionId, int year)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.vehicleService.GetPriceByMakeIdModelIdVersionId(makeId, modelId, versionId, year));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchPrice);
            }
        }

        public UifJsonResult GetColors()
        {
            try
            {
                if (colors.Count == 0)
                {
                    colors = DelegateService.vehicleService.GetColors();
                }
                return new UifJsonResult(true, colors.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetColors);
            }

        }

        public UifJsonResult GetRatingZonesByPrefixId(int prefixId)
        {
            try
            {
                List<RatingZone> ratingZones = DelegateService.underwritingService.GetRatingZonesByPrefixId(prefixId);
                return new UifJsonResult(true, ratingZones.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchRatingZone);
            }
        }

        public UifJsonResult GetLimitsRcByPrefixIdProductIdPolicyTypeId(int prefixId, int productId, int policyTypeId)
        {
            try
            {
                List<UNMO.LimitRc> limitsRc = DelegateService.underwritingService.GetLimitsRcByPrefixIdProductIdPolicyTypeId(prefixId, productId, policyTypeId);
                return new UifJsonResult(true, limitsRc.OrderBy(x => x.LimitSum).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchLimitRC);
            }
        }

        public UifJsonResult GetGroupCoverages(int productId)
        {
            try
            {
                List<UNMO.GroupCoverage> groupCoverages = DelegateService.underwritingService.GetProductCoverageGroupRiskByProductId(productId);
                return new UifJsonResult(true, groupCoverages.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetGroupCoverages);
            }
        }

        public ActionResult GetCoveragesByProductIdGroupCoverageId(int policyId, int groupCoverageId)
        {
            try
            {
                var coverages = DelegateService.vehicleService.GetCompanyCoveragesByProductIdGroupCoverageId(policyId, groupCoverageId);
                return new UifJsonResult(true, coverages);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCoverages);
            }
        }

        public ActionResult GetRiskLists(CompanyVehicle companyVehicle, CompanyPolicy policyModel)
        {
            try
            {
                RiskComboDTO combos = new RiskComboDTO();

                combos.Makes = DelegateService.vehicleService.GetMakes();
                combos.Types = DelegateService.vehicleService.GetTypes();
                combos.Uses = DelegateService.vehicleService.GetUses();
                combos.Colors = DelegateService.vehicleService.GetColors();
                if (fuels.Count == 0)
                {
                    fuels = DelegateService.vehicleService.GetFuels();
                }
                combos.Fuels = fuels;
                combos.CompanyServiceTypes = DelegateService.vehicleService.GetListCompanyServiceType();
                if (policyModel?.Id > 0)
                {
                    combos.CompanyRisks = DelegateService.underwritingService.GetCiaRiskByTemporalId(policyModel.Id, false);
                }
                if (policyModel?.Product?.Id > 0)
                {
                    combos.GroupCoverages = DelegateService.underwritingService.GetProductCoverageGroupRiskByProductId(policyModel.Product.Id);
                }

                if (companyVehicle?.Make?.Id > 0)
                {
                    combos.Models = DelegateService.vehicleService.GetModelsByMakeId(companyVehicle.Make.Id);
                    if (companyVehicle?.Model?.Id > 0)
                    {
                        combos.Versions = DelegateService.vehicleService.GetVersionsByMakeIdModelId(companyVehicle.Make.Id, companyVehicle.Model.Id);
                        if (companyVehicle?.Version?.Id > 0)
                        {
                            combos.Years = DelegateService.vehicleService.GetYearsByMakeIdModelIdVersionId(companyVehicle.Make.Id, companyVehicle.Model.Id, companyVehicle.Version.Id);
                        }
                    }
                }

                if (policyModel?.Prefix?.Id > 0)
                {
                    combos.RatingZones = DelegateService.underwritingService.GetRatingZonesByPrefixId(policyModel.Prefix.Id);
                    if (policyModel?.Product?.Id > 0 && policyModel?.PolicyType?.Id > 0)
                    {
                        combos.LimitRcs = DelegateService.underwritingService.GetLimitsRcByPrefixIdProductIdPolicyTypeId(policyModel.Prefix.Id, policyModel.Product.Id, policyModel.PolicyType.Id);
                    }
                }


                return new UifJsonResult(true, combos);
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "BusinessException")
                {
                    return new UifJsonResult(false, ex.Message);
                }
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorUpdatePolicy);
            }
        }

        public ActionResult GetPremium(int policyId, RiskVehicleModelsView riskModel, List<CompanyCoverage> coverages, List<DynamicConcept> dynamicProperties, List<CompanyAccessory> accessories, AdditionalDataModelsView additionalData)
        {
            try
            {
                if (policyId > 0 && riskModel != null)
                {
                    if (Convert.ToDecimal(riskModel?.Price) > 0 && coverages?.Any() != null)
                    {
                        CompanyVehicle vehicle = ModelAssembler.CreateVehicle(riskModel, additionalData);
                        vehicle.Risk.Coverages = coverages;
                        vehicle.Risk.Clauses = riskModel.Clauses;
                        vehicle.Accesories = accessories;
                        vehicle.Risk.Text = riskModel.Text;
                        vehicle.Risk.Beneficiaries = riskModel.Beneficiaries;
                        vehicle.Risk.DynamicProperties = dynamicProperties;
                        vehicle.Risk.Status = (RiskStatusType)riskModel.Status;

                        var companyVehicle = DelegateService.vehicleService.GetCompanyPremium(policyId, vehicle);
                        return new UifJsonResult(true, companyVehicle);
                    }
                    else
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.EnterInsuredSelectCoverages);
                    }
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorTempNoExist);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public ActionResult GetMinPremiunRelationByPrefixAndProduct(int PrefixId, string ProductName)
        {
            var result = new UifJsonResult(false, App_GlobalResources.Language.MinPremiunRelationQueryError);

            try
            {
                var dto = DelegateService.CompanyUnderwritingParamApplicationService.GetApplicationMinPremiunRelationByPrefixIdAndProductName(PrefixId, ProductName);

                if (dto.ErrorDTO.ErrorType == ErrorType.Ok)
                {
                    var dtoFilter = dto.MinPremiunRelationDTO.
                        Where(x => x.EndorsementType.Description.Contains("Expedición")).
                        Select(x => x.RiskMinPremiun);
                    result = new UifJsonResult(true, dtoFilter); 
                }
            }
            catch
            {

            }

            return result;
        }

        public ActionResult SaveRisk(int temporalId, RiskVehicleModelsView riskModel, List<CompanyCoverage> coverages, List<DynamicConcept> dynamicProperties, List<CompanyAccessory> accessories, AdditionalDataModelsView additionalData)
        {
            try
            {
               
                if (ModelState.IsValid)
                {
                    CompanyVehicle vehicle = ModelAssembler.CreateVehicle(riskModel, additionalData);
                    vehicle.Risk.Coverages = coverages;
                    vehicle.Accesories = accessories;
                  //  vehicle.Risk.Clauses = Riskclauses;
                    vehicle.Risk.DynamicProperties = dynamicProperties;

                    var companyVehicle = DelegateService.vehicleService.SaveCompanyRisk(temporalId, vehicle);
                    return new UifJsonResult(true, companyVehicle);
                }
                else
                {
                    string errorMessage = GetErrorMessages();
                    return new UifJsonResult(false, errorMessage);
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "BusinessException")
                {
                    return new UifJsonResult(false, ex.Message);
                }
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveRisk);
            }
        }

        public ActionResult DeleteRisk(int policyId, int id)
        {
            try
            {
                DelegateService.vehicleService.DeleteCompanyRisk(policyId, id);
                return new UifJsonResult(true, true);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorDeleteRisk);
            }
        }

        public ActionResult RunRules(int policyId, int? ruleSetId)
        {
            try
            {
                CompanyVehicle vehicle = new CompanyVehicle
                {
                    Risk = new CompanyRisk
                    {
                        CoveredRiskType = CoveredRiskType.Vehicle,
                        IsPersisted = true,
                        Policy = new CompanyPolicy
                        {
                            Id = policyId,
                            IsPersisted = false
                        }
                    },
                };

                if (ruleSetId.GetValueOrDefault() > 0)
                {
                    vehicle = DelegateService.vehicleService.RunRulesRisk(vehicle, ruleSetId.Value);
                }

                return new UifJsonResult(true, vehicle);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        /// <summary>
        /// Metodo para actualizacion de poliza, se utiliza si el sistema detecta cambios en la pantalla principal de poliza para que
        /// actualice los riesgos y coberturas con los nuevos parametros. El metodo debe estar en todos los ramos haciendo
        /// la adecuacion al modelo correspondiente
        /// </summary>
        /// <param name="tempId">temporal de la poliza</param>
        /// <returns></returns>
        public UifJsonResult UpdateRisks(int temporalId)
        {
            try
            {
                var companyPolicy = DelegateService.vehicleService.UpdateCompanyRisks(temporalId, false);
                return new UifJsonResult(true, companyPolicy);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorUpdatePolicy);
            }

        }

        /// <summary>
        /// Metodo para validar si los riesgos a emitir existen en otras polizas
        /// </summary>
        /// <param name="tempId">temporal de la poliza</param>
        /// <returns></returns>
        public UifJsonResult ExistRiskByTemporalId(int tempId)
        {
            var message = "";
            try
            {
                message = DelegateService.vehicleService.ExistCompanyRiskByTemporalId(tempId);
                return new UifJsonResult(true, message);
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "BusinessException")
                {
                    return new UifJsonResult(false, ex.Message);
                }

                return new UifJsonResult(false, message);
            }

        }

        public ActionResult ConvertProspectToInsured(int temporalId, int individualId, string documentNumber)
        {
            try
            {
                var result = DelegateService.vehicleService.ConvertProspectToInsured(temporalId, individualId, documentNumber);
                return new UifJsonResult(true, result);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorConvertingProspectIntoIndividual);
            }
        }

        private string GetErrorMessages()
        {
            StringBuilder sb = new StringBuilder();
            foreach (ModelState item in ModelState.Values)
            {
                if (item.Errors.Count > 0)
                {
                    sb.Append(item.Errors[0].ErrorMessage).Append(", ");
                }
            }
            return sb.ToString().Remove(sb.ToString().Length - 2);
        }

        private decimal CalculateTotalPremium(List<CompanyVehicle> risks)
        {
            var totalPremium = risks.Where(z => z.Risk != null).Select(x => x.Risk?.Premium).Sum();
            return (decimal)totalPremium;
        }

        public UifJsonResult GetListCompanyServiceType()
        {
            try
            {
                List<CompanyServiceType> listServiceType = new List<CompanyServiceType>();
                listServiceType = DelegateService.vehicleService.GetListCompanyServiceType();
                return new UifJsonResult(true, listServiceType);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryServiceTypes);
            }
        }

        #endregion

        #region Texts

        public ActionResult SaveTexts(int riskId, TextsModelsView textModel)
        {
            UnderwritingController underwritingController = new UnderwritingController();
            try
            {
                var companyText = ModelAssembler.CreateText(textModel);
                if (!string.IsNullOrEmpty(companyText.TextBody))
                    companyText.TextBody = underwritingController.unicode_iso8859(companyText.TextBody);
                var CompanyText = DelegateService.vehicleService.SaveCompanyTexts(riskId, companyText);
                return new UifJsonResult(true, CompanyText);
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "BusinessException")
                {
                    return new UifJsonResult(false, ex.Message);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveText);
                }
            }
        }

        private bool GetCityExempt(int BranchId) {

            try
            {
                return  DelegateService.vehicleService.GetCityExempt(BranchId);
            }
            catch (Exception ex) 
            {
                return false;
            }

        }

        public ActionResult GetFasecoldaByPlate(CompanyVehicle vehicle, int branchId) {
            // R1 no tiene esta validacion 
            //if (GetCityExempt(branchId)) {
            //    return new UifJsonResult(true, vehicle);
            //}else
            //{
                CompanyVehicle result = new CompanyVehicle();
                RequestFasecoldaSISA request = new RequestFasecoldaSISA() { Plate = vehicle.LicensePlate, Engine = "" ,  Chassis =  "" };
                ResponseFasecoldaSISA response = DelegateService.ExternalServiceWeb.ExecuteWebServiceSISA(request);

               
                if (response.PoliciesInfo == null)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorFasecoldaPlate);
                }

                if (response.PoliciesInfo != null && response.PoliciesInfo.Count > 0)
                {
                    return new UifJsonResult(true, response.PoliciesInfo[0]);
                }
                else
                {
                    CompanyVehicle objVehicle = DelegateService.vehicleService.GetVehicleByLicensePlate(vehicle.LicensePlate);
                    if (objVehicle == null)
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorFasecoldaInfo);
                    }
                    else {
                        ResponsePoliciesInfo responsePolicy = new ResponsePoliciesInfo{
                            Plate = objVehicle.LicensePlate,
                            Chassis = objVehicle.ChassisSerial,
                            Engine = objVehicle.EngineSerial,
                            GuiedCode = objVehicle.Fasecolda.Description,
                            Model = Convert.ToInt16(objVehicle.Year)

                        };
                        return new UifJsonResult(true, responsePolicy);
                    }
                    
                }
           // }

        }

        #endregion

        #region Clauses

        public ActionResult SaveClauses(int riskId, List<CompanyClause> clauses)
        {
            try
            {
                if (clauses != null && clauses.Any())
                {
                    var companyclauses = DelegateService.vehicleService.SaveCompanyClauses(riskId, clauses);
                   // Riskclauses = companyclauses;

                    return new UifJsonResult(true, companyclauses);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorSelectedClauses);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveClauses);
            }
        }

        #endregion

        #region Beneficiaries

        public ActionResult SaveBeneficiaries(int riskId, List<CompanyBeneficiary> beneficiaries)
        {
            try
            {
                CompanyVehicle risk = DelegateService.vehicleService.GetCompanyVehicleByRiskId(riskId);

                if (risk.Risk.Id > 0)
                {
                    risk.Risk.Beneficiaries = beneficiaries;

                    risk = DelegateService.vehicleService.CreateVehicleTemporal(risk, false);

                    if (risk != null)
                    {
                        return new UifJsonResult(true, risk.Risk.Beneficiaries);
                    }
                    else
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveBeneficiaries);
                    }
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorTempNoExist);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveBeneficiaries);
            }
        }

        #endregion

        #region Accessories

        public ActionResult GetAccessories()
        {
            if (accessoryDescriptions.Count == 0)
            {
                accessoryDescriptions = DelegateService.vehicleService.GetAccessories();
            }
            return new UifSelectResult(accessoryDescriptions.OrderBy(x => x.Description));
        }

        public ActionResult GetCoveragesAccessoriesByProductIdGroupCoverageIdPrefixId(int productId, int groupCoverageId, int prefixId)
        {
            try
            {
                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesAccessoriesByProductIdGroupCoverageIdPrefixId(productId, groupCoverageId, prefixId);

                if (coverages?.Count > 0)
                {
                    return new UifJsonResult(true, true);
                }
                else
                {
                    return new UifJsonResult(true, false);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCoveragesAccesories);
            }
        }

        #endregion

        #region AdditionalData

        public ActionResult GetBodies()
        {
            if (bodies.Count == 0)
            {
                bodies = DelegateService.vehicleService.GetBodies();
            }
            return new UifJsonResult(true, bodies.OrderBy(x => x.Description));
        }

        public ActionResult GetFuels()
        {
            if (fuels.Count == 0)
            {
                fuels = DelegateService.vehicleService.GetFuels();
            }
            return new UifJsonResult(true, fuels.OrderBy(x => x.Description));
        }

        public ActionResult SaveAdditionalData(int riskId, AdditionalDataModelsView additionalDataModel)
        {
            try
            {
                CompanyVehicle risk = DelegateService.vehicleService.GetCompanyVehicleByRiskId(riskId);

                if (risk.Risk.Id > 0)
                {
                    if (additionalDataModel.InsuredId.HasValue)
                    {
                        risk.Risk.SecondInsured = new CiaUnderwritingModel.CompanyIssuanceInsured
                        {
                            IndividualId = additionalDataModel.InsuredId.Value,
                            Name = additionalDataModel.InsuredName,
                            IndividualType = IndividualType.Person,
                            CustomerType = CustomerType.Individual
                        };
                    }

                    risk.Version.Fuel = new CompanyFuel
                    {
                        Id = additionalDataModel.FuelType
                    };
                    risk.Version.Body = new CompanyBody
                    {
                        Id = additionalDataModel.BodyType
                    };
                    risk.NewPrice = Convert.ToDecimal(additionalDataModel.NewPrice);

                    risk = DelegateService.vehicleService.CreateVehicleTemporal(risk, false);

                    if (risk != null)
                    {
                        return new UifJsonResult(true, risk);
                    }
                    else
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveAdditionalData);
                    }
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.NoExistRisk);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveAdditionalData);
            }
        }

        #endregion

        #region Coverage

        public ActionResult Coverage()
        {
            return View();
        }

        public ActionResult GetCoverageByCoverageId(int coverageId, int groupCoverageId, int policyId)
        {
            try
            {
                var companyCoverage = DelegateService.underwritingService.GetCompanyCoverageByCoverageId(coverageId, groupCoverageId, policyId);
                return new UifJsonResult(true, companyCoverage);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCoverage);
            }
        }

        public ActionResult GetCoveragesByProductIdGroupCoverageIdPrefixId(int productId, int groupCoverageId, int prefixId, string coveragesAdd)
        {
            try
            {
                string[] idCoverages = coveragesAdd.Split(',');
                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(productId, groupCoverageId, prefixId);
                coverages = coverages.Where(c => (!idCoverages.Any(x => Convert.ToInt32(x) == c.Id)) && c.IsVisible == true).ToList();
                return new UifJsonResult(true, coverages.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCoverage);
            }
        }

        public ActionResult QuotationCoverage(int tempId, int riskId, CompanyCoverage coverage, CoverageModelsView coverageModel)
        {
            coverage.CurrentFrom = Convert.ToDateTime(coverageModel.CurrentFrom);
            coverage.CurrentTo = Convert.ToDateTime(coverageModel.CurrentTo);
            coverage.CalculationType = (Core.Services.UtilitiesServices.Enums.CalculationType)coverageModel.CalculationTypeId;
            coverage.LimitAmount = coverageModel.LimitAmount;
            coverage.SubLimitAmount = coverageModel.SubLimitAmount;
            coverage.Rate = coverageModel.Rate;
            coverage.RateType = (RateType)coverageModel.RateType;
            coverage.PremiumAmount = coverageModel.PremiumAmount;

            if (coverageModel.DeductibleId.GetValueOrDefault() > 0)
            {
                coverage.Deductible = new CompanyDeductible
                {
                    Id = coverageModel.DeductibleId.Value
                };
            }
            CompanyPolicy vehiclePolicy = new CompanyPolicy { Id = tempId, Endorsement = new CompanyEndorsement { EndorsementType = (EndorsementType)coverageModel.EndorsementType } };
            CompanyVehicle vehicle = new CompanyVehicle { Risk = new CompanyRisk { Id = riskId, Policy = vehiclePolicy } };
            coverage = DelegateService.vehicleService.QuotateCompanyCoverage(vehicle, coverage, true, true);
            coverage.CoverStatusName = EnumsHelper.GetItemName<CoverageStatusType>(coverage.CoverStatus);

            return new UifJsonResult(true, coverage);
        }

        /// <summary>
        /// Saves the coverages.
        /// </summary>
        /// <param name="policyId">The policy identifier.</param>
        /// <param name="riskId">The risk identifier.</param>
        /// <param name="coverages">The coverages.</param>
        /// <returns></returns>
        public ActionResult SaveCoverages(int policyId, int riskId, List<CompanyCoverage> coverages)
        {
            try
            {
                DelegateService.vehicleService.SaveCompanyCoverages(policyId, riskId, coverages);
                return new UifJsonResult(true, true);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveCoverages);
            }
        }

        public ActionResult ExcludeCoverage(int temporalId, int riskId, int riskCoverageId, string description)
        {
            try
            {
                var companyCoverages = DelegateService.vehicleService.ExcludeCompanyCoverage(temporalId, riskId, riskCoverageId, description);
                return new UifJsonResult(true, companyCoverages);

            }
            catch (Exception)
            {

                return new UifJsonResult(false, "Error excluyendo cobertura");
            }

        }

        public ActionResult GetRegularExpression() {

            List<ValidationRegularExpression> listRegular = DelegateService.utilitiesServiceCore.GetAllValidationRegularExpressions();
            if (listRegular.Count > 0 && listRegular.Where(x => x.FieldDescription.ToUpper().Trim() == "VALIDACION PLACA").Count() > 0)
            {
                return new UifJsonResult(true, listRegular.Where(x => x.FieldDescription.ToUpper().Trim() == "VALIDACION PLACA").FirstOrDefault().ParameterValue);
            }
            else 
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ValidateExpression);
            }

        }

        #endregion
    }
}