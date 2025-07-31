using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
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
using TPLCO = Sistran.Core.Application.Vehicles.ThirdPartyLiabilityService.Models;
using VECO = Sistran.Core.Application.Vehicles.Models;
using TRANS = Sistran.Core.Application.Transports.TransportBusinessService;
using Sistran.Core.Application.Vehicles.Models;
using Sistran.Company.Application.UnderwritingServices.Enums;


namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Controllers
{
    public class RiskThirdPartyLiabilityController : Controller
    {
        UnderwritingController underwritingController = new UnderwritingController();
        private static List<VECO.Make> makes = new List<VECO.Make>();
        private static List<VECO.Type> types = new List<VECO.Type>();
        private static List<TPLCO.Shuttle> shuttles = new List<TPLCO.Shuttle>();

        #region CompanyThirdPartyLiability
        public ActionResult ThirdPartyLiability()
        {
            RiskThirdPartyLiabilityViewModel risk = new RiskThirdPartyLiabilityViewModel();
            return View(risk);
        }
        public ActionResult GetAllLicencePlates()
        {
            List<string> lincencePlates = DelegateService.thirdPartyLiabilityService.GetAllLicencePlates();

            return new UifSelectResult(lincencePlates.OrderBy(x => x).ToList());
        }
        public ActionResult GetMakes()
        {
            if (makes.Count == 0)
            {
                makes = DelegateService.thirdPartyLiabilityService.GetMakes();
            }
            return new UifSelectResult(makes.OrderBy(x => x.Description).ToList());
        }
        public ActionResult GetModelsByMakeId(int makeId)
        {
            List<VECO.Model> models = DelegateService.thirdPartyLiabilityService.GetModelsByMakeId(makeId);
            if (models.Count > 0)
            {
                return new UifJsonResult(true, models);
            }
            else
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryModels);
            }
        }
        public ActionResult GetVersionsByMakeIdModelId(int makeId, int modelId)
        {
            List<Sistran.Core.Application.Vehicles.Models.Version> versions = DelegateService.thirdPartyLiabilityService.GetVersionsByMakeIdModelId(makeId, modelId);
            if (versions.Count > 0)
            {
                return new UifJsonResult(true, versions);
            }
            else
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryVersions);
            }
        }
        public JsonResult GetYearsByMakeIdModelIdVersionId(int makeId, int modelId, int versionId)
        {
            List<VECO.Year> years = DelegateService.thirdPartyLiabilityService.GetYearsByMakeIdModelIdVersionId(makeId, modelId, versionId);
            return new UifJsonResult(true, years.Where(x => x.Price != 0).OrderByDescending(x => x.Description).ToList());
        }
        public ActionResult GetTypes()
        {
            if (types.Count == 0)
            {
                types = DelegateService.thirdPartyLiabilityService.GetTypes();
            }
            return new UifSelectResult(types.OrderBy(y => y.Description).ToList());
        }
        public ActionResult GetShuttles()
        {
            if (shuttles.Count == 0)
            {
                shuttles = DelegateService.thirdPartyLiabilityService.GetShuttlesEnabled();
            }
            return new UifSelectResult(shuttles.OrderBy(x => x.Description).ToList());
        }
        public JsonResult GetServicesTypeByProduct(int productId)
        {
            try
            {
                List<VECO.ServiceType> serviceTypes = DelegateService.thirdPartyLiabilityService.GetServiceTypesByProductId(productId);
                return new UifJsonResult(true, serviceTypes.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryServiceTypes);
            }
        }
        public JsonResult GetRatingZonesByPrefixId(int prefixId)
        {
            try
            {
                List<RatingZone> ratingZones = DelegateService.underwritingService.GetRatingZonesByPrefixId(prefixId);
                return new UifJsonResult(true, ratingZones.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryChargingZones);
            }
        }
        public JsonResult GetRateTypes()
        {
            try
            {
                return new UifJsonResult(true, EnumsHelper.GetItems<RateType>());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryRateTypes);
            }
        }
        public JsonResult GetGroupCoverages(int productId)
        {
            try
            {
                List<GroupCoverage> groupCoverages = DelegateService.underwritingService.GetProductCoverageGroupRiskByProductId(productId);
                return new UifJsonResult(true, groupCoverages.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryCoverageGroups);
            }
        }
        public JsonResult GetDeductiblesByProductId(int productId)
        {
            try
            {
                List<Deductible> deductibles = DelegateService.thirdPartyLiabilityService.GetDeductiblesByProductId(productId);
                return new UifJsonResult(true, deductibles.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorConsultingDeductibles);
            }
        }
        public JsonResult GetRisksByTemporalId(int temporalId)
        {
            try
            {
                List<CompanyTplRisk> risks = DelegateService.thirdPartyLiabilityService.GetThirdPartyLiabilitiesByTemporalId(temporalId);
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
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchRisk);
            }
        }
        public ActionResult GetRiskById(EndorsementType endorsementType, int temporalId, int id)
        {
            try
            {
                var risks = DelegateService.thirdPartyLiabilityService.GetCompanyRiskById(endorsementType, temporalId, id);
                return new UifJsonResult(true, risks);
            }
            catch (Exception ex)
            {
                string m = ex.Message;
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchRisk);
            }
        }
        public JsonResult GetCoveragesByProductIdGroupCoverageId(int temporalId, int productId, int groupCoverageId, int prefixId)
        {
            try
            {
                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(productId, groupCoverageId, prefixId);
                coverages = GetCoveragesByRiskId(temporalId, coverages);
                coverages = coverages.Where(x => x.IsSelected == true).ToList();

                int coveragePrincipal = coverages.FirstOrDefault(x => x.IsPrimary == true).Id;
                List<Deductible> deductibles = DelegateService.underwritingService.GetDeductiblesByCoverageId(coveragePrincipal);
                var result = new
                {
                    Listcoverages = coverages.OrderBy(x => x.CoverNum),
                    ListDeductible = deductibles.OrderBy(x => x.Id)
                };

                return new UifJsonResult(true, result);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCoverages);
            }
        }
        private List<CompanyCoverage> GetCoveragesByRiskId(int temporalId, List<CompanyCoverage> coverages)
        {
            CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);

            if (companyPolicy.Id > 0)
            {
                foreach (CompanyCoverage item in coverages)
                {
                    item.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                    item.CurrentFrom = companyPolicy.CurrentFrom;
                    item.CurrentTo = companyPolicy.CurrentTo;
                    if (companyPolicy.Endorsement.EndorsementType == EndorsementType.Modification)
                    {
                        item.CoverStatusName = EnumsHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Included);
                        item.CoverStatus = CoverageStatusType.Included;
                    }
                    else
                    {
                        item.CoverStatusName = EnumsHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Original);
                        item.CoverStatus = CoverageStatusType.Original;
                    }
                }
            }
            return coverages.OrderBy(x => x.CoverNum).ToList();
        }
        public ActionResult GetPremium(int policyId, RiskThirdPartyLiabilityViewModel riskModel, List<CompanyCoverage> coverages, List<DynamicConcept> dynamicProperties)
        {
            try
            {
                if (coverages.Count > 0)
                {
                    CompanyTplRisk riskThirdPartyLiability = ModelAssembler.CreateThirdPartyLiability(riskModel);
                    riskThirdPartyLiability.Risk.Coverages = coverages;
                    riskThirdPartyLiability.Risk.DynamicProperties = dynamicProperties;
                    var companyTpl = DelegateService.thirdPartyLiabilityService.GetCompanyPremium(policyId, riskThirdPartyLiability);
                    return new UifJsonResult(true, companyTpl);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.EnterInsuredSelectCoverages);
                }
            }
            catch (Exception ex)
            {

                return new UifJsonResult(false, ex.GetBaseException().Message);
            }
        }
        public ActionResult SaveRisk(int temporalId, RiskThirdPartyLiabilityViewModel riskModel, List<CompanyCoverage> coverages, List<DynamicConcept> dynamicProperties)
        {
            try
            {

                ModelState.Remove("riskModel.ServiceType");
                ModelState.Remove("riskModel.RatingZone");
                ModelState.Remove("riskModel.RateType");
                ModelState.Remove("riskModel.passengerQuantity");
                ModelState.Remove("riskModel.YearModel");
                ModelState.Remove("riskModel.Shuttle");
                if (!riskModel.RePoweredVehicle)
                {
                    ModelState.Remove("riskModel.RepoweringYear");
                }

                if (ModelState.IsValid)
                {
                    CompanyTplRisk thirdPartyLiability = ModelAssembler.CreateThirdPartyLiability(riskModel);
                    thirdPartyLiability.Risk.Coverages = coverages;
                    thirdPartyLiability.Risk.DynamicProperties = dynamicProperties;
                    CompanyTplRisk companyTpl = DelegateService.thirdPartyLiabilityService.SaveCompanyRisk(temporalId, thirdPartyLiability);
                    return new UifJsonResult(true, companyTpl);
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
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveRisk);
                }
            }
        }
        public ActionResult DeleteRisk(int temporalId, int riskId)
        {
            try
            {
                DelegateService.thirdPartyLiabilityService.DeleteCompanyRisk(temporalId, riskId);
                return new UifJsonResult(true, true);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorDeleteRisk);
            }
        }
        public ActionResult RunRules(int temporalId, int? ruleSetId)
        {
            try
            {
                CompanyPolicy thirdPartyLiabilityPolicy = new CompanyPolicy { Id = temporalId };
                CompanyTplRisk thirdPartyLiability = new CompanyTplRisk
                {
                    Risk = new CompanyRisk { CoveredRiskType = CoveredRiskType.Vehicle }

                };
                List<CompanyTplRisk> companyTplRisks = new List<CompanyTplRisk> { thirdPartyLiability };
                if (ruleSetId.GetValueOrDefault() > 0)
                {
                    thirdPartyLiability.Risk.IsPersisted = true;
                    thirdPartyLiability = DelegateService.thirdPartyLiabilityService.RunRulesRisk(thirdPartyLiability, ruleSetId.Value);
                }

                return new UifJsonResult(true, thirdPartyLiability);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }
        public ActionResult ConvertProspectToInsured(int temporalId, int individualId, string documentNumber)
        {
            try
            {
                var result = DelegateService.thirdPartyLiabilityService.ConvertProspectToInsured(temporalId, individualId, documentNumber);
                return new UifJsonResult(true, true);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorConvertingProspectIntoIndividual);
            }
        }
        private string GetErrorMessages()
        {
            StringBuilder sb = new StringBuilder();
            foreach (ModelState item in ModelState.Values.Where(x => x.Errors.Count > 0))
            {
                sb.Append(item.Errors[0].ErrorMessage).Append(", ");
            }
            return sb.ToString().Remove(sb.ToString().Length - 2);
        }
        #endregion

        #region Beneficiary

        public ActionResult SaveBeneficiaries(int riskId, List<CompanyBeneficiary> beneficiaries)
        {
            try
            {
                CompanyTplRisk risk = DelegateService.thirdPartyLiabilityService.GetCompanyTplRiskByRiskId(riskId);

                if (risk.Risk.Id > 0)
                {
                    risk.Risk.Beneficiaries = beneficiaries;
                    DelegateService.thirdPartyLiabilityService.CreateThirdPartyLiabilityTemporal(risk, false);
                    return new UifJsonResult(true, beneficiaries);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorTempNoExist);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveBeneficiaries);
            }
        }

        #endregion

        #region Texts

        public ActionResult SaveTexts(int riskId, TextsModelsView textModel)
        {
            try
            {
                var companyText = ModelAssembler.CreateText(textModel);
                var txt = DelegateService.thirdPartyLiabilityService.SaveCompanyTexts(riskId, companyText);
                return new UifJsonResult(true, txt);
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

        #endregion

        #region Clauses

        public ActionResult SaveClauses(int riskId, List<CompanyClause> clauses)
        {
            try
            {
                if (clauses != null && clauses.Any())
                {
                    var companyclauses = DelegateService.thirdPartyLiabilityService.SaveCompanyClauses(riskId, clauses);

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

        #region Coverage

        public ActionResult Coverage(CoverageModelsView coverageModelsView)
        {
            ModelState.Clear();
            CoverageModelsView coverage = new CoverageModelsView();
            return View(coverage);
        }

        public ActionResult GetCoveragesByRiskId(int riskId)
        {
            try
            {
                List<Coverage> coverages = new List<Coverage>();
                CompanyTplRisk risk = DelegateService.thirdPartyLiabilityService.GetCompanyTplRiskByRiskId(riskId);
                return new UifJsonResult(true, risk.Risk.Coverages.OrderBy(x => x.CoverNum));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCoverages);
            }
        }

        public ActionResult GetCoverageByCoverageId(int coverageId, int riskId, int temporalId, int groupCoverageId)
        {
            try
            {
                var companyCoverage = DelegateService.underwritingService.GetCompanyCoverageByCoverageId(coverageId, groupCoverageId, temporalId);

                CompanyPolicy tplPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                CompanyCoverage coverage = DelegateService.underwritingService.GetCompanyCoverageByCoverageIdProductIdGroupCoverageId(coverageId, tplPolicy.Product.Id, groupCoverageId);

                if (coverage.RuleSetId.HasValue)
                {
                    coverage.EndorsementType = tplPolicy.Endorsement.EndorsementType;
                    coverage.CurrentFrom = tplPolicy.CurrentFrom;
                    coverage.CurrentTo = tplPolicy.CurrentTo;
                    coverage.Days = Convert.ToInt32((coverage.CurrentTo.Value - coverage.CurrentFrom).TotalDays);

                    if (coverage.EndorsementType == EndorsementType.Modification)
                    {
                        coverage.CoverStatus = CoverageStatusType.Included;
                    }

                    coverage.CoverStatusName = EnumsHelper.GetItemName<CoverageStatusType>(coverage.CoverStatus);
                    CompanyTplRisk thirdPartyLiability = new CompanyTplRisk
                    {
                        Risk = new CompanyRisk
                        {
                            Id = riskId
                        }
                    };

                    thirdPartyLiability.Risk.Coverages = new List<CompanyCoverage> { coverage };
                    thirdPartyLiability.Risk.Policy = tplPolicy;
                    coverage = DelegateService.thirdPartyLiabilityService.RunRulesCompanyCoverage(thirdPartyLiability, coverage, coverage.RuleSetId.Value);
                }
                return new UifJsonResult(true, coverage);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCoveragesAccesories);
            }
        }

        public JsonResult GetCoveragesByProductIdGroupCoverageIdPrefixId(int productId, int groupCoverageId, int prefixId, string coveragesAdd)
        {
            try
            {
                string[] idCoverages = coveragesAdd.Split(',');
                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(productId, groupCoverageId, prefixId);
                coverages = coverages.Where(c => (!idCoverages.Any(x => Convert.ToInt32(x) == c.Id)) && c.IsVisible == true).ToList();
                return new UifJsonResult(true, coverages.OrderBy(x => x.CoverNum));

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCoverages);
            }
        }

        public ActionResult GetCalculeTypes()
        {
            try
            {
                return new UifSelectResult(EnumsHelper.GetItems<Core.Services.UtilitiesServices.Enums.CalculationType>());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryCalculationTypes);
            }
        }

        public JsonResult GetDeductiblesByCoverageId(int coverageId)
        {
            try
            {
                List<Deductible> deductibles = DelegateService.underwritingService.GetDeductiblesByCoverageId(coverageId);
                return new UifJsonResult(true, deductibles.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorConsultingDeductibles);
            }
        }

        public ActionResult QuotationCoverage(CompanyCoverage coverage, CoverageModelsView coverageModel)
        {
            try
            {

                coverage.CurrentFrom = Convert.ToDateTime(coverageModel.CurrentFrom);
                coverage.CurrentTo = Convert.ToDateTime(coverageModel.CurrentTo);
                coverage.DeclaredAmount = coverageModel.DeclaredAmount;
                coverage.LimitAmount = coverageModel.LimitAmount;
                coverage.SubLimitAmount = coverageModel.SubLimitAmount;
                coverage.LimitOccurrenceAmount = coverageModel.LimitOccurrenceAmount.GetValueOrDefault();
                coverage.LimitClaimantAmount = coverageModel.LimitClaimantAmount.GetValueOrDefault();
                coverage.MaxLiabilityAmount = coverageModel.MaxLiabilityAmount.GetValueOrDefault();
                coverage.CalculationType = (Core.Services.UtilitiesServices.Enums.CalculationType)coverageModel.CalculationTypeId;
                coverage.RateType = (RateType)coverageModel.RateType;
                coverage.Rate = coverageModel.Rate;
                coverage.PremiumAmount = coverageModel.PremiumAmount;
                if (coverageModel.DeductibleId != null)
                {

                    if (coverage.Deductible == null)
                    {
                        coverage.Deductible = new CompanyDeductible
                        {
                            Id = (int)coverageModel.DeductibleId,
                            Description = coverageModel.DeductibleDescription
                        };
                    }
                    else
                    {
                        coverage.Deductible.Id = (int)coverageModel.DeductibleId;
                        coverage.Deductible.Description = coverageModel.DeductibleDescription;
                        
                    }
                }


                CompanyTplRisk companyTplRisk = new CompanyTplRisk
                {
                    Risk = new CompanyRisk
                    {
                        Id = coverageModel.RiskId,
                        Policy = new CompanyPolicy
                        {
                            Id = coverageModel.TemporalId,
                            Endorsement = new CompanyEndorsement
                            {
                                EndorsementType = (EndorsementType)coverageModel.EndorsementType
                            }
                        }
                    }
                };
                coverage = DelegateService.thirdPartyLiabilityService.QuotateCompanyCoverage(companyTplRisk, coverage, false, true);
                coverage.CoverStatusName = EnumsHelper.GetItemName<CoverageStatusType>(coverage.CoverStatus);
                return new UifJsonResult(true, coverage);



            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQuotationCoverage);
            }
        }


        public ActionResult SaveCoverages(int temporalId, int riskId, List<CompanyCoverage> coverages)
        {
            try
            {
                DelegateService.thirdPartyLiabilityService.SaveCompanyCoverages(temporalId, riskId, coverages);
                return new UifJsonResult(true, true);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveCoverages);
            }

        }

        public ActionResult GetPolicyModelsView(int temporalId)
        {
            try
            {
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                if (companyPolicy != null)
                {
                    PolicyModelsView policyModel = new PolicyModelsView();
                    policyModel.Id = companyPolicy.Id;
                    policyModel.PrefixId = companyPolicy.Prefix.Id;
                    policyModel.ProductId = companyPolicy.Product.Id;
                    policyModel.PolicyType = companyPolicy.PolicyType.Id;
                    policyModel.EndorsementType = Convert.ToInt32(companyPolicy.Endorsement.EndorsementType.Value);
                    policyModel.HolderId = companyPolicy.Holder.IndividualId;

                    if (companyPolicy.TemporalType == TemporalType.Quotation)
                    {
                        policyModel.Title = companyPolicy.TemporalTypeDescription;
                    }
                    else
                    {
                        policyModel.Title = App_GlobalResources.Language.Temporal;
                    }

                    policyModel.Title += ": " + companyPolicy.Id.ToString() + " " + companyPolicy.Endorsement.EndorsementTypeDescription;

                    return new UifJsonResult(true, policyModel);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchTemp);
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
                var companyPolicy = DelegateService.thirdPartyLiabilityService.UpdateCompanyRisks(temporalId);
                return new UifJsonResult(true, companyPolicy);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);

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

        public ActionResult ExcludeCoverage(int temporalId, int riskId, int riskCoverageId, string description)
        {
            try
            {
                var companyCoverages = DelegateService.thirdPartyLiabilityService.ExcludeCompanyCoverage(temporalId, riskId, riskCoverageId, description);
                return new UifJsonResult(true, companyCoverages);

            }
            catch (Exception)
            {

                return new UifJsonResult(false, "Error excluyendo cobertura");
            }
        }

        #endregion

        #region AdditionalData
        public ActionResult SaveAdditionalData(int riskId, AdditionalDataTPLModelsView additionalDataModel)
        {
            try
            {
                CompanyTplRisk risk = DelegateService.thirdPartyLiabilityService.GetCompanyTplRiskByRiskId(riskId);

                if (risk.Risk.Id > 0)
                {
                    risk.PhoneNumber = additionalDataModel.PhoneNumber;
                    risk.Tons = additionalDataModel.Tons;
                    risk.TrailerQuantity = additionalDataModel.TrailerQuantity;
                    risk = DelegateService.thirdPartyLiabilityService.CreateThirdPartyLiabilityTemporal(risk, false);

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

        public ActionResult GetCargoTypes()
        {
            try
            {
                List<CargoType> CargoTypes = DelegateService.thirdPartyLiabilityService.GetCargoTypes();
                return new UifJsonResult(true, CargoTypes.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryServiceTypes);
            }
        }


        #endregion
    }
}