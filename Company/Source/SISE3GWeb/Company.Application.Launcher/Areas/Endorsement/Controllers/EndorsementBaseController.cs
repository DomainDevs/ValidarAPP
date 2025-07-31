using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.Marines.MarineBusinessService.Models.Base;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    using Endorsement = Application.UnderwritingServices.Models.Endorsement;
    [Authorize]
    public class EndorsementBaseController : Controller
    {
        private string message = string.Empty;


        public ActionResult GetAgentsByPolicyIdEndorsementId(int policyId, int endorsementId, decimal premium)
        {
            try
            {
                if (policyId > 0 && endorsementId > 0)
                {
                    List<IssuanceAgency> agency = DelegateService.underwritingService.GetAgentsByPolicyIdEndorsementId(policyId, endorsementId);
                    for (int i = 0; i < agency.Count; i++)
                    {
                        if (premium > 0)
                        {
                            agency[i].Commissions[0].Amount = premium * ((agency[i].Commissions[0].Percentage + agency[i].Commissions[0].PercentageAdditional) / 100) * (agency[i].Participation / 100);
                        }
                    }
                    return new UifJsonResult(true, agency);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorTempNoExist);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchTemp);
            }
        }

        public ActionResult GetTemporalById(int id)
            {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(id, false);

                if (policy != null)
                {
                    policy = GetPolicyDescriptions(policy);

                    return new UifJsonResult(true, policy);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorTempNoExist);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchTemp);
            }
        }

        public ActionResult CreateEndorsement(int temporalId, decimal policyNumber, CompanyModification companyModification)
        {
            try
            {
                if (temporalId > 0)
                {
                    string message = ValidateEndorsement(temporalId);

                    if (message == "")
                    {
                        CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                        policy.UserId = SessionHelper.GetUserId();
                        policy.User = new CompanyPolicyUser { UserId = SessionHelper.GetUserId() };

                        CompanyPolicyResult companyPolicyResult = CreateCompanyPolicyResult(policy, companyModification);
                        if (companyPolicyResult.Errors != null && companyPolicyResult.Errors.Any())
                        {
                            return new UifJsonResult(companyPolicyResult.Errors.First().StateData, companyPolicyResult.Errors.First().Error);
                        }
                        else if (companyPolicyResult.InfringementPolicies != null && companyPolicyResult.InfringementPolicies.Count > 0)
                        {
                            return new UifJsonResult(true, companyPolicyResult.InfringementPolicies);
                        }
                        else
                        {
                            string additionalFinancing = "";
                            if (policy.PaymentPlan.PremiumFinance != null && companyPolicyResult.PromissoryNoteNumCode > 0)
                            {
                                additionalFinancing = App_GlobalResources.Language.LabelPromissoryNote + ": " + companyPolicyResult.PromissoryNoteNumCode + ". ID " + App_GlobalResources.Language.LabelUser + ": " + policy.User.UserId;
                            }
                            message = App_GlobalResources.Language.SuccessfullyEndorsementGenerated + "." + App_GlobalResources.Language.LabelNumberPolicy + ": " + policyNumber.ToString() + "." + App_GlobalResources.Language.EndorsementNumber + ": " + companyPolicyResult.EndorsementNumber.ToString() + ". Endorsement Id" + ": " + companyPolicyResult.EndorsementId.ToString() + ". " + additionalFinancing;
                            return new UifJsonResult(true, message);
                        }
                    }
                    else
                    {
                        return new UifJsonResult(false, message);
                    }
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.NoExistTemporaryEmit);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public CompanyPolicyResult CreateCompanyPolicyResult(CompanyPolicy policy, CompanyModification companyModification)
        {
            policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
            CompanyPolicyResult companyPolicyResult = new CompanyPolicyResult();
            switch (policy.Product.CoveredRisk.SubCoveredRiskType)
            {
                case SubCoveredRiskType.Vehicle:
                    companyPolicyResult = DelegateService.vehicleService.CreateCompanyPolicy(policy.Id, (int)policy.TemporalType, false);
                    break;
                case SubCoveredRiskType.ThirdPartyLiability:
                    companyPolicyResult = DelegateService.thirdPartyLiabilityService.CreateCompanyPolicy(policy.Id, (int)policy.TemporalType, false);
                    break;
                case SubCoveredRiskType.Property:
                    companyPolicyResult = DelegateService.propertyService.CreateCompanyPolicy(policy.Id, (int)policy.TemporalType, false);
                    break;
                case SubCoveredRiskType.Liability:
                    DelegateService.liabilityService.GetCompanyLiabilitiesByTemporalId(policy.Id)
                        .ForEach(x => { x.Risk.Policy = policy; DelegateService.liabilityService.CreateLiabilityTemporal(x, false); });
                    companyPolicyResult = DelegateService.liabilityService.CreateCompanyPolicy(policy.Id, (int)policy.TemporalType, false, companyModification);
                    break;
                case SubCoveredRiskType.Surety:
                    DelegateService.suretyService.GetCompanySuretiesByTemporalId(policy.Id)
                        .ForEach(x => { x.Risk.Policy = policy; DelegateService.suretyService.CreateSuretyTemporal(x, false); });
                    companyPolicyResult = DelegateService.suretyService.CreateCompanyPolicy(policy.Id, (int)policy.TemporalType, false, companyModification);
                    break;
                case SubCoveredRiskType.Lease:
                    companyPolicyResult = DelegateService.suretyService.CreateCompanyPolicy(policy.Id, (int)policy.TemporalType, false, companyModification);
                    break;
                case SubCoveredRiskType.Transport:
                    companyPolicyResult = DelegateService.TransportBusinessService.CreateCompanyPolicy(policy.Id, (int)policy.TemporalType, false);
                    break;
                case SubCoveredRiskType.Fidelity:
                    return null;
                case SubCoveredRiskType.Aircraft:
                    return DelegateService.aircraftBusinessService.CreateCompanyPolicy(policy.Id, (int)policy.TemporalType);
                    break;
                case SubCoveredRiskType.Marine:
                    List<CompanyMarine> companyMarineRisks = DelegateService.marineBusinessService.GetCompanyMarinesByTemporalId(policy.Id);
                    return DelegateService.marineBusinessService.CreateCompanyPolicy(policy.Id, (int)policy.TemporalType);
                    break;
                case SubCoveredRiskType.FidelityNewVersion:
                    return DelegateService.fidelityService.CreateCompanyPolicy(policy.Id, (int)policy.TemporalType);
                    break;
                case SubCoveredRiskType.JudicialSurety:
                    DelegateService.judicialSuretyService.GetCompanyJudgementsByTemporalId(policy.Id)
                        .ForEach(x => { x.Risk.Policy = policy; DelegateService.judicialSuretyService.CreateJudgementTemporal(x, false); });
                    return DelegateService.judicialSuretyService.CreateCompanyPolicy(policy.Id, (int)policy.TemporalType, companyModification);
                    break;
                default:
                    return null;
            }
            return companyPolicyResult;
        }

        private CompanyPolicy GetPolicyDescriptions(CompanyPolicy policy)
        {
            var imapper = ModelAssembler.CreateMappCompanyEndorsementPolicyDescriptionBranch();
            if (policy.Branch != null && string.IsNullOrEmpty(policy.Branch.Description))
            {
                policy.Branch = imapper.Map<Branch, CompanyBranch>(DelegateService.commonService.GetBranchById(policy.Branch.Id));
            }
            var imap = ModelAssembler.CreateMappCompanyEndorsementPolicyDescriptionBranch();

            if (string.IsNullOrEmpty(policy.Prefix.Description))
            {
                policy.Prefix = imap.Map<Prefix, CompanyPrefix>(DelegateService.commonService.GetPrefixById(policy.Prefix.Id));
            }

            if (string.IsNullOrEmpty(policy.Product.Description))
            {
                policy.Product = DelegateService.productService.GetCompanyProductById(policy.Product.Id);
            }

            if (policy.Endorsement.Text == null)
            {
                policy.Endorsement.Text = policy.Text;
            }
            var fiscalResponsibility = DelegateService.uniquePersonServiceCore.GetFiscalResponsibilityByIndividualId(policy.Holder.IndividualId);
            if (fiscalResponsibility.Count > 0)
            {
                policy.Holder.FiscalResponsibility = fiscalResponsibility;
            }
            var EmailElectronic = DelegateService.uniquePersonServiceCore.GetEmailsByIndividualId(policy.Holder.IndividualId);
            if (EmailElectronic.Count > 0)
            {
                foreach (Email email in EmailElectronic)
                {
                    if (email.IsPrincipal != true)
                    {
                        policy.Holder.Email = email.Description;
                    }
                }
            }

            return policy;
        }

        protected CompanyPolicy CalculatePolicyAmounts(CompanyPolicy policy, List<CompanyRisk> risks)
        {
            policy.PayerComponents = DelegateService.underwritingService.CalculatePayerComponentsByCompanyPolicy(policy, risks);

            policy.Summary = DelegateService.underwritingService.CalculateSummaryByCompanyPolicy(policy, risks);

            if (policy.Summary != null)
            {
                ComponentValueDTO componentValueDTO = Mapper.Map<CompanySummary, ComponentValueDTO>(policy.Summary);
                QuotaFilterDTO quotaFilterDto = new QuotaFilterDTO { PlanId = policy.PaymentPlan.Id, CurrentFrom = policy.CurrentFrom, IssueDate = policy.IssueDate, ComponentValueDTO = componentValueDTO };
                policy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(quotaFilterDto);
                policy.Agencies = DelegateService.underwritingService.CalculateCommissionsByCompanyPolicy(policy, risks);
            }
            else
            {
                policy.PaymentPlan.Quotas = new List<Quota>();
                foreach (IssuanceAgency agency in policy.Agencies)
                {
                    if (agency.Commissions != null)
                    {
                        foreach (IssuanceCommission item in agency.Commissions)
                        {
                            item.CalculateBase = 0;
                            item.Amount = 0;
                        }
                    }
                }
            }

            return policy;
        }

        protected string GetErrorMessages()
        {
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
            .Select(x => new { x.Key, x.Value.Errors })
            .ToArray();
            if (errors?.Count() > 0)
            {
                return string.Join("", errors.SelectMany(x => x.Errors).Select(m => m.ErrorMessage).ToList());
            }
            else
            {
                return "";
            }
            //StringBuilder sb = new StringBuilder();
            //foreach (ModelState item in ModelState.Values)
            //{
            //    if (item.Errors.Count > 0)
            //    {
            //        sb.Append(item.Errors[0].ErrorMessage).Append(", ");
            //    }
            //}
            //return sb.ToString().Remove(sb.ToString().Length - 2);
        }

        public ActionResult GetEndorsementsByFilterPolicy(int branchId, int prefixId, decimal policyNumber)
        {
            try
            {
                List<Sistran.Core.Application.UnderwritingServices.Models.Endorsement> endorsements = DelegateService.underwritingService.GetEndorsementsByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyNumber);

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
        #region Validaciones
        public string ValidateEndorsement(int temporalId)
        {
            string message = string.Empty;
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);

                if (policy != null)
                {

                    if (policy.Endorsement.EndorsementType != EndorsementType.Cancellation
                        && policy.Endorsement.EndorsementType != EndorsementType.Nominative_cancellation)
                    {
                        List<CompanyRisk> risks = DelegateService.underwritingService.GetCompanyRisksByTemporalId(temporalId, false);

                        if (risks.Count > 0)
                        {
                            bool validEndorsement = true;
                            foreach (CompanyRisk risk in risks)
                            {

                                if (policy.PolicyOrigin == PolicyOrigin.Collective)
                                {
                                    if (risk.Status != null)
                                    {
                                        validEndorsement = false;
                                        message = string.Empty;
                                    }
                                }

                                if (risk.Status != null && risk.Status.Value != RiskStatusType.Excluded)
                                {
                                    validEndorsement = false;
                                    message = string.Empty;
                                }
                                switch (policy.Endorsement.EndorsementType)
                                {
                                    case EndorsementType.Modification:
                                        switch (policy.Product.CoveredRisk.CoveredRiskType)
                                        {
                                            case CoveredRiskType.Surety:
                                                message = ValidateDate(risk, policy, ValidateTyeEndorsement.DateSurety);
                                                break;

                                            default:
                                                message = ValidateDate(risk, policy, ValidateTyeEndorsement.DateDefault);
                                                break;

                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }

                            if (validEndorsement)
                            {
                                message = App_GlobalResources.Language.NoCanEndorsementsWithoutRisks;
                            }

                            return message;
                        }
                        else
                        {
                            return App_GlobalResources.Language.NoCanEndorsementsWithoutRisks;
                        }
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return App_GlobalResources.Language.NoCanEndorsementsWithoutRisks;
                }
            }
            catch (Exception)
            {
                return App_GlobalResources.Language.ErrorValideEndorsement;
            }
        }

        private string ValidateDate(CompanyRisk risk, CompanyPolicy policy, ValidateTyeEndorsement validateTyeEndorsement)
        {
            message = string.Empty;
            DateTime? CurrentFrom = null;
            DateTime? CurrentTo = null;
            var query = new List<CompanyCoverage>();
            switch (validateTyeEndorsement)
            {

                case ValidateTyeEndorsement.DateSurety:
                    CurrentTo = risk.Coverages.Where(x => x.CoverStatus == CoverageStatusType.Modified).Max(i => i.CurrentTo);
                    query = risk.Coverages.Where(x => x.CoverStatus == CoverageStatusType.Modified).Select(x => x).ToList();
                    CurrentFrom = query.Any() ? query.Min(o => o.CurrentFrom) : (DateTime?)null;
                    if (CurrentTo != null)
                    {
                        if (policy.CurrentTo < CurrentTo)
                        {
                            message = App_GlobalResources.Language.ErrorDatesCoveragesPolicy;
                        }
                    }
                    if (CurrentFrom != null)
                    {
                        if (CurrentFrom < policy.CurrentFrom)
                        {
                            message = App_GlobalResources.Language.ErrorDatesFromCoveragesPolicy;
                        }
                    }
                    break;
                case ValidateTyeEndorsement.DateDefault:
                    CurrentTo = risk.Coverages.Where(x => x.CoverStatus == CoverageStatusType.Modified).Max(i => i.CurrentTo);
                    query = risk.Coverages.Where(x => x.CoverStatus == CoverageStatusType.Modified).Select(x => x).ToList();
                    CurrentFrom = query.Any() ? query.Min(o => o.CurrentFrom) : (DateTime?)null;
                    if (CurrentTo != null)
                    {
                        if (CurrentTo > policy.CurrentTo)
                        {
                            message = App_GlobalResources.Language.ErrorDatesCoverageGreaterEndorsement;
                        }
                    }
                    if (CurrentFrom != null)
                    {
                        if (CurrentFrom > policy.CurrentFrom)
                        {
                            message = App_GlobalResources.Language.ErrorDatesFromCoverageGreaterEndorsement;
                        }
                    }
                    break;
                default:
                    break;

            }
            return message;
        }
        enum ValidateTyeEndorsement
        {
            DateSurety = 1,
            DateDefault = 2
        }


        private List<PoliciesAut> ValidateInfringementPolicies(CompanyPolicy policy)
        {
            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();

            //CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);

            if (policy != null)
            {

                if (policy.InfringementPolicies != null && policy.InfringementPolicies.Count > 0)
                {
                    infringementPolicies.AddRange(policy.InfringementPolicies.Where(x => x.Type == Application.AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == Application.AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive).ToList());
                }

                List<CompanyRisk> risks = DelegateService.underwritingService.GetCompanyRisksByTemporalId(policy.Id, false);

                foreach (CompanyRisk risk in risks)
                {

                    if (risk.InfringementPolicies != null && risk.InfringementPolicies.Count > 0)
                    {
                        infringementPolicies.AddRange(risk.InfringementPolicies.Where(x => x.Type == Application.AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == Application.AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive).ToList());
                    }
                }
            }

            return infringementPolicies;
        }

        #endregion



        public ActionResult GetTemporalEndorsementByPolicyId(int policyId)
        {
            try
            {
                Endorsement endorsement = DelegateService.endorsementBaseService.GetTemporalEndorsementByPolicyId(policyId);

                if (endorsement == null)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.MsgPolicyWithoutTemporal);
                }
                return new UifJsonResult(true, endorsement);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryTemporary);
            }
        }
    }
}