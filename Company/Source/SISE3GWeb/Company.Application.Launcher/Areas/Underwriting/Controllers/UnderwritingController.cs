using Sistran.Company.Application.ProductServices.Models;
using Sistran.Company.Application.SarlaftApplicationServices.DTO;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.CommonService.Models.Base;
using Sistran.Core.Application.ModelServices.Models.Underwriting;
using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
using Sistran.Core.Application.ProductServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using AircraftDTO = Sistran.Company.Application.Aircrafts.AircraftApplicationService.DTOs;
using CiaPolicy = Sistran.Company.Application.UnderwritingServices.Models;
using MarineDTOs = Sistran.Company.Application.Marines.MarineApplicationService.DTOs;
using SarlaftEnum = Sistran.Company.Application.SarlaftBusinessServices.Enum;
using UnderEnum = Sistran.Company.Application.UnderwritingServices.Enums;
using UPV1 = Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Controllers
{
    using App_GlobalResources;
    using Application.AuthorizationPoliciesServices.Enums;
    using AutoMapper;
    using Endorsement.Models;
    using Sistran.Company.Application.ExternalProxyServices.Models;
    using Sistran.Company.Application.Location.LiabilityServices.Models;
    using Sistran.Company.Application.Location.PropertyServices.Models;
    using Sistran.Company.Application.Sureties.JudicialSuretyServices.Models;
    using Sistran.Company.Application.Sureties.SuretyServices.Models;
    using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
    using Sistran.Company.Application.Vehicles.VehicleServices.Models;
    using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;
    using Sistran.Core.Application.UnderwritingServices.Models;
    using Sistran.Core.Framework.UIF.Web.Helpers.Enums;
    using System.Text.RegularExpressions;
    using ModelAssembler = Models.ModelAssembler;
    using UNSE = Sistran.Company.Application.UnderwritingServices.Enums;

    [Authorize]
    public class UnderwritingController : Controller
    {
        private static List<UPV1.CoInsuranceCompany> coInsuranceCompanies = new List<UPV1.CoInsuranceCompany>();
        private static List<Sistran.Core.Application.CommonService.Models.Parameter> parameters = new List<Sistran.Core.Application.CommonService.Models.Parameter>();
        private static List<DefaultValue> defaultValuesUnderwriting = new List<DefaultValue>();


        public ActionResult Index(int? temporalId)
        {
            if (temporalId.HasValue)
            {
                CompanyPolicy policy = DelegateService.underwritingService.CompanyGetPolicyByTemporalId(temporalId.Value, false);

                if (policy != null)
                {
                    SearchViewModel searchViewModel = new SearchViewModel();
                    List<AuthorizationRequest> authorizationRequests = DelegateService.AuthorizationPoliciesService.GetAuthorizationRequestsByKey(temporalId.ToString());

                    if (authorizationRequests.Any(x => x.Status == TypeStatus.Authorized) && authorizationRequests.All(x => x.Status != TypeStatus.Rejected))
                    {
                        int countAuthorized = authorizationRequests.Count(x => x.Status == TypeStatus.Authorized);
                        int countPending = authorizationRequests.Count(x => x.Status == TypeStatus.Pending);
                        searchViewModel.Message = string.Format(Language.MessageModifyPendingAcceptPolicies, countPending, countAuthorized);

                        searchViewModel.BranchId = policy.Branch.Id;
                        searchViewModel.PrefixId = policy.Prefix.Id;
                        searchViewModel.PolicyNumber = policy.DocumentNumber.ToString();
                        searchViewModel.EndorsementId = policy.Endorsement.Id;
                        this.TempData["searchViewModel"] = searchViewModel;
                        return this.RedirectToAction("Search", "Search", new { area = "Endorsement" });
                    }
                }
            }
            return this.View();
        }

        public ActionResult ValidateMainInsured(List<CompanyRiskInsured> riskInsured)
        {
            try
            {
                List<CompanyRiskInsured> result = riskInsured.Where(z => z.Insured?.CustomerType == CustomerType.Prospect).ToList();
                // List<CompanyRiskInsured> validate = riskInsured.Where(z => z.Beneficiaries[0].CustomerType == CustomerType.Prospect).ToList();
                if (result.Any() /*&& validate.Any()*/)
                {
                    return new UifJsonResult(false, result);
                }
                else
                {
                    return new UifJsonResult(true, result);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex);
                throw;
            }

        }

        public ActionResult ValidateGetSarlaft(int IndividualId)
        {
            try
            {
                List<SarlaftExonerationtDTO> Exoneration = DelegateService.SarlaftApplicationServices.GetSarlaftExoneration(IndividualId);
                if (Exoneration.Count > 0 && Exoneration[0].IsExonerated)
                {
                    return new UifJsonResult(true, SarlaftEnum.SarlaftValidationState.ACCURATE);

                }

                List<SarlaftDTO> result = DelegateService.SarlaftApplicationServices.GetSarlaft(IndividualId);
                if (result.Count > 0)
                {
                    SarlaftDTO objSarlaft = result.OrderByDescending(x => x.FillingDate).FirstOrDefault();
                    DateTime fillingDate = (DateTime)objSarlaft.FillingDate;
                    if (DateTime.Now.Subtract(fillingDate).Days > 365)
                    {
                        return new UifJsonResult(true, SarlaftEnum.SarlaftValidationState.EXPIRED);
                    }
                    else
                    {
                        int PARAM = DelegateService.commonService.GetParameterByParameterId((int)UnderEnum.Parameter.Daysdiscountedays).NumberParameter.Value;
                        if (DateTime.Now.Subtract(fillingDate).Days > (365 - PARAM))
                        {
                            return new UifJsonResult(true, SarlaftEnum.SarlaftValidationState.OVERCOME);
                        }
                        else
                        {
                            if (objSarlaft.PendingEvent)
                            {
                                return new UifJsonResult(true, SarlaftEnum.SarlaftValidationState.PENDING);
                            }
                            else
                            {
                                return new UifJsonResult(true, SarlaftEnum.SarlaftValidationState.ACCURATE);
                            }
                        }
                    }
                }
                else
                {
                    return new UifJsonResult(true, SarlaftEnum.SarlaftValidationState.NOT_EXISTS);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, "");
            }
        }

        public List<SarlaftExonerationtDTO> ValidateGetSarlaftsExoneration(int IndividualId)
        {
            try
            {
                List<SarlaftExonerationtDTO> result = DelegateService.SarlaftApplicationServices.GetSarlaftExoneration(IndividualId);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #region Agents

        public ActionResult GetPercentageCommissionByAgentIdAgencyIdProductId(int agentId, int agencyId, int productId)
        {
            try
            {
                ProductAgencyCommiss productAgencyCommiss = DelegateService.productService.GetCommissByAgentIdAgencyIdProductId(agentId, agencyId, productId);
                return new UifJsonResult(true, productAgencyCommiss.CommissPercentage);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCommisionPercentage);
            }
        }

        public ActionResult GetCommissions(int temporalId, IssuanceAgency agency, List<IssuanceAgency> agencies)
        {
            try
            {
                List<IssuanceAgency> Agencies = DelegateService.underwritingService.GetCompanyCommissionsByTempId(temporalId, agency, agencies);
                return new UifJsonResult(true, Agencies);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCommision);
            }

        }

        public ActionResult SaveCommissions(int temporalId, List<IssuanceAgency> agencies)
        {
            try
            {
                List<IssuanceAgency> Agencies = DelegateService.underwritingService.SaveCompanyCommissions(temporalId, agencies);
                return new UifJsonResult(true, Agencies);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.GetBaseException().Message);
            }
        }

        #endregion

        #region Beneficiaries

        public ActionResult GetBeneficiaryTypes()
        {
            try
            {
                List<CompanyBeneficiaryType> beneficiaryTypes = DelegateService.underwritingService.GetCompanyBeneficiaryTypes();
                return new UifJsonResult(true, beneficiaryTypes);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBeneficiaryTypes);
            }
        }

        public ActionResult GetBeneficiariesByDescription(string description, InsuredSearchType insuredSearchType, int customerTyp = 1/*CustomerType? customerType = CustomerType.Individual*/)
        {
            try
            {
                List<Beneficiary> beneficiaries = DelegateService.underwritingService.GetBeneficiariesByDescription(description, insuredSearchType, (CustomerType)customerTyp);
                return new UifJsonResult(true, beneficiaries.OrderBy(x => x.Name));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchBeneficiaries);
            }
        }

        public ActionResult SaveBeneficiaries(int temporalId, List<CiaPolicy.CompanyBeneficiary> beneficiaries)
        {
            try
            {
                List<CompanyBeneficiary> beneficiariesModel = DelegateService.underwritingService.SaveCompanyBeneficiary(temporalId, beneficiaries);
                return new UifJsonResult(false, beneficiariesModel);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveBeneficiaries);
            }
        }
        public CiaPolicy.CompanyBeneficiary GetBeneficiaryByPrefixId(int prefixId)
        {
            try
            {
                return DelegateService.underwritingService.GetBeneficiaryByPrefixId(prefixId);
            }
            catch (Exception)
            {
                return null;
            }

        }
        #endregion

        #region Clauses

        public ActionResult GetClausesByLevelsConditionLevelId(Core.Application.UnderwritingServices.Enums.EmissionLevel levels, int conditionLevelId)
        {
            try
            {
                List<Clause> clauses = DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(levels, conditionLevelId);
                List<Clause> uniqueClauses = clauses.GroupBy(test => test.Id)
                   .Select(grp => grp.First())
                   .ToList();

                return new UifJsonResult(true, uniqueClauses.OrderBy(x => x.Name));
            }
            catch (Exception)
            {
                return new UifJsonResult(true, App_GlobalResources.Language.ErrorSearchClauses);
            }
        }

        public ActionResult SaveClauses(int temporalId, List<CiaPolicy.CompanyClause> clauses)
        {
            try
            {
                List<CompanyClause> result = DelegateService.underwritingService.SaveCompanyClauses(temporalId, clauses);
                return new UifJsonResult(true, result);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveClauses);
            }
        }

        #endregion

        #region Coinsurance

        public ActionResult GetBusinessTypes()
        {
            return new UifJsonResult(true, EnumsHelper.GetItems<BusinessType>());
        }

        public JsonResult GetCoInsuranceCompaniesByDescription(string query)
        {
            if (coInsuranceCompanies.Count == 0)
            {
                coInsuranceCompanies = DelegateService.uniquePersonServiceV1.GetCoInsuranceCompanies();
            }

            int insuranceId = 0;
            if (int.TryParse(query, out insuranceId))
            {
                return null;
            }
            else
            {
                IEnumerable<UPV1.CoInsuranceCompany> dataFiltered = coInsuranceCompanies.Where(x => x.Description.Contains(query.ToUpper()));
                return this.Json(dataFiltered, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveCoinsurance(CoInsuranceModelsView coInsuranceModel, List<CompanyIssuanceCoInsuranceCompany> assignedCompanies)
        {
            try
            {
                if (coInsuranceModel.BusinessType == BusinessType.Accepted)
                {
                    CompanyIssuanceCoInsuranceCompany coInsuranceCompany = new CompanyIssuanceCoInsuranceCompany();

                    //coInsuranceCompany = ModelAssembler.CreateAcceptedCoInsuranceCompany(coInsuranceModel);
                    CiaPolicy.CompanyPolicy policy = DelegateService.underwritingService.SaveCompanyCoinsurance(coInsuranceCompany, assignedCompanies, coInsuranceModel.BusinessType, coInsuranceModel.TemporalId);
                    return new UifJsonResult(true, policy);
                }
                else
                {
                    CompanyIssuanceCoInsuranceCompany coInsuranceCompany = new CompanyIssuanceCoInsuranceCompany();
                    CiaPolicy.CompanyPolicy policy = DelegateService.underwritingService.SaveCompanyCoinsurance(coInsuranceCompany, assignedCompanies, coInsuranceModel.BusinessType, coInsuranceModel.TemporalId);
                    return new UifJsonResult(true, policy);
                }



            }

            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveBusinessType);
            }
        }

        #endregion

        #region PaymentPlan

        public ActionResult GetPaymentPlanByProductId(int productId)
        {
            try
            {
                List<PaymentPlan> paymentPlans = DelegateService.underwritingService.GetPaymentPlansByProductId(productId);
                return new UifJsonResult(true, paymentPlans.OrderBy(u => u.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchPaymentPlan);
            }
        }

        public ActionResult GetPaymentPlanIdByProductId(int productId)
        {
            try
            {
                List<PaymentPlan> paymentPlans = DelegateService.underwritingService.GetPaymentPlansByProductId(productId);
                return new UifJsonResult(true, paymentPlans.Where(p => p.IsDefault == true));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchPaymentPlan);
            }
        }

        public ActionResult GetPaymentSchedules()
        {
            try
            {
                List<int> listPaymentSchedules = DelegateService.productService.GetPaymentSchedules();
                return new UifJsonResult(true, listPaymentSchedules);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchTemp);
            }
        }

        public ActionResult GetQuotasbyPaymentPlanId(int paymentPlanId, CiaPolicy.CompanySummary summary, DateTime currentFrom, DateTime currentTo, DateTime issueDate)
        {
            try
            {
                CiaPolicy.CompanyPolicy policy = new CiaPolicy.CompanyPolicy
                {
                    PaymentPlan = new CiaPolicy.CompanyPaymentPlan { Id = paymentPlanId },
                    Summary = summary,
                    CurrentFrom = currentFrom,
                    CurrentTo = currentTo,
                    IssueDate = issueDate
                };
                ComponentValueDTO componentValueDTO = Mapper.Map<CompanySummary, ComponentValueDTO>(policy.Summary);
                QuotaFilterDTO quotaFilterDto = new QuotaFilterDTO { PlanId = policy.PaymentPlan.Id, CurrentFrom = policy.CurrentFrom, IssueDate = policy.IssueDate, ComponentValueDTO = componentValueDTO };
                List<Quota> quotas = DelegateService.underwritingService.CalculateQuotas(quotaFilterDto);
                return new UifJsonResult(true, quotas);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCalculateQuotas);
            }
        }

        public ActionResult GetPaymentPlanScheduleByPolicyId(int policyId)
        {
            try
            {
                int paymentPlanScheduleId = DelegateService.underwritingService.GetPaymentPlanScheduleByPolicyId(policyId);
                return new UifJsonResult(true, paymentPlanScheduleId);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorPaymentPlanSchedule);
            }
        }

        public ActionResult GetPaymentPlanScheduleByPolicyEndorsementId(int policyId, int endorsementId)
        {
            try
            {
                int paymentPlanScheduleId = DelegateService.underwritingService.GetPaymentPlanScheduleByPolicyEndorsementId(policyId, endorsementId);
                return new UifJsonResult(true, paymentPlanScheduleId);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorPaymentPlanSchedule);
            }
        }

        public ActionResult SavePaymentPlan(int temporalId, CiaPolicy.CompanyPaymentPlan paymentPlan, List<Quota> quotas)
        {
            try
            {
                CompanyPaymentPlan result = DelegateService.underwritingService.SaveCompanyPaymentPlan(temporalId, paymentPlan, quotas);
                return new UifJsonResult(true, result);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSavePaymentPlan);
            }
        }
        public ActionResult SavePaymentPremiumFinance(int temporalId, CiaPolicy.CompanyPaymentPlan premiumFinance)
        {
            try
            {
                int MaxPercentejeToFinanceParameter = DelegateService.commonService.GetParameterByParameterId((int)UnderEnum.Parameter.MaxPercentejeToFinanceParameter).NumberParameter.Value;
                if (MaxPercentejeToFinanceParameter > 0)
                {
                    if (premiumFinance.PremiumFinance.PercentagetoFinance > MaxPercentejeToFinanceParameter)
                    {
                        return new UifJsonResult(false, (App_GlobalResources.Language.WarningPercentageMax));
                    }
                }

                if (MaxPercentejeToFinanceParameter < 0)
                {
                    if (premiumFinance.PremiumFinance.PercentagetoFinance > MaxPercentejeToFinanceParameter)
                    {
                        return new UifJsonResult(false, (App_GlobalResources.Language.WarningPercentageMin));
                    }
                }

                CompanyPaymentPlan result = DelegateService.underwritingService.SaveCompanyPremiumFinance(temporalId, premiumFinance);
                return new UifJsonResult(true, result);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSavePaymentPlan);
            }
        }

        #endregion

        #region Texts

        public ActionResult GetTextsByNameLevelIdConditionLevelId(string name, int levelId, int conditionLevelId)
        {
            try
            {
                List<Text> texts = DelegateService.underwritingService.GetTextsByNameLevelIdConditionLevelId(name, levelId, conditionLevelId);
                return new UifJsonResult(true, texts);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchText);
            }
        }

        public ActionResult SaveTexts(int temporalId, TextsModelsView textModel)
        {
            try
            {
                CompanyText companyText = ModelAssembler.CreateText(textModel);
                if (!string.IsNullOrEmpty(companyText.TextBody))
                {
                    companyText.TextBody = this.unicode_iso8859(companyText.TextBody);
                }

                CompanyText result = DelegateService.underwritingService.SaveCompanyTexts(temporalId, companyText);
                return new UifJsonResult(true, result);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveText);
            }
        }

        public string unicode_iso8859(string text)
        {
            text = text = text.Replace("\u001a", "");
            System.Text.Encoding iso = System.Text.Encoding.GetEncoding("iso8859-1");
            text = Regex.Replace(text, @"[/']", " ", RegexOptions.None);
            byte[] isoByte = iso.GetBytes(text);
            return iso.GetString(isoByte);
        }

        #endregion

        #region Issuance

        public ActionResult Quotation()
        {
            ContextModel contextModel = new ContextModel(TemporalType.Quotation);
            this.Session["USER_ACCESS_PERMISSIONS_KEY_CONTEXT_MODEL"] = contextModel;
            this.Session["USER_ACCESS_PERMISSIONS_KEY_CONTEXT"] = $"{contextModel.TemporalType.ToString().ToUpper()}";
            PolicyModelsView policyModel = new PolicyModelsView
            {
                DaysValidity = DelegateService.commonService.GetParameterByParameterId((int)UnderEnum.Parameter.SubscriptionDays).NumberParameter.Value,
            };
            int Value = DelegateService.commonService.GetParameterByParameterId((int)UnderEnum.Parameter.SubscriptionDays).NumberParameter.Value;
            return this.View("Quotation", policyModel);
        }

        public ActionResult GetJustificationSarlaft()
        {
            try
            {
                List<CompanyJustificationSarlaft> listSarlaft = DelegateService.underwritingService.GetJustificationSarlaft();
                listSarlaft = listSarlaft.Where(x => x.Enabled == true).OrderBy(x => x.Description).ToList();
                return new UifJsonResult(true, listSarlaft);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public ActionResult Temporal()
        {
            ContextModel contextModel = new ContextModel(TemporalType.Policy);
            this.Session["USER_ACCESS_PERMISSIONS_KEY_CONTEXT_MODEL"] = contextModel;
            this.Session["USER_ACCESS_PERMISSIONS_KEY_CONTEXT"] = $"{contextModel.TemporalType.ToString().ToUpper()}";
            return this.View();
        }

        public ActionResult IssueWithEvent()
        {
            return this.View();
        }

        public ActionResult UnderwritingPersonOnline(PolicyModelsView policyModel)
        {/*Persona en linea*/
            return this.View("Underwriting", policyModel);
        }

        public ActionResult GetParameterByDescription(string description)
        {
            try
            {
                if (parameters.Count == 0)
                {
                    parameters = this.GetParameters();
                }

                return new UifJsonResult(true, parameters.First(x => x.Description == description).Value);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchParameter);
            }
        }

        private List<Sistran.Core.Application.CommonService.Models.Parameter> GetParameters()
        {
            parameters.Add(new Sistran.Core.Application.CommonService.Models.Parameter { Description = "Currency", Value = DelegateService.commonService.GetParameterByParameterId((int)ParametersTypes.Currency).NumberParameter.Value });

            parameters.Add(new Sistran.Core.Application.CommonService.Models.Parameter { Description = "BusinessType", Value = BusinessType.CompanyPercentage });

            parameters.Add(new Sistran.Core.Application.CommonService.Models.Parameter { Description = "Country", Value = DelegateService.commonService.GetParameterByParameterId((int)ParametersTypes.Country).NumberParameter.Value });

            return parameters;
        }

        public UifJsonResult GetHoldersByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType, TemporalType temporalType)
        {
            try
            {
                List<Holder> holders = DelegateService.underwritingService.GetHoldersByDocument(description, customerType);
                if (holders != null && holders.Any())
                {
                    return new UifJsonResult(true, holders);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorHolderNotFoundInsured);
                }


            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchPolicyholder);
            }
        }


        public UifJsonResult GetHoldersByDocumentOrDescription(string description, CustomerType? customerType)
        {
            try
            {
                List<Holder> holders = DelegateService.underwritingService.GetHoldersByDocument(description, customerType);
                if (holders != null && holders.Any())
                {
                    return new UifJsonResult(true, holders);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorHolderNotFound);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchPolicyholder);
            }
        }

        public UifJsonResult GetHoldersByIndividualId(string individualId, CustomerType? customerType)
        {
            try
            {

                Tuple<Holder, List<IssuanceCompanyName>> result = DelegateService.underwritingService.GetHolderByIndividualId(individualId, customerType);
                if (result != null)
                {
                    return new UifJsonResult(true, new
                    {
                        holder = result.Item1,
                        details = result.Item2
                    });
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorHolderNotFound);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchPolicyholder);
            }
        }

        public UifJsonResult GetIndividualDetailsByIndividualId(int individualId, CustomerType customerType)
        {
            try
            {
                List<UPV1.CompanyName> details = DelegateService.uniquePersonServiceV1.GetNotificationAddressesByIndividualId(individualId, customerType);
                return new UifJsonResult(true, details);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSeachIndividualDetail);
            }
        }



        public UifJsonResult CompanyGetNotificationByIndividualId(int individualId, CustomerType customerType)
        {
            try
            {
                List<UPV1.CompanyName> details = DelegateService.uniquePersonServiceV1.CompanyGetNotificationByIndividualId(individualId, customerType);
                return new UifJsonResult(true, details);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSeachIndividualDetail);
            }
        }


        public UifJsonResult GetAgenciesByAgentIdDescription(int agentId, string description)
        {
            List<UPV1.Agency> agencies = DelegateService.uniquePersonServiceV1.GetAgenciesByAgentIdDescription(agentId, description);

            if (agencies.Count == 1)
            {
                if (agencies[0].Agent.DateDeclined > DateTime.MinValue)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorIntermediaryDischarged);
                }
                else if (agencies[0].DateDeclined > DateTime.MinValue)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorAgencyDischarged);
                }
                else
                {
                    return new UifJsonResult(true, agencies);
                }
            }
            else
            {
                return new UifJsonResult(true, agencies);
            }
        }

        public ActionResult GetAgenciesByAgentId(int agentId)
        {
            List<UPV1.Agency> agencies = DelegateService.uniquePersonServiceV1.GetAgenciesByAgentId(agentId);
            return new UifSelectResult(agencies.OrderBy(x => x.FullName));
        }

        public ActionResult GetUserAgenciesByAgentId(int agentId)
        {
            List<Sistran.Core.Application.UniqueUserServices.Models.UserAgency> agencies = DelegateService.uniqueUserService.GetAgenciesByAgentIdUserId(agentId, SessionHelper.GetUserId());
            return new UifJsonResult(true, agencies.OrderBy(x => x.FullName).ToList());
        }

        public ActionResult GetAgencyByAgentIdAgencyId(int agentId, int agencyId)
        {
            try
            {
                UPV1.Agency agency = DelegateService.uniquePersonServiceV1.GetAgencyByAgentIdAgentAgencyId(agentId, agencyId);

                if (agency != null)
                {
                    if (agency.DateDeclined > DateTime.MinValue)
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorIntermediaryDischarged);
                    }
                    else
                    {
                        return new UifJsonResult(true, agency);
                    }
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorIntermediaryNoExist);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorValidIntermediary);
            }
        }

        public ActionResult GetPrefixes()
        {
            List<Prefix> prefixes = DelegateService.commonService.GetPrefixes();
            return new UifJsonResult(true, prefixes.OrderBy(x => x.Description).ToList());
        }

        public ActionResult GetPrefixesByAgentIdAgents(int agentId)
        {
            List<BasePrefix> prefixes = DelegateService.uniquePersonServiceV1.GetPrefixesByAgentId(agentId);

            if (prefixes.Count > 0)
            {
                return new UifJsonResult(true, prefixes.OrderBy(x => x.Description).ToList());
            }
            else
            {
                return new UifJsonResult(false, App_GlobalResources.Language.MessageAgentWithoutPrefix);
            }
        }

        public ActionResult GetBranches()
        {
            List<Branch> branches = DelegateService.uniqueUserService.GetBranchesByUserId(SessionHelper.GetUserId());
            return new UifJsonResult(true, branches.OrderBy(x => x.Description).ToList());
        }

        public ActionResult GetSalePointsByBranchId(int branchId)
        {
            List<SalePoint> salePoints = DelegateService.uniqueUserService.GetSalePointsByBranchIdUserId(branchId, SessionHelper.GetUserId());
            return new UifJsonResult(true, salePoints.OrderBy(x => x.Description).ToList());
        }

        public ActionResult GetProductsByAgentIdPrefixId(int agentId, int prefixId, bool isCollective)
        {
            try
            {
                List<CompanyProduct> products = DelegateService.underwritingService.GetCompanyProductsByAgentIdPrefixId(agentId, prefixId, isCollective);
                return new UifJsonResult(true, products);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.MessageAgentWithoutProduct);
            }

        }

        public ActionResult GetPolicyTypesByProductId(int productId)
        {
            try
            {
                List<PolicyType> policyTypes = DelegateService.commonService.GetPolicyTypesByProductId(productId);
                if (policyTypes.Count > 0)
                {
                    return new UifJsonResult(true, policyTypes.OrderBy(x => x.Description).ToList());
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorPolicyTypesForBranch);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchPolicyType);
            }
        }

        public UifJsonResult GetCurrenciesByProductId(int productId)
        {
            try
            {
                if (parameters.Count == 0)
                {
                    try
                    {
                        parameters = this.GetParameters();
                    }
                    catch (Exception ex)
                    {
                        Debug.Write(ex.GetBaseException().Message);
                    }

                }
                List<Currency> currencies = DelegateService.productService.GetCurrenciesByProductId(productId);
                return new UifJsonResult(true, currencies.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCurrency);
            }
        }

        public UifJsonResult GetExchangeRateByCurrencyId(int currencyId)
        {
            if (parameters.Count == 0)
            {
                this.GetParameters();
            }

            ExchangeRate exchangeRate = null;

            if (currencyId != (int)parameters.First(x => x.Description == "Currency").Value)
            {
                DateTime IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Today);
                exchangeRate = DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(IssueDate, currencyId);
            }
            else
            {
                exchangeRate = new ExchangeRate
                {
                    Currency = new Currency
                    {
                        Id = currencyId
                    },
                    RateDate = DateTime.Now,
                    SellAmount = 1
                };
            }

            return new UifJsonResult(true, exchangeRate);
        }

        public UifJsonResult GetTemporalByIdTemporalType(int id, TemporalType temporalType, int? policieId)
        {
            try
            {
                CompanyPolicy policies = DelegateService.underwritingService.CompanyGetTemporalByIdTemporalType(id, temporalType);

                return new UifJsonResult(true, policies);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }





        public UifJsonResult GetPoliciesByQuotationIdVersionPrefixId(int operationId, int version, int prefixId, int branchId)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetPolicyByPendingOperation(operationId);
                if (policy.Id > 0)
                {
                    List<CompanyPolicy> policies = DelegateService.underwritingService.GetCompanyPoliciesByQuotationIdVersionPrefixId(policy.Id, version, prefixId, branchId).OrderBy(x => x.Endorsement.QuotationVersion).ToList();
                    return new UifJsonResult(true, policies);
                }
                else if (policy.TemporalType != null && policy.TemporalType != TemporalType.Quotation)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.TempNotType);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorTempNoExist);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchQuotation);
            }
        }

        public ActionResult SaveTemporal(PolicyModelsView policyModel, List<DynamicConcept> dynamicProperties)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    if (policyModel.CurrentTo > policyModel.CurrentFrom)
                    {
                        CiaPolicy.CompanyPolicy policy = ModelAssembler.CreatePolicy(policyModel);
                        if (policy != null)
                        {
                            policy.DynamicProperties = dynamicProperties;
                            policy.UserId = SessionHelper.GetUserId();
                            if (policy.Request != null)
                            {
                                policy = DelegateService.massiveService.SaveCompanyRequestTemporal(policy);
                            }
                            policy = DelegateService.underwritingService.SaveCompanyTemporal(policy, false);
                            return new UifJsonResult(true, policy);
                        }
                        else
                        {
                            return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
                        }
                    }
                    else
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorFromDateToDate);
                    }
                }
                else
                {
                    string errorMessage = this.GetErrorMessages();
                    return new UifJsonResult(false, errorMessage);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.GetBaseException().Message);
            }
        }




        public ActionResult CreatePolicyWihtoutEvents(int temporalId)
        {
            try
            {
                CiaPolicy.CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                if (policy != null)
                {
                    policy.InfringementPolicies.Clear();
                    switch (policy.Product.CoveredRisk.SubCoveredRiskType)
                    {
                        case SubCoveredRiskType.Vehicle:
                            List<CompanyVehicle> vehicles =
                                DelegateService.vehicleService.GetCompanyVehiclesByTemporalId(policy.Id);
                            vehicles.ForEach(x => x.Risk.InfringementPolicies.Clear());
                            policy = DelegateService.vehicleService.CreateEndorsement(policy, vehicles);
                            break;
                        case SubCoveredRiskType.ThirdPartyLiability:
                            List<CompanyTplRisk> thirdPartyLiabilities =
                                DelegateService.thirdPartyLiabilityService.GetThirdPartyLiabilitiesByTemporalId(
                                    policy.Id);
                            thirdPartyLiabilities.ForEach(x => x.Risk.InfringementPolicies.Clear());
                            policy = DelegateService.thirdPartyLiabilityService.CreateEndorsement(policy,
                                thirdPartyLiabilities);
                            break;
                        case SubCoveredRiskType.Property:
                            List<CompanyPropertyRisk> companyPropertyRisks =
                                DelegateService.propertyService.GetCompanyPropertiesByTemporalId(policy.Id);
                            companyPropertyRisks.ForEach(x => x.Risk.InfringementPolicies.Clear());
                            policy = DelegateService.propertyService.CreateEndorsement(policy, companyPropertyRisks);
                            break;
                        case SubCoveredRiskType.Liability:
                            List<CompanyLiabilityRisk> companyLiabilityRisks =
                                DelegateService.liabilityService.GetCompanyLiabilitiesByTemporalId(policy.Id);
                            companyLiabilityRisks.ForEach(x => x.Risk.InfringementPolicies.Clear());
                            policy = DelegateService.liabilityService.CreateEndorsement(policy, companyLiabilityRisks);
                            break;
                        case SubCoveredRiskType.Surety:
                        case SubCoveredRiskType.Lease:
                            List<CompanyContract> companySurety =
                                DelegateService.suretyService.GetCompanySuretiesByTemporalId(policy.Id);
                            companySurety.ForEach(x => x.Risk.InfringementPolicies.Clear());
                            policy = DelegateService.suretyService.CreateEndorsement(policy, companySurety);
                            break;
                        case SubCoveredRiskType.JudicialSurety:
                            List<CompanyJudgement> companyJudgements =
                                DelegateService.judicialSuretyService.GetCompanyJudgementsByTemporalId(policy.Id);
                            companyJudgements.ForEach(x => x.Risk.InfringementPolicies.Clear());
                            policy = DelegateService.judicialSuretyService.CreateEndorsement(policy, companyJudgements);
                            break;
                    }
                    return new UifJsonResult(true, policy);
                }
                else
                {
                    return new UifJsonResult(true, true);
                }
            }
            catch (Exception e)
            {

                return new UifJsonResult(false, e.Message);
            }
        }

        public ActionResult CreatePolicy(int temporalId, int temporalType)
        {
            try
            {
                UifJsonResult UifJsonResult = new UifJsonResult(false, null);
                int userId = Helpers.SessionHelper.GetUserId();
                CompanyCoveredRisk companyCoveredRisk = DelegateService.underwritingService.GetCompanyCoveredRiskByTemporalId(temporalId, false);
                Sistran.Core.Application.CommonService.Models.Parameter parameter = new Sistran.Core.Application.CommonService.Models.Parameter();
                if (companyCoveredRisk != null)
                {
                    switch (companyCoveredRisk.SubCoveredRiskType)
                    {
                        case SubCoveredRiskType.Vehicle:
                            CompanyPolicyResult companyPolicy = DelegateService.vehicleService.CreateCompanyPolicy(temporalId, temporalType, false);
                            if (companyPolicy.Errors != null && companyPolicy.Errors.Any())
                            {
                                UifJsonResult = new UifJsonResult(companyPolicy.Errors.First().StateData, companyPolicy.Errors.First().Error);
                            }
                            else if (companyPolicy.InfringementPolicies != null && companyPolicy.InfringementPolicies.Count > 0)
                            {
                                UifJsonResult = new UifJsonResult(true, companyPolicy.InfringementPolicies);
                            }
                            else
                            {
                                UifJsonResult = new UifJsonResult(true, companyPolicy.Message);
                            }
                            break;
                        case SubCoveredRiskType.ThirdPartyLiability:
                            CompanyPolicyResult companyPolicyThirdPartyLiability = DelegateService.thirdPartyLiabilityService.CreateCompanyPolicy(temporalId, temporalType, false);
                            if (companyPolicyThirdPartyLiability.Errors != null && companyPolicyThirdPartyLiability.Errors.Any())
                            {
                                UifJsonResult = new UifJsonResult(companyPolicyThirdPartyLiability.Errors.First().StateData, companyPolicyThirdPartyLiability.Errors.First().Error);
                            }
                            if (companyPolicyThirdPartyLiability.InfringementPolicies != null && companyPolicyThirdPartyLiability.InfringementPolicies.Count > 0)
                            {
                                UifJsonResult = new UifJsonResult(true, companyPolicyThirdPartyLiability.InfringementPolicies);
                            }
                            else
                            {
                                UifJsonResult = new UifJsonResult(true, companyPolicyThirdPartyLiability.Message);
                            }
                            break;
                        case SubCoveredRiskType.Property:
                            CompanyPolicyResult companyPolicyProperty = DelegateService.propertyService.CreateCompanyPolicy(temporalId, temporalType, false);
                            if (companyPolicyProperty.Errors != null && companyPolicyProperty.Errors.Any())
                            {
                                UifJsonResult = new UifJsonResult(companyPolicyProperty.Errors.First().StateData, companyPolicyProperty.Errors.First().Error);
                            }
                            if (companyPolicyProperty.InfringementPolicies != null && companyPolicyProperty.InfringementPolicies.Count > 0)
                            {
                                UifJsonResult = new UifJsonResult(true, companyPolicyProperty.InfringementPolicies);
                            }
                            else
                            {

                                UifJsonResult = new UifJsonResult(true, companyPolicyProperty.Message);
                            }
                            break;
                        case SubCoveredRiskType.Liability:
                            CompanyPolicyResult companyPolicyLiability = DelegateService.liabilityService.CreateCompanyPolicy(temporalId, temporalType, false, null);
                            if (companyPolicyLiability.Errors != null && companyPolicyLiability.Errors.Any())
                            {
                                UifJsonResult = new UifJsonResult(companyPolicyLiability.Errors.First().StateData, companyPolicyLiability.Errors.First().Error);
                            }
                            if (companyPolicyLiability.InfringementPolicies != null && companyPolicyLiability.InfringementPolicies.Count > 0)
                            {
                                UifJsonResult = new UifJsonResult(true, companyPolicyLiability.InfringementPolicies);
                            }
                            else
                            {
                                UifJsonResult = new UifJsonResult(true, companyPolicyLiability);
                            }
                            parameter = DelegateService.commonService.GetParameterByParameterId(12191);
                            if (parameter.NumberParameter == 0 || parameter.NumberParameter == 1)
                            {
                                companyPolicyLiability.IsReinsured = companyPolicyLiability.IsReinsured;
                            }
                            else if (parameter.NumberParameter == 2)
                            {
                                companyPolicyLiability.IsReinsured = true;
                            }
                            break;
                        case SubCoveredRiskType.Surety:
                            CompanyPolicyResult companyPolicySurety = DelegateService.suretyService.CreateCompanyPolicy(temporalId, temporalType, false, null);
                            if (companyPolicySurety.Errors != null && companyPolicySurety.Errors.Any())
                            {
                                UifJsonResult = new UifJsonResult(companyPolicySurety.Errors.First().StateData, companyPolicySurety.Errors.First().Error);
                                return UifJsonResult;
                            }
                            if (companyPolicySurety.InfringementPolicies != null && companyPolicySurety.InfringementPolicies.Count > 0)
                            {
                                UifJsonResult = new UifJsonResult(true, companyPolicySurety.InfringementPolicies);
                            }
                            else
                            {
                                UifJsonResult = new UifJsonResult(true, companyPolicySurety);
                            }
                            parameter = DelegateService.commonService.GetParameterByParameterId(12191);
                            if (parameter.NumberParameter == 2)
                            {
                                companyPolicySurety.IsReinsured = true;
                            }
                            break;
                        case SubCoveredRiskType.JudicialSurety:
                            CompanyPolicyResult companyPolicyJudicialSurety = DelegateService.judicialSuretyService.CreateCompanyPolicy(temporalId, temporalType, null);
                            if (companyPolicyJudicialSurety.Errors != null && companyPolicyJudicialSurety.Errors.Any())
                            {
                                UifJsonResult = new UifJsonResult(companyPolicyJudicialSurety.Errors.First().StateData, companyPolicyJudicialSurety.Errors.First().Error);
                            }
                            if (companyPolicyJudicialSurety.InfringementPolicies != null && companyPolicyJudicialSurety.InfringementPolicies.Count > 0)
                            {
                                UifJsonResult = new UifJsonResult(true, companyPolicyJudicialSurety.InfringementPolicies);
                            }
                            else
                            {
                                UifJsonResult = new UifJsonResult(true, companyPolicyJudicialSurety);
                            }
                            parameter = DelegateService.commonService.GetParameterByParameterId(12191);
                            if (parameter.NumberParameter == 2)
                            {
                                companyPolicyJudicialSurety.IsReinsured = true;
                            }
                            break;
                        case SubCoveredRiskType.Aircraft:
                            AircraftDTO.EndorsementDTO endorsementAircraftDTO = DelegateService.aircraftApplicationService.CreateEndorsement(temporalId);
                            UifJsonResult = new UifJsonResult(true, "El número de póliza es: " + endorsementAircraftDTO.PolicyNumber.ToString());
                            break;
                        case SubCoveredRiskType.Marine:
                            MarineDTOs.EndorsementDTO endorsementMarineDTO = DelegateService.marineApplicationService.CreateEndorsement(temporalId);
                            UifJsonResult = new UifJsonResult(true, "El número de póliza es: " + endorsementMarineDTO.PolicyNumber.ToString());
                            break;
                        case SubCoveredRiskType.FidelityNewVersion:
                            var companyPolicyFidelity = DelegateService.fidelityService.CreateCompanyPolicy(temporalId, temporalType);
                            if (companyPolicyFidelity.Errors != null && companyPolicyFidelity.Errors.Any())
                            {
                                UifJsonResult = new UifJsonResult(companyPolicyFidelity.Errors.First().StateData, companyPolicyFidelity.Errors.First().Error);
                            }
                            if (companyPolicyFidelity.InfringementPolicies != null && companyPolicyFidelity.InfringementPolicies.Count > 0)
                            {
                                UifJsonResult = new UifJsonResult(true, companyPolicyFidelity.InfringementPolicies);
                            }
                            else
                            {
                                UifJsonResult = new UifJsonResult(true, companyPolicyFidelity.Message);
                            }
                            break;
                        case SubCoveredRiskType.Transport:
                            CompanyPolicyResult companyPolicyTransport = DelegateService.TransportBusinessService.CreateCompanyPolicy(temporalId, temporalType, false);
                            if (companyPolicyTransport.Errors != null && companyPolicyTransport.Errors.Any())
                            {
                                UifJsonResult = new UifJsonResult(companyPolicyTransport.Errors.First().StateData, companyPolicyTransport.Errors.First().Error);
                            }
                            if (companyPolicyTransport.InfringementPolicies != null && companyPolicyTransport.InfringementPolicies.Count > 0)
                            {
                                UifJsonResult = new UifJsonResult(true, companyPolicyTransport.InfringementPolicies);
                            }
                            else
                            {
                                UifJsonResult = new UifJsonResult(true, companyPolicyTransport.Message);
                            }
                            break;
                        case SubCoveredRiskType.Lease:
                            CompanyPolicyResult companyPolicyLease = DelegateService.suretyService.CreateCompanyPolicy(temporalId, temporalType, false, null);
                            if (companyPolicyLease.Errors != null && companyPolicyLease.Errors.Any())
                            {
                                UifJsonResult = new UifJsonResult(companyPolicyLease.Errors.First().StateData, companyPolicyLease.Errors.First().Error);
                                return UifJsonResult;
                            }
                            if (companyPolicyLease.InfringementPolicies != null && companyPolicyLease.InfringementPolicies.Count > 0)
                            {
                                UifJsonResult = new UifJsonResult(true, companyPolicyLease.InfringementPolicies);
                            }
                            else
                            {
                                UifJsonResult = new UifJsonResult(true, companyPolicyLease);
                            }
                            break;


                            //if (endorsementDTO.Success)
                            //{
                            //    UifJsonResult = new UifJsonResult(true, "El número de póliza es: " + endorsementDTO.PolicyNumber.ToString());
                            //    UifJsonResult = new UifJsonResult(true, endorsementDTO.Message);
                            //}
                            //else
                            //{
                            //    UifJsonResult = new UifJsonResult(false, endorsementDTO.Message);
                            //}


                    }

                    return UifJsonResult;
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreatePolicy);
                }

            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "BusinessException")
                {
                    return new UifJsonResult(false, ex.GetBaseException().Message);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreatePolicy);
                }
            }

        }

        [HttpPost]
        public ActionResult GetIssueWithPolicies(int? temporalId)
        {
            try
            {
                int userId = Helpers.SessionHelper.GetUserId();
                List<IssueWithPolicies> list = DelegateService.AuthorizationPoliciesService.GetIssueWithPolicies(temporalId, userId);
                return new UifJsonResult(true, list);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetTemporal);
            }
        }

        [HttpPost]
        public ActionResult CreatePolicyWihtoutPolicies(int temporalId)
        {
            try
            {
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);

                if (companyPolicy != null)
                {
                    DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.Individual, temporalId.ToString(), null, "0");
                    CompanyPolicyResult companyPolicyResult = null;

                    if (companyPolicy.Endorsement.EndorsementType == EndorsementType.LastEndorsementCancellation)
                    {
                        CompanyEndorsement companyEndorsement = new CompanyEndorsement
                        {
                            Id = companyPolicy.Endorsement.Id,
                            PolicyId = companyPolicy.Endorsement.PolicyId,
                            EndorsementReasonId = companyPolicy.Endorsement.EndorsementReasonId,
                            TemporalId = companyPolicy.Id,
                            TicketDate = companyPolicy.Endorsement.TicketDate,
                            TicketNumber = companyPolicy.Endorsement.TicketNumber,
                            Text = companyPolicy.Endorsement.Text,
                            IssueDate = companyPolicy.IssueDate,
                            UserId = companyPolicy.UserId
                        };

                        switch (companyPolicy.Product.CoveredRisk.SubCoveredRiskType)
                        {
                            case SubCoveredRiskType.Vehicle:
                                companyPolicy = DelegateService.vehicleReversionServiceCia.CreateEndorsementReversion(companyEndorsement, true);
                                break;

                            case SubCoveredRiskType.Surety:
                            case SubCoveredRiskType.Lease:
                                companyPolicy = DelegateService.suretyReversionEndorsement.CreateEndorsementReversion(companyEndorsement, true);
                                break;

                            case SubCoveredRiskType.Property:
                                companyPolicyResult = DelegateService.propertyService.CreateCompanyPolicy(companyPolicy.Id, (int)companyPolicy.TemporalType, true);
                                break;

                            case SubCoveredRiskType.Liability:
                                companyPolicyResult = DelegateService.liabilityService.CreateCompanyPolicy(companyPolicy.Id, (int)companyPolicy.TemporalType, true, null);
                                break;

                            case SubCoveredRiskType.ThirdPartyLiability:
                                companyPolicyResult = DelegateService.thirdPartyLiabilityService.CreateCompanyPolicy(companyPolicy.Id, (int)companyPolicy.TemporalType, true);
                                break;

                            case SubCoveredRiskType.Transport:
                                companyPolicyResult = DelegateService.TransportBusinessService.CreateCompanyPolicy(companyPolicy.Id, (int)companyPolicy.TemporalType, true);
                                break;

                            default:
                                return new UifJsonResult(false, Language.InvalidTemporal + companyPolicy.Product.CoveredRisk.SubCoveredRiskType);
                        }

                        if (companyPolicyResult == null)
                        {
                            companyPolicyResult = new CompanyPolicyResult
                            {
                                DocumentNumber = companyPolicy.DocumentNumber,
                                EndorsementId = companyPolicy.Endorsement.Id,
                                EndorsementNumber = companyPolicy.Endorsement.Number
                            };
                        }
                    }
                    else
                    {
                        switch (companyPolicy.Product.CoveredRisk.SubCoveredRiskType)
                        {
                            case SubCoveredRiskType.Vehicle:
                                List<string> existsRiskAuthorization = DelegateService.vehicleService.ExistsRiskAuthorization(companyPolicy.Id);

                                if (existsRiskAuthorization == null)
                                {
                                    companyPolicyResult = DelegateService.vehicleService.CreateCompanyPolicy(companyPolicy.Id, (int)companyPolicy.TemporalType, true);
                                }
                                else
                                {
                                    return new UifJsonResult(false, string.Join(" : ", existsRiskAuthorization));
                                }

                                break;

                            case SubCoveredRiskType.Surety:
                            case SubCoveredRiskType.Lease:
                                companyPolicyResult = DelegateService.suretyService.CreateCompanyPolicy(companyPolicy.Id, (int)companyPolicy.TemporalType, true, null);
                                break;

                            case SubCoveredRiskType.Property:
                                companyPolicyResult = DelegateService.propertyService.CreateCompanyPolicy(companyPolicy.Id, (int)companyPolicy.TemporalType, true);
                                break;

                            case SubCoveredRiskType.Liability:
                                companyPolicyResult = DelegateService.liabilityService.CreateCompanyPolicy(companyPolicy.Id, (int)companyPolicy.TemporalType, true, null);
                                break;

                            case SubCoveredRiskType.ThirdPartyLiability:
                                companyPolicyResult = DelegateService.thirdPartyLiabilityService.CreateCompanyPolicy(companyPolicy.Id, (int)companyPolicy.TemporalType, true);
                                break;

                            case SubCoveredRiskType.Transport:
                                companyPolicyResult = DelegateService.TransportBusinessService.CreateCompanyPolicy(companyPolicy.Id, (int)companyPolicy.TemporalType, true);
                                break;

                            default:
                                return new UifJsonResult(false, Language.InvalidTemporal + companyPolicy.Product.CoveredRisk.SubCoveredRiskType);
                        }
                    }

                    DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.Individual, temporalId.ToString(), null, companyPolicyResult.EndorsementId.ToString());


                    if (companyPolicy.Endorsement.EndorsementType == EndorsementType.Emission)
                    {
                        companyPolicyResult.Message = string.Format(Language.PolicyNumber, companyPolicyResult.DocumentNumber);
                    }
                    else
                    {
                        companyPolicyResult.Message = string.Format(Language.EndorsementNumber1,
                            companyPolicyResult.DocumentNumber, companyPolicyResult.EndorsementNumber,
                            companyPolicyResult.EndorsementId);
                    }

                    if (companyPolicyResult.PromissoryNoteNumCode != 0)
                    {
                        companyPolicyResult.Message += string.Format(Language.PromissoryNote, companyPolicyResult.PromissoryNoteNumCode, companyPolicy.UserId);
                    }
                    return new UifJsonResult(true, companyPolicyResult.Message);
                }

                return new UifJsonResult(false, Language.ErrorGetTemporal + temporalId);
            }
            catch (Exception)
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.Individual, temporalId.ToString(), null, null);
                return new UifJsonResult(false, Language.ErrorIssuance);
            }
        }

        public ActionResult GetTitle(int temporalId)
        {
            try
            {
                CiaPolicy.CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                if (policy.Id > 0)
                {
                    if (policy.TemporalType == TemporalType.Quotation)
                    {
                        policy.TemporalTypeDescription = App_GlobalResources.Language.Quotation;
                    }
                    else
                    {
                        policy.TemporalTypeDescription = App_GlobalResources.Language.Temporal;
                    }

                    if (string.IsNullOrEmpty(policy.Endorsement.EndorsementTypeDescription))
                    {
                        policy.Endorsement.EndorsementTypeDescription = EnumsHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
                    }

                    return new UifJsonResult(true, policy);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorTempNoExist);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorUpdateRecord);
            }
        }

        private string GetErrorMessages()
        {
            StringBuilder sb = new StringBuilder();
            foreach (ModelState item in this.ModelState.Values)
            {
                if (item.Errors.Count > 0 && !sb.ToString().Contains(item.Errors[0].ErrorMessage))
                {
                    sb.Append(item.Errors[0].ErrorMessage).Append(", ");
                }
            }
            return sb.ToString().Remove(sb.ToString().Length - 2);
        }

        #endregion



        public ActionResult SaveAdditionalDAta(int temporalId, AdditionalDataTemporalModelsView additionalDataModel)
        {
            try
            {
                bool? calculateMinPremium = DelegateService.underwritingService.SaveCompanyAdditionalDAta(temporalId, additionalDataModel.CalculateMinimumPremium);
                return new UifJsonResult(true, calculateMinPremium);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveText);
            }
        }

        public ActionResult SaveCompanyCorrelativePolicy(int temporalId, AdditionalDataTemporalModelsView additionalDataModel)
        {
            try
            {
                int? correlativePolicy = DelegateService.underwritingService.SaveCompanyCorrelativePolicy(temporalId, additionalDataModel.CorrelativePolicyNumber);
                return new UifJsonResult(true, correlativePolicy);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorAdditionalData);
            }
        }

        public UifJsonResult GetModuleDateIssue()
        {
            return new UifJsonResult(true, DelegateService.commonService.GetModuleDateIssue(3, DateTime.Today));
        }

        public ActionResult RunRulesPolicyPre(PolicyModelsView policyModel, List<DynamicConcept> dynamicProperties)
        {
            try
            {
                CompanyPolicy policy = ModelAssembler.CompanyCreatePolicy(policyModel);
                if (policy != null)
                {
                    policy.DynamicProperties = dynamicProperties;
                    if (policy.Holder.InsuredId == 0 && policy.Holder.CustomerType != CustomerType.Prospect)
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorHolderNoInsuredRole);
                    }
                    CompanyPolicy companyPolicy = DelegateService.underwritingService.RunRulesCompanyPolicyPre(policy);
                    return new UifJsonResult(true, companyPolicy);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorRunningPreRules);
                }

            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "BusinessException")
                {
                    return new UifJsonResult(false, ex.Message);
                }
                return new UifJsonResult(false, ex.Message);
            }

        }

        #region busqueda personas

        public ActionResult GetInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType, TemporalType temporalType)
        {
            try
            {
                List<CompanyIssuanceInsured> insureds = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType, temporalType);
                return new UifJsonResult(true, insureds);

            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.GetBaseException().Message);
                //return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPerson);
            }
        }

        public UifJsonResult ExistProductAgentByAgentIdPrefixIdProductId(int agentId, int prefixId, int productId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.productService.ExistProductAgentByAgentIdPrefixIdProductId(agentId, prefixId, productId));
            }
            catch (Exception ex)
            {
                this.Response.StatusCode = 500;
                this.Response.StatusDescription = App_GlobalResources.Language.ErrorSearchProductAgent;
                return new UifJsonResult(false, ex.Message);
            }
        }
        #endregion



        #region endorsement
        public ActionResult GetEndorsementsByFilterPolicy(int branchId, int prefixId, decimal policyNumber)
        {
            try
            {
                List<CompanyEndorsement> endorsements = DelegateService.underwritingService.GetCiaEndorsementsByFilterPolicy(branchId, prefixId, policyNumber);
                if (endorsements != null)
                {
                    return new UifJsonResult(true, endorsements);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorPolicyNoExist);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchPolicy);
            }
        }
        #endregion
        #region validaciones
        private string ValidateSurety(int temporalId, CiaPolicy.CompanyPolicy policy)
        {
            string msj = string.Empty;
            try
            {
                msj = DelegateService.underwritingService.ValidateCompanySurety(temporalId, policy);
            }
            catch (Exception ex)
            {
                msj = ex.GetBaseException().ToString();
            }
            return msj;
        }
        #endregion

        #region Search Advanced
        public PartialViewResult TemporalAdvancedSearch()
        {
            return this.PartialView();
        }

        public ActionResult GetTemporalAdvancedSearch(CiaPolicy.CompanyPolicy policy)
        {
            try
            {
                List<CiaPolicy.CompanyPolicy> policies = new List<CiaPolicy.CompanyPolicy>();
                if (policy.Holder != null && policy.Holder.IndividualId == 0)
                {
                    policy.Holder = null;
                }
                else
                {
                    policy.Holder.CustomerType = CustomerType.Individual;
                    policy.Holder.IndividualType = IndividualType.Company;
                }
                policies = DelegateService.underwritingService.GetCompanyTemporalPoliciesByCompanyPolicy(policy);
                if (policies.Count > 0)
                {
                    return new UifJsonResult(true, policies);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorTemporalsNoExist);
                }

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetTemporal);
            }
        }

        public ActionResult GetHoldersByQuery(string query, InsuredSearchType insuredSearchType)
        {
            try
            {
                List<Holder> holders = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(query, insuredSearchType, CustomerType.Individual);
                if (holders != null)
                {
                    return this.Json(holders, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return this.Json(new List<Holder>(), JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {
                return this.Json(App_GlobalResources.Language.ErrorSearchPolicyholder, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        public ActionResult CreateNewVersionQuotation(int operationId)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.CreateNewVersionQuotation(operationId);


                if (policy != null)
                {
                    return new UifJsonResult(true, policy);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorQuotationNoExist);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateNewVersionQuotation);
            }
        }

        public ActionResult CreateTemporalFromQuotation(int operationId)
        {
            try
            {
                CompanyPolicy policy = DelegateService.quotationService.CreateCompanyTemporalFromQuotation(operationId);
                policy.UserId = SessionHelper.GetUserId();
                policy = DelegateService.underwritingService.CompanySavePolicyTemporal(policy, false);

                if (policy != null)
                {
                    return new UifJsonResult(true, policy);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorQuotationNoExist);
                }
            }

            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorConvertQuotation);
            }
        }


        private List<Quota> CalculateQuotas(CiaPolicy.CompanyPolicy policy)
        {
            ComponentValueDTO componentValueDTO = Mapper.Map<CompanySummary, ComponentValueDTO>(policy.Summary);
            QuotaFilterDTO quotaFilterDto = new QuotaFilterDTO { PlanId = policy.PaymentPlan.Id, CurrentFrom = policy.CurrentFrom, IssueDate = policy.IssueDate, ComponentValueDTO = componentValueDTO };
            return DelegateService.underwritingService.CalculateQuotas(quotaFilterDto);
        }

        public ActionResult UpdatePolicyComponents(int policyId)
        {
            try
            {
                CompanyPolicy companyPolicy = DelegateService.underwritingService.UpdatePolicyComponents(policyId);
                return new UifJsonResult(true, companyPolicy);
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
        public ActionResult GetPolicyLists(CompanyPolicy policyModel, bool isCollective)
        {
            try
            {
                PolicyCombosDTO combos = new PolicyCombosDTO();
                foreach (IssuanceAgency item in policyModel.Agencies)
                {
                    //if (item.IsPrincipal == true)
                    //{
                    List<Sistran.Core.Application.UniqueUserServices.Models.UserAgency> agencies = DelegateService.uniqueUserService.GetAgenciesByAgentIdUserId(item.Agent.IndividualId, SessionHelper.GetUserId());
                    if (item.IsPrincipal)
                    {
                        combos.UserAgencies = DelegateService.uniqueUserService.GetAgenciesByAgentIdUserId(item.Agent.IndividualId, SessionHelper.GetUserId());
                    }
                    
                    if (combos.BasePrefixes == null || (combos.BasePrefixes != null && combos.BasePrefixes.Count == 0))
                    {
                        combos.BasePrefixes = DelegateService.uniquePersonServiceV1.GetPrefixesByAgentId(item.Agent.IndividualId);
                    }

                    combos.CompanyProducts = DelegateService.underwritingService.GetCompanyProductsByAgentIdPrefixId(item.Agent.IndividualId, policyModel.Prefix.Id, isCollective);
                    //}

                }
                combos.Branches = DelegateService.uniqueUserService.GetBranchesByUserId(SessionHelper.GetUserId());
                combos.SalePoints = DelegateService.uniqueUserService.GetSalePointsByBranchIdUserId(policyModel.Branch.Id, policyModel.UserId);

                if (policyModel.Branch.SalePoints != null && policyModel.Branch.SalePoints.Count > 0)
                {
                    combos.SalePoints = DelegateService.uniqueUserService.GetSalePointsByBranchIdUserId(policyModel.Branch.Id, SessionHelper.GetUserId());
                }
                combos.PolicyTypes = DelegateService.commonService.GetPolicyTypesByProductId(policyModel.Product.Id);

                if (parameters.Count == 0)
                {
                    try
                    {
                        parameters = this.GetParameters();
                    }
                    catch (Exception ex)
                    {
                        Debug.Write(ex.GetBaseException().Message);
                    }

                }
                combos.Currencies = DelegateService.productService.GetCurrenciesByProductId(policyModel.Product.Id);
                combos.CompanyJustificationSarlafts = DelegateService.underwritingService.GetJustificationSarlaft();
                combos.Branches = combos.Branches.OrderBy(x => x.Description).ToList();
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

        public ActionResult GetDefaultValues()
        {
            try
            {
                if (defaultValuesUnderwriting.Count == 0)
                {
                    DefaultValue defaultValue = new DefaultValue
                    {
                        UserId = SessionHelper.GetUserId(),
                        ModuleId = (int)Sistran.Core.Application.UtilitiesServices.Enums.Modules.Subscription,
                        SubModuleId = (int)Sistran.Core.Application.UtilitiesServices.Enums.SubModules.Individuales,
                        ViewName = "/Underwriting/Underwriting"
                    };
                    defaultValuesUnderwriting = DelegateService.commonService.GetDefaultValueByDefaultValue(defaultValue);
                }

                return new UifJsonResult(true, defaultValuesUnderwriting);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchParameter);
            }
        }

        public CiaPolicy.CompanyRisk ConvertProspectToInsured(CiaPolicy.CompanyRisk risk, int individualId, string documentNumber)
        {
            CompanyRisk companyRisk = DelegateService.underwritingService.ConvertProspectToInsured(risk, individualId, documentNumber);
            return companyRisk;
        }

        public ActionResult ConvertProspectToHolder(int temporalId, int individualId, string documentNumber)

        {
            try
            {
                bool result = DelegateService.underwritingService.ConvertProspectToHolder(temporalId, individualId, documentNumber);
                return new UifJsonResult(true, result);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorConvertingProspectIntoIndividual);
            }
        }

        #region Advanced search quoation
        public PartialViewResult QuotationAdvancedSearch()
        {
            return this.PartialView();
        }

        public ActionResult GetPoliciesByPolicy(Policy policy)
        {
            try
            {
                List<Policy> policies = DelegateService.quotationService.GetPoliciesByPolicy(policy);
                if (policies.Count > 0)
                {
                    return new UifJsonResult(true, policies.OrderBy(b => b.Endorsement.QuotationId).ToList());
                }
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorTempNoExist);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchQuotation);
            }
        }

        public ActionResult GetUserPersonsByAccount(string account)
        {
            try
            {
                List<User> userPersons = DelegateService.uniqueUserService.GetUserPersonsByAccount(account);
                return this.Json(userPersons, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return this.Json(App_GlobalResources.Language.ErrorSearchPolicyholder, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


        public UifJsonResult DeleteTemporalByOperationId(int operationId, long documentNum, int prefixId, int branchId)
        {
            try
            {

                string result = DelegateService.underwritingService.DeleteTemporalByOperationId(operationId, documentNum, prefixId, branchId);
                return new UifJsonResult(true, result);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorDeleteTemporal);
            }

        }

        public ActionResult GetHoldersByQueryAdv(string query, InsuredSearchType insuredSearchType)
        {
            try
            {
                List<Holder> holders = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(query, insuredSearchType, null);
                if (holders != null)
                {
                    return this.Json(holders, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return this.Json(new List<Holder>(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return this.Json(App_GlobalResources.Language.ErrorSearchPolicyholder, JsonRequestBehavior.AllowGet);
            }
        }


        public UifJsonResult GetUserAgenciesByAgentIdDescription(int agentId, string description)
        {
            try
            {
                List<Sistran.Core.Application.UniqueUserServices.Models.UserAgency> agencies = DelegateService.uniqueUserService.GetCompanyUserAgenciesByAgentIdDescription(agentId, description, SessionHelper.GetUserId());
                return new UifJsonResult(true, agencies);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public UifJsonResult GetAgenciesByAgentIdDesciptionProductId(int agentId, string description, int productId)
        {
            int idUser = SessionHelper.GetUserId();
            List<ProductAgency> agencies = DelegateService.productService.GetAgenciesByAgentIdDesciptionProductIdUserId(agentId, description, productId, idUser);

            if (agencies.Count == 1)
            {
                if (agencies[0].Agent.DateDeclined > DateTime.MinValue)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorIntermediaryDischarged);
                }
                else if (agencies[0].DateDeclined > DateTime.MinValue)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorAgencyDischarged);
                }
                else
                {
                    return new UifJsonResult(true, agencies);
                }
            }
            else
            {
                return new UifJsonResult(true, agencies);
            }
        }

        public UifJsonResult GetAgenciesByDesciption(string agentId, string description, string productId)
        {
            try
            {
                int idUser = SessionHelper.GetUserId();
                List<ProductAgency> agencies = DelegateService.productService.GetAgenciesByAgentIdDesciptionProductIdUserId(Convert.ToInt16(agentId), description, Convert.ToInt16(productId), idUser);
                if (agencies.Count == 1)
                {
                    if (agencies[0].Agent.DateDeclined > DateTime.MinValue)
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorIntermediaryDischarged);
                    }
                    else if (agencies[0].DateDeclined > DateTime.MinValue)
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorAgencyDischarged);
                    }
                    else
                    {
                        List<CompanyPolicyAgent> agenciesCompany = ModelAssembler.CreateAgenciesCompany(agencies);
                        return new UifJsonResult(true, agenciesCompany);
                    }
                }
                else
                {
                    List<CompanyPolicyAgent> agenciesCompany = ModelAssembler.CreateAgenciesCompany(agencies);
                    return new UifJsonResult(true, agenciesCompany);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }

        }

        public UifJsonResult GetDefaultAgent()
        {
            UserAgency agencies = DelegateService.uniqueUserService.GetAgenciesByAgentIdDescriptionUserId(0, "", SessionHelper.GetUserId()).FirstOrDefault();
            return new UifJsonResult(true, agencies);
        }

        public UifJsonResult GetPolicyByQuotationId(int quotationId, int prefixId, int branchId)
        {
            try
            {
                Policy policy = DelegateService.quotationService.GetPolicyByQuotationId(quotationId, prefixId, branchId);
                return new UifJsonResult(true, policy);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQuotation);
            }
        }

        public UifJsonResult GetCalculateMinPremiumByProductId(int productId)
        {
            try
            {
                bool calculate = DelegateService.productService.GetCalculateMinPremiumByProductId(productId);
                return new UifJsonResult(true, calculate);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQuotation);
            }
        }


        public ActionResult GetAuthorizedPolicies()
        {
            try
            {
                List<NotificationUser> notificationsUser = DelegateService.uniqueUserService.GetNotificationByUser(SessionHelper.GetUserId(), null, false);//commonService.GetNotificationByUser(SessionHelper.GetUserId(), null, false);
                notificationsUser = notificationsUser.Where(x => x.Parameters.Count > 0).ToList();
                return new UifJsonResult(true, notificationsUser);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        #region Policy Previsora

        public ActionResult CompanySaveTemporal(PolicyModelsView policyModel, List<DynamicConcept> dynamicProperties,bool polities)
        {
          

            try
            {
              
                if (this.ModelState.IsValid)
                {
                    if (policyModel.CurrentTo > policyModel.CurrentFrom)
                    {
                        CompanyPolicy policy = ModelAssembler.CompanyCreatePolicy(policyModel);
                        policy.IssueDate = policy.IssueDate.ToString().ToDate(DateHelper.FormatFullDate);
                        policy.DynamicProperties = dynamicProperties ?? new List<DynamicConcept>();
                        if (policy != null)
                        {

                            policy.UserId = SessionHelper.GetUserId();
                            policy = DelegateService.underwritingService.CompanySavePolicyTemporal(policy, false,polities);
                            if (policy.Summary.Risks != null && policy.Prefix.Id == (int)UNSE.PrefixType.Automoviles)
                            {
                                policy = DelegateService.vehicleService.UpdateQuotationRisk(policy.Id, false);
                            }
                            if (policy.Prefix.Id == (int)PrefixTypeMinPremium.Lease || policy.Prefix.Id == (int)PrefixTypeMinPremium.Surety)
                            {
                                DelegateService.suretyService.UpdateRisks(policy.Id);
                            }
                          
                            return new UifJsonResult(true, policy);
                        }
                        else
                        {
                            return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
                        }
                    }
                    else
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorFromDateToDate);
                    }
                    
                }
                else
                {
                    string errorMessage = this.GetErrorMessages();
                    return new UifJsonResult(false, errorMessage);
                }

            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.GetBaseException().Message);
            }

        }

        #region Cotizacion
        public ActionResult GetQuotationById(CompanyPolicy policy)
        {
            try
            {
                List<CompanyPolicy> policies = DelegateService.underwritingService.GetQuotationById(policy.Endorsement.QuotationId, policy.Holder.IndividualId, policy.UserId, policy.CurrentFrom, policy.CurrentTo);
                // List<Policy> policiess = DelegateService.quotationService.GetQuotationById(cotiz);

                if (policies.Count > 0)
                {
                    return new UifJsonResult(true, policies.OrderByDescending(b => b.IssueDate).ToList());
                }
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorTempNoExist);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchQuotation);
            }
        }
        #endregion

        #region CompanyPremiumFinance

        public ActionResult SaveCompanyPremiumFinance(int temporalId, CiaPolicy.CompanyPaymentPlan companyPaymentPlan)
        {
            try
            {
                CompanyPaymentPlan result = DelegateService.underwritingService.CompanySavePremiumFinance(temporalId, companyPaymentPlan);
                return new UifJsonResult(true, result);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveClauses);
            }
        }

        #endregion

        #region Surcharges
        [HttpPost]
        public ActionResult SaveSurcharges(int temporalId, List<SurchargesViewModel> surcharges)
        {
            try
            {
                CompanySummaryComponent result = DelegateService.underwritingService.CompanySaveSurcharge(temporalId, ModelAssembler.CreateCompanySummarySurcharge(surcharges));
                return new UifJsonResult(true, result);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveClauses);
            }
        }

        [HttpGet]
        public UifJsonResult GetSurcharges()
        {
            try
            {
                List<SurchargesViewModel> surchargesViewModel = new List<SurchargesViewModel>();
                surchargesViewModel = ModelAssembler.CreateViewSurcharges(DelegateService.underwritingService.GetCompanySurcharges());
                return new UifJsonResult(true, surchargesViewModel.OrderBy(x => x.Id).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetSurcharge);
            }
        }


        [HttpPost]
        public UifJsonResult GetSurchargesByQuotation(int Id)
        {
            try
            {
                List<SurchargesViewModel> surchargesViewModel = new List<SurchargesViewModel>();
                CompanyPolicy companyPolicy = DelegateService.underwritingService.CompanyGetTemporalByIdTemporalType(Id, TemporalType.Quotation);
                surchargesViewModel = ModelAssembler.CreateViewSurcharges(companyPolicy.SummaryComponent.Surcharge);
                return new UifJsonResult(true, surchargesViewModel.OrderBy(x => x.Id).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetSurcharge);
            }
        }

        #endregion

        #endregion

        #region TablesExpreses
        public UifJsonResult GetTablesExpreses()
        {
            try
            {
                List<TablesExpresesModelsView> listTablesExpresesModelsView = new List<TablesExpresesModelsView>();
                TablesExpresesModelsView tablesExpresesModelsView = new TablesExpresesModelsView();
                ExpensesServiceModel expensesServiceModel = DelegateService.UnderwritingParamServiceWeb.GetExpenseServiceModel();
                listTablesExpresesModelsView = ModelAssembler.MappTablesExpreses(expensesServiceModel);
                return new UifJsonResult(true, listTablesExpresesModelsView);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult SaveTablesExpreses(int temporalId, List<TablesExpresesModelsView> tablesExpresesModelsViews)
        {
            try
            {
                //CiaPolicy.CompanySummaryComponent discountsCompany = new CompanySummaryComponent();
                //discountsCompany = ModelAssembler.MappCreateCompanySummaryDiscount(discounts);
                //var result = DelegateService.underwritingService.CompanySaveDiscounts(temporalId, discountsCompany);
                return new UifJsonResult(true, "");
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveClauses);
            }
        }

        #endregion

        #region Discounts
        public UifJsonResult GetDiscounts()
        {
            try
            {
                List<DiscountsModelsView> discountViewModel = new List<DiscountsModelsView>();
                discountViewModel = this.GetListDiscounts();
                return new UifJsonResult(true, discountViewModel.OrderBy(x => x.Id).ToList());
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex + " " + App_GlobalResources.Language.ErrorGetDiscount);
            }
        }

        private List<DiscountsModelsView> GetListDiscounts()
        {
            DiscountsServiceModel discountsServiceModel = new DiscountsServiceModel();
            List<DiscountsModelsView> listDiscountsModelsViews = new List<DiscountsModelsView>();
            discountsServiceModel = DelegateService.UnderwritingParamServiceWeb.GetDiscountServiceModel();
            listDiscountsModelsViews = ModelAssembler.MappCreateDiscount(DelegateService.UnderwritingParamServiceWeb.GetDiscountServiceModel());
            return listDiscountsModelsViews;
        }

        public ActionResult SaveDiscounts(int temporalId, List<DiscountsModelsView> discounts)
        {
            try
            {
                CiaPolicy.CompanySummaryComponent discountsCompany = new CompanySummaryComponent();
                discountsCompany = ModelAssembler.MappCreateCompanySummaryDiscount(discounts);
                CompanySummaryComponent result = DelegateService.underwritingService.CompanySaveDiscounts(temporalId, discountsCompany);
                return new UifJsonResult(true, result);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveClauses);
            }
        }
        #endregion

        #region Taxes 
        [HttpGet]
        public UifJsonResult GetTaxesByQuotation(string id)
        {
            try
            {
                return null; // por ahora
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetSurcharge);
            }
        }
        #endregion

        #region helpers
        public UifJsonResult GetRoundRate(int rateDecimal, decimal rate)
        {
            try
            {
                return new UifJsonResult(true, CurrencyHelper.RoundDecimal(rate, rateDecimal));
            }
            catch (Exception ex)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorParameterYearBisiesto + ex.Message.ToString());
            }
        }
        #endregion



        public ActionResult DeleteRiskByTemporalId(int temporalId)
        {
            try
            {
                List<CompanyRisk> risks = DelegateService.underwritingService.GetCiaRiskByTemporalId(temporalId, false);

                bool result = false;
                bool tmpResult = false;
                if (risks != null)
                {
                    foreach (CompanyRisk risk in risks)
                    {
                        result = DelegateService.utilitiesServiceCore.DeletePendingOperation(risk.Id);
                        tmpResult = DelegateService.underwritingService.DeleteRisk(risk.Id);
                    }
                    if (result && tmpResult)
                    {
                        return new UifJsonResult(true, "Riesgos eliminados correctamente");
                    }
                    else
                    {
                        return new UifJsonResult(false, "Error");
                    }
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

        /// <summary>
        /// Realiza el armado de la direccion dependiendo de las abreviaturas (tablas COMM.CO_NOMENCLATURES)
        /// </summary>
        /// <param name="Abreviatura"></param>
        /// <returns></returns>
        public JsonResult GetNomenclaturesTask(string query)
        {
            try
            {
                string[] strings = query.Split(' ');
                string lastPart = strings[strings.Length - 1];
                if (string.IsNullOrEmpty(lastPart))
                {
                    return Json(new List<Application.Common.Entities.CoNomenclatures>(), JsonRequestBehavior.AllowGet);
                }
                string result = "";
                foreach (string item in query.Split(' '))
                {
                    if (item != lastPart)
                    {
                        result += item + " ";
                    }
                }
                List<Company.Application.CommonServices.Models.Nomenclature> nomenclatures = DelegateService.commonService.GetNomenclaturesTask(0, lastPart, "", false);
                if (nomenclatures != null && nomenclatures.Count > 0)
                {
                    var result1 = nomenclatures.Select(x => new { nomenclature = result + " " + x.Nomenclatura, Abreviatura = result + " " + x.Abreviatura }).ToList();
                    return Json(result1.OrderBy(x => x.nomenclature), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new List<Application.Common.Entities.CoNomenclatures>(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (System.Exception)
            {
                return Json(App_GlobalResources.Language.Error, JsonRequestBehavior.DenyGet);
            }

        }

        public ActionResult DeleteEndorsementControl(int EndorsementId)
        {
            try
            {
                DelegateService.utilitiesServiceCore.DeleteEndorsementControl(EndorsementId);
                return new UifJsonResult(true, true);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, false);
            }
        }



        public ActionResult GetTransformAddress(string query)
        {
            try
            {
                List<Company.Application.CommonServices.Models.Nomenclature> nomenclatures = DelegateService.commonService.GetTransformAddress(query);
                return new UifJsonResult(true, nomenclatures);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryAssuranceMode);
            }
        }

        public ActionResult GetExtendedParameterByParameterId()
        {
            try
            {
                Sistran.Core.Application.CommonService.Models.Parameter parameter =
                    DelegateService.commonService.GetParameterByParameterId(2204);

                return new UifJsonResult(true, parameter?.AmountParameter);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, "19.25");
            }
        }

        public ActionResult UpdateProspect(int operationId, int individualId)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(operationId, false);

                switch ((PrefixTypeMinPremium)policy.Prefix.Id)
                {
                    case PrefixTypeMinPremium.Surety:
                        policy.Summary = DelegateService.suretyService.ConvertProspectToInsuredRisk(policy, individualId);
                        break;
                    case PrefixTypeMinPremium.JudicialSurety:
                        policy.Summary = DelegateService.judicialSuretyService.ConvertProspectToInsuredRisk(policy, individualId);
                        break;
                    case PrefixTypeMinPremium.Vehicle:
                        //pendiente validación para autos
                        break;
                    case PrefixTypeMinPremium.Liability:
                        policy.Summary = DelegateService.liabilityService.ConvertProspectToInsuredRisk(policy, individualId);
                        break;
                    default:
                        break;

                }

                return new UifJsonResult(true, policy.Summary);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, "");
            }
        }
        public UifJsonResult GetDate()
        {
            try
            {
                DateTime dateNow = DelegateService.commonService.GetDate();
                return new UifJsonResult(true, dateNow);

            }
            catch (Exception ex)
            {

                return new UifJsonResult(false, ex.Message);
            }
        }

        /// <summary>
        /// Obtine numero de decimales por producto moneda
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="currencyId">The currency identifier.</param>
        /// <returns></returns>
        public UifJsonResult GetDecimalByProductIdCurrencyId(int productId, short currencyId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.operationQuotaService.GetDecimalByProductIdCurrencyId(productId, currencyId));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex);
            }
        }


        public UifJsonResult GetPayerPaymetComponents(int EndorsementId, int Quota)
        {
            try
            {
                var components = DelegateService.accountingApplicationService.GetPayerPaymentComponentsByEndorsementIdQuotaNumber(EndorsementId, Quota);
                return new UifJsonResult(true, components);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex);
            }
        }

        public UifJsonResult GetRiskByPolicyId(int policyId, int tempId, int prefixId, int temporalType, bool? isCopyRisk)
        {
            try
            {
                switch ((PrefixTypeMinPremium)prefixId)
                {
                    case PrefixTypeMinPremium.Lease:
                    case PrefixTypeMinPremium.Surety:
                        List<CompanyContract> companyContracts = DelegateService.suretyService.GetCompanySuretiesByPolicyId(policyId);
                        if (companyContracts != null)
                        {
                            foreach (CompanyContract companyContract in companyContracts)
                            {
                                companyContract.Risk.IsPersisted = true;
                                companyContract.Risk.Status = RiskStatusType.Original;
                                companyContract.Risk.OriginalStatus = RiskStatusType.Original;

                                //tarifar poliza para que apliquen los valores actuales y guardar riesgo asociado al temporal de emision
                                CompanyContract contract = DelegateService.suretyService.GetCompanyPremium(tempId, companyContract, temporalType);

                                if (contract == null)
                                {
                                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorAvaliableOperationQuota);
                                }
                                else
                                {
                                    foreach (CompanyCoverage Coverage in contract.Risk.Coverages)
                                    {
                                        Coverage.IsVisible = isCopyRisk.GetValueOrDefault(true);
                                        Coverage.EndorsementType = EndorsementType.Emission;
                                        Coverage.CoverStatusName = string.Empty;
                                        Coverage.CoverStatus = CoverageStatusType.Original;
                                        Coverage.CoverageOriginalStatus = CoverageStatusType.Original;
                                    }

                                    contract.Risk.RiskId = 0;

                                    DelegateService.suretyService.SaveCompanyRisk(tempId, contract);
                                }
                            }
                            return new UifJsonResult(true, companyContracts);
                        }
                        else
                        {

                        }
                        break;
                    case PrefixTypeMinPremium.JudicialSurety:
                        List<CompanyJudgement> companyJudgements = DelegateService.judicialSuretyService.GetCompanyJudicialSuretyByPolicyId(policyId);
                        foreach (CompanyJudgement companyJudgement in companyJudgements)
                        {
                            companyJudgement.Risk.IsPersisted = true;
                            companyJudgement.Risk.Status = RiskStatusType.Original;
                            companyJudgement.Risk.OriginalStatus = RiskStatusType.Original;

                            //tarifar poliza para que apliquen los valores actuales y guardar riesgo asociado al temporal de emision
                            CompanyJudgement judgement = DelegateService.judicialSuretyService.GetCompanyPremium(tempId, companyJudgement);

                            foreach (CompanyCoverage Coverage in judgement.Risk.Coverages)
                            {
                                Coverage.IsVisible = isCopyRisk.GetValueOrDefault(true);
                                Coverage.EndorsementType = EndorsementType.Emission;
                                Coverage.CoverStatusName = string.Empty;
                                Coverage.CoverStatus = CoverageStatusType.Original;
                                Coverage.CoverageOriginalStatus = CoverageStatusType.Original;
                            }

                            judgement.Risk.RiskId = 0;

                            DelegateService.judicialSuretyService.SaveCompanyRisk(judgement, tempId);
                        }
                        return new UifJsonResult(true, companyJudgements);
                        break;
                    case PrefixTypeMinPremium.Vehicle:
                        List<CompanyVehicle> vehicles = DelegateService.vehicleService.GetCompanyVehiclesByPolicyId(policyId);
                        foreach (CompanyVehicle vehicle in vehicles)
                        {
                            vehicle.Risk.IsPersisted = true;
                            vehicle.Risk.Status = RiskStatusType.Original;
                            vehicle.Risk.OriginalStatus = RiskStatusType.Original;

                            //tarifar poliza para que apliquen los valores actuales y guardar riesgo asociado al temporal de emision
                            CompanyVehicle cVehicle = DelegateService.vehicleService.GetCompanyPremium(tempId, vehicle);

                            foreach (CompanyCoverage Coverage in cVehicle.Risk.Coverages)
                            {
                                Coverage.IsVisible = isCopyRisk.GetValueOrDefault(true);
                                Coverage.EndorsementType = EndorsementType.Emission;
                                Coverage.CoverStatusName = string.Empty;
                                Coverage.CoverStatus = CoverageStatusType.Original;
                                Coverage.CoverageOriginalStatus = CoverageStatusType.Original;
                            }

                            cVehicle.Risk.RiskId = 0;

                            DelegateService.vehicleService.SaveCompanyRisk(tempId, cVehicle);
                        }
                        return new UifJsonResult(true, vehicles);
                        break;
                    case PrefixTypeMinPremium.Liability:
                        List<CompanyLiabilityRisk> companyLiabilityRisks = DelegateService.liabilityService.GetCompanyLiebilitiesByPolicyId(policyId);
                        foreach (CompanyLiabilityRisk companyLiabilityRisk in companyLiabilityRisks)
                        {
                            companyLiabilityRisk.Risk.IsPersisted = true;
                            companyLiabilityRisk.Risk.Status = RiskStatusType.Original;
                            companyLiabilityRisk.Risk.OriginalStatus = RiskStatusType.Original;

                            //tarifar poliza para que apliquen los valores actuales y guardar riesgo asociado al temporal de emision
                            CompanyLiabilityRisk companyLiability = DelegateService.liabilityService.GetCompanyPremium(tempId, companyLiabilityRisk);

                            foreach (CompanyCoverage Coverage in companyLiability.Risk.Coverages)
                            {
                                Coverage.IsVisible = isCopyRisk.GetValueOrDefault(true);
                                Coverage.EndorsementType = EndorsementType.Emission;
                                Coverage.CoverStatusName = string.Empty;
                                Coverage.CoverStatus = CoverageStatusType.Original;
                                Coverage.CoverageOriginalStatus = CoverageStatusType.Original;
                            }

                            companyLiabilityRisk.Risk.RiskId = 0;

                            DelegateService.liabilityService.SaveRisk(companyLiability, tempId, companyLiabilityRisk.Risk.RiskId, null);
                        }
                        return new UifJsonResult(true, companyLiabilityRisks);
                        break;
                    default:
                        break;
                }
                return null;
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }


        }

        public UifJsonResult GetRiskByTemporalId(int tempId, int tempPolicyId, int prefixId, int temporalType, bool? isCopyRisk)
        {
            try
            {
                switch ((PrefixTypeMinPremium)prefixId)
                {
                    case PrefixTypeMinPremium.Lease:
                    case PrefixTypeMinPremium.Surety:
                        List<CompanyContract> companyContracts = DelegateService.suretyService.GetCompanySuretiesByTemporalId(tempId);
                        if (companyContracts != null && companyContracts.Count > 0)
                        {

                            foreach (CompanyContract companyContract in companyContracts)
                            {
                                companyContract.Risk.IsPersisted = true;
                                companyContract.Risk.Status = RiskStatusType.Original;
                                companyContract.Risk.OriginalStatus = RiskStatusType.Original;

                                //tarifar poliza para que apliquen los valores actuales y guardar riesgo asociado al temporal de emision
                                CompanyContract contract = DelegateService.suretyService.GetCompanyPremium(tempId, companyContract, temporalType);

                                if (contract == null)
                                {
                                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorAvaliableOperationQuota);
                                }
                                else
                                {
                                    foreach (CompanyCoverage Coverage in contract.Risk.Coverages)
                                    {
                                        Coverage.IsVisible = isCopyRisk.GetValueOrDefault(true);
                                        Coverage.EndorsementType = EndorsementType.Emission;
                                        Coverage.CoverStatusName = string.Empty;
                                        Coverage.CoverStatus = CoverageStatusType.Original;
                                        Coverage.CoverageOriginalStatus = CoverageStatusType.Original;
                                    }

                                    contract.Risk.Id = 0;

                                    DelegateService.suretyService.SaveCompanyRisk(tempPolicyId, contract);
                                }
                            }
                        }
                        return new UifJsonResult(true, companyContracts);

                    case PrefixTypeMinPremium.JudicialSurety:
                        List<CompanyJudgement> companyJudgements = DelegateService.judicialSuretyService.GetCompanyJudgementsByTemporalId(tempId);

                        foreach (CompanyJudgement companyJudgement in companyJudgements)
                        {
                            companyJudgement.Risk.IsPersisted = true;
                            companyJudgement.Risk.Status = RiskStatusType.Original;
                            companyJudgement.Risk.OriginalStatus = RiskStatusType.Original;

                            //tarifar poliza para que apliquen los valores actuales y guardar riesgo asociado al temporal de emision
                            CompanyJudgement judgement = DelegateService.judicialSuretyService.GetCompanyPremium(tempId, companyJudgement);

                            foreach (CompanyCoverage Coverage in judgement.Risk.Coverages)
                            {
                                Coverage.IsVisible = isCopyRisk.GetValueOrDefault(true);
                                Coverage.EndorsementType = EndorsementType.Emission;
                                Coverage.CoverStatusName = string.Empty;
                                Coverage.CoverStatus = CoverageStatusType.Original;
                                Coverage.CoverageOriginalStatus = CoverageStatusType.Original;
                            }

                            judgement.Risk.Id = 0;

                            DelegateService.judicialSuretyService.SaveCompanyRisk(judgement, tempPolicyId);
                        }
                        return new UifJsonResult(true, companyJudgements);

                    case PrefixTypeMinPremium.Liability:
                        List<CompanyLiabilityRisk> companyLiabilityRisks = DelegateService.liabilityService.GetCompanyLiabilitiesByTemporalId(tempId);
                        foreach (CompanyLiabilityRisk companyLiabilityRisk in companyLiabilityRisks)
                        {
                            companyLiabilityRisk.Risk.IsPersisted = true;
                            companyLiabilityRisk.Risk.Status = RiskStatusType.Original;
                            companyLiabilityRisk.Risk.OriginalStatus = RiskStatusType.Original;

                            //tarifar poliza para que apliquen los valores actuales y guardar riesgo asociado al temporal de emision
                            CompanyLiabilityRisk companyLiability = DelegateService.liabilityService.GetCompanyPremium(tempId, companyLiabilityRisk);

                            foreach (CompanyCoverage Coverage in companyLiability.Risk.Coverages)
                            {
                                Coverage.IsVisible = isCopyRisk.GetValueOrDefault(true);
                                Coverage.EndorsementType = EndorsementType.Emission;
                                Coverage.CoverStatusName = string.Empty;
                                Coverage.CoverStatus = CoverageStatusType.Original;
                                Coverage.CoverageOriginalStatus = CoverageStatusType.Original;
                            }

                            companyLiability.Risk.Id = 0;
                            companyLiabilityRisk.Risk.RiskId = 0;
                            DelegateService.liabilityService.SaveRisk(companyLiability, tempPolicyId, companyLiabilityRisk.Risk.RiskId, null);
                        }
                        return new UifJsonResult(true, companyLiabilityRisks);
                    default:
                        return new UifJsonResult(false, null);
                }

            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public UifJsonResult GetRiskByTemporald(int tempId)
        {
            try
            {
                var riskTemporal = DelegateService.suretyService.GetCompanySuretiesByTemporalId(tempId);
                return new UifJsonResult(true, riskTemporal);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public UifJsonResult GetRateCoveragesByCoverageIdPolicyId(int policyId, int coverageId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.underwritingService.GetRateCoveragesByCoverageIdPolicyId(policyId, coverageId));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }
    }
}