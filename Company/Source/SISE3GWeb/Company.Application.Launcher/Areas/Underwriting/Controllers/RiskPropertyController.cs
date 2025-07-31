using AutoMapper;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Location.PropertyServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using LOMO = Sistran.Core.Application.Locations.Models;
namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Controllers
{
    public class RiskPropertyController : Controller
    {
        private static List<Country> countries = new List<Country>();
        private static List<Country> countriesCommon = new List<Country>();
        private static List<LOMO.Suffix> suffixes = new List<LOMO.Suffix>();
        private static List<LOMO.ApartmentOrOffice> aparmentOrOffices = new List<LOMO.ApartmentOrOffice>();
        private static List<LOMO.RouteType> routeTypes = new List<LOMO.RouteType>();
        private static List<LOMO.RiskUse> riskUses = new List<LOMO.RiskUse>();
        private static List<LOMO.ConstructionType> constructionTypes = new List<LOMO.ConstructionType>();
        UnderwritingController underwritingController = new UnderwritingController();

        #region Property

        public ActionResult Property()
        {
            RiskPropertyViewModel risk = new RiskPropertyViewModel();
            return View(risk);
        }



        public ActionResult GetRouteTypes()
        {
            try
            {
                if (routeTypes.Count == 0)
                {
                    routeTypes = DelegateService.propertyService.GetRouteTypes();
                }

                var list = routeTypes.Select(item => new { item.Id, item.Description, item.SimilarStreetTypeCd }).ToList();

                return new UifJsonResult(true, list.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {

                return new UifSelectResult(false, "Error Obteniendo Direccion");
            }

        }

        public ActionResult GetSuffixes()
        {
            try
            {
                if (suffixes.Count == 0)
                {
                    suffixes = DelegateService.propertyService.GetSuffixes();
                }

                var list = suffixes.Select(item => new { item.Id, item.Description }).ToList();

                return new UifJsonResult(true, list.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {

                return new UifJsonResult(false, "Error Obteniendo Sufijos");
            }

        }

        public ActionResult GetApartmentsOrOffices()
        {
            try
            {
                if (aparmentOrOffices.Count == 0)
                {
                    aparmentOrOffices = DelegateService.propertyService.GetAparmentOrOffices();
                }

                var list = aparmentOrOffices.Select(item => new { item.Id, item.Description }).ToList();

                return new UifJsonResult(true, list.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {

                return new UifJsonResult(false, "Error Obteniendo datos");
            }

        }

        public List<CompanyPropertyRisk> GetTemporalById(int id)
        {
            try
            {
                var risks = DelegateService.propertyService.GetTemporalById(id);
                return risks;

            }
            catch (Exception)
            {
                return null;
            }
        }

        [IgnoreValidation]
        public ActionResult GetRiskById(int temporalId, int id)
        {
            try
            {
                CompanyPropertyRisk risks = DelegateService.propertyService.GetCompanyRiskByRiskId(temporalId, id);
                return new UifJsonResult(true, risks);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchRisk);
            }
        }

        public ActionResult GetInsuredObjectsByTemporalIdRiskId(int temporalId, int riskId)
        {

            try
            {
                var risks = DelegateService.propertyService.GetCompanyInsuredObjectsByTemporalIdRiskId(temporalId, riskId);


                return new UifJsonResult(true, risks);

            }
            catch
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchRisk);
            }

        }

        public ActionResult GetInsuredObjectByTemporalIdRiskIdInsuredObjectId(int temporalId, int riskId, int insuredObjectId)
        {
            try
            {
                var risks = DelegateService.propertyService.GetCompanyInsuredObjectByTemporalIdRiskIdInsuredObjectId(temporalId, riskId, insuredObjectId);
                return new UifJsonResult(true, risks);
            }
            catch
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchRisk);
            }
        }

        public JsonResult GetCountries()
        {
            try
            {
                if (countries.Count == 0)
                {
                    countries = DelegateService.commonService.GetCountries();
                    countriesCommon = DelegateService.commonService.GetCountries();
                }
                var list = countries.Select(item => new { item.Id, item.Description }).ToList();
                return new UifJsonResult(true, list.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, null);
            }

        }

        public JsonResult GetRisksByTemporalId(int temporalId)
        {
            try
            {
                var risks = DelegateService.propertyService.GetTemporalById(temporalId);
                return new UifJsonResult(true, risks);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchRisk);
            }
        }

        [HttpPost]
        public ActionResult DeleteRisk(int temporalId, int riskId)
        {
            try
            {

                bool result = DelegateService.propertyService.DeleteCompanyRisk(temporalId, riskId);
                return new UifJsonResult(true, result);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorDeleteRisk);
            }
        }


        public JsonResult GetStatesByCountryId(int countryId = 0)
        {
            try
            {
                Country country = (from c in countries where c.Id == countryId select c).FirstOrDefault();
                if (country != null)
                {
                    var list = country.States.Select(item => new { item.Id, item.Description }).ToList();
                    return new UifJsonResult(true, list.OrderBy(x => x.Description));
                }
                else
                {
                    return new UifJsonResult(true, new List<State>());
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorConsultingDepartments);
            }

        }
        public ActionResult GetStatesByCountryIdNew()
        {
            try
            {
                return new UifSelectResult(new List<State>());

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorConsultingDepartments); throw;
            }

        }

        public ActionResult GetCitiesByCountryIdStateId(int countryId, int stateId)
        {
            List<City> cities = (from c in countries
                                 from s in c.States
                                 where c.Id == countryId && s.Id == stateId
                                 select s.Cities).FirstOrDefault();

            if (cities != null)
            {
                return new UifJsonResult(true, cities.OrderBy(x => x.Description));
            }
            else
            {
                return new UifJsonResult(true, new List<City>());
            }
        }

        public ActionResult GetRiskActivitiesByProductId(int productId)
        {
            try
            {
                List<RiskActivity> riskActivities = DelegateService.underwritingService.GetRiskActivitiesByProductIdDescription(productId);
                riskActivities.ForEach(x => x.Description = x.Description + "(" + x.Id.ToString() + ")");
                return new UifJsonResult(true, riskActivities.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCoverage);
            }
        }

        [HttpPost]
        public ActionResult GetDaneCodeByCountryIdByStateIdByCityId(int countryId, int stateId, int cityId)
        {
            try
            {

                string testcg = (from c in countries
                                 from s in c.States
                                 from cy in s.Cities
                                 where c.Id == countryId && s.Id == stateId && cy.Id == cityId
                                 select cy.Description).FirstOrDefault();

                string daneCode = (from c in countries
                                   from s in c.States
                                   from cy in s.Cities
                                   where c.Id == countryId && s.Id == stateId && cy.Id == cityId
                                   select cy.DANECode).FirstOrDefault();

                if (!string.IsNullOrEmpty(daneCode))
                {
                    return new UifJsonResult(true, daneCode);
                }
                else
                {
                    return new UifJsonResult(false, "");
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, new { ErrorMsg = ex.Message });
            }
        }
        public JsonResult GetRatingZonesByPrefixIdCountryIdStateId(int prefixId, int countryId, int stateId)
        {
            try
            {
                RatingZone ratingZone = DelegateService.underwritingService.GetRatingZonesByPrefixIdCountryIdStateId(prefixId, countryId, stateId);
                return new UifJsonResult(true, ratingZone);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, new { ErrorMsg = ex.Message });
            }
        }
        public JsonResult GetDaneCodeByQuery(string query)
        {
            List<City> city = (from c in countriesCommon
                               from s in c.States
                               from cy in s.Cities
                               select cy).ToList();
            var dataFiltered = city.Where(d => d.DANECode != null && d.DANECode.Contains(query));
            if (!String.IsNullOrEmpty(query))
            {
                return Json(dataFiltered, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(city, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetStateCityByDaneCode(string daneCode, int countryId)
        {
            try
            {
                City city = (from c in countriesCommon
                             from s in c.States
                             from cy in s.Cities
                             where c.Id == countryId && cy.DANECode == daneCode
                             select cy).FirstOrDefault();
                if (city != null)
                {
                    return new UifJsonResult(true, city);
                }
                else
                {
                    return new UifJsonResult(false, null);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, new { ErrorMsg = ex.Message });
            }
        }
        public JsonResult GetGroupCoverages(int productId)
        {
            try
            {
                List<GroupCoverage> groupCoverages = DelegateService.underwritingService.GetProductCoverageGroupRiskByProductId(productId);
                return new UifJsonResult(true, groupCoverages.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryCoverageGroups);
            }
        }
        public ActionResult GetInsuredsObjectsByProductIdGroupCoverageId(int productId, int groupCoverageId, int prefixId)
        {
            try
            {
                List<CompanyInsuredObject> companyInsuredObjects = DelegateService.underwritingService.GetCompanyInsuredObjectsByProductIdGroupCoverageId(productId, groupCoverageId, prefixId);
                if (companyInsuredObjects != null && companyInsuredObjects.Any())
                {
                    return new UifJsonResult(true, companyInsuredObjects.OrderBy(x => x.Description).ToList());
                }
                else
                {
                    return new UifJsonResult(true, null);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorConsultingInsuranceObjects);
            }
        }
        public ActionResult GetInsuredObjectsByProductIdGroupCoverageId(Boolean allInsuredObject, RiskPropertyViewModel riskModel, Boolean isSelected = false)
        {
            try
            {
                if (riskModel != null)
                {
                    CompanyPropertyRisk risk = ModelAssembler.CreatePropertyRisk(riskModel);

                    var companyInsuredObjects = DelegateService.propertyService.GetInsuredObjectsByProductIdGroupCoverageId(allInsuredObject, risk, isSelected);
                    if (companyInsuredObjects != null)
                    {
                        return new UifJsonResult(true, companyInsuredObjects.OrderBy(x => x.Description).ToList());
                    }
                    else
                    {
                        return new UifJsonResult(true, companyInsuredObjects.OrderBy(x => x.Description).ToList());
                    }
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryCoverageGroups);
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "BusinessException")
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.NoInsuranceObjects);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryCoverageGroups);
                }
            }
        }

        #endregion

        #region Reglas
        private List<CompanyInsuredObject> Runrules(int groupCoverageId, List<CompanyInsuredObject> companyInsuredObjects, int temporalId, CompanyPropertyRisk companyPropertyRisk)
        {

            var risks = DelegateService.propertyService.Runrules(groupCoverageId, companyInsuredObjects, temporalId, companyPropertyRisk);
            return risks;
        }

        #endregion
        [HttpPost]
        public ActionResult SaveRisk(int temporalId, RiskPropertyViewModel riskModel, List<CompanyInsuredObject> insuredObjects, List<DynamicConcept> dynamicProperties)
        {
            try
            {
                ModelState.Remove("riskModel.AdditionalDataViewModel.ConstructionYear");
                ModelState.Remove("riskModel.AdditionalDataViewModel.RiskAge");
                ModelState.Remove("riskModel.AdditionalDataViewModel.Latitude");
                ModelState.Remove("riskModel.AdditionalDataViewModel.Longitude");
                ModelState.Remove("riskModel.AdditionalDataViewModel.irregularPlant");
                ModelState.Remove("riskModel.AdditionalDataViewModel.irregularHeight");
                ModelState.Remove("riskModel.AdditionalDataViewModel.PreviousDamage");
                ModelState.Remove("riskModel.AdditionalDataViewModel.Repaired");
                ModelState.Remove("riskModel.AdditionalDataViewModel.ReinforcedStructure");
                ModelState.Remove("riskModel.AdditionalDataViewModel.FloorNumber");
                ModelState.Remove("riskModel.AdditionalDataViewModel.RiskUse");
                ModelState.Remove("riskModel.AdditionalDataViewModel.RiskType");
                ModelState.Remove("riskModel.AdditionalDataViewModel.ConstructionType");
                ModelState.Remove("riskModel.AdditionalDataViewModel.DeclarationPeriod");


                if (ModelState.IsValid)
                {
                    CompanyPropertyRisk propertyRisk = ModelAssembler.CreatePropertyRisk(riskModel);
                    propertyRisk.Risk.Clauses = riskModel.Clauses;
                    propertyRisk.Risk.Text = riskModel.Text;
                    if (insuredObjects == null)
                    {
                        insuredObjects = new List<CompanyInsuredObject>();
                    }
                    propertyRisk.InsuredObjects = insuredObjects;
                    propertyRisk.Risk.DynamicProperties = dynamicProperties;
                    var risks = DelegateService.propertyService.SaveCompanyRisk(temporalId, propertyRisk);
                    return new UifJsonResult(true, risks);
                }
                else
                {
                    string errorMessage = GetErrorMessages();
                    return new UifJsonResult(false, errorMessage);
                }
            }
            catch (Exception ex)
            {
                if (riskModel != null && riskModel.RiskId > 0)
                {
                    DelegateService.underwritingService.DeleteCompanyRisksByRiskId((int)riskModel.RiskId, false);
                }
                return new UifJsonResult(false, ex.GetBaseException().Message);
            }
        }

        public ActionResult DeleteInsuredObjectByRiskIdInsuredObjectId(int temporalId, int riskId, int insuredObjectId)
        {
            try
            {

                var companyObject = DelegateService.propertyService.DeleteInsuredObjectByRiskIdInsuredObjectId(temporalId, riskId, insuredObjectId);
                return new UifJsonResult(true, companyObject);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorRemovingObjectInsurance);
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


        public ActionResult ConvertProspectToInsured(int temporalId, int individualId, string documentNumber)
        {
            try
            {

                var result = DelegateService.propertyService.ConvertProspectToInsured(temporalId, individualId, documentNumber);

                return new UifJsonResult(true, result);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorConvertingProspectIntoIndividual);
            }
        }

        public ActionResult GetCompanyRisksByTemporalId(int temporalId)
        {
            try
            {
                List<CompanyRisk> risks = DelegateService.underwritingService.GetCompanyRisksByTemporalId(temporalId, false);

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

        #region reglas

        public ActionResult RunRulesRisk(int policyId, int? ruleSetId)
        {
            try
            {
                CompanyPolicy policy = new CompanyPolicy
                {
                    Id = policyId,
                    IsPersisted = false
                };

                CompanyPropertyRisk propertyRisk = new CompanyPropertyRisk

                {
                    Risk = new CompanyRisk
                    {
                        CoveredRiskType = CoveredRiskType.Location,
                        Policy = policy
                    }
                };

                if (ruleSetId.GetValueOrDefault() > 0)
                {
                    propertyRisk.Risk.IsPersisted = true;
                    propertyRisk = DelegateService.propertyService.RunRulesRisk(propertyRisk, ruleSetId.Value);
                }

                return new UifJsonResult(true, propertyRisk);
            }
            catch (Exception ex)
            {
                throw new Exception(App_GlobalResources.Language.ErrorRunningPreRules, ex);
            }
        }

        public ActionResult RunRulesCoverage(int riskId, CompanyCoverage coverage, int ruleSetId)
        {
            try
            {
                CompanyPropertyRisk risk = DelegateService.propertyService.GetCompanyPropertyRiskByRiskId(riskId);
                if (risk != null)
                {
                    return new UifJsonResult(true, DelegateService.propertyService.RunRulesCoverage(risk, coverage, coverage.RuleSetId.Value));
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(App_GlobalResources.Language.ErrorRunningPreRules, ex);
            }
        }
        #endregion

        #region AdditionalData

        public ActionResult GetConstructionType()
        {
            if (constructionTypes.Count == 0)
            {
                constructionTypes = DelegateService.propertyService.GetConstructionTypes();
            }
            return new UifJsonResult(true, constructionTypes);
        }

        public JsonResult GetRiskTypes()
        {
            try
            {
                List<RiskType> riskTypes = DelegateService.propertyService.GetRiskTypes();
                return new UifJsonResult(true, riskTypes.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetTypes);
            }

        }
        public ActionResult GetRiskUses()
        {
            try
            {
                if (riskUses.Count == 0)
                {
                    riskUses = DelegateService.propertyService.GetRiskUses();
                }
                return new UifJsonResult(true, riskUses);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetUses);
            }
        }


        public ActionResult SaveAdditionalData(int riskId, PropertyAdditionalDataViewModel propertyAdditionalDataViewModel)
        {
            try
            {
                var immap = ModelAssembler.CreateMapAdditionalData();
                var companyPropertyRisk = immap.Map<PropertyAdditionalDataViewModel, CompanyPropertyRisk>(propertyAdditionalDataViewModel);
                var propertyRisk = DelegateService.propertyService.SaveAdditionalData(riskId, companyPropertyRisk);
                return new UifJsonResult(true, propertyRisk);
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "BusinessException")
                {
                    return new UifJsonResult(false, ex.Message);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveAdditionalData);
                }

            }
        }

        #endregion
        #region Texts

        public ActionResult SaveTexts(int riskId, TextsModelsView textModel)
        {
            try
            {
                var companyText = ModelAssembler.CreateText(textModel);               
                var CompanyText = DelegateService.propertyService.SaveTexts(riskId, companyText);
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

        public ActionResult SaveTextsByCoverageId(int riskId, int coverageId, TextsModelsView textModel)
        {
            try
            {
                CompanyPropertyRisk risk = DelegateService.propertyService.GetCompanyPropertyRiskByRiskId(riskId);
                if (risk != null)
                {
                    CompanyCoverage coverageRisk = risk.Risk.Coverages.Where(x => x.Id == coverageId).FirstOrDefault();
                    coverageRisk.Text = ModelAssembler.CreateText(textModel);

                    risk = DelegateService.propertyService.CreatePropertyTemporal(risk, false);
                    if (risk != null)
                    {
                        return new UifJsonResult(true, risk.Risk.Text);
                    }
                    else
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveTexts);
                    }
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.NoExistCoverage);
                }

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveTexts);
            }
        }

        #endregion
        #region Clauses

        public ActionResult SaveClauses(int temporalId, int riskId, List<CompanyClause> clauses)
        {
            try
            {
                if (clauses != null && clauses.Any())
                {
                    var companyclauses = DelegateService.underwritingService.SaveCompanyClauses(temporalId, clauses);
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

        public ActionResult SaveClausesByCoverageId(int riskId, int coverageId, List<CompanyClause> clauses)
        {
            try
            {
                if (clauses != null)
                {
                    var companyCoverage = DelegateService.underwritingService.SaveCompanyClauses(riskId, clauses);
                    return new UifJsonResult(true, companyCoverage);
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
        #region Beneficiary

        public ActionResult SaveBeneficiaries(int riskId, List<CompanyBeneficiary> beneficiaries)
        {
            try
            {
                CompanyPropertyRisk risk = DelegateService.propertyService.GetCompanyPropertyRiskByRiskId(riskId);
                if (risk.Risk.Id > 0)
                {
                    risk.Risk.Beneficiaries = beneficiaries;
                    risk = DelegateService.propertyService.CreatePropertyTemporal(risk, false);

                    if (risk != null)
                    {
                        return new UifJsonResult(true, beneficiaries);
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
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveBeneficiaries);
            }
        }

        #endregion
        #region Coverages
        public ActionResult SaveCoverage(int riskId, List<CompanyCoverage> coverageModel, int temporalId, int insuredObjectId, string insuredObjectDesc, int? declarationPeriodId, int? billingPeriodId)
        {
            try
            {
                if (coverageModel != null)
                {
                    CompanyPropertyRisk risk = DelegateService.propertyService.GetCompanyPropertyRiskByRiskId(riskId);
                    if (risk != null)
                    {
                        if (risk.Risk.Coverages != null)
                        {
                            risk.Risk.Coverages?.RemoveAll(x => x.InsuredObject.Id == insuredObjectId);
                            risk.Risk.Coverages.AddRange(coverageModel);
                        }
                        else
                        {
                            risk.Risk.Coverages = new List<CompanyCoverage>();
                            risk.Risk.Coverages?.RemoveAll(x => x.InsuredObject.Id == insuredObjectId);
                            risk.Risk.Coverages.AddRange(coverageModel);
                        }
                        risk.Risk.Coverages.ForEach(x => { x.CurrentFrom = risk.Risk.Policy.CurrentFrom; x.CurrentTo = risk.Risk.Policy.CurrentTo; });
                        CompanyInsuredObject insuredObject = risk.InsuredObjects.Where(x => x.Id == insuredObjectId).FirstOrDefault();
                        if (insuredObject == null)
                        {
                            risk.InsuredObjects.Add(risk.Risk.Coverages.Where(x => x.InsuredObject.Id == insuredObjectId).FirstOrDefault().InsuredObject);
                            risk.InsuredObjects.Where(z => z.Id == insuredObjectId).FirstOrDefault().Description = insuredObjectDesc;
                        }
                        foreach (var item in risk.InsuredObjects)
                        {
                            if (item.IsDeclarative && item.Id == insuredObjectId)
                            {
                                risk.DeclarationPeriod = new DeclarationPeriod
                                {
                                    Id = Convert.ToInt32(declarationPeriodId)
                                };
                                risk.DeclarationPeriodCode = Convert.ToInt32(declarationPeriodId);
                                risk.AdjustPeriod = new AdjustPeriod
                                {
                                    Id = Convert.ToInt32(billingPeriodId)
                                };
                                risk.BillingPeriodDepositPremium = Convert.ToInt32(billingPeriodId);
                            }
                        }

                        risk = DelegateService.propertyService.CreatePropertyTemporal(risk, false);
                        if (risk != null)
                        {
                            return new UifJsonResult(true, true);
                        }
                        else
                        {
                            return new UifJsonResult(false, null);
                        }
                    }
                    else
                    {
                        return new UifJsonResult(false, null);
                    }
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.MessageValidateCoverage);
                }

            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveRisk);
            }
        }

        public ActionResult GetCoveragesByInsuredObjectIdGroupCoverageIdProductId(int insuredObjectId, int groupCoverageId, int productId)
        {
            try
            {

                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByInsuredObjectIdGroupCoverageIdProductId(insuredObjectId, groupCoverageId, productId).Where(u => u.IsSelected == true).OrderBy(u => u.Number).ToList();
                if (coverages != null && coverages.Any())
                {
                    return new UifJsonResult(true, coverages);
                }
                else
                {
                    return new UifJsonResult(true, null);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryCoverageGroups);
            }
        }



        public ActionResult GetTemporalCoveragesByRiskIdInsuredObjectId(int riskId, int insuredObjectId)
        {
            List<CompanyCoverage> coverages;
            try
            {
                coverages = DelegateService.propertyService.GetTemporalCoveragesByRiskIdInsuredObjectId(riskId, insuredObjectId);
                return new UifJsonResult(true, coverages.OrderBy(x => x.CoverNum));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCoverages);
            }
        }

        public UifJsonResult GetCalculateCoveragesByInsuredObjectId(int riskId, CompanyInsuredObject insuredObject,
            decimal depositPremiumPercent, decimal rate, DateTime currentFrom, DateTime currentTo, decimal insuredLimit, bool runRulesPre, bool runRulesPost)
        {
            List<CompanyCoverage> coverages;
            try
            {
                coverages = DelegateService.propertyService.GetCalculateCoveragesByInsuredObjectId(riskId, insuredObject, depositPremiumPercent,
                    rate, currentFrom, currentTo, insuredLimit, runRulesPre, runRulesPost);
                return new UifJsonResult(true, coverages.OrderBy(x => x.CoverNum));

            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCoverages);
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

        public ActionResult GetRateTypes()
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

        public JsonResult GetFirstRiskTypes()
        {
            try
            {
                return new UifJsonResult(true, EnumsHelper.GetItems<FirstRiskType>());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCoveredRiskTypes);
            }
        }

        public ActionResult GetCalculeTypes()
        {
            try
            {
                return new UifJsonResult(true, EnumsHelper.GetItems<Core.Services.UtilitiesServices.Enums.CalculationType>());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryCalculationTypes);
            }
        }

        public JsonResult GetTotalLineBusinessByPrefixId(int prefixId)
        {
            try
            {
                List<LineBusiness> lineBusiness = DelegateService.commonService.GetLinesBusinessByPrefixId(prefixId);
                return new UifJsonResult(true, lineBusiness.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetLineBusiness);
            }
        }

        public JsonResult GetCoverages(int productId, int coverageGroupId, int prefixId)
        {
            try
            {
                List<CompanyCoverage> coverages = new List<CompanyCoverage>();
                coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(productId, coverageGroupId, prefixId);
                return new UifJsonResult(true, coverages.OrderBy(x => x.CoverNum));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCoverages);
            }

        }

        public ActionResult GetCoveragesByCoverageId(int productId, int coverageGroupId, int prefixId, int coverageId)
        {
            try
            {
                var coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(productId, coverageGroupId, prefixId);
                if (coverages != null && coverages.Any())
                {
                    coverages = coverages.Where(x => x.Id == coverageId).ToList();
                    return new UifJsonResult(true, coverages.OrderBy(x => x.CoverNum));
                }
                else
                {
                    return new UifJsonResult(false, "Error no existen Coberturas");
                }
            }
            catch (Exception)
            {

                return new UifJsonResult(false, "Error Obteniendo Coberturas");
            }

        }

        public ActionResult GetCoveragesByCoveragesAdd(int productId, int coverageGroupId, int prefixId, string coveragesAdd, int insuredObjectId)
        {
            List<CompanyCoverage> coverages = new List<CompanyCoverage>();

            try
            {
                coverages = DelegateService.propertyService.GetCoveragesByCoveragesAdd(productId, coverageGroupId, prefixId, coveragesAdd, insuredObjectId);

                if (coverages != null && coverages.Any())
                {
                    return new UifJsonResult(true, coverages.OrderBy(x => x.CoverNum));
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCoverages);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCoverages);
            }
        }

        public ActionResult GetPremium(RiskPropertyViewModel riskModel, List<DynamicConcept> dynamicProperties)
        {
            CompanyPropertyRisk companyRiskProperty;
            try
            {
                if (riskModel != null)
                {
                    companyRiskProperty = ModelAssembler.CreatePropertyRisk(riskModel);
                    dynamicProperties.AsParallel().ForAll(x => { if (x.TypeName == null) x.TypeName = ""; });
                    companyRiskProperty.Risk.DynamicProperties = dynamicProperties;
                    var riskProperty = DelegateService.propertyService.GetPremium(companyRiskProperty);
                    return new UifJsonResult(true, riskProperty);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorTempNoExist);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, null);
            }
        }
        public ActionResult GetDiferences(int riskId, List<CompanyCoverage> defaultCoverages, List<CompanyCoverage> allCoverages)
        {
            try
            {
                CompanyPropertyRisk risk = DelegateService.propertyService.GetCompanyPropertyRiskByRiskId(riskId);
                List<CompanyCoverage> quotationCoverages = new List<CompanyCoverage>();
                allCoverages = (from x in allCoverages
                                where !(from c in defaultCoverages select c.Id).Contains(x.Id)
                                select x).ToList();
                foreach (var item in allCoverages)
                {
                    item.LimitAmount = item.DeclaredAmount;
                }
                defaultCoverages.AddRange(allCoverages);

                foreach (var item in defaultCoverages)
                {
                    quotationCoverages.Add(DelegateService.propertyService.QuotateCoverage(risk, item, false, false));
                }

                return new UifJsonResult(true, quotationCoverages);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public ActionResult QuotationCoverage(int riskId, CompanyCoverage coverage)
        {
            try
            {
                CompanyPropertyRisk risk = DelegateService.propertyService.GetCompanyPropertyRiskByRiskId(riskId);
                coverage = DelegateService.propertyService.QuotateCoverage(risk, coverage, false, true);
                return new UifJsonResult(true, coverage);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQuotationCoverage);
            }
        }

        public ActionResult GetCoverageToAddByRiskId(int tempId, int riskId, int coverageId, int insuredObjectId)
        {

            try
            {
                var coverage = DelegateService.propertyService.GetCoverageToAddByRiskId(tempId, riskId, coverageId, insuredObjectId);

                return new UifJsonResult(true, coverage);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, "Error obteniendo cobertura");
            }
        }

        public ActionResult GetAllyCoverageByCoverage(int tempId, int riskId, int groupCoverageId, CompanyCoverage coverage)
        {
            List<CompanyCoverage> allyCoverages;
            try
            {
                allyCoverages = DelegateService.propertyService.GetAllyCoverageByCoverage(tempId, riskId, groupCoverageId, coverage);
                return new UifJsonResult(true, allyCoverages);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, "Error obteniendo coberturas");
            }
        }


        public ActionResult ExcludeCoverage(int temporalId, int riskId, int riskCoverageId, string description)
        {
            CompanyCoverage coverage;
            try
            {
                coverage = DelegateService.propertyService.ExcludeCoverage(temporalId, riskId, riskCoverageId, description);
                return new UifJsonResult(true, coverage);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, "Error excluyendo coberturas");
            }
        }

        public UifJsonResult GetDeclarationPeriods()
        {
            try
            {
                List<CompanyDeclarationPeriod> companyDeclarationPeriods = DelegateService.propertyService.GetCompanyDeclarationPeriods();
                return new UifJsonResult(true, companyDeclarationPeriods.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDeclarationPeriods);
            }
        }

        public UifJsonResult GetBillingPeriods()
        {
            try
            {
                List<CompanyAdjustPeriod> companyAdjustPeriods = DelegateService.propertyService.GetCompayAdjustPeriods();
                return new UifJsonResult(true, companyAdjustPeriods.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBillingPeriods);
            }
        }

        #endregion
        #region InsuredObject

        public ActionResult SaveInsuredObject(int riskId, CompanyInsuredObject objectModel, int tempId, int groupCoverageId)
        {
            try
            {
                bool result = DelegateService.propertyService.SaveInsuredObject(riskId, objectModel, tempId, groupCoverageId);
                if (result)
                {
                    return new UifJsonResult(true, result);
                }
                else
                {
                    return new UifJsonResult(false, result);
                }

            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.GetBaseException().Message);
            }
        }

        public ActionResult InsuredObject(int temporalId = 0, int riskId = 0, int insuredObjectId = 0)
        {
            InsuredObjectViewModel insuredObjectModel = new InsuredObjectViewModel();
            return View(insuredObjectModel);
        }

        public UifJsonResult GetLineBusinessByPrefixId(int prefixId)
        {
            try
            {
                List<LineBusiness> lb = new List<LineBusiness>();
                LineBusiness lineBusiness = new LineBusiness();
                lb = DelegateService.commonService.GetLinesBusinessByPrefixId(prefixId);
                lineBusiness = (LineBusiness)lb.FirstOrDefault();
                return new UifJsonResult(true, lineBusiness);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorRetrievingBusinessLine);
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
                var companyPolicy = DelegateService.propertyService.UpdateRisks(temporalId);
                return new UifJsonResult(true, companyPolicy);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message.ToString());
            }

        }

        /// <summary>
        /// Realiza el armado de la direccion dependiendo de las abreviaturas (tablas COMM.CO_NOMENCLATURES)
        /// </summary>
        /// <param name="Abreviatura"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetNomenclaturesTask(string Nomenclatura)
        {
            try
            {
                var strings = Nomenclatura.Split(' ');

                var lastPart = strings[strings.Length - 1];

                if (String.IsNullOrEmpty(lastPart))
                {
                    return Json(new List<Application.Common.Entities.CoNomenclatures>(), JsonRequestBehavior.AllowGet);
                }

                string resulted = "";

                foreach (var item in Nomenclatura.Split(' '))
                {
                    if (item != lastPart)
                    {
                        resulted += item + " ";
                    }
                }
                var nomenclatures = DelegateService.commonService.GetNomenclaturesTask(0, lastPart, "", false);

                if (nomenclatures != null)
                {

                    var result1 = nomenclatures.Select(x => new { nomenclature = resulted + " " + x.Nomenclatura, Abreviatura = resulted + " " + x.Abreviatura }).ToList();
                    return Json(result1.OrderBy(x => x.nomenclature), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new List<Application.Common.Entities.CoNomenclatures>(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (System.Exception)
            {

                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult GetCoverageByCoverageId(int coverageId, int riskId, int temporalId, int groupCoverageId)
        {
            try
            {
                CompanyCoverage coverage = DelegateService.propertyService.GetCoverageByCoverageId(coverageId, riskId, temporalId, groupCoverageId);
                return new UifJsonResult(true, coverage);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCoverage);
            }
        }
        #endregion
        public ActionResult GetSubActivityRisksByActivityRiskId(int ActivityRiskId)
        {
            try
            {
                List<CompanyRiskSubActivity> SubActivityRisks = new List<CompanyRiskSubActivity>();
                SubActivityRisks = DelegateService.propertyService.GetSubActivities(ActivityRiskId);
                return new UifJsonResult(true, SubActivityRisks);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQuerySubActivitiesRisk);
            }
        }

        public ActionResult GetAssuranceMode()
        {
            try
            {
                List<CompanyAssuranceMode> AssuranceModes = new List<CompanyAssuranceMode>();
                AssuranceModes = DelegateService.propertyService.GetAssuranceMode();
                return new UifJsonResult(true, AssuranceModes);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryAssuranceMode);
            }
        }
        public ActionResult GetNomenclatures()
        {
            try
            {
                List<Nomenclature> nomenclatures = new List<Nomenclature>();
                nomenclatures = DelegateService.commonService.GetNomenclatures();
                return new UifJsonResult(true, nomenclatures);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryAssuranceMode);
            }
        }
    }
}
