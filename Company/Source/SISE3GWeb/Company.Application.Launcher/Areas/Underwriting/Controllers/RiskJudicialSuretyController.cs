using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Sistran.Company.Application.Sureties.JudicialSuretyServices.Models;
using Sistran.Company.Application.Sureties.Models;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.Sureties.JudicialSuretyServices.Enums;
using Sistran.Core.Application.Sureties.JudicialSuretyServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using UNMO = Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Controllers
{
    public class RiskJudicialSuretyController : Controller
    {
        UnderwritingController underwritingController = new UnderwritingController();
        private static List<Country> countries = DelegateService.commonService.GetCountries();
        private static List<Article> ListArticle = new List<Article>();

        public ActionResult JudicialSurety()
        {
            return View();
        }

        public ActionResult CoverageJudicialSurety()
        {
            return View();
        }

        public ActionResult GetRisksByTemporalId(int temporalId)
        {
            try
            {
                List<CompanyJudgement> companyJudgements = DelegateService.judicialSuretyService.GetRisksByTemporalId(temporalId);

                if (companyJudgements != null)
                {
                    return new UifJsonResult(true, companyJudgements);
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
                var risk = DelegateService.judicialSuretyService.GetRiskById(endorsementType, temporalId, id);
                return new UifJsonResult(true, risk);

            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchRisk);
            }
        }

        /// <summary>
        /// Metodo para obtener las capacidades del tomador y asegurador
        /// </summary>
        ///// <returns></returns>
        public ActionResult GetInsuredType()
        {
            try
            {
                return new UifJsonResult(true, EnumsHelper.GetItems<CapacityOf>());
            }
            catch (System.Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetFiles);
            }
        }

        ///// <summary>
        ///// funcion para cargar los tipos de juzgado
        ///// </summary>
        ///// <returns></returns>
        public ActionResult GetCourt()
        {
            try
            {
                List<Court> ListCourt = new List<Court>();
                ListCourt = DelegateService.judicialSuretyService.GetCourts();
                return new UifJsonResult(true, ListCourt);

            }
            catch (System.Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBranchs);
            }
        }

        ///// <summary>
        ///// Funcion para obtener la lista de tipos de articulos
        ///// </summary>
        ///// <returns></returns>
        public ActionResult GetArticle()
        {
            try
            {
                if (ListArticle.Count == 0)
                {
                    ListArticle = DelegateService.judicialSuretyService.GetArticles();
                }
                return new UifJsonResult(true, ListArticle);

            }
            catch (System.Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBranchs);
            }
        }
        public ActionResult GetArticlesByProduct(int productId)
        {
            try
            {
                List<Article> ListArticleProduct = new List<Article>();
                ListArticleProduct = DelegateService.judicialSuretyService.GetArticlesByProductId(productId);
                return new UifJsonResult(true, ListArticleProduct);

            }
            catch (System.Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBranchs);
            }
        }

        ///// <summary>
        ///// Funcion para obtener el listado de paises
        ///// </summary>
        ///// <returns></returns>
        public ActionResult GetCountries()
        {
            try
            {
                if (countries == null || countries.Count == 0)
                {
                    countries = DelegateService.commonService.GetCountries();
                }
                if (countries != null && countries.Any())
                {
                    var list = countries.Select(item => new { item.Id, item.Description }).ToList();
                    return new UifJsonResult(true, list.OrderBy(x => x.Description).ToList());
                }
                else
                {
                    return new UifJsonResult(true, "Error Obteniendo Paises");
                }
            }
            catch (Exception)
            {

                return new UifJsonResult(false, "Error Obteniendo Paises");
            }

        }

        ///// <summary>
        ///// Funcion para obtener las ciudades por id de pais
        ///// </summary>
        ///// <param name="countryId"></param>
        ///// <returns></returns>
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

        ///// <summary>
        ///// Funcion para obtener los municipios por departamento
        ///// </summary>
        ///// <param name="countryId"></param>
        ///// <param name="stateId"></param>
        ///// <returns></returns>
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

        ///// <summary>
        ///// FUncion para obtener el listado de coberturas por Id
        ///// </summary>
        ///// <param name="policyId"></param>
        ///// <param name="groupCoverageId"></param>
        ///// <returns></returns>
        public ActionResult GetCoveragesByProductIdGroupCoverageId(int policyId, int groupCoverageId)
        {
            try
            {

                List<CompanyCoverage> coverages = DelegateService.judicialSuretyService.GetCoveragesByProductIdGroupCoverageId(policyId, groupCoverageId);
                return new UifJsonResult(true, coverages);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCoverages);
            }
        }


        public ActionResult GetCoverageByCoverageId(int coverageId, int groupCoverageId, int policyId)
        {
            try
            {

                CompanyCoverage coverage = DelegateService.judicialSuretyService.GetCoverageByCoverageId(coverageId, groupCoverageId, policyId);
                return new UifJsonResult(true, coverage);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCoverage);
            }
        }

        public ActionResult GetGroupCoverages(int productId)
        {
            try
            {
                List<UNMO.GroupCoverage> groupCoverages = DelegateService.underwritingService.GetProductCoverageGroupRiskByProductId(productId);
                if (groupCoverages != null)
                {
                    return new UifJsonResult(true, groupCoverages.OrderBy(x => x.Description).ToList());
                }
                else
                {
                    return new UifJsonResult(true, null);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(true, null);
            }
        }

        /// <summary>
        /// Funcion para guardar el riesgo del Ramo Judicial
        /// </summary>
        /// <param name="temporalId"></param>
        /// <param name="riskModel"></param>
        /// <param name="coverages"></param>
        /// <param name="dynamicProperties"></param>
        /// <param name="accessories"></param>
        /// <param name="additionalData"></param>
        /// <returns></returns>

        public ActionResult SaveRisk(int temporalId, RiskJudicialSuretyModelsView riskModel, List<CompanyCoverage> coverages, List<DynamicConcept> dynamicProperties)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CompanyJudgement companyJudgement = ModelAssembler.CreateCompanyJudgement(riskModel);
                    companyJudgement.Risk.Coverages = coverages ?? new List<CompanyCoverage>();
                    companyJudgement.Risk.DynamicProperties = dynamicProperties;
                    var resultCompanyJudgement = DelegateService.judicialSuretyService.SaveCompanyRisk(companyJudgement, temporalId);
                    return new UifJsonResult(true, resultCompanyJudgement);
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


        //SaveGuarantees
        public ActionResult SaveGuarantees(int riskId, List<CiaRiskSuretyGuarantee> guarantees)
        {
            try
            {
                List<CiaRiskSuretyGuarantee> lstsGuarantees = DelegateService.judicialSuretyService.SaveGuarantees(riskId, guarantees);
                return new UifJsonResult(true, lstsGuarantees);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveGuarantees);
            }
        }

        public ActionResult GetInsuredGuaranteeRelationPolicy(int guaranteeId)
        {
            try
            {
                var guarantees = DelegateService.judicialSuretyService.GetInsuredGuaranteeRelationPolicy(guaranteeId);
                return new UifJsonResult(true, guarantees);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchGuarantees);
            }
        }

        public ActionResult SaveAdditionalData(AdditionalDataJudicialSuretyModelsView additionalDataModel)
        {
            var additionalData = (dynamic)null;
            CompanyJudgement risk;
            try
            {
                var mapper = ModelAssembler.CreateMapAdditionalDataJudicialSurety();
                additionalData = mapper.Map<AdditionalDataJudicialSuretyModelsView, CompanyJudgement>(additionalDataModel);
                risk = DelegateService.judicialSuretyService.SaveAdditionalData(additionalData);
                return new UifJsonResult(true, risk);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveAdditionalData);
            }
        }

        public ActionResult DeleteRisk(int policyId, int id)
        {
            try
            {
                bool result = false;
                result = DelegateService.judicialSuretyService.DeleteRisk(policyId, id);
                if (result)
                {
                    return new UifJsonResult(true, true);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorDeleteRisk);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorDeleteRisk);
            }
        }

        private string GetErrorMessages()
        {
            int Id = 0;
            StringBuilder sb = new StringBuilder();
            foreach (ModelState item in ModelState.Values)
            {

                if (item.Errors.Count > 0)
                {
                    sb.Append(item.Errors[0].ErrorMessage).Append(", ");
                }
                Id = Id + 1;
            }
            return sb.ToString().Remove(sb.ToString().Length - 2);
        }

        public ActionResult RunRules(int policyId, int? ruleSetId)
        {
            try
            {
                CompanyJudgement judgement = new CompanyJudgement
                {
                    Risk = new CompanyRisk
                    {
                        CoveredRiskType = CoveredRiskType.Surety,
                        IsPersisted = true,
                        Policy = new CompanyPolicy
                        {
                            Id = policyId,
                            IsPersisted = false
                        }
                    }
                };
                if (ruleSetId.GetValueOrDefault() > 0)
                {
                    judgement = DelegateService.judicialSuretyService.RunRules(judgement, ruleSetId);
                }
                return new UifJsonResult(true, judgement);
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
            CompanyPolicy companyPolicy = DelegateService.judicialSuretyService.UpdateRisks(temporalId);
            return new UifJsonResult(true, companyPolicy);
        }

        public ActionResult ConvertProspectToInsured(int temporalId, int individualId, string documentNumber)
        {
            try
            {
                DelegateService.judicialSuretyService.ConvertProspectToInsured(temporalId, individualId, documentNumber);
                return new UifJsonResult(true, true);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorConvertingProspectIntoIndividual);
            }
        }

        #region Beneficiary

        public ActionResult SaveBeneficiaries(int riskId, List<CompanyBeneficiary> beneficiaries)
        {
            try
            {
                List<CompanyBeneficiary> resultBeneficiaries = DelegateService.judicialSuretyService.SaveBeneficiaries(riskId, beneficiaries);
                return new UifJsonResult(true, resultBeneficiaries);
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
                CompanyText companyText = ModelAssembler.CreateText(textModel);
                CompanyText resultCompanyText = DelegateService.judicialSuretyService.SaveTexts(riskId, companyText);
                return new UifJsonResult(true, resultCompanyText);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveText);
            }
        }

        #endregion

        #region Clauses

        public ActionResult SaveClauses(int riskId, List<CompanyClause> clauses)
        {
            try
            {
                clauses = DelegateService.judicialSuretyService.SaveClauses(riskId, clauses);
                return new UifJsonResult(true, clauses);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveClauses);
            }
        }

        #endregion

        #region Coverage

        public ActionResult QuotationCoverage(int tempId, int riskId, CompanyCoverage coverage, CoverageModelsView coverageModel, RiskJudicialSuretyModelsView riskModel, bool runRules)
        {
            coverage = ModelAssembler.CreateCoverageModelsViewToCompanyCoverage(coverage, coverageModel);
            CompanyPolicy judgementPolicy = new CompanyPolicy { Id = tempId, Endorsement = new CompanyEndorsement { EndorsementType = (EndorsementType)coverageModel.EndorsementType } };
            CompanyJudgement judgement = new CompanyJudgement
            {
                Risk = new CompanyRisk
                {
                    Id = riskId,
                    Policy = judgementPolicy
                },
                InsuredActAs = (CapacityOf)riskModel.IdInsuredActsAs,
                HolderActAs = (CapacityOf)riskModel.IdHolderActAs
            };
            coverage = DelegateService.judicialSuretyService.QuotateCoverage(judgement, coverage, false, runRules);
            coverage.CoverStatusName = EnumsHelper.GetItemName<CoverageStatusType>(coverage.CoverStatus);

            return new UifJsonResult(true, coverage);
        }

        public ActionResult GetCoveragesByProductIdGroupCoverageIdPrefixId(int productId, int groupCoverageId, int prefixId, string coveragesAdd)
        {
            try
            {
                string[] idCoverages = coveragesAdd.Split(',');
                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(productId, groupCoverageId, prefixId);
                if (coverages != null)
                {
                    if (idCoverages != null && idCoverages.Length > 0)
                    {
                        coverages = coverages.Where(c => (!idCoverages.Any(x => x == Convert.ToString(c.Id))) && c.IsVisible == true).ToList();
                        return new UifJsonResult(true, coverages.OrderBy(x => x.Description));
                    }
                    else
                    {
                        return new UifJsonResult(true, coverages.OrderBy(x => x.Description));
                    }
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCoverage);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCoverage);
            }
        }

        public ActionResult SaveCoverages(int policyId, int riskId, List<CompanyCoverage> coverages)
        {
            try
            {
                DelegateService.judicialSuretyService.SaveCoverages(policyId, riskId, coverages);
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
                CompanyCoverage companyCoverage = DelegateService.judicialSuretyService.ExcludeCoverage(temporalId, riskId, riskCoverageId, description);
                return new UifJsonResult(true, companyCoverage);

            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorEcludeCoverage);
            }

        }


        #endregion
        public ActionResult GetCalculeTypes()
        {
            try
            {
                return new UifJsonResult(true, EnumsHelper.GetItems<CalculationType>());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryCalculationTypes);
            }
        }
        [HttpPost]
        public ActionResult GetDeductiblesByCoverageId(int coverageId)
        {
            try
            {
                List<Deductible> deductibles = DelegateService.underwritingService.GetDeductiblesByCoverageId(coverageId);
                if (deductibles != null && deductibles.Any())
                {
                    return new UifJsonResult(true, deductibles.OrderBy(x => x.Description));
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ConsultingDeductiblesNull);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorConsultingDeductibles);
            }
        }

        public ActionResult GetPremium(int policyId, RiskJudicialSuretyModelsView riskModel, List<CompanyCoverage> coverages, List<DynamicConcept> dynamicProperties)
        {
            try
            {
                CompanyJudgement companyJudgement = ModelAssembler.CreateCompanyJudgement(riskModel);
                companyJudgement.Risk.Coverages = coverages;
                companyJudgement.Risk.DynamicProperties = dynamicProperties;
                CompanyJudgement riskJudgement = DelegateService.judicialSuretyService.GetPremium(policyId, companyJudgement);  //como llego al GetPremium...??
                return new UifJsonResult(true, riskJudgement);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.GetBaseException().Message);
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

        public ActionResult GetInsuredGuaranteeByIndividualId(int individualId)
        {
            try
            {
                var guarantees = DelegateService.suretyService.GetInsuredGuaranteeByIndividualId(individualId);
                return new UifJsonResult(true, guarantees);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchGuarantees);
            }
        }

        public ActionResult GetRiskActivitiesByProductId(int productId)
        {
            try
            {
                List<RiskActivity> riskActivities = DelegateService.underwritingService.GetRiskActivitiesByProductIdDescription(productId);
                return new UifJsonResult(true, riskActivities.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetActivityRisk);
            }
        }
    }
}