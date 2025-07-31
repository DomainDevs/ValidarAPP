using Sistran.Company.Application.Aircrafts.AircraftApplicationService.DTOs;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
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
    public class RiskAircraftController : Controller
    {
        // GET: Underwriting/RiskAircraft
        public ActionResult Aircraft()
        {
            return View();
        }
        public ActionResult RiskAircraftCoverage()
        {
            return View();
        }
       
        public ActionResult GetPolicyType(int prefixId, int id)
        {
            try
            {
                PolicyTypeDTO companyPolicy = DelegateService.aircraftApplicationService.GetPolicyTypeByPolicyTypeIdPrefixId(id, prefixId);
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
              List<AircraftDTO> risks = DelegateService.aircraftApplicationService.GetAircraftsByTemporalId(temporalId);

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
                List<GroupCoverageDTO> groupCoverages = DelegateService.aircraftApplicationService.GetGroupCoveragesByPrefixIdProductId(0, productId);

                return new UifJsonResult(true, groupCoverages.OrderBy(x => x.Description).ToList());
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetGroupCoverages);
            }
        }
 
        public ActionResult SaveRisk(int temporalId, AircraftDTO riskModel, List<DynamicConcept> dynamicProperties)
        {
            try
            {
            
                if (ModelState.IsValid)
                {
                    AircraftDTO companyaircraft = DelegateService.aircraftApplicationService.SaveAircraft(riskModel);
                    return new UifJsonResult(true, companyaircraft);
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
                var risks = DelegateService.aircraftApplicationService.GetAircraftByRiskId(id);
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
                AircraftDTO aircraftDTO = new AircraftDTO
                {
                    PolicyId = policyId
                };

                if (ruleSetId.GetValueOrDefault() > 0)
                {
                    aircraftDTO = DelegateService.aircraftApplicationService.RunRulesRisk(aircraftDTO, ruleSetId.Value);
                }

                return new UifJsonResult(true, aircraftDTO);
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
                bool result = DelegateService.aircraftApplicationService.DeleteCompanyRisk(temporalId, riskId);
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
                List<CompanyBeneficiary> beneficiariesList = DelegateService.aircraftApplicationService.SaveBeneficiaries(riskId, beneficiaries, false);



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
                return new UifJsonResult(true, DelegateService.aircraftApplicationService.GetTextsByRiskId(riskId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetTexts);
            }
        }

        public ActionResult SaveTexts(int riskId, TextDTO textModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var companyaircraft = DelegateService.aircraftApplicationService.SaveText(riskId, textModel);
                    return new UifJsonResult(true, companyaircraft);
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

        public ActionResult SaveClauses(int temporalId, int riskId, List<ClauseDTO> clauses)
        {
            try
            {
                var companyaircraftes = DelegateService.aircraftApplicationService.SaveClauses(temporalId, riskId, clauses);
                return new UifJsonResult(true, companyaircraftes.OrderBy(x => x.Name).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveClauses);
            }
        }

        #endregion

        #region Coverages

        public UifJsonResult GetCoveragesByInsuredObjectId(int insuredObjectId, List<CoverageDTO> coverages)
        {
            try
            {
                List<CoverageDTO> coveragesNames = DelegateService.aircraftApplicationService.GetCoveragesByInsuredObjectId(insuredObjectId);
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
                List<CoverageDTO> coverages = DelegateService.aircraftApplicationService.GetCoveragesByRiskId(riskId, temporalId);
                return new UifJsonResult(true, coverages);

            }
            catch (Exception)
            {

                return new UifJsonResult(false, false);
            }

        }

        public UifJsonResult GetCoveragesByInsuredObjectIdGroupCoverageIdProductId(AircraftDTO aircraftDTO, InsuredObjectDTO insuredObject, int groupCoverageId, int productId,
            /*int depositPremiumPercent,*/ int idRate, decimal rate, DateTime currentFrom, DateTime currentTo, int insuredLimit, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                List<CoverageDTO> coverageGroup = DelegateService.aircraftApplicationService.GetCoveragesByProductIdGroupCoverageIdInsuredObjectId(productId, groupCoverageId, insuredObject.Id);
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
                    coverageGroup = DelegateService.aircraftApplicationService.QuotateCoverages(coverageGroup, aircraftDTO, runRulesPre, runRulesPost);

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
                List<SelectObjectDTO> calculationTypes = DelegateService.aircraftApplicationService.GetCalculationTypes();

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
                List<SelectObjectDTO> rateType = DelegateService.aircraftApplicationService.GetRateTypes();

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
                List<DeductibleDTO> deductibleType = DelegateService.aircraftApplicationService.GetDeductiblesByCoverageId(coverageId);
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
                List<InsuredObjectDTO> secureObjects = DelegateService.aircraftApplicationService.GetInsuredObjectsByProductIdGroupCoverageId(productId, groupCoverageId, prefixId);
                secureObjects = secureObjects.Where(x => x.IsSelected == true).ToList();
                return new UifJsonResult(true, secureObjects.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryDeductiblesByCoverageId);
            }
        }
        public UifJsonResult GetInsuredObjectsByProductIdGroupCoverageId_inCoverages(int productId, int groupCoverageId, int prefixId, List<InsuredObjectDTO> insuredObjects)
        {
            try
            {
                List<InsuredObjectDTO> secureObjects = DelegateService.aircraftApplicationService.GetInsuredObjectsByProductIdGroupCoverageId(productId, groupCoverageId, prefixId);
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
                CoverageDTO excludeCoverage = DelegateService.aircraftApplicationService.ExcludeCoverage(temporalId, riskId, coverageId);
                return new UifJsonResult(true, excludeCoverage);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorDeleteCoverage);
            }

        }

        public ActionResult SaveCoverages(int policyId, int riskId, List<CoverageDTO> coverages, int insuredObjectId)
        {
            try
            {
                AircraftDTO aircraftApplicationService = DelegateService.aircraftApplicationService.SaveCoverages(policyId, riskId, coverages, insuredObjectId);
                if (aircraftApplicationService != null)
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
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveCoverages);
            }
        }
        public ActionResult SaveInsuredObject(int riskId, InsuredObjectDTO insuredObjectDTO, int tempId, int groupCoverageId)
        {
            try
            {
                bool SaveInsuredObject = DelegateService.aircraftApplicationService.SaveInsuredObject(riskId, insuredObjectDTO, tempId, groupCoverageId);
                return new UifJsonResult(true, true);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.GetBaseException().Message);
            }
        }
        public ActionResult GetTemporalCoveragesByRiskIdInsuredObjectId(int riskId, int insuredObjectId)
        {
            List<CoverageDTO> coverages;
            try
            {
                coverages = DelegateService.aircraftApplicationService.GetTemporalCoveragesByRiskIdInsuredObjectId(riskId, insuredObjectId);
                return new UifJsonResult(true, coverages);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCoverages);
            }
        }
        public ActionResult QuotationCoverage(CoverageDTO coverage, AircraftDTO aircraftDTO, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                coverage = DelegateService.aircraftApplicationService.QuotateCoverage(coverage, aircraftDTO, runRulesPre, runRulesPost);
                coverage.CoverStatusName = EnumsHelper.GetItemName<CoverageStatusType>(coverage.CoverStatus);
                return new UifJsonResult(true, coverage);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQuotationCoverage);
            }
        }

        public ActionResult GetComboboxes(int prefixId)
        {
            try
            {   
                return new UifJsonResult(true, DelegateService.aircraftApplicationService.GetCombosRisk(prefixId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorLoadCombos);
            }
        }

        public ActionResult GetModels(int makeId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.aircraftApplicationService.GetModelsByMakeId(makeId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorLoadCombos);
            }
        }

        #endregion
    }
}