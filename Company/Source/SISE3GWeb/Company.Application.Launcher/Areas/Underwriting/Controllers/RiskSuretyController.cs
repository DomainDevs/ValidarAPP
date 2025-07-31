using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Sistran.Company.Application.Sureties.Models;
using Sistran.Company.Application.Sureties.SuretyServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.DTOs;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Services.UtilitiesServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Controllers
{
    public class RiskSuretyController : Controller
    {
        UnderwritingController underwritingController = new UnderwritingController();
        private static List<SuretyContractType> suretyContractType = new List<SuretyContractType>();
        private static List<SuretyContractCategories> suretyContractCategories = new List<SuretyContractCategories>();
        private static List<Sistran.Core.Application.CommonService.Models.Parameter> parameters = new List<Sistran.Core.Application.CommonService.Models.Parameter>();

        /// <summary>
        /// Riesgo de Cumplimiento
        /// </summary>
        /// <returns></returns>
        public ActionResult Surety()
        {
            RiskSuretyModelsView risk = new RiskSuretyModelsView();
            return View(risk);
        }
        public List<Sistran.Core.Application.CommonService.Models.Parameter> GetParameters()
        {
            try
            {
                parameters.Add(new Sistran.Core.Application.CommonService.Models.Parameter { Description = "EmisionDefaultCountry", Value = DelegateService.commonService.GetParameterByParameterId((int)ParametersTypes.EmisionDefaultCountry).NumberParameter.Value });
                return parameters;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the cia risk by temporal identifier.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <returns></returns>
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

        public ActionResult GetSuretyContractTypes()
        {
            try
            {
                if (suretyContractType.Count == 0)
                {
                    suretyContractType = DelegateService.underwritingService.GetSuretyContractTypes();
                }

                return new UifJsonResult(true, suretyContractType.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult GetSuretyContractCategories()
        {
            try
            {
                if (suretyContractCategories.Count == 0)
                {
                    suretyContractCategories = DelegateService.underwritingService.GetSuretyContractCategories();
                }
                return new UifJsonResult(true, suretyContractCategories.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorContractObject);
            }
        }

        public ActionResult GetAvailableAmountByIndividualId(int individualId, int prefixCd, DateTime issueDate)
        {
            try
            {
                List<Amount> amt = DelegateService.suretyService.GetAvailableAmountByIndividualId(individualId, prefixCd, issueDate);
                if (amt != null)
                {
                    if (amt[0].Value == 0 && amt[1].Value == 0)
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.SecuredWithoutOperative);
                    }
                }

                return new UifJsonResult(true, amt);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchOperativeQuota);
            }
        }

        public ActionResult GetRiskSuretyById(int temporalId, int id)
        {
            try
            {
                var companyContract = DelegateService.suretyService.GetRiskSuretyById(temporalId, id);
                if (companyContract != null)
                {
                    return new UifJsonResult(true, companyContract);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.NoRiskWasFound);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchRisk);
            }
        }

        /// <summary>
        /// Gets the temporal by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public List<CompanyContract> GetTemporalById(int id)
        {
            try
            {

                return DelegateService.suretyService.GetRiskSuretiesById(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult ValidateAvailableAmountByTemporalId(int temporalId)
        {
            try
            {
                var result = DelegateService.suretyService.ValidateRiskByTemporalId(temporalId);
                return new UifJsonResult(true, result);

            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "BusinessException")
                {
                    return new UifJsonResult(false, ex.GetBaseException().Message);
                }
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveTemp);
            }
        }

        public ActionResult GetRisksSuretyByTemporalId(int temporalId)
        {
            try
            {
                var contracts = DelegateService.suretyService.GetCompanySuretiesByTemporalId(temporalId);
                if (contracts != null && contracts.Any())
                {
                    return new UifSelectResult(contracts);
                }
                else
                {
                    return new UifSelectResult(new CompanyContract());
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchRisk);
            }
        }

        public ActionResult SaveRisk(RiskSuretyModelsView riskModel, List<CompanyCoverage> coverages, List<DynamicConcept> dynamicProperties, Boolean validate = true)
        {

            var contract = ModelAssembler.CreateRiskSurety(riskModel);
            CompanyContract risk = DelegateService.suretyService.GetCompanySuretyByRiskId(contract.Risk.Id);
            contract.Guarantees = risk?.Guarantees ?? new List<CiaRiskSuretyGuarantee>();

            contract.Risk.Text = new CompanyText
            {
                TextBody = riskModel.ContractObject
            };

            riskModel.ContractorName = riskModel.ContractorName.Replace("%26", "&");
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(riskModel.TemporalId, false);
                if (policy.TemporalType == TemporalType.Quotation || policy.TemporalType == TemporalType.TempQuotation)
                {
                    ModelState.Remove("riskModel.Available");
                    ModelState.Remove("riskModel.OperatingQuota");
                    ModelState.Remove("riskModel.InsuredName");
                    ModelState.Remove("riskModel.InsuredDetailId");
                    ModelState.Remove("riskModel.ContractorDetailId");
                }

                if (riskModel.IsNational)
                {
                    ModelState.Remove("riskModel.StateId");
                    ModelState.Remove("riskModel.CityId");
                }

                if (ModelState.IsValid)
                {

                    contract.Risk.Coverages = coverages;
                    contract.Risk.DynamicProperties = dynamicProperties;
                    var companycontract = DelegateService.suretyService.SaveCompanyRisk(riskModel.TemporalId, contract);
                    //CompanyContract riskSurety = DelegateService.suretyService.CompanySaveCompanySuretyQuotation(contract);
                    return new UifJsonResult(true, companycontract);
                }
                else
                {
                    //comment
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

        public ActionResult DeleteRisk(int temporalId, int riskId)
        {
            try
            {
                var result = DelegateService.suretyService.DeleteRisk(temporalId, riskId);
                return new UifJsonResult(result, result);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorDeleteRisk);
            }
        }

        public UifJsonResult GetLineBusinessByPrefixId(int prefixId)
        {
            try
            {
                LineBusiness lineBusiness = new LineBusiness();
                lineBusiness = DelegateService.commonService.GetLinesBusinessByPrefixId(prefixId).FirstOrDefault();
                return new UifJsonResult(true, lineBusiness);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorRetrievingBusinessLine);
            }
        }

        public ActionResult GetTotalLineBusinessByPrefixId(int prefixId)
        {
            List<LineBusiness> lineBusiness = DelegateService.commonService.GetLinesBusinessByPrefixId(prefixId);
            return new UifSelectResult(lineBusiness.OrderBy(x => x.Description));
        }

        public ActionResult CrossGuarantees()
        {
            return View();
        }

        public JsonResult GetDate()
        {
            DateTime dateNew = DelegateService.commonService.GetDate();
            return Json(dateNew, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConvertProspectToInsured(int temporalId, int individualId, string documentNumber)
        {
            try
            {

                var result = DelegateService.suretyService.ConvertProspectToInsured(temporalId, individualId, documentNumber);
                return new UifJsonResult(result, result);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorConvertingProspectIntoIndividual);
            }
        }

        #region Texts

        public ActionResult SaveTexts(int riskId, TextsModelsView textModel)
        {
            UnderwritingController underwritingController = new UnderwritingController();
            try
            {
                var text = ModelAssembler.CreateText(textModel);
                if (!string.IsNullOrEmpty(text.TextBody))
                    text.TextBody = underwritingController.unicode_iso8859(text.TextBody);
                var textResult = DelegateService.suretyService.SaveTexts(riskId, text);
                return new UifJsonResult(true, textResult);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveText);
            }
        }

        public ActionResult SaveContractObject(int riskId, TextsModelsView textModel)
        {
            try
            {
                var text = ModelAssembler.CreateText(textModel);
                var textResult = DelegateService.suretyService.SaveContractObject(riskId, text);
                return new UifJsonResult(true, textResult);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveContractObject);
            }
        }
        #endregion

        #region Coverage

        public ActionResult Coverage(SuretyCoverageModelsView suretyCoverageModelsView)
        {
            ModelState.Clear();
            SuretyCoverageModelsView coverage = new SuretyCoverageModelsView();
            return View(coverage);
        }

        public ActionResult SaveCoverages(int tempId, int riskId, List<CompanyCoverage> coverages)
        {
            try
            {
                if (coverages == null)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.CoveragesAreCompulsory);
                }
                DelegateService.suretyService.SaveCoverages(tempId, riskId, coverages);
                return new UifJsonResult(true, true);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveCoverages);
            }
        }

        public ActionResult QuotationCoverage(CompanyCoverage coverage, SuretyCoverageModelsView coverageModel, bool runRulesPost, int policyId, List<CompanyCoverage> listCompanyCoverage = null)
        {
            try
            {
                coverage.Id = coverageModel.CoverageId;
                coverage.CurrentFrom = Convert.ToDateTime(coverageModel.CurrentFrom);
                coverage.CurrentTo = Convert.ToDateTime(coverageModel.CurrentTo);
                coverage.CalculationType = (Core.Services.UtilitiesServices.Enums.CalculationType)coverageModel.CalculationTypeId;
                coverage.LimitAmount = coverageModel.LimitAmount;
                coverage.SubLimitAmount = coverageModel.SubLimitAmount;
                coverage.DeclaredAmount = coverageModel.DeclaredAmount;
                coverage.Rate = coverageModel.Rate;
                coverage.RateType = (RateType)coverageModel.RateType;
                coverage.PremiumAmount = coverageModel.PremiumAmount;
                coverage.ContractAmountPercentage = coverageModel.ContractAmountPercentage;
                coverage.Text = ModelAssembler.CreateText(new TextsModelsView() { TextBody = coverageModel.Text });
                if (coverageModel.DeductibleId > 0)
                {
                    coverage.Deductible = new CompanyDeductible
                    {
                        Id = (int)coverageModel.DeductibleId,
                        Description = coverageModel.DeductibleDescription
                    };
                }
                var coverages = DelegateService.suretyService.QuotationSuretyCoverages(coverageModel.TemporalId, coverageModel.RiskId, coverage, runRulesPost, listCompanyCoverage, policyId);
                coverages.Find(x => x.Id == coverage.Id).IsPostcontractual = coverage.IsPostcontractual;
                return new UifJsonResult(true, coverages);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQuotationCoverage);
            }
        }


        /// <summary>
        /// Retarifar Coberturas si cambio el valor del Contrato
        /// </summary>
        /// <param name="coverage">The coverage.</param>
        /// <param name="coverageModel">The coverage model.</param>
        /// <param name="runRulesPost">if set to <c>true</c> [run rules post].</param>
        /// <param name="listCompanyCoverage">The list company coverage.</param>
        /// <returns></returns>
        public ActionResult QuotationCoverages(int temporalId, int riskId, List<CompanyCoverage> listCompanyCoverage, decimal contractVale, int policyId)
        {
            try
            {
                List<CompanyCoverage> coverageTarif = new List<CompanyCoverage>();
                foreach (CompanyCoverage coverage in listCompanyCoverage)
                {
                    if (coverage.ContractAmountPercentage != 0 && coverage.PremiumAmount != 0 && coverage.SubLimitAmount != 0)
                    {
                        coverage.LimitAmount = contractVale * coverage.ContractAmountPercentage / (int)Helpers.Enums.TarifationType.Porcentaje;
                        coverage.SubLimitAmount = contractVale * coverage.ContractAmountPercentage / (int)Helpers.Enums.TarifationType.Porcentaje;
                        var coverages = DelegateService.suretyService.QuotationSuretyCoverages(temporalId, riskId, coverage, true, listCompanyCoverage, policyId);
                        if (coverages?.Count > 0)
                        {
                            var coverageAdd = coverages.FirstOrDefault(x => x.Id == coverage.Id);
                            if (coverageAdd != null)
                            {
                                coverageTarif.Add(coverageAdd);
                            }
                        }
                    }
                    else
                    {
                        coverageTarif.Add(coverage);
                    }

                }
                return new UifJsonResult(true, coverageTarif);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQuotationCoverage);
            }
        }
        public ActionResult GetPolicyModelsView(int temporalId)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);

                if (policy != null)
                {
                    var policyModel = ModelAssembler.CreateSuretyPolicy(policy);
                    if (policy.TemporalType == TemporalType.Quotation)
                    {
                        policyModel.Title = policy.TemporalTypeDescription;
                    }
                    else
                    {
                        policyModel.Title = App_GlobalResources.Language.LabelTemporal;
                    }
                    policyModel.Title += ": " + policy.Id.ToString() + " " + policy.Endorsement.EndorsementTypeDescription;

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


        public ActionResult GetCoveragesByRiskId(int riskId)
        {
            try
            {
                CompanyContract risk = DelegateService.suretyService.GetCompanySuretyByRiskId(riskId);
                return new UifJsonResult(true, risk.Risk.Coverages);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCoverages);
            }
        }

        public ActionResult GetCoveragesByCoverageId(int productId, int coverageGroupId, int prefixId, int coverageId)
        {
            List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(productId, coverageGroupId, prefixId);
            coverages = coverages.Where(x => x.Id == coverageId).ToList();
            return new UifSelectResult(coverages.OrderBy(x => x.Description));
        }

        public ActionResult GetGroupCoverages(int productId)
        {
            try
            {
                List<GroupCoverage> groupCoverages = DelegateService.underwritingService.GetProductCoverageGroupRiskByProductId(productId);
                return new UifJsonResult(true, groupCoverages);
            }
            catch (Exception ex)
            {

                Response.StatusCode = 500;
                return new UifJsonResult(false, ex.Message);
            }

        }

        public ActionResult GetCoveragesByProductIdGroupCoverageId(RiskSuretyModelsView riskModel)
        {
            try
            {
                if (riskModel == null)
                {
                    return Json(App_GlobalResources.Language.ErrorSearchCoverages);
                }
                //riskModel = riskModel.Replace("%26", "&");
                //var riskSuretyModelsView = JsonConvert.DeserializeObject<RiskSuretyModelsView>(riskModel, new IsoDateTimeConverter { DateTimeFormat = DateHelper.FormatDate });
                var contract = ModelAssembler.CreateRiskSurety(riskModel);
                var coverages = DelegateService.suretyService.GetCoveragesByProductIdGroupCoverageId(contract.Risk.Policy.Endorsement.TemporalId, contract);
                return new UifJsonResult(true, coverages);
                //JsonRequestBehavior.AllowGet
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "BusinessException")
                {
                    return new UifJsonResult(false, ex.GetBaseException().Message);
                }
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCoverages);

            }
        }



        public static bool IsSimpleType(Type type)
        {
            return
                type.IsValueType ||
                type.IsPrimitive ||
                new Type[]
                {
            typeof(String),
            typeof(Decimal),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(Guid)
                }.Contains(type) ||
                Convert.GetTypeCode(type) != TypeCode.Object;
        }

        #endregion

        #region Guarantee

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

        public ActionResult GetInsuredGuaranteeRelationPolicy(int guaranteeId)
        {
            try
            {
                var guarantees = DelegateService.suretyService.GetInsuredGuaranteeRelationPolicy(guaranteeId);
                return new UifJsonResult(true, guarantees);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchGuarantees);
            }
        }

        //SaveGuarantees
        public ActionResult SaveGuarantees(int riskId, List<CiaRiskSuretyGuarantee> guarantees)
        {
            try
            {
                List<InsuredGuaranteeLog> guaranteesLog = new List<InsuredGuaranteeLog>();
                CompanyContract risk = DelegateService.suretyService.GetCompanySuretyByRiskId(riskId);
                if (risk != null)
                {
                    foreach (CiaRiskSuretyGuarantee guarantee in guarantees)
                    {
                        if (guarantee.InsuredGuarantee.InsuredGuaranteeLog == null)
                        {
                            guaranteesLog = DelegateService.uniquePersonServiceV1.GetInsuredGuaranteeLogs(risk.Contractor.IndividualId, guarantee.Id);
                            guarantee.InsuredGuarantee.InsuredGuaranteeLog = guaranteesLog;
                        }

                    }
                    risk.Guarantees = guarantees;
                    risk = DelegateService.suretyService.CreateSuretyTemporal(risk, false);
                    if (risk != null)
                    {
                        return new UifJsonResult(true, guarantees);
                    }
                    else
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveGuarantees);
                    }
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorTempNoExist);
                }

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveRisk);
            }
        }

        #endregion

        #region Beneficiary

        public ActionResult SaveBeneficiaries(int riskId, List<CompanyBeneficiary> beneficiaries)
        {
            try
            {
                CompanyContract risk = DelegateService.suretyService.GetCompanySuretyByRiskId(riskId);

                if (risk.Risk.Id > 0)
                {
                    risk.Risk.Beneficiaries = beneficiaries;

                    risk = DelegateService.suretyService.CreateSuretyTemporal(risk, false);

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
        #region Clauses

        public ActionResult SaveClauses(int riskId, List<CompanyClause> clauses)
        {
            try
            {
                if (clauses != null)
                {
                    clauses = DelegateService.suretyService.SaveClauses(riskId, clauses);
                    return new UifJsonResult(true, clauses);
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



        public ActionResult RunRules(int tempId, int? ruleSetId)
        {
            try
            {
                var contract = DelegateService.suretyService.RunRulesRiskSurety(tempId, ruleSetId);
                return new UifJsonResult(true, contract);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.GetBaseException().Message);
            }
        }

        public ActionResult ExcludeCoverage(int temporalId, int riskId, int riskCoverageId, string description)
        {
            try
            {
                var coverage = DelegateService.suretyService.ExcludeCoverage(temporalId, riskId, riskCoverageId, description);
                return new UifJsonResult(true, coverage);
            }
            catch (Exception ex)
            {

                return new UifJsonResult(false, ex.GetBaseException().Message);
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
                var companyPolicy = DelegateService.suretyService.UpdateRisks(temporalId);
                return new UifJsonResult(true, companyPolicy);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }

        }

        public ActionResult GetCoverageByCoverageId(int coverageId, int riskId, int temporalId, int groupCoverageId)
        {
            try
            {
                var coverage = DelegateService.suretyService.GetCoverageByCoverageId(coverageId, riskId, temporalId, groupCoverageId);
                return new UifJsonResult(true, coverage);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCoverage);
            }
        }

        /// <summary>
        /// Creates the coverage.
        /// </summary>
        /// <param name="coverage">The coverage.</param>
        /// <param name="coverageOld">The coverage old.</param>
        /// <returns></returns>
        private CompanyCoverage CreateCoverage(CompanyCoverage coverage, CompanyCoverage coverageOld)
        {
            CompanyCoverage CoverageNew = new CompanyCoverage();
            CoverageNew = coverageOld;
            CoverageNew.Rate = coverage.Rate;
            return CoverageNew;

        }

        public ActionResult GuaranteeUnderwriting()
        {
            return View();
        }

        public ActionResult GetPremium(RiskSuretyModelsView riskModel, List<CompanyCoverage> coverages, List<DynamicConcept> dynamicProperties, CompanyText contractObject)
        {
            try

            {
                var riskSurety = ModelAssembler.CreateRiskSurety(riskModel);
                riskSurety.Risk.Coverages = coverages;
                riskSurety.Risk.DynamicProperties = dynamicProperties;
                riskSurety.ContractObject = contractObject;
                var companyRiskSurety = DelegateService.suretyService.GetPremium(riskSurety);
                return new UifJsonResult(true, companyRiskSurety);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }
        #region Helpers
        public UifJsonResult GetLeapYear()
        {
            try
            {
                return new UifJsonResult(true, DateHelper.LeapYear);
            }
            catch (Exception ex)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorParameterYearBisiesto + ex.Message.ToString());
            }
        }
        public ActionResult GetCoveragesByProductIdGroupCoverageIdPrefixId(int productId, int groupCoverageId, int prefixId, string coveragesAdd)
        {
            try
            {
                string[] idCoverages = null;
                if (!string.IsNullOrEmpty(coveragesAdd))
                {
                    idCoverages = coveragesAdd.Split(',');
                }
                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(productId, groupCoverageId, prefixId);
                if (idCoverages?.Length > 0 && coverages != null)
                {
                    coverages = coverages.Where(c => (!idCoverages.Where(z => !string.IsNullOrEmpty(z)).Any(x => Convert.ToInt32(x) == c.Id)) && c.IsVisible == true).ToList();
                }
                return new UifJsonResult(true, coverages?.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCoverage);
            }
        }


        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public ActionResult IsConsortiumindividualId(int individualId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.suretyService.IsConsortiumindividualId(individualId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchOperativeQuota);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public ActionResult GetAvailableCumulus(int individualId, int prefixCd, DateTime issueDate)
        {
            try
            {
                int TempSubscription_CurrencyCode = 0;
                return new UifJsonResult(true, DelegateService.suretyService.GetAvailableCumulus(individualId, TempSubscription_CurrencyCode, prefixCd, issueDate));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchOperativeQuota);
            }
        }
        /// <summary>
        /// Metodo de consulta parametro de validacion para definir obligatoriedad de contragarantias
        /// </summary>
        /// <returns></returns>
        public ActionResult IsGuaranteeMandatory()
        {
            try
            {
                var resp = DelegateService.utilitiesServiceCore.EnableCrossGuarantees();

                return new UifJsonResult(true, resp);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCrossGaranteesMandatory);
            }
        }

        public ActionResult GetRiskSureyLists(int productId)
        {
            try
            {
                ComboListDTO comboList = new ComboListDTO();
                comboList = DelegateService.underwritingService.GetRiskListsByProductId(productId);
                return new UifJsonResult(true, comboList);
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

        /// <summary>
        /// Tarifa coberturas luego de eliminar un registro
        /// </summary>
        /// <param name="tempId"></param>
        /// <param name="riskId"></param>
        /// <param name="coverages"></param>
        /// <returns></returns>
        public ActionResult GetListCoverageTemporal(int tempId, int riskId, List<CompanyCoverage> coverages)
        {
            if (coverages != null)
            {
                SaveCoverages(tempId, riskId, coverages);
                List<CompanyContract> contracts = DelegateService.suretyService.GetCompanySuretiesByTemporalId(tempId);
                if (contracts != null && contracts.Any())
                {
                    List<CompanyCoverage> ListCoveragesTemporal = new List<CompanyCoverage>();
                    var companyContract = contracts.First(x => x.Risk.Id == riskId);
                    ListCoveragesTemporal = companyContract.Risk.Coverages;
                    return new UifJsonResult(true, ListCoveragesTemporal);
                }
                else
                {
                    return new UifSelectResult(false, new CompanyCoverage());
                }
            }
            else
            {
                return new UifSelectResult(false, new CompanyCoverage());
            }
        }

        public UifJsonResult GetOperatingQuotaEventByIndividualIdByLineBusinessId(int IndividualId, int LineBusinessId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.operationQuotaService.GetOperatingQuotaEventByIndividualIdByLineBusinessId(IndividualId, LineBusinessId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCreateConsortium);
            }

        }
        public ActionResult GetCountries()
        {
            try
            {

                List<Country> countries = DelegateService.commonService.GetCountries();
                return new UifJsonResult(true, countries.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorSearchCountry);
            }

        }

        public UifJsonResult GetCumuloCupoConsortiumEventByConsortiumIdByLineBusinessId(int IndividualId, int LineBusinessId,bool? Endorsement,int Id)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.consortiumService.GetCumuloCupoConsortiumEventByConsortiumIdByLineBusinessId(IndividualId, LineBusinessId, Endorsement,Id));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetAgent);
            }
        }

        public UifJsonResult GetCumulusQuotaConsortiumByIndividualIdByLineBusinessId(int IndividualId, int LineBusinessId)

        {
            try
            {
                return new UifJsonResult(true, DelegateService.consortiumService.GetCumulusQuotaConsortiumByIndividualIdByLineBusinessId(IndividualId, LineBusinessId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetAgent);
            }
        }
        public ActionResult GetStatesByCountry(int idCountry)
        {
            try
            {

                List<State> states = DelegateService.commonService.GetStatesByCountryId(idCountry);
                return new UifJsonResult(true, states.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorSearchCountry);
            }

        }
        public ActionResult GetCitiesByCountryIdStateId(int countryId, int stateId)
        {
            try
            {

                List<City> cities = DelegateService.commonService.GetCitiesByCountryIdStateId(countryId, stateId);
                return new UifJsonResult(true, cities.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorSearchCountry);
            }

        }

        public UifJsonResult GetCumuloCupoEconomicGroupByIndividualIdByLineBusinessId(int IndividualId, int LineBusinessId)

        {
            try
            {
                return new UifJsonResult(true, DelegateService.EconomicGroupService.GetCumuloCupoEconomicGroupByIndividualIdByLineBusinessId(IndividualId, LineBusinessId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetAgent);
            }
        }


        public UifJsonResult GetDefaultCountry()
        {
            try
            {
                if (parameters.Count == 0)
                {
                    GetParameters();
                }
                return new UifJsonResult(true, parameters.Where(x => x.Description == "EmisionDefaultCountry"));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, null);
            }

        }

        public UifJsonResult GetValidityParticipantCupoInConsortium(int consortiumId, long AmountInsured, int LinebusinessId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.consortiumService.GetValidityParticipantCupoInConsortium(consortiumId, AmountInsured, LinebusinessId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetAgent);
            }
        }

        public UifJsonResult GetIdentificationPersonOrCompanyByIndividualId(int individualId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.uniquePersonServiceV1.GetIdentificationPersonOrCompanyByIndividualId(individualId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetAgent);
            }
        }

        //public UifJsonResult GetCompanyByIndividualId(int individualId)
        //{
        //    try
        //    {
        //        return new UifJsonResult(true, DelegateService.uniquePersonServiceCore.GetCompanyByIndividualId(individualId));
        //    }
        //    catch (Exception)
        //    {
        //        return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetAgent);
        //    }
        //}

        public UifJsonResult GetSecureType(int IndividualId, int LinebusinessId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.operationQuotaService.GetSecureType(IndividualId, LinebusinessId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCreateConsortium);
            }

        }
    }
}