using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.Location.LiabilityServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Services.UtilitiesServices.Enums;
using UUModel = Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Controllers
{
    public class RiskLiabilityController : Controller
    {
        private static List<Country> countries = DelegateService.commonService.GetCountries();
        private static List<CompanyCoverage> coverages = new List<CompanyCoverage>();
        UnderwritingController underwritingController = new UnderwritingController();

        #region
        public ActionResult Liability()
        {
            RiskLiabilityViewModel risk = new RiskLiabilityViewModel();
            return View(risk);
        }

        public ActionResult GetRisksByTemporalId(int temporalId)
        {
            try
            {
                List<CompanyLiabilityRisk> liabilityRisk = DelegateService.liabilityService.GetCompanyLiabilitiesByTemporalId(temporalId);
                if (liabilityRisk != null)
                {
                    return new UifJsonResult(true, liabilityRisk);
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

        public ActionResult GetTemporalById(int id)
        {
            try
            {
                var risks = DelegateService.liabilityService.GetTemporalById(id);
                return new UifSelectResult(risks);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchRisk);
            }
        }

        public ActionResult GetRiskById(int id)
        {
            try
            {
                CompanyLiabilityRisk risk = DelegateService.liabilityService.GetRiskById(id);
                return new UifJsonResult(true, risk);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchRisk);
            }
        }

        public ActionResult GetCountries()
        {
            try
            {
                var list = countries.Select(item => new { item.Id, item.Description }).ToList();

                return new UifJsonResult(true, list.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, null);
            }

        }

        public ActionResult DeleteRisk(int temporalId, int riskId)
        {
            try
            {
                bool result = DelegateService.liabilityService.DeleteRisk(temporalId, riskId);
                return new UifJsonResult(true, result);
            }
            catch (Exception ex)
            {

                if (ex.GetType().Name == "BusinessException")
                {
                    return new UifJsonResult(false, ex.GetBaseException().Message);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorDeleteRisk);
                }

            }
        }

        public ActionResult GetStatesByCountryId(int countryId = 0)
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
            return new UifSelectResult(new List<State>());
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
                List<UUModel.RiskActivity> riskActivities = DelegateService.underwritingService.GetRiskActivitiesByProductIdDescription(productId);
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
            List<City> city = (from c in countries
                               from s in c.States
                               from cy in s.Cities
                               select cy).ToList();

            var dataFiltered = city.Where(d => d.DANECode != null && d.DANECode.Contains(query));

            if (!String.IsNullOrEmpty(query))
                return Json(dataFiltered, JsonRequestBehavior.AllowGet);
            else
                return Json(city, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetStateCityByDaneCode(string daneCode, int countryId)
        {
            try
            {
                City city = (from c in countries
                             from s in c.States
                             from cy in s.Cities
                             where c.Id == countryId && cy.DANECode == daneCode
                             select cy).FirstOrDefault();

                if (city != null)
                    return new UifJsonResult(true, city);
                else
                    return new UifJsonResult(false, null);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, new { ErrorMsg = ex.Message });
            }
        }
        public ActionResult GetGroupCoverages(int productId)
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

        public ActionResult GetInsuredObjectsByProductIdGroupCoverageId(int productId, int groupCoverageId, int prefixId)
        {
            try
            {
                List<CompanyInsuredObject> companyInsuredObjects = DelegateService.underwritingService.GetCompanyInsuredObjectsByProductIdGroupCoverageId(productId, groupCoverageId, prefixId);
                return Json(companyInsuredObjects, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "BusinessException")
                {
                    return new UifJsonResult(false, ex.GetBaseException().Message);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryCoverageGroups);
                }
            }
        }

        public ActionResult SaveRisk(RiskLiabilityViewModel riskModel, List<CompanyCoverage> coverages, List<Sistran.Core.Application.RulesScriptsServices.Models.DynamicConcept> dynamicProperties)
        {
            try
            {
                CompanyLiabilityRisk liabilityRisk = new CompanyLiabilityRisk();
                liabilityRisk = ModelAssembler.CreateLiabilityRisk(riskModel);
                liabilityRisk.Risk.Coverages = coverages ?? new List<CompanyCoverage>();
                liabilityRisk.Risk.DynamicProperties = dynamicProperties;
                liabilityRisk = DelegateService.liabilityService.SaveRisk(liabilityRisk, riskModel.TemporalId, riskModel.RiskId, riskModel.EndorsementType);

                return new UifJsonResult(true, liabilityRisk);
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "BusinessException")
                {
                    return new UifJsonResult(false, ex.GetBaseException().Message);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveRisk);
                }
            }
        }

        public ActionResult GetPremium(RiskLiabilityViewModel riskModel, List<CompanyCoverage> coverages, List<DynamicConcept> dynamicProperties)
        {
            try
            {
                if (riskModel.TemporalId > 0)
                {
                    CompanyLiabilityRisk liabilityRisk = ModelAssembler.CreateLiabilityRisk(riskModel);
                    liabilityRisk = DelegateService.liabilityService.GetPremium(liabilityRisk, coverages, dynamicProperties, riskModel.TemporalId);
                    return new UifJsonResult(true, liabilityRisk);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchTemp);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }
        private string GetErrorMessages()
        {
            StringBuilder sb = new StringBuilder();
            int cont = 0;
            foreach (ModelState item in ModelState.Values)
            {
                if (item.Errors.Count > 0)
                {
                    sb.Append(item.Errors[0].ErrorMessage).Append(", ");
                }
                cont++;
            }
            return sb.ToString().Remove(sb.ToString().Length - 2);
        }

        #endregion

        public ActionResult ConvertProspectToInsured(int temporalId, int individualId, string documentNumber)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.liabilityService.ConvertProspectToInsured(temporalId, individualId, documentNumber));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorConvertingProspectIntoIndividual);
            }
        }

        #region Texts
        public ActionResult GetTextsByName(string name)
        {
            try
            {
                List<UUModel.Text> texts = DelegateService.underwritingService.GetTextsByNameLevelIdConditionLevelId(name, (int)Core.Application.UnderwritingServices.Enums.EmissionLevel.Coverage, (int)ConditionLevelType.Prefix);
                return new UifJsonResult(true, texts);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchText);
            }
        }

        public ActionResult SaveTexts(int riskId, TextsModelsView textModel)
        {
            try
            {
                CompanyLiabilityRisk risk = DelegateService.liabilityService.GetCompanyLiabilityByRiskId(riskId);
                if (risk.Risk.Id > 0)
                {
                    risk.Risk.Text = ModelAssembler.CreateText(textModel);

                    if (risk != null)
                    {
                        DelegateService.liabilityService.CreateLiabilityTemporal(risk, false);
                        return new UifJsonResult(true, risk.Risk.Text);
                    }
                    else
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveText);
                    }
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.NoExistRisk);
                }
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveText);
            }
        }
        #endregion

        #region Clauses

        public ActionResult GetClauses()
        {
            try
            {
                List<Clause> clauses = DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(Core.Application.UnderwritingServices.Enums.EmissionLevel.Risk, (int)CoveredRiskType.Location);
                return new UifJsonResult(true, clauses.OrderBy(x => x.Title));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchClauses);
            }
        }

        public ActionResult SaveClauses(int riskId, List<CompanyClause> clauses)
        {
            try
            {
                bool result = DelegateService.liabilityService.SaveClause(riskId, clauses);
                return new UifJsonResult(true, clauses);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveClauses);
            }
        }

        #endregion

        #region Coverages

        public ActionResult GetCoveragesByProductIdGroupCoverageId(int temporalId, int productId, int groupCoverageId, int prefixId)
        {
            try
            {
                List<CompanyCoverage> coverages = DelegateService.liabilityService.GetCoveragesByProductIdGroupCoverageId(temporalId, productId, groupCoverageId, prefixId);
                return new UifJsonResult(true, coverages);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryCoverageGroups);
            }
        }

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
                CompanyLiabilityRisk liabilityRisk = DelegateService.liabilityService.GetCompanyLiabilityByRiskId(riskId);
                return new UifJsonResult(true, liabilityRisk.Risk.Coverages);
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
                List<CompanyCoverage> coverage = DelegateService.liabilityService.GetCoverageByCoverageId(coverageId, riskId, temporalId, groupCoverageId);
                return new UifJsonResult(true, coverage);
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
                coverages = coverages.Where(c => (!idCoverages.Any(x => (String.IsNullOrEmpty(x) ? 0 : Convert.ToInt32(x)) == c.Id)) && c.IsVisible == true).ToList();
                return new UifJsonResult(true, coverages.OrderBy(x => x.Description));
            }
            catch (Exception ex)
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

        public ActionResult GetRateTypes()
        {
            try
            {
                return new UifSelectResult(EnumsHelper.GetItems<RateType>());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryRateTypes);
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

        public ActionResult QuotationCoverage(CompanyCoverage coverage, int riskId, bool runRulesPost)
        {
            try
            {
                CompanyLiabilityRisk risk = DelegateService.liabilityService.GetCompanyLiabilityByRiskId(riskId);
                coverage = DelegateService.liabilityService.QuotationCompanyCoverage(risk, coverage, false, runRulesPost);
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
                Boolean result = DelegateService.liabilityService.SaveCoverages(temporalId, riskId, coverages);
                if (result)
                {
                    return new UifJsonResult(true, true);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.NoExistTemporaryNoHaveCoverages);
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
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveCoverages);
                }
            }
        }

        public ActionResult ExcludeCoverage(int temporalId, int riskId, int riskCoverageId, string description)
        {
            CompanyCoverage coverage = DelegateService.liabilityService.ExcludeCoverage(temporalId, riskId, riskCoverageId, description);
            return new UifJsonResult(true, coverage);
        }

        public ActionResult GetAllyCoverageByCoverage(int tempId, int riskId, int groupCoverageId, CompanyCoverage coverage)
        {
            try
            {
                List<CompanyCoverage> coverages = DelegateService.liabilityService.GetAllyCoverageByCoverage(tempId, riskId, groupCoverageId, coverage);
                return new UifJsonResult(true, coverages);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingAlliedCoverage);
            }
        }

        public ActionResult GetAddCoverageByCoverage(int tempId, int riskId, int groupCoverageId, CompanyCoverage coverage)
        {
            try
            {
                List<CompanyCoverage> coverages = DelegateService.liabilityService.GetAddCoveragesByCoverage(tempId, riskId, groupCoverageId, coverage);
                return new UifJsonResult(true, coverages);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingAlliedCoverage);
            }
        }

        #endregion

        #region Beneficiary

        public ActionResult SaveBeneficiaries(int riskId, List<CompanyBeneficiary> beneficiaries)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.liabilityService.SaveBeneficiaries(riskId, beneficiaries));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveBeneficiaries);
            }
        }

        #endregion

        #region reglas

        public ActionResult RunRulesRiskPreLiability(int policyId, int? ruleSetId)
        {
            try
            {
                CompanyLiabilityRisk liabilityRisk = DelegateService.liabilityService.RunRulesRiskPreLiability(policyId, ruleSetId);
                return new UifJsonResult(true, liabilityRisk);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public ActionResult RunRulesCoveragePreLiability(int temporalId, int riskId, List<CompanyCoverage> coverages)
        {
            try
            {
                coverages = coverages ?? new List<CompanyCoverage>();
                List<CompanyCoverage> companyLiabilityCoverages = DelegateService.liabilityService.RunRulesCoveragesPreLiability(temporalId, riskId, coverages);
                return new UifJsonResult(true, companyLiabilityCoverages);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Metodo utilizado para la ejecución de las reglas POST de coverturas
        /// </summary>
        /// <param name="temporalId">temporal de la poliza</param>
        /// <param name="riskId">Riesgo que se esta validando</param>
        /// <param name="coverages">Coberturas asociadas al riesgo</param>
        /// <returns></returns>
        public ActionResult RunRulesPostCoverages(int temporalId, int riskId, List<CompanyCoverage> coverages)
        {
            try
            {
                List<CompanyCoverage> companyLiabilityCoverages = DelegateService.liabilityService.RunRulesPostCoverages(temporalId, riskId, coverages);
                return new UifJsonResult(true, companyLiabilityCoverages);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
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
                CompanyPolicy companyPolicy = DelegateService.liabilityService.UpdateRisks(temporalId);
                return new UifJsonResult(true, companyPolicy);
            }
            catch (Exception ex)
            {

                if (ex.GetType().Name == "BusinessException")
                {
                    return new UifJsonResult(false, ex.GetBaseException().Message);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorUpdatePolicy);
                }
            }

        }

        #endregion

        public ActionResult GetSubActivityRisksByActivityRiskId(int ActivityRiskId)
        {
            try
            {
                List<CompanyRiskSubActivity> SubActivityRisks = new List<CompanyRiskSubActivity>();
                SubActivityRisks = DelegateService.liabilityService.GetSubActivities(ActivityRiskId);
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
                AssuranceModes = DelegateService.liabilityService.GetAssuranceMode();
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