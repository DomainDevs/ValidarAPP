using AIR = Sistran.Company.Application.Aircrafts.AircraftApplicationService.DTOs;
using MAR = Sistran.Company.Application.Marines.MarineApplicationService.DTOs;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Controllers
{
    public class RiskMarineController : Controller
    {
        // GET: Underwriting/RiskMarine
        public ActionResult Marine()
        {
            return View();
        }
        public ActionResult RiskMarineCoverage()
        {
            return View();
        }
        public ActionResult GetPolicyType(int prefixId, int id)
        {
            try
            {
                MAR.PolicyTypeDTO companyPolicy = DelegateService.marineApplicationService.GetPolicyTypeByPolicyTypeIdPrefixId(id, prefixId);
                if (companyPolicy != null)
                {
                    return new UifJsonResult(true, companyPolicy.IsFloating);
                }
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPolicyType);


            }
            catch (Exception)
            {

                throw;
            }


        }

        public ActionResult GetCiaRiskByTemporalId(int temporalId)
        {
            try
            {
                //List<CompanyRisk> risks = DelegateService.underwritingService.GetCiaRiskByTemporalId(temporalId, false);
                List<MAR.MarineDTO> risks = DelegateService.marineApplicationService.GetMarinesByTemporalId(temporalId);

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

        public UifJsonResult GetGroupCoverages(int productId)
        {
            try
            {
                List<MAR.GroupCoverageDTO> groupCoverages = DelegateService.marineApplicationService.GetGroupCoveragesByPrefixIdProductId(0, productId);

                return new UifJsonResult(true, groupCoverages.OrderBy(x => x.Description).ToList());
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetGroupCoverages);
            }
        }
             
        public UifJsonResult GetStateByCountryId(int countryId)
        {

            try
            {
                List<MAR.StateDTO> MarineTypes = DelegateService.marineApplicationService.GetStatesByCountryId(countryId);
                return new UifJsonResult(true, MarineTypes.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetMarineTypes);
            }

        }

        public UifJsonResult GetCitiesByStateIdByCountryId(int countryId, int stateId)
        {

            try
            {
                List<MAR.CityDTO> MarineTypes = DelegateService.marineApplicationService.GetCitiesByContryIdStateId(countryId, stateId);
                return new UifJsonResult(true, MarineTypes.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetMarineTypes);
            }

        }

        public UifJsonResult GetCitiesByContryIdStateId(int countryId, int stateId)
        {

            try
            {
                List<MAR.CityDTO> MarineTypes = DelegateService.marineApplicationService.GetCitiesByContryIdStateId(countryId, stateId);
                return new UifJsonResult(true, MarineTypes.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetMarineTypes);
            }

        }
          
        public ActionResult SaveRisk(int temporalId, MAR.MarineDTO riskModel, List<DynamicConcept> dynamicProperties)
        {
            try
            {
                //ModelState.Remove("riskModel.Texts");
                //ModelState.Remove("riskModel.Concepts");
                //ModelState.Remove("riskModel.Clauses");
                //ModelState.Remove("riskModel.Beneficiaries");
                //ModelState.Remove("riskModel.OriginalRiskId");
                //ModelState.Remove("riskModel.RiskId");

                //if (!riskModel.IsFloating)
                //{
                //    ModelState.Remove("riskModel.DeclarationPeriodId");
                //    ModelState.Remove("riskModel.BillingPeriodId");
                //    ModelState.Remove("riskModel.AnualBudget");
                //    ModelState.Remove("riskModel.LimitMaxRealeaseAmount");
                //    ModelState.Remove("riskModel.RiskId");
                //    ModelState.Remove("riskModel.OriginalRiskId");
                //}
                //else
                //{
                //    ModelState.Remove("riskModel.FromCountryId");
                //    ModelState.Remove("riskModel.FromStateId");
                //    ModelState.Remove("riskModel.FromCityId");
                //    ModelState.Remove("riskModel.ToCountryId");
                //    ModelState.Remove("riskModel.ToStateId");
                //    ModelState.Remove("riskModel.ToCityId");
                //    ModelState.Remove("riskModel.ViaId");
                //}

                if (ModelState.IsValid)
                {
                    var companyMarine = DelegateService.marineApplicationService.SaveMarine(riskModel);
                    return new UifJsonResult(true, companyMarine);
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

        public ActionResult GetRiskById(int id)
        {
            try
            {
                var risks = DelegateService.marineApplicationService.GetMarineByRiskId(id);
                return new UifJsonResult(true, risks);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchRisk);
            }
        }

        public ActionResult RunRules(int policyId, int? ruleSetId)
        {
            try
            {
                MAR.MarineDTO MarineDTO = new MAR.MarineDTO
                {
                    PolicyId = policyId
                };

                if (ruleSetId.GetValueOrDefault() > 0)
                {
                    MarineDTO = DelegateService.marineApplicationService.RunRulesRisk(MarineDTO, ruleSetId.Value);
                }

                return new UifJsonResult(true, MarineDTO);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public UifJsonResult UpdateRisks(int temporalId)
        {
            try
            {
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                return new UifJsonResult(true, companyPolicy);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorUpdatePolicy);
            }
        }
        public ActionResult DeleteRisk(int temporalId, int riskId)
        {
            try
            {
                bool result = DelegateService.marineApplicationService.DeleteCompanyRisk(temporalId, riskId);
                return new UifJsonResult(true, result);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorDeleteRisk);
            }
        }
        public ActionResult SaveBeneficiaries(int riskId, List<CompanyBeneficiary> beneficiaries)
        {
            try
            {
                List<CompanyBeneficiary> beneficiariesList = DelegateService.marineApplicationService.SaveBeneficiaries(riskId, beneficiaries, false);



                if (beneficiariesList != null)
                {
                    return new UifJsonResult(true, beneficiariesList);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveBeneficiaries);
                }

            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveBeneficiaries);
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

        #region Texts

        public ActionResult GetTextsByRiskId(int riskId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.marineApplicationService.GetTextsByRiskId(riskId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetTexts);
            }
        }

        public ActionResult SaveTexts(int riskId, MAR.TextDTO textModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var companyMarine = DelegateService.marineApplicationService.SaveText(riskId, textModel);
                    return new UifJsonResult(true, companyMarine);
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
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveTexts);
                }
            }

        }

        #endregion
        #region Clauses

        public ActionResult SaveClauses(int temporalId, int riskId, List<MAR.ClauseDTO> clauses)
        {
            try
            {
                var companyMarine = DelegateService.marineApplicationService.SaveClauses(temporalId, riskId, clauses);
                return new UifJsonResult(true, companyMarine.OrderBy(x => x.Name).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveClauses);
            }
        }

        #endregion

        #region Coverages

        public UifJsonResult GetCoveragesByInsuredObjectId(int insuredObjectId, List<MAR.CoverageDTO> coverages)
        {
            try
            {
                List<MAR.CoverageDTO> coveragesNames = DelegateService.marineApplicationService.GetCoveragesByInsuredObjectId(insuredObjectId);
                coveragesNames = coveragesNames.Where(x => x.IsSelected == false).ToList();
                if (coverages != null && coverages.Count > 0)
                {
                    coveragesNames = (from x in coveragesNames
                                      where !(from c in coverages select c.Id).Contains(x.Id)
                                      select x).ToList();
                }

                if (coveragesNames.Count > 0)
                {
                    return new UifJsonResult(true, coveragesNames.OrderBy(x => x.Description).ToList());
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCoverageBy);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCoverageBy);
            }
        }

        public UifJsonResult GetCoveragesByRiskId(int riskId, int temporalId)
        {
            try
            {
                List<MAR.CoverageDTO> coverages = DelegateService.marineApplicationService.GetCoveragesByRiskId(riskId, temporalId);
                return new UifJsonResult(true, coverages);

            }
            catch (Exception)
            {

                return new UifJsonResult(false, false);
            }

        }

        public UifJsonResult GetCoveragesByInsuredObjectIdGroupCoverageIdProductId(MAR.MarineDTO MarineDTO, MAR.InsuredObjectDTO insuredObject, int groupCoverageId, int productId,
            /*int depositPremiumPercent, */int idRate, decimal rate, DateTime currentFrom, DateTime currentTo, int insuredLimit, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                List<MAR.CoverageDTO> coverageGroup = DelegateService.marineApplicationService.GetCoveragesByProductIdGroupCoverageIdInsuredObjectId(productId, groupCoverageId, insuredObject.Id);
                coverageGroup = coverageGroup.Where(x => x.IsSelected).ToList();
                if (coverageGroup != null && coverageGroup.Count > 0)
                {
                    foreach (var item in coverageGroup)
                    {
                        item.RateTypeId = idRate;
                        item.Rate = rate;
                        item.CurrentFrom = currentFrom;
                        item.CurrentTo = currentTo;
                        item.DeclaredAmount = insuredLimit;
                        item.LimitAmount = insuredLimit;
                        item.SubLimitAmount = insuredLimit;
                        item.MaxLiabilityAmount = insuredLimit;
                        item.LimitOccurrenceAmount = insuredLimit;
                        item.LimitClaimantAmount = insuredLimit;
                        //item.DepositPremiumPercent = depositPremiumPercent;
                        item.InsuredObject = insuredObject;
                        item.CoverStatusName = EnumsHelper.GetItemName<CoverageStatusType>(item.CoverStatus);
                    }
                    coverageGroup = DelegateService.marineApplicationService.QuotateCoverages(coverageGroup, MarineDTO, runRulesPre, runRulesPost);

                    return new UifJsonResult(true, coverageGroup.OrderBy(x => x.Description).ToList());
                }
                else
                {
                    if (coverageGroup != null && coverageGroup.Count == 0)
                    {
                        return new UifJsonResult(true, coverageGroup);
                    }
                    else
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCoverages);
                    }
                }
            }

            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCoverages);
            }
        }

        public UifJsonResult GetCalculationTypes()
        {
            try
            {
                List<MAR.SelectObjectDTO> calculationTypes = DelegateService.marineApplicationService.GetCalculationTypes();

                return new UifJsonResult(true, calculationTypes.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryCalculationTypes);
            }
        }
        public UifJsonResult GetRateTypes()
        {
            try
            {
                List<MAR.SelectObjectDTO> rateType = DelegateService.marineApplicationService.GetRateTypes();

                return new UifJsonResult(true, rateType.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryRateTypes);
            }
        }

        public UifJsonResult GetDeductiblesByCoverageId(int coverageId)
        {
            try
            {
                List<MAR.DeductibleDTO> deductibleType = DelegateService.marineApplicationService.GetDeductiblesByCoverageId(coverageId);
                return new UifJsonResult(true, deductibleType.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryDeductiblesByCoverageId);
            }
        }

        public UifJsonResult GetInsuredObjectsByProductIdGroupCoverageId(int productId, int groupCoverageId, int prefixId)
        {
            try
            {
                List<MAR.InsuredObjectDTO> secureObjects = DelegateService.marineApplicationService.GetInsuredObjectsByProductIdGroupCoverageId(productId, groupCoverageId, prefixId);
                secureObjects = secureObjects.Where(x => x.IsSelected == true).ToList();
                return new UifJsonResult(true, secureObjects.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryDeductiblesByCoverageId);
            }
        }
        public UifJsonResult GetInsuredObjectsByProductIdGroupCoverageId_inCoverages(int productId, int groupCoverageId, int prefixId, List<MAR.InsuredObjectDTO> insuredObjects)
        {
            try
            {
                List<MAR.InsuredObjectDTO> secureObjects = DelegateService.marineApplicationService.GetInsuredObjectsByProductIdGroupCoverageId(productId, groupCoverageId, prefixId);
                if (insuredObjects != null && insuredObjects.Count > 0)
                {
                    secureObjects = (from x in secureObjects
                                     where !(from c in insuredObjects select c.Id).Contains(x.Id)
                                     select x).ToList();
                }
                return new UifJsonResult(true, secureObjects.OrderBy(x => x.Description).ToList());
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryDeductiblesByCoverageId);
            }
        }
        public ActionResult ExcludeCoverage(int temporalId, int riskId, int coverageId)
        {
            try
            {
                MAR.CoverageDTO excludeCoverage = DelegateService.marineApplicationService.ExcludeCoverage(temporalId, riskId, coverageId);
                return new UifJsonResult(true, excludeCoverage);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorDeleteCoverage);
            }

        }

        public ActionResult SaveCoverages(int policyId, int riskId, List<MAR.CoverageDTO> coverages, int insuredObjectId)
        {
            try
            {
                MAR.MarineDTO MarineApplicationService = DelegateService.marineApplicationService.SaveCoverages(policyId, riskId, coverages, insuredObjectId);
                if (MarineApplicationService != null)
                {
                    return new UifJsonResult(true, true);
                    //return new UifJsonResult(true, MarineApplicationService);
                }
                else
                {
                    return new UifJsonResult(true, false);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveCoverages);
            }
        }
        public ActionResult SaveInsuredObject(int riskId, MAR.InsuredObjectDTO insuredObjectDTO, int tempId, int groupCoverageId)
        {
            try
            {
                bool SaveInsuredObject = DelegateService.marineApplicationService.SaveInsuredObject(riskId, insuredObjectDTO, tempId, groupCoverageId);
                return new UifJsonResult(true, true);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.GetBaseException().Message);
            }
        }
        public ActionResult GetTemporalCoveragesByRiskIdInsuredObjectId(int riskId, int insuredObjectId)
        {
            List<MAR.CoverageDTO> coverages;
            try
            {
                coverages = DelegateService.marineApplicationService.GetTemporalCoveragesByRiskIdInsuredObjectId(riskId, insuredObjectId);
                return new UifJsonResult(true, coverages);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCoverages);
            }
        }
        public ActionResult QuotationCoverage(MAR.CoverageDTO coverage, MAR.MarineDTO MarineDTO, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                coverage = DelegateService.marineApplicationService.QuotateCoverage(coverage, MarineDTO, runRulesPre, runRulesPost);
                coverage.CoverStatusName = EnumsHelper.GetItemName<CoverageStatusType>(coverage.CoverStatus);
                return new UifJsonResult(true, coverage);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQuotationCoverage);
            }
        }
        public ActionResult GetUsesMarine(int prefixId)
        {
            try
            {
                
                return new UifJsonResult(true, DelegateService.marineApplicationService.GetUsesMarine(prefixId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorLoadCombos);
            }
        }
        #endregion
    }
}