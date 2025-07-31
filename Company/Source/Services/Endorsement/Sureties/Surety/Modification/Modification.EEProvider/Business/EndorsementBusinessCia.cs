using System;
using System.Collections.Generic;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.EEProvider;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using SEM = Sistran.Company.Application.Sureties.SuretyServices.Models;
namespace Sistran.Company.Application.SuretyModificationService.EEProvider.Business
{
    using System.Collections.Concurrent;
    using System.Linq;
    using Sistran.Company.Application.SuretyModificationService.EEProvider.Assemblers;
    using Sistran.Company.Application.SuretyModificationService.EEProvider.Resources;
    using Sistran.Company.Application.SuretyModificationService.EEProvider.Services;
    using Sistran.Core.Application.UnderwritingServices.Models;
    using Sistran.Core.Application.Utilities.Helper;
    using Sistran.Core.Framework.BAF;
    using Sistran.Core.Services.UtilitiesServices.Enums;
    using TP = Sistran.Core.Application.Utilities.Utility;

    /// <summary>
    /// Endosos Autos
    /// </summary>
    public class SuretyEndorsementBusinessCia
    {
        UnderwritingServiceEEProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="SuretyEndorsementBusinessCia" /> class.
        /// </summary>
        public SuretyEndorsementBusinessCia()
        {
            provider = new UnderwritingServiceEEProvider();
        }
        /// <summary>
        /// Creates the policy temporal.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <param name="isMasive">if set to <c>true</c> [is masive].</param>
        /// <param name="isSaveTemp">if set to <c>true</c> [is save temporary].</param>
        /// <returns></returns>
        public CompanyPolicy CreatePolicyTemporal(CompanyEndorsement companyEndorsement, bool isMasive, bool isSaveTemp)
        {
            return CreateEndorsement(companyEndorsement);
        }

