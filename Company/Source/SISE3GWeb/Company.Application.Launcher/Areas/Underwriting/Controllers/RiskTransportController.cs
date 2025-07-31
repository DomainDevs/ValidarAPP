using Sistran.Company.Application.Transports.TransportApplicationService.DTOs;
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
    public class RiskTransportController : Controller
    {
        // GET: Underwriting/RiskTransport
        public ActionResult Transport()
        {
            return View();
        }
        public ActionResult RiskTransportCoverage()
        {
            return View();
        }
        public UifJsonResult GetLeapYear()
        {
            try
            {
                bool LeapYear = DelegateService.transportApplicationService.GetLeapYear();
                return new UifJsonResult(true, LeapYear);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }
        public ActionResult GetPolicyType(int prefixId, int id)
        {
            try
            {
                PolicyTypeDTO companyPolicy = DelegateService.transportApplicationService.GetPolicyTypeByPolicyTypeIdPrefixId(id, prefixId);
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
                List<TransportDTO> risks = DelegateService.transportApplicationService.GetTransportsByTemporalId(temporalId);

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
                List<GroupCoverageDTO> groupCoverages = DelegateService.transportApplicationService.GetGroupCoveragesByPrefixIdProductId(0, productId);

                return new UifJsonResult(true, groupCoverages.OrderBy(x => x.Description).ToList());
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetGroupCoverages);
            }
        }

        public UifJsonResult GetCargoTypes()
        {
            try
            {
                List<CargoTypeDTO> cargoTypes = DelegateService.transportApplicationService.GetCargoTypes();
                return new UifJsonResult(true, cargoTypes.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCargoTypes);
            }
        }

        public UifJsonResult GetPackagingTypes()
        {
            try
            {
                List<PackagingTypeDTO> packagingTypes = DelegateService.transportApplicationService.GetPackagingTypes();
                return new UifJsonResult(true, packagingTypes.OrderBy(x => x.Description).ToList());
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPackagingTypes);
            }
        }

        public UifJsonResult GetTransportTypes()
        {
            try
            {
                List<TransportTypeDTO> transportTypes = DelegateService.transportApplicationService.GetTransportTypes();
                return new UifJsonResult(true, transportTypes.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetTransportTypes);
            }
        }

        public UifJsonResult GetCountries()
        {

            try
            {
                List<CountryDTO> transportTypes = DelegateService.transportApplicationService.GetCountries();
                return new UifJsonResult(true, transportTypes.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetTransportTypes);
            }

        }

        public UifJsonResult GetStateByCountryId(int countryId)
        {

            try
            {
                List<StateDTO> transportTypes = DelegateService.transportApplicationService.GetStatesByCountryId(countryId);
                return new UifJsonResult(true, transportTypes.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetTransportTypes);
            }

        }

        public UifJsonResult GetCitiesByStateIdByCountryId(int countryId, int stateId)
        {

            try
            {
                List<CityDTO> transportTypes = DelegateService.transportApplicationService.GetCitiesByContryIdStateId(countryId, stateId);
                return new UifJsonResult(true, transportTypes.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetTransportTypes);
            }

        }

        public UifJsonResult GetCitiesByContryIdStateId(int countryId, int stateId)
        {

            try
            {
                List<CityDTO> transportTypes = DelegateService.transportApplicationService.GetCitiesByContryIdStateId(countryId, stateId);
                return new UifJsonResult(true, transportTypes.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetTransportTypes);
            }

        }

        public UifJsonResult GetViaTypes()
        {

            try
            {
                List<ViaTypeDTO> transportTypes = DelegateService.transportApplicationService.GetViaTypes();
                return new UifJsonResult(true, transportTypes.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetTransportTypes);
            }

        }

        public UifJsonResult GetDeclarationPeriods()
        {
            try
            {
                List<DeclarationPeriodDTO> transportTypes = DelegateService.transportApplicationService.GetDeclarationPeriods();
                return new UifJsonResult(true, transportTypes.OrderBy(x => x.Description).ToList());
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
                List<BillingPeriodDTO> transportTypes = DelegateService.transportApplicationService.GetBillingPeriods();
                return new UifJsonResult(true, transportTypes.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBillingPeriods);
            }

        }

        public ActionResult SaveRisk(int temporalId, TransportDTO riskModel, List<DynamicConcept> dynamicProperties)
        {
            try
            {

                if (!riskModel.IsFloating)
                {
                    ModelState.Remove("riskModel.DeclarationPeriodId");
                    ModelState.Remove("riskModel.BillingPeriodId");
                    ModelState.Remove("riskModel.AnualBudget");
                    ModelState.Remove("riskModel.LimitMaxRealeaseAmount");
                    ModelState.Remove("riskModel.RiskId");
                    ModelState.Remove("riskModel.OriginalRiskId");
                }
                else
                {
                    ModelState.Remove("riskModel.FromCountryId");
                    ModelState.Remove("riskModel.FromStateId");
                    ModelState.Remove("riskModel.FromCityId");
                    ModelState.Remove("riskModel.ToCountryId");
                    ModelState.Remove("riskModel.ToStateId");
                    ModelState.Remove("riskModel.ToCityId");
                    ModelState.Remove("riskModel.ViaId");
                }

                if (ModelState.IsValid)
                {
                    var companytransport = DelegateService.transportApplicationService.SaveTransport(temporalId, riskModel);
                    return new UifJsonResult(true, companytransport);
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
                var risks = DelegateService.transportApplicationService.GetTransportByRiskId(id);
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
                TransportDTO transportDTO = new TransportDTO
                {
                    PolicyId = policyId
                };

                if (ruleSetId.GetValueOrDefault() > 0)
                {
                    transportDTO = DelegateService.transportApplicationService.RunRulesRisk(transportDTO, ruleSetId.Value);
                }

                return new UifJsonResult(true, transportDTO);
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
                var companyTransport = DelegateService.TransportBusinessService.UpdateCompanyRisks(temporalId);

                return new UifJsonResult(true, companyTransport);
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
                bool result = DelegateService.transportApplicationService.DeleteCompanyRisk(temporalId, riskId);
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
                List<CompanyBeneficiary> beneficiariesList = DelegateService.transportApplicationService.SaveBeneficiaries(riskId, beneficiaries, false);



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

        public JsonResult GetRiskCommercialClasses(string query)
        {
            try
            {
                return Json(DelegateService.transportApplicationService.GetRiskCommercialClasses(query), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.Error, JsonRequestBehavior.DenyGet);
            }
        }

        public UifJsonResult GetHolderTypes()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.transportApplicationService.GetHolderTypes());
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, "Error");
            }
        }
        public ActionResult GetDiferences(int riskId, List<CompanyCoverage> defaultCoverages, List<CompanyCoverage> allCoverages)
        {
            try
            {
                TransportDTO risk = DelegateService.transportApplicationService.GetTransportByRiskId(riskId);
                List<CompanyCoverage> quotationCoverages = new List<CompanyCoverage>();
                allCoverages = (from x in allCoverages
                                where !(from c in defaultCoverages select c.Id).Contains(x.Id)
                                select x).ToList();
                defaultCoverages.AddRange(allCoverages);

                foreach (var item in defaultCoverages)
                {
                    CoverageDTO coverageDTO = DelegateService.transportApplicationService.CreateCoverageDTO(item);
                    coverageDTO = DelegateService.transportApplicationService.QuotateCoverage(coverageDTO, risk, false, false);
                    CompanyCoverage companyCoverage = DelegateService.transportApplicationService.CreateCompanyCoverage(coverageDTO);
                    quotationCoverages.Add(companyCoverage);
                }

                return new UifJsonResult(true, quotationCoverages);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #region Texts

        public ActionResult GetTextsByRiskId(int riskId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.transportApplicationService.GetTextsByRiskId(riskId));
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
                    var companytransport = DelegateService.transportApplicationService.SaveText(riskId, textModel);
                    return new UifJsonResult(true, companytransport);
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
                var companytransportes = DelegateService.transportApplicationService.SaveClauses(temporalId, riskId, clauses);
                return new UifJsonResult(true, companytransportes.OrderBy(x => x.Name).ToList());
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
                List<CoverageDTO> coveragesNames = DelegateService.transportApplicationService.GetCoveragesByInsuredObjectId(insuredObjectId);
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
                List<CoverageDTO> coverages = DelegateService.transportApplicationService.GetCoveragesByRiskId(riskId, temporalId);
                return new UifJsonResult(true, coverages);

            }
            catch (Exception)
            {

                return new UifJsonResult(false, false);
            }

        }

        public UifJsonResult GetCoveragesByInsuredObjectIdGroupCoverageIdProductId(TransportDTO transportDTO, InsuredObjectDTO insuredObject, int groupCoverageId, int productId,
            DateTime currentFrom, DateTime currentTo)
        {
            try
            {
                List<CoverageDTO> coverageGroup = DelegateService.transportApplicationService.GetCoveragesByProductIdGroupCoverageIdInsuredObjectId(productId, groupCoverageId, insuredObject.Id);
                coverageGroup = coverageGroup.Where(x => x.IsSelected).ToList();
                if (coverageGroup != null && coverageGroup.Count > 0)
                {
                    foreach (var item in coverageGroup)
                    {
                        item.RateTypeId = insuredObject.RateTypeId;
                        item.Rate = insuredObject.Rate;
                        item.CurrentFrom = currentFrom;
                        item.CurrentTo = currentTo;
                        item.DeclaredAmount = insuredObject.InsuredLimitAmount;
                        item.LimitAmount = insuredObject.InsuredLimitAmount;
                        item.SubLimitAmount = insuredObject.InsuredLimitAmount;
                        item.LimitOccurrenceAmount = insuredObject.InsuredLimitAmount;
                        item.LimitClaimantAmount = insuredObject.InsuredLimitAmount;
                        item.DepositPremiumPercent = insuredObject.DepositPremiumPercentage;
                        item.InsuredObject = insuredObject;
                        item.CoverStatusName = EnumsHelper.GetItemName<CoverageStatusType>(item.CoverStatus);
                    }

                    if ((transportDTO.IsFloating))
                    {
                        transportDTO.InsuredObjects.Where(item => item.Id == insuredObject.Id).Select(x => {
                            x.InsuredLimitAmount = insuredObject.InsuredLimitAmount;
                            x.DepositPremiumPercentage = insuredObject.DepositPremiumPercentage;
                            x.Rate = insuredObject.Rate;
                            x.RateTypeId = insuredObject.RateTypeId;
                            return x;
                        });
                        coverageGroup.ForEach(x => x.InsuredObject = transportDTO.InsuredObjects.Where(item => item.Id.Equals(x.InsuredObject.Id)).FirstOrDefault());
                    }
                    coverageGroup = DelegateService.transportApplicationService.QuotateCoverages(coverageGroup, transportDTO, true, true);

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
                List<SelectObjectDTO> calculationTypes = DelegateService.transportApplicationService.GetCalculationTypes();

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
                List<SelectObjectDTO> rateType = DelegateService.transportApplicationService.GetRateTypes();

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
                List<DeductibleDTO> deductibleType = DelegateService.transportApplicationService.GetDeductiblesByCoverageId(coverageId);
                return new UifJsonResult(true, deductibleType.OrderBy(x => x.Description));
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
                List<InsuredObjectDTO> secureObjects = DelegateService.transportApplicationService.GetInsuredObjectsByProductIdGroupCoverageId(productId, groupCoverageId, prefixId);
                secureObjects = secureObjects.Where(x => x.IsSelected == true).ToList();
                if (secureObjects.Count > 0)
                {
                    return new UifJsonResult(true, secureObjects.OrderBy(x => x.Description).ToList());
                }
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorConsultingInsuredObjects);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorConsultingInsuredObjects);
            }
        }
        public UifJsonResult GetInsuredObjectsByProductIdGroupCoverageId_inCoverages(int productId, int groupCoverageId, int prefixId, List<InsuredObjectDTO> insuredObjects)
        {
            try
            {
                List<InsuredObjectDTO> secureObjects = DelegateService.transportApplicationService.GetInsuredObjectsByProductIdGroupCoverageId(productId, groupCoverageId, prefixId);
                if (insuredObjects != null && insuredObjects.Count > 0)
                {
                    //secureObjects = (from x in secureObjects
                    //                 where !(from c in insuredObjects select c.Id).Contains(x.Id)
                    //                 select x).ToList();

                    secureObjects = secureObjects.Where(x => x.IsSelected == true).ToList();
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
                CoverageDTO excludeCoverage = DelegateService.transportApplicationService.ExcludeCoverage(temporalId, riskId, coverageId);
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
                TransportDTO transportApplicationService = DelegateService.transportApplicationService.SaveCoverages(policyId, riskId, coverages, insuredObjectId);
                if (transportApplicationService != null)
                {
                    return new UifJsonResult(true, true);
                    //return new UifJsonResult(true, transportApplicationService);
                }
                else
                {
                    return new UifJsonResult(true, false);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveCoverages);
            }
        }
        public ActionResult SaveInsuredObject(int riskId, InsuredObjectDTO insuredObjectDTO, int tempId, int groupCoverageId)
        {
            try
            {
                bool SaveInsuredObject = DelegateService.transportApplicationService.SaveInsuredObject(riskId, insuredObjectDTO, tempId, groupCoverageId);
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
                coverages = DelegateService.transportApplicationService.GetTemporalCoveragesByRiskIdInsuredObjectId(riskId, insuredObjectId);
                return new UifJsonResult(true, coverages.OrderBy(x => x.Number).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCoverages);
            }
        }
        public ActionResult QuotationCoverage(CoverageDTO coverage, TransportDTO transportDTO, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                coverage = DelegateService.transportApplicationService.QuotateCoverage(coverage, transportDTO, runRulesPre, runRulesPost);
                coverage.CoverStatusName = EnumsHelper.GetItemName<CoverageStatusType>(coverage.CoverStatus);
                return new UifJsonResult(true, coverage);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQuotationCoverage);
            }
        }
        public ActionResult QuotationCoverages(List<CoverageDTO> coverages, TransportDTO transportDTO, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                coverages = DelegateService.transportApplicationService.QuotateCoverages(coverages, transportDTO, runRulesPre, runRulesPost);
                if (coverages != null && coverages.Count > 0)
                {
                    foreach (var coverage in coverages)
                    {
                        coverage.CoverStatusName = EnumsHelper.GetItemName<CoverageStatusType>(coverage.CoverStatus);
                    }
                }
                return new UifJsonResult(true, coverages);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.InnerException.Message);
            }
        }
        public ActionResult GetCoveragesByCoveragesAdd(int productId, int coverageGroupId, int prefixId, string coveragesAdd, int insuredObjectId)
        {
            List<CompanyCoverage> coverages = new List<CompanyCoverage>();

            try
            {
                coverages = DelegateService.transportApplicationService.GetCoveragesByCoveragesAdd(productId, coverageGroupId, prefixId, coveragesAdd, insuredObjectId);

                if (coverages.Count() == 0)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCoveragesAdd);
                }
                else {
                    if (coverages != null && coverages.Any())
                    {
                        return new UifJsonResult(true, coverages.OrderBy(x => x.CoverNum));
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

        public static void SaveDataFromEmision(CompanyPolicyResult model)
        {

        }

        public ActionResult GetCoverageByCoverageId(int coverageId, int temporalId, int groupCoverageId)
        {
            try
            {
                CompanyCoverage coverage = DelegateService.transportApplicationService.GetCoverageByCoverageId(coverageId, temporalId, groupCoverageId);
                return new UifJsonResult(true, coverage);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCoverage);
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


        #endregion
    }
}