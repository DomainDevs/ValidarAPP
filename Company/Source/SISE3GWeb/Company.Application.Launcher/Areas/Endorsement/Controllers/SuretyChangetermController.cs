using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Linq;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Controllers;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Company.Application.UnderwritingServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class SuretyChangeTermController : ChangeTermController
    {
        public ActionResult CreateTemporal(ChangeTermViewModel changeTermModel)
        {
            UnderwritingController underwritingController = new UnderwritingController();
            try
            {
                var CompanyPolicy = ModelAssembler.CreateCompanyEndorsement(changeTermModel);
                //if (!string.IsNullOrEmpty(CompanyPolicy.Text.TextBody))
                if (CompanyPolicy.Text != null)
                {
                    CompanyPolicy.Text.TextBody = underwritingController.unicode_iso8859(CompanyPolicy.Text.TextBody);
                }                   
                var policy = DelegateService.suretyChangeTermServiceCompany.CreateTemporal(CompanyPolicy, false);
                if (policy != null)
                {
                    return new UifJsonResult(true, policy);
                }
                else
                {
                    return new UifJsonResult(false, string.Format(App_GlobalResources.Language.EndorrsementNotChange, App_GlobalResources.Language.ChangeTermEndorsement));
                }
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }

        }


        public ActionResult CreateEndorsementChangeTerm(ChangeTermViewModel changeTermModel)
        {
            try
            {
                if (changeTermModel != null)
                {
                    var companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(changeTermModel.TemporalId.Value, false);
                    companyPolicy.UserId = SessionHelper.GetUserId();
                    companyPolicy.User = new CompanyPolicyUser { UserId = SessionHelper.GetUserId() };
                    companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);

                    companyPolicy = ModelAssembler.CreateCompanyEndorsement(changeTermModel);
                    var policy = DelegateService.suretyChangeTermServiceCompany.CreateEndorsementChangeTerm(companyPolicy.Endorsement);
                    if (policy.FirstOrDefault().Errors != null && policy.FirstOrDefault().Errors.Any())
                    {
                        return new UifJsonResult(policy.FirstOrDefault().Errors.First().StateData, policy.FirstOrDefault().Errors.First().Error);
                    }
                    return new UifJsonResult(true, policy);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
                }
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }

        }









        //    public ActionResult CreateTemporal(ChangeTermViewModel changeTermViewModel)
        //    {
        //        try
        //        {
        //            if (String.IsNullOrEmpty(changeTermViewModel.CurrentFrom))
        //            {
        //                ModelState.AddModelError("CurrentFrom", Language.LabelChangeTermDateTransfer);
        //            }
        //            if (ModelState.IsValid)
        //            {
        //                CompanyPolicy policy = new CompanyPolicy();
        //                if (changeTermViewModel.TemporalId.HasValue)
        //                {
        //                    policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(changeTermViewModel.TemporalId.Value, false);
        //                }

        //                policy.Endorsement = new CompanyEndorsement
        //                {
        //                    Id = changeTermViewModel.EndorsementId.Value,
        //                    PolicyId = changeTermViewModel.PolicyId.Value,
        //                    EndorsementReasonId = changeTermViewModel.EndorsementReasonId,
        //                    EndorsementType = EndorsementType.ChangeTermEndorsement,
        //                    CurrentFrom = Convert.ToDateTime(changeTermViewModel.CurrentFrom),
        //                    CurrentTo = Convert.ToDateTime(changeTermViewModel.CurrentTo),
        //                    EndorsementDays = changeTermViewModel.Days,
        //                    Text = new CompanyText
        //                    {
        //                        TextBody = changeTermViewModel.Text,
        //                        Observations = changeTermViewModel.Observations
        //                    }
        //                };


        //                if (policy.CurrentFrom != Convert.ToDateTime(changeTermViewModel.CurrentFrom))
        //                {
        //                    policy.CurrentFrom = Convert.ToDateTime(changeTermViewModel.CurrentFrom);
        //                    policy.CurrentTo = Convert.ToDateTime(changeTermViewModel.CurrentTo);
        //                    policy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);
        //                    policy.BeginDate = Convert.ToDateTime(changeTermViewModel.CurrentFrom);
        //                    policy = ChangeTermPolicy(policy);
        //                }

        //                return new UifJsonResult(true, policy);
        //            }
        //            else
        //            {
        //                string errorMessage = GetErrorMessages();
        //                return new UifJsonResult(false, errorMessage);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
        //        }
        //    }

        //    private CompanyPolicy ChangeTermPolicy(CompanyPolicy policy)
        //    {
        //        PendingOperation pendingOperation = new PendingOperation();
        //        List<CompanyRisk> risks = new List<CompanyRisk>();
        //        List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
        //        List<CompanyCoverage> companyCoverages = new List<CompanyCoverage>();
        //        if (policy.Id == 0)
        //        {
        //            var companyContracts = DelegateService.suretyService.GetCompanyContractsByPolicyId(policy.Endorsement.PolicyId);
        //            if (companyContracts == null || !companyContracts.Any())
        //            {
        //                return null;
        //            }
        //            var endorsement = policy.Endorsement;
        //            if (endorsement != null)
        //            {
        //                var suretyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(endorsement.Id);

        //                suretyPolicy.UserId = SessionHelper.GetUserId();
        //                suretyPolicy.Endorsement = policy.Endorsement;
        //                suretyPolicy.TemporalType = TemporalType.Endorsement;
        //                suretyPolicy.TemporalTypeDescription = EnumsHelper.GetItemName<TemporalType>(TemporalType.Endorsement);
        //                suretyPolicy.CurrentFrom = policy.CurrentFrom;
        //                suretyPolicy.CurrentTo = policy.CurrentTo;
        //                suretyPolicy.IssueDate = policy.IssueDate;
        //                policy = suretyPolicy;
        //            }

        //            policy.Summary = new CompanySummary
        //            {
        //                RiskCount = 0
        //            };
        //            policy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(policy.Product.Id, policy.Prefix.Id);
        //            policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);

        //            Policy corePolicy = Mapper.Map<CompanyPolicy, Policy>(policy);

        //            risks = ModelAssembler.CreateCompanyRisk(DelegateService.endorsementSuretyChangeTermService.QuotateChangeTerm(corePolicy));

        //            risks.ForEach(x => x.AmountInsured = x.Coverages.Sum(y => y.LimitAmount));
        //            foreach (SUM.CompanyContract contract in companyContracts)
        //            {
        //                if (contract.Beneficiaries[0].IdentificationDocument == null)
        //                {
        //                    foreach (CompanyBeneficiary item in contract.Beneficiaries)
        //                    {
        //                        Beneficiary beneficiary = new Beneficiary();
        //                        beneficiary = DelegateService.underwritingService.GetBeneficiariesByDescription(item.IndividualId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
        //                        item.IdentificationDocument = beneficiary.IdentificationDocument;
        //                        item.Name = beneficiary.Name;
        //                        item.CustomerType = beneficiary.CustomerType;
        //                    }
        //                }

        //                contract.Status = RiskStatusType.Included;
        //                contract.Available = 0;
        //                contract.OperatingQuota = new Application.UniquePersonService.Models.OperatingQuota
        //                {
        //                    Amount = new Amount
        //                    {
        //                        Value = 0
        //                    }
        //                };

        //                List<CompanyCoverage> endorsementCoverages = new List<CompanyCoverage>();

        //                foreach (CompanyCoverage c in risks.First(x => x.RiskId == contract.RiskId).Coverages)
        //                {
        //                    CompanyCoverage comCov = new CompanyCoverage();
        //                    comCov.AccumulatedDeductAmount = c.AccumulatedDeductAmount;
        //                    comCov.AccumulatedLimitAmount = c.AccumulatedLimitAmount;
        //                    comCov.AccumulatedPremiumAmount = c.AccumulatedPremiumAmount;
        //                    comCov.AccumulatedSubLimitAmount = c.AccumulatedSubLimitAmount;
        //                    comCov.CalculationType = c.CalculationType;
        //                    comCov.Clauses = c.Clauses;
        //                    comCov.ContractAmountPercentage = c.ContractAmountPercentage;
        //                    //comCov.CoverageAllied = c.CoverageAllied;
        //                    comCov.CoverageOriginalStatus = c.CoverageOriginalStatus;
        //                    comCov.CoverNum = c.CoverNum;
        //                    comCov.CoverStatus = c.CoverStatus;
        //                    comCov.CoverStatusName = c.CoverStatusName;
        //                    comCov.CurrencyCode = c.CurrencyCode;
        //                    comCov.CurrentFrom = c.CurrentFrom;
        //                    comCov.CurrentTo = c.CurrentTo;
        //                    comCov.Days = c.Days;
        //                    comCov.DeclaredAmount = c.DeclaredAmount;
        //                    comCov.Deductible = c.Deductible;
        //                    comCov.Description = c.Description;
        //                    comCov.DiffMinPremiumAmount = c.DiffMinPremiumAmount;
        //                    comCov.DynamicProperties = c.DynamicProperties;
        //                    comCov.EndorsementId = c.EndorsementId;
        //                    comCov.EndorsementLimitAmount = c.EndorsementLimitAmount;
        //                    comCov.EndorsementSublimitAmount = c.EndorsementSublimitAmount;
        //                    comCov.EndorsementType = c.EndorsementType;
        //                    comCov.ExcessLimit = c.ExcessLimit;
        //                    comCov.FirstRiskType = c.FirstRiskType;
        //                    comCov.FlatRatePorcentage = c.FlatRatePorcentage;
        //                    comCov.Id = c.Id;
        //                    if (comCov.InsuredObject != null)
        //                    {
        //                        if (c.InsuredObject != null)
        //                        {
        //                            comCov.InsuredObject = new CompanyInsuredObject();
        //                            comCov.InsuredObject.Amount = c.InsuredObject.Amount;
        //                            comCov.InsuredObject.Description = c.InsuredObject.Description;
        //                            comCov.InsuredObject.Id = c.InsuredObject.Id;
        //                            comCov.InsuredObject.IsDeclarative = c.InsuredObject.IsDeclarative;
        //                            comCov.InsuredObject.IsMandatory = c.InsuredObject.IsMandatory;
        //                            comCov.InsuredObject.IsSelected = c.InsuredObject.IsSelected;

        //                            comCov.InsuredObject.Premium = c.InsuredObject.Premium;
        //                            comCov.InsuredObject.SmallDescription = c.InsuredObject.SmallDescription;
        //                        }
        //                    }
        //                    comCov.IsAccMinPremium = c.IsAccMinPremium;
        //                    comCov.IsChild = c.IsChild;
        //                    comCov.IsDeclarative = c.IsDeclarative;
        //                    comCov.IsImpression = c.IsImpression;
        //                    comCov.IsMandatory = c.IsMandatory;
        //                    comCov.IsMinPremiumDeposit = c.IsMinPremiumDeposit;
        //                    comCov.IsPrimary = c.IsPrimary;
        //                    comCov.IsSelected = c.IsSelected;
        //                    comCov.IsSublimit = c.IsSublimit;
        //                    comCov.IsVisible = c.IsVisible;
        //                    comCov.LimitAmount = c.LimitAmount;
        //                    comCov.LimitClaimantAmount = c.LimitClaimantAmount;
        //                    comCov.LimitOccurrenceAmount = c.LimitOccurrenceAmount;
        //                    comCov.MainCoverageId = c.MainCoverageId;
        //                    comCov.MainCoveragePercentage = c.MainCoveragePercentage;
        //                    comCov.MaxLiabilityAmount = c.MaxLiabilityAmount;
        //                    comCov.Number = c.Number;
        //                    comCov.OriginalLimitAmount = c.OriginalLimitAmount;
        //                    comCov.OriginalSubLimitAmount = c.OriginalSubLimitAmount;
        //                    comCov.PercentageContract = c.PercentageContract;
        //                    comCov.PosRuleSetId = c.PosRuleSetId;
        //                    comCov.PremiumAmount = c.PremiumAmount;
        //                    comCov.PrintDescription = c.PrintDescription;
        //                    comCov.PrintDescriptionLimit = c.PrintDescriptionLimit;
        //                    comCov.Rate = c.Rate;
        //                    comCov.RateType = c.RateType;
        //                    comCov.RiskCoverageId = c.RiskCoverageId;
        //                    comCov.RuleSetId = c.RuleSetId;
        //                    comCov.ScriptId = c.ScriptId;
        //                    comCov.ShortTermPercentage = c.ShortTermPercentage;
        //                    comCov.SubLimitAmount = c.SubLimitAmount;
        //                    comCov.SublimitPercentage = c.SublimitPercentage;
        //                    comCov.SubLineBusiness = c.SubLineBusiness;
        //                    comCov.Text = c.Text;
        //                    endorsementCoverages.Add(comCov);
        //                }

        //                contract.Coverages = endorsementCoverages;

        //                contract.AmountInsured = risks.First(x => x.RiskId == contract.RiskId).Coverages.Sum(x => x.LimitAmount);
        //                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(policy.Product.Id, contract.GroupCoverage.Id, policy.Prefix.Id);
        //                foreach (CompanyCoverage coverage in contract.Coverages)
        //                {
        //                    coverage.SubLineBusiness = coverages.First(x => x.Id == coverage.Id).SubLineBusiness;
        //                    coverage.Number = coverages.First(x => x.Id == coverage.Id).Number;
        //                    coverage.CoverStatus = CoverageStatusType.Included;
        //                    coverage.EndorsementType = policy.Endorsement.EndorsementType;
        //                }
        //                companyCoverages = contract.Coverages;



        //                contract.Policy = policy;
        //                var result = DelegateService.suretyService.CreateContractTemporal(contract, false);
        //                if (result.InfringementPolicies != null)
        //                    infringementPolicies.AddRange(result.InfringementPolicies);
        //            }
        //        }
        //        else
        //        {
        //            List<SUM.CompanyContract> contracts = DelegateService.suretyService.GetCompanySuretiesByTemporalId(policy.Id);

        //            Policy corePolicy = Mapper.Map<CompanyPolicy, Policy>(policy);

        //            risks = ModelAssembler.CreateCompanyRisk(DelegateService.endorsementSuretyChangeTermService.QuotateChangeTerm(corePolicy));

        //            foreach (SUM.CompanyContract contract in contracts)
        //            {

        //                List<CompanyCoverage> endorsementCoverages = new List<CompanyCoverage>();

        //                foreach (CompanyCoverage c in risks.First(x => x.RiskId == contract.RiskId).Coverages)
        //                {
        //                    CompanyCoverage comCov = new CompanyCoverage();
        //                    comCov.AccumulatedDeductAmount = c.AccumulatedDeductAmount;
        //                    comCov.AccumulatedLimitAmount = c.AccumulatedLimitAmount;
        //                    comCov.AccumulatedPremiumAmount = c.AccumulatedPremiumAmount;
        //                    comCov.AccumulatedSubLimitAmount = c.AccumulatedSubLimitAmount;
        //                    comCov.CalculationType = c.CalculationType;
        //                    comCov.Clauses = c.Clauses;
        //                    comCov.ContractAmountPercentage = c.ContractAmountPercentage;
        //                    //comCov.CoverageAllied = c.CoverageAllied;
        //                    comCov.CoverageOriginalStatus = c.CoverageOriginalStatus;
        //                    comCov.CoverNum = c.CoverNum;
        //                    comCov.CoverStatus = c.CoverStatus;
        //                    comCov.CoverStatusName = c.CoverStatusName;
        //                    comCov.CurrencyCode = c.CurrencyCode;
        //                    comCov.CurrentFrom = c.CurrentFrom;
        //                    comCov.CurrentTo = c.CurrentTo;
        //                    comCov.Days = c.Days;
        //                    comCov.DeclaredAmount = c.DeclaredAmount;
        //                    comCov.Deductible = c.Deductible;
        //                    comCov.Description = c.Description;
        //                    comCov.DiffMinPremiumAmount = c.DiffMinPremiumAmount;
        //                    comCov.DynamicProperties = c.DynamicProperties;
        //                    comCov.EndorsementId = c.EndorsementId;
        //                    comCov.EndorsementLimitAmount = c.EndorsementLimitAmount;
        //                    comCov.EndorsementSublimitAmount = c.EndorsementSublimitAmount;
        //                    comCov.EndorsementType = c.EndorsementType;
        //                    comCov.ExcessLimit = c.ExcessLimit;
        //                    comCov.FirstRiskType = c.FirstRiskType;
        //                    comCov.FlatRatePorcentage = c.FlatRatePorcentage;
        //                    comCov.Id = c.Id;
        //                    if (comCov.InsuredObject != null)
        //                    {
        //                        if (c.InsuredObject != null)
        //                        {
        //                            comCov.InsuredObject = new CompanyInsuredObject();
        //                            comCov.InsuredObject.Amount = c.InsuredObject.Amount;
        //                            comCov.InsuredObject.Description = c.InsuredObject.Description;
        //                            comCov.InsuredObject.Id = c.InsuredObject.Id;
        //                            comCov.InsuredObject.IsDeclarative = c.InsuredObject.IsDeclarative;
        //                            comCov.InsuredObject.IsMandatory = c.InsuredObject.IsMandatory;
        //                            comCov.InsuredObject.IsSelected = c.InsuredObject.IsSelected;

        //                            comCov.InsuredObject.Premium = c.InsuredObject.Premium;
        //                            comCov.InsuredObject.SmallDescription = c.InsuredObject.SmallDescription;
        //                        }
        //                    }
        //                    comCov.IsAccMinPremium = c.IsAccMinPremium;
        //                    comCov.IsChild = c.IsChild;
        //                    comCov.IsDeclarative = c.IsDeclarative;
        //                    comCov.IsImpression = c.IsImpression;
        //                    comCov.IsMandatory = c.IsMandatory;
        //                    comCov.IsMinPremiumDeposit = c.IsMinPremiumDeposit;
        //                    comCov.IsPrimary = c.IsPrimary;
        //                    comCov.IsSelected = c.IsSelected;
        //                    comCov.IsSublimit = c.IsSublimit;
        //                    comCov.IsVisible = c.IsVisible;
        //                    comCov.LimitAmount = c.LimitAmount;
        //                    comCov.LimitClaimantAmount = c.LimitClaimantAmount;
        //                    comCov.LimitOccurrenceAmount = c.LimitOccurrenceAmount;
        //                    comCov.MainCoverageId = c.MainCoverageId;
        //                    comCov.MainCoveragePercentage = c.MainCoveragePercentage;
        //                    comCov.MaxLiabilityAmount = c.MaxLiabilityAmount;
        //                    comCov.Number = c.Number;
        //                    comCov.OriginalLimitAmount = c.OriginalLimitAmount;
        //                    comCov.OriginalSubLimitAmount = c.OriginalSubLimitAmount;
        //                    comCov.PercentageContract = c.PercentageContract;
        //                    comCov.PosRuleSetId = c.PosRuleSetId;
        //                    comCov.PremiumAmount = c.PremiumAmount;
        //                    comCov.PrintDescription = c.PrintDescription;
        //                    comCov.PrintDescriptionLimit = c.PrintDescriptionLimit;
        //                    comCov.Rate = c.Rate;
        //                    comCov.RateType = c.RateType;
        //                    comCov.RiskCoverageId = c.RiskCoverageId;
        //                    comCov.RuleSetId = c.RuleSetId;
        //                    comCov.ScriptId = c.ScriptId;
        //                    comCov.ShortTermPercentage = c.ShortTermPercentage;
        //                    comCov.SubLimitAmount = c.SubLimitAmount;
        //                    comCov.SublimitPercentage = c.SublimitPercentage;
        //                    comCov.SubLineBusiness = c.SubLineBusiness;
        //                    comCov.Text = c.Text;
        //                    endorsementCoverages.Add(comCov);
        //                }

        //                contract.Coverages = endorsementCoverages;

        //                contract.AmountInsured = risks.First(x => x.RiskId == contract.RiskId).Coverages.Sum(x => x.LimitAmount);
        //                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(policy.Product.Id, contract.GroupCoverage.Id, policy.Prefix.Id);
        //                foreach (CompanyCoverage coverage in contract.Coverages)
        //                {
        //                    coverage.SubLineBusiness = coverages.First(x => x.Id == coverage.Id).SubLineBusiness;
        //                    coverage.Number = coverages.First(x => x.Id == coverage.Id).Number;
        //                    coverage.CoverStatus = CoverageStatusType.Included;
        //                    coverage.EndorsementType = policy.Endorsement.EndorsementType;
        //                }
        //                companyCoverages = contract.Coverages;


        //                SUM.CompanyContract contractEvents = DelegateService.suretyService.CreateContractTemporal(contract, false);
        //                if (contractEvents.InfringementPolicies != null)
        //                {
        //                    contract.InfringementPolicies = contractEvents.InfringementPolicies;
        //                    infringementPolicies.AddRange(contract.InfringementPolicies);
        //                }
        //            }
        //        }
        //        policy.IsPersisted = true;

        //        List<CompanyRisk> companyRisks = new List<CompanyRisk>();
        //        risks.ForEach(x =>
        //        {
        //            x.Coverages = null;
        //            CompanyRisk companyRisk = new CompanyRisk();
        //            Mapper.CreateMap(x.GetType(), companyRisk.GetType());
        //            Mapper.Map(x, companyRisk);
        //            companyRisk.Coverages = companyCoverages;
        //            companyRisks.Add(companyRisk);
        //        });

        //        policy = CalculatePolicyAmounts(policy, companyRisks);

        //        policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
        //        policy.InfringementPolicies.AddRange(infringementPolicies);
        //        return policy;
        //    }


        //    public ActionResult CreateEndorsementChangeTerm(ChangeTermViewModel changeTermViewModel)
        //    {
        //        try
        //        {
        //            if (changeTermViewModel.TemporalId.HasValue)
        //            {
        //                if (Convert.ToDateTime(changeTermViewModel.CancelationFrom) > Convert.ToDateTime(changeTermViewModel.CurrentFrom))
        //                {
        //                    ModelState.AddModelError("CurrentFrom", Language.ChangeTermDateGreaterPolicy);
        //                }
        //                if (ModelState.IsValid)
        //                {
        //                    CompanyPolicy policy = new CompanyPolicy();
        //                    policy.CurrentFrom = Convert.ToDateTime(changeTermViewModel.CancelationFrom);
        //                    policy.Endorsement = new CompanyEndorsement
        //                    {
        //                        Id = changeTermViewModel.EndorsementId.Value,
        //                        PolicyId = changeTermViewModel.PolicyId.Value,
        //                        EndorsementReasonId = changeTermViewModel.EndorsementReasonId,
        //                        EndorsementType = EndorsementType.Cancellation,
        //                        CurrentFrom = Convert.ToDateTime(changeTermViewModel.CancelationFrom),
        //                        CurrentTo = Convert.ToDateTime(changeTermViewModel.EndorsementTo),
        //                        EndorsementDays = changeTermViewModel.Days,
        //                    };
        //                    CancellationPolicy(policy);

        //                    CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(changeTermViewModel.TemporalId.Value, false);
        //                    companyPolicy = CreatePolicyEndorsement(companyPolicy);
        //                    if (companyPolicy.InfringementPolicies.Count > 0)
        //                    {
        //                        companyPolicy.Id = policy.Id;
        //                        return new UifJsonResult(true, companyPolicy);
        //                    }
        //                    return new UifJsonResult(true, companyPolicy.Endorsement.Number);
        //                }
        //                else
        //                {
        //                    string errorMessage = GetErrorMessages();
        //                    return new UifJsonResult(false, errorMessage);
        //                }
        //            }
        //            else
        //            {
        //                return new UifJsonResult(false, App_GlobalResources.Language.NoExistTemporaryEmit);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return new UifJsonResult(false, ex.Message);
        //        }
        //    }
        //    private CompanyPolicy CancellationPolicy(CompanyPolicy policy)
        //    {
        //        int cancellationFactor = -1;
        //        List<CompanyRisk> risks = new List<CompanyRisk>();
        //        CompanyPolicy companyPolicy = new CompanyPolicy();
        //        List<CompanyCoverage> companyCoverages = new List<CompanyCoverage>();

        //        try
        //        {

        //            var companyContracts = DelegateService.suretyService.GetCompanyContractsByPolicyId(policy.Endorsement.PolicyId);
        //            if (companyContracts == null || !companyContracts.Any())
        //            {
        //                return null;
        //            }



        //            var endorsement = policy.Endorsement;
        //            if (endorsement != null)
        //            {
        //                companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(endorsement.Id);
        //                companyPolicy.Endorsement.CancellationTypeId = (int)CancellationType.BeginDate;
        //                companyPolicy.UserId = SessionHelper.GetUserId();
        //                companyPolicy.Endorsement = policy.Endorsement;
        //                companyPolicy.Endorsement.EndorsementType = EndorsementType.Cancellation;
        //                companyPolicy.TemporalType = TemporalType.Endorsement;
        //                companyPolicy.TemporalTypeDescription = EnumsHelper.GetItemName<TemporalType>(TemporalType.Endorsement);
        //                companyPolicy.CurrentFrom = policy.CurrentFrom;

        //            }

        //            companyPolicy.Summary = new CompanySummary
        //            {
        //                RiskCount = 0
        //            };

        //            Policy corePolicy = Mapper.Map<CompanyPolicy, Policy>(companyPolicy);

        //            risks = ModelAssembler.CreateCompanyRisk(DelegateService.endorsementSuretyCancellationService.QuotateCancellation(corePolicy, cancellationFactor));

        //            Parallel.ForEach(risks, x => x.AmountInsured = x.Coverages.Sum(y => y.LimitAmount));
        //            companyPolicy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(companyPolicy.Product.Id, companyPolicy.Prefix.Id);
        //            companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);
        //            foreach (SUM.CompanyContract contract in companyContracts)
        //            {
        //                if (contract.Beneficiaries[0].IdentificationDocument == null)
        //                {
        //                    foreach (CompanyBeneficiary item in contract.Beneficiaries)
        //                    {
        //                        Beneficiary beneficiary = new Beneficiary();
        //                        beneficiary = DelegateService.underwritingService.GetBeneficiariesByDescription(item.IndividualId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
        //                        item.IdentificationDocument = beneficiary.IdentificationDocument;
        //                        item.Name = beneficiary.Name;
        //                        item.CustomerType = beneficiary.CustomerType;
        //                    }
        //                }

        //                contract.Status = RiskStatusType.Excluded;
        //                contract.Available = 0;
        //                contract.OperatingQuota = new Application.UniquePersonService.Models.OperatingQuota
        //                {
        //                    Amount = new Amount
        //                    {
        //                        Value = 0
        //                    }
        //                };

        //                List<CompanyCoverage> endorsementCoverages = new List<CompanyCoverage>();
        //                foreach (CompanyCoverage c in risks.First(x => x.RiskId == contract.RiskId).Coverages)
        //                {
        //                    CompanyCoverage comCov = new CompanyCoverage();
        //                    comCov.AccumulatedDeductAmount = c.AccumulatedDeductAmount;
        //                    comCov.AccumulatedLimitAmount = c.AccumulatedLimitAmount;
        //                    comCov.AccumulatedPremiumAmount = c.AccumulatedPremiumAmount;
        //                    comCov.AccumulatedSubLimitAmount = c.AccumulatedSubLimitAmount;
        //                    comCov.CalculationType = c.CalculationType;
        //                    comCov.Clauses = c.Clauses;
        //                    comCov.ContractAmountPercentage = c.ContractAmountPercentage;
        //                    //comCov.CoverageAllied = c.CoverageAllied;
        //                    comCov.CoverageOriginalStatus = c.CoverageOriginalStatus;
        //                    comCov.CoverNum = c.CoverNum;
        //                    comCov.CoverStatus = c.CoverStatus;
        //                    comCov.CoverStatusName = c.CoverStatusName;
        //                    comCov.CurrencyCode = c.CurrencyCode;
        //                    comCov.CurrentFrom = c.CurrentFrom;
        //                    comCov.CurrentTo = c.CurrentTo;
        //                    comCov.Days = c.Days;
        //                    comCov.DeclaredAmount = c.DeclaredAmount;
        //                    comCov.Deductible = c.Deductible;
        //                    comCov.Description = c.Description;
        //                    comCov.DiffMinPremiumAmount = c.DiffMinPremiumAmount;
        //                    comCov.DynamicProperties = c.DynamicProperties;
        //                    comCov.EndorsementId = c.EndorsementId;
        //                    comCov.EndorsementLimitAmount = c.EndorsementLimitAmount;
        //                    comCov.EndorsementSublimitAmount = c.EndorsementSublimitAmount;
        //                    comCov.EndorsementType = c.EndorsementType;
        //                    comCov.ExcessLimit = c.ExcessLimit;
        //                    comCov.FirstRiskType = c.FirstRiskType;
        //                    comCov.FlatRatePorcentage = c.FlatRatePorcentage;
        //                    comCov.Id = c.Id;
        //                    if (comCov.InsuredObject != null)
        //                    {
        //                        if (c.InsuredObject != null)
        //                        {
        //                            comCov.InsuredObject = new CompanyInsuredObject();
        //                            comCov.InsuredObject.Amount = c.InsuredObject.Amount;
        //                            comCov.InsuredObject.Description = c.InsuredObject.Description;
        //                            comCov.InsuredObject.Id = c.InsuredObject.Id;
        //                            comCov.InsuredObject.IsDeclarative = c.InsuredObject.IsDeclarative;
        //                            comCov.InsuredObject.IsMandatory = c.InsuredObject.IsMandatory;
        //                            comCov.InsuredObject.IsSelected = c.InsuredObject.IsSelected;

        //                            comCov.InsuredObject.Premium = c.InsuredObject.Premium;
        //                            comCov.InsuredObject.SmallDescription = c.InsuredObject.SmallDescription;
        //                        }
        //                    }
        //                    comCov.IsAccMinPremium = c.IsAccMinPremium;
        //                    comCov.IsChild = c.IsChild;
        //                    comCov.IsDeclarative = c.IsDeclarative;
        //                    comCov.IsImpression = c.IsImpression;
        //                    comCov.IsMandatory = c.IsMandatory;
        //                    comCov.IsMinPremiumDeposit = c.IsMinPremiumDeposit;
        //                    comCov.IsPrimary = c.IsPrimary;
        //                    comCov.IsSelected = c.IsSelected;
        //                    comCov.IsSublimit = c.IsSublimit;
        //                    comCov.IsVisible = c.IsVisible;
        //                    comCov.LimitAmount = c.LimitAmount;
        //                    comCov.LimitClaimantAmount = c.LimitClaimantAmount;
        //                    comCov.LimitOccurrenceAmount = c.LimitOccurrenceAmount;
        //                    comCov.MainCoverageId = c.MainCoverageId;
        //                    comCov.MainCoveragePercentage = c.MainCoveragePercentage;
        //                    comCov.MaxLiabilityAmount = c.MaxLiabilityAmount;
        //                    comCov.Number = c.Number;
        //                    comCov.OriginalLimitAmount = c.OriginalLimitAmount;
        //                    comCov.OriginalSubLimitAmount = c.OriginalSubLimitAmount;
        //                    comCov.PercentageContract = c.PercentageContract;
        //                    comCov.PosRuleSetId = c.PosRuleSetId;
        //                    comCov.PremiumAmount = c.PremiumAmount;
        //                    comCov.PrintDescription = c.PrintDescription;
        //                    comCov.PrintDescriptionLimit = c.PrintDescriptionLimit;
        //                    comCov.Rate = c.Rate;
        //                    comCov.RateType = c.RateType;
        //                    comCov.RiskCoverageId = c.RiskCoverageId;
        //                    comCov.RuleSetId = c.RuleSetId;
        //                    comCov.ScriptId = c.ScriptId;
        //                    comCov.ShortTermPercentage = c.ShortTermPercentage;
        //                    comCov.SubLimitAmount = c.SubLimitAmount;
        //                    comCov.SublimitPercentage = c.SublimitPercentage;
        //                    comCov.SubLineBusiness = c.SubLineBusiness;
        //                    comCov.Text = c.Text;
        //                    endorsementCoverages.Add(comCov);
        //                }

        //                contract.Coverages = endorsementCoverages;

        //                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(companyPolicy.Product.Id, contract.GroupCoverage.Id, companyPolicy.Prefix.Id);
        //                foreach (CompanyCoverage coverage in contract.Coverages)
        //                {
        //                    coverage.SubLineBusiness = coverages.First(x => x.Id == coverage.Id).SubLineBusiness;
        //                    coverage.Number = coverages.First(x => x.Id == coverage.Id).Number;
        //                    coverage.CoverStatus = CoverageStatusType.Excluded;
        //                    coverage.EndorsementType = companyPolicy.Endorsement.EndorsementType;
        //                }
        //                companyCoverages = contract.Coverages;


        //                contract.Policy = companyPolicy;
        //                var result = DelegateService.suretyService.CreateContractTemporal(contract, false);
        //            }

        //            List<CompanyRisk> companyRisks = new List<CompanyRisk>();
        //            risks.ForEach(x =>
        //            {
        //                x.Coverages = null;
        //                CompanyRisk companyRisk = new CompanyRisk();
        //                Mapper.CreateMap(x.GetType(), companyRisk.GetType());
        //                Mapper.Map(x, companyRisk);
        //                companyRisk.Coverages = companyCoverages;
        //                companyRisks.Add(companyRisk);
        //            });

        //            companyPolicy = CalculatePolicyAmounts(companyPolicy, companyRisks);

        //            DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);
        //            CreatePolicyEndorsement(companyPolicy);
        //            return companyPolicy;
        //        }
        //        catch (Exception)
        //        {
        //            if (policy != null)
        //            {
        //                DelegateService.underwritingService.DeleteTemporalByOperationId(policy.Id, 0, 0, 0);
        //            }
        //            throw;
        //        }
        //    }
    }
}