        /// <summary>
        /// Creates the endorsement.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Endoso Vacio</exception>
        private CompanyPolicy CreateEndorsement(CompanyEndorsement companyEndorsement)
        {
            if (companyEndorsement == null)
            {
                throw new ArgumentException(Errors.ErrorEndorsement);
            }
            try
            {
                CompanyPolicy policy = new CompanyPolicy();
                if (companyEndorsement.TemporalId > 0)
                {
                    policy = CreateTempByTempId(companyEndorsement);
                }
                else
                {
                    policy = CreateTempByEndorsement(companyEndorsement);
                }

                return policy;
            }
            catch (Exception)
            {

                throw;
            }
        }
        private CompanyPolicy CreateTempByTempId(CompanyEndorsement companyEndorsement)
        {
            try
            {
                if (companyEndorsement == null)
                {
                    throw new ArgumentException(Errors.ErrorEndorsement);
                }
                var policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyEndorsement.TemporalId, false);
                if (policy != null)
                {
                    policy.Agencies?.ForEach(x =>
                    {
                        if (x.DateDeclined != null)
                        {
                            throw new ArgumentException(Errors.ErrorAgentEmpty);
                        }
                    });
                    policy.CurrentFrom = companyEndorsement.CurrentFrom;
                    policy.CurrentTo = companyEndorsement.CurrentTo;
                    policy.IssueDate = companyEndorsement.IssueDate;
                    policy.Endorsement.EndorsementReasonId = companyEndorsement.EndorsementReasonId;
                    policy.Endorsement.EndorsementType = EndorsementType.Modification;
                    policy.Endorsement.EndorsementTypeDescription = EnumHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
                    policy.Endorsement.Text = new CompanyText
                    {
                        TextBody = companyEndorsement.Text.TextBody,
                        Observations = companyEndorsement.Text.Observations
                    };
                    policy.Endorsement.ModificationTypeId = companyEndorsement.ModificationTypeId;
                    policy.UserId = companyEndorsement.UserId;
                    policy.Holder = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(policy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();
                    #region REQ_269
                    policy.Endorsement.TicketNumber = companyEndorsement.TicketNumber;
                    policy.Endorsement.TicketDate = companyEndorsement.TicketDate;
                    #endregion

                    if (companyEndorsement.BusinessTypeDescription != 0)
                    {
                        policy.CoInsuranceCompanies.Where(x => x.EndorsementNumber != null).ToList().ForEach(y => y.EndorsementNumber = companyEndorsement.BusinessTypeDescription.ToString());
                    }
                    policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);

                    if (policy.Product.IsCollective)
                    {
                        if (companyEndorsement.DescriptionRisk != null)
                        {
                            List<SEM.CompanyContract> companySuretys = new List<SEM.CompanyContract>();
                            SEM.CompanyContract surety = new SEM.CompanyContract();
                            companyEndorsement.Id = DelegateService.underwritingService.GetCurrentEndorsementByPolicyIdLicensePlateId(companyEndorsement.PolicyId, companyEndorsement.DescriptionRisk).Id;

                            companySuretys = DelegateService.suretyService.GetCompanySuretyByEndorsementId(companyEndorsement.Id);
                            surety = companySuretys.Where(x => x.Risk.Description == companyEndorsement.DescriptionRisk).FirstOrDefault();

                            policy.Endorsement.EndorsementType = EndorsementType.Modification;
                            policy.Endorsement.EndorsementTypeDescription = EnumHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
                            policy.CurrentFrom = Convert.ToDateTime(companyEndorsement.CurrentFrom);
                            policy.CurrentTo = Convert.ToDateTime(companyEndorsement.CurrentTo);
                            policy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);
                            surety.Risk.Policy = policy;
                            SEM.CompanyContract risk = GetDataModification(surety, CoverageStatusType.NotModified);


                            List<SEM.CompanyContract> suretys = DelegateService.suretyService.GetCompanySuretiesByTemporalId(companyEndorsement.TemporalId);

                            risk.Risk.Policy = policy;
                            risk = DelegateService.suretyService.CreateSuretyTemporal(risk, true);
                            risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                        }
                    }


                    return policy;
                }
                else
                {
                    throw new BusinessException(Errors.ErrorTemporalNotFound);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.GetBaseException().Message, ex);
            }

        }


        /// <summary>
        /// Creates the temporary by endorsement.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException">
        /// </exception>
        private CompanyPolicy CreateTempByEndorsement(CompanyEndorsement companyEndorsement)
        {
            if (companyEndorsement == null)
            {
                throw new ArgumentException(Errors.ErrorEndorsement);
            }
            int Userid = BusinessContext.Current?.UserId ?? companyEndorsement.UserId;
            var policy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyEndorsement.Id);
            if (policy != null)
            {
                policy.Agencies?.ForEach(x =>
                {
                    if (x.DateDeclined != null)
                    {
                        throw new ArgumentException(Errors.ErrorAgentEmpty);
                    }
                });
                List<SEM.CompanyContract> companySureties = new List<SEM.CompanyContract>();

                if (policy.Product.IsCollective)
                {
                    if (companyEndorsement.DescriptionRisk != null)
                    {
                        companyEndorsement.Id = DelegateService.underwritingService.GetCurrentEndorsementByPolicyIdLicensePlateId(companyEndorsement.PolicyId, companyEndorsement.DescriptionRisk).Id;
                        if (companyEndorsement.Id != 0)
                        {
                            companySureties = DelegateService.suretyService.GetCompanySuretiesByPolicyId(companyEndorsement.PolicyId);
                        }
                        else
                        {
                            throw new BusinessException(Errors.ErrorLicensePlate);
                        }
                    }
                    else
                    {
                        throw new BusinessException(Errors.ErrorLicensePlate);
                    }
                }
                else
                {
                    companySureties = DelegateService.suretyService.GetCompanySuretiesByPolicyId(companyEndorsement.PolicyId);
                }

                policy.UserId = Userid;
                policy.CurrentFrom = Convert.ToDateTime(companyEndorsement.CurrentFrom);
                policy.CurrentTo = Convert.ToDateTime(companyEndorsement.CurrentTo);
                policy.EffectPeriod = DelegateService.commonService.GetExtendedParameterByParameterId(1027).NumberParameter.Value;
                policy.Endorsement.Text = new CompanyText
                {
                    TextBody = companyEndorsement.Text.TextBody,
                    Observations = companyEndorsement.Text.Observations
                };
                policy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(policy.Product.Id, policy.Prefix.Id);
                policy.Endorsement.EndorsementReasonId = companyEndorsement.EndorsementReasonId;
                policy.Endorsement.EndorsementType = EndorsementType.Modification;
                policy.Endorsement.EndorsementTypeDescription = EnumHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
                policy.TemporalType = TemporalType.Endorsement;
                policy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(policy.TemporalType);
                policy.Endorsement.ModificationTypeId = companyEndorsement.ModificationTypeId;
                policy.Holder = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(policy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();
                var mapper = ModelAssembler.CreateMapCompanyClause();
                List<CompanyClause> companyClauses = mapper.Map<List<Clause>, List<CompanyClause>>(DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.General, policy.Prefix.Id).Where(x => x.IsMandatory).ToList());
                if (companyClauses != null && companyClauses.Count > 0)
                {
                    if (policy.Clauses != null && policy.Clauses.Count > 0)
                    {
                        foreach (var item in companyClauses)
                        {
                            if (!policy.Clauses.Exists(x => x.Id == item.Id))
                            {
                                policy.Clauses.Add(item);
                            }
                        }
                    }
                    else
                    {
                        policy.Clauses = companyClauses;
                    }
                }

                policy.Summary = new CompanySummary
                {
                    RiskCount = 0
                };

                #region REQ_269
                policy.Endorsement.TicketNumber = companyEndorsement.TicketNumber;
                policy.Endorsement.TicketDate = companyEndorsement.TicketDate;
                policy.IssueDate = companyEndorsement.IssueDate;
                #endregion
                if (companyEndorsement.BusinessTypeDescription != 0)
                {
                    policy.CoInsuranceCompanies.Where(x => x.EndorsementNumber != null).ToList().ForEach(y => y.EndorsementNumber = companyEndorsement.BusinessTypeDescription.ToString());
                }
                //Se ajusta el guardado de componentes para que el valor de la cobertura de en 0
                if(policy.PayerComponents!= null)
                {
                    var companyComponentsPayer = policy.PayerComponents.ToList();
                    foreach (var item in companyComponentsPayer)
                    {

                        item.Amount = 0;
                        item.AmountExpense = 0;
                        item.BaseAmount = 0;
                    }
                    policy.PayerComponents = companyComponentsPayer;
                }
               
                policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                companySureties?.AsParallel().ForAll(x => x.Risk.Policy = policy);

                TP.Parallel.ForEach(companySureties, item =>
                {
                    var risk = GetDataModification(item, CoverageStatusType.NotModified);
                    risk.Risk.Coverages.AsParallel().ForAll(x => x.EndorsementType = EndorsementType.Modification);
                    risk = DelegateService.suretyService.CreateSuretyTemporal(risk, false);
                    risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                });

                return policy;
            }
            else
            {
                throw new BusinessException(Errors.ErrorTemporalNotFound);
            }
        }


        /// <summary>
        /// Gets the data modification.
        /// </summary>
        /// <param name="risk">The risk.</param>
        /// <param name="suretyPolicy">The surety policy.</param>
        /// <param name="coverageStatusType">Type of the coverage status.</param>
        /// <returns></returns>
        public SEM.CompanyContract GetDataModification(SEM.CompanyContract risk, CoverageStatusType coverageStatusType)
        {
            if (risk?.Risk?.Coverages == null)
            {
                throw new Exception(Errors.ErrorRiskEmpty);
            }
            risk.Risk.Description = risk.Risk.MainInsured.Name;

            if (risk.Risk?.Beneficiaries?[0].IdentificationDocument == null)
            {
                List<CompanyBeneficiary> beneficiaries = new List<CompanyBeneficiary>();
                ConcurrentBag<string> error = new ConcurrentBag<string>();
                if (risk.Risk.Beneficiaries != null)
                {
                    risk.Risk.Beneficiaries.AsParallel().ForAll(
                        item =>
                        {
                            var beneficiary = DelegateService.underwritingServiceCore.GetBeneficiariesByDescription(item.IndividualId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
                            if (beneficiary != null)
                            {
                                item.IdentificationDocument = beneficiary.IdentificationDocument;
                                item.Name = beneficiary.Name;
                            }
                            else
                            {
                                error.Add(Errors.ErrorBeneficiaryNotFound);
                            }
                        }
                        );
                    if (error.Any())
                    {
                        throw new Exception(string.Join(",", error));
                    }
                }
                else
                {
                    throw new Exception(Errors.ErrorBeneficiaryEmpty);
                }
            }
            if (risk != null && risk.Risk.Premium == 0)
            {
                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(risk.Risk.Policy.Product.Id, risk.Risk.GroupCoverage.Id, risk.Risk.Policy.Prefix.Id);
                if (coverages != null && coverages.Count > 0)
                {
                    //Se dejan  las que vienen el endoso Anterior Definicion Taila
                    List<CompanyCoverage> coveragesAll = new List<CompanyCoverage>();
                    var coveragesData = risk.Risk.Coverages;
                    coveragesData.AsParallel().ForAll(item =>
                    {
                        var coverageLocal = coverages.FirstOrDefault(z => z.Id == item.Id);
                        if (coverageLocal == null)
                            return;

                        item.RuleSetId = coverageLocal.RuleSetId;
                        item.PosRuleSetId = coverageLocal.PosRuleSetId;
                        item.ScriptId = coverageLocal.ScriptId;
                        item.InsuredObject = coverageLocal.InsuredObject;
                        item.OriginalLimitAmount = item.LimitAmount;
                        item.OriginalSubLimitAmount = item.SubLimitAmount;
                        item.CoverageOriginalStatus = item.CoverStatus;
                        item.CoverStatus = coverageStatusType;
                        item.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(coverageStatusType));
                        item.OriginalRate = item.Rate;
                        item.AccumulatedPremiumAmount = 0;
                        item.FlatRatePorcentage = 0;
                        item.PremiumAmount = 0;
                        item.IsSelected = coverageLocal.IsSelected;
                        item.IsMandatory = coverageLocal.IsMandatory;
                        item.IsVisible = coverageLocal.IsVisible;
                        item.CurrentFromOriginal = item.CurrentFrom;
                        item.CurrentToOriginal = item.CurrentTo == null ? DateTime.MinValue : (DateTime)item.CurrentTo;
                        item.IsPrimary = coverageLocal.IsPrimary;
                        item.IsPostcontractual = coverageLocal.IsPostcontractual;
                        item.EndorsementSublimitAmount = 0;
                        item.EndorsementLimitAmount = 0;
                    });
                    risk.Risk.Coverages = coveragesData;
                    risk.Risk.AmountInsured = risk.Risk.Coverages.Sum(x => x.LimitAmount);
                }
                else
                {
                    throw new Exception(Errors.ErrorCoverages);
                }
            }
            return risk;

        }
    }
}
