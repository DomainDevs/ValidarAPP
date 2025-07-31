using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.DeclarationBusinessServiceEEProvider.Assembler;
using Sistran.Company.Application.DeclarationBusinessServiceEEProvider.Resources;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TM = System.Threading.Tasks;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.DeclarationBusinessServiceEEProvider.Business
{
    public class DeclarationPropertyBusiness
    {

        BaseBusinessCia provider;

        public DeclarationPropertyBusiness()
        {
            provider = new BaseBusinessCia();
        }

        /// <summary>
        /// creates endorsement
        /// </summary>
        /// <param name="companyPolicy"></param>
        /// <returns></returns>
        public CompanyPolicy CreateEndorsementDeclaration(CompanyPolicy companyPolicy, Dictionary<string, object> formValues)
        {
            try
            {
                if (companyPolicy == null)
                {
                    throw new ArgumentException("Poliza Vacia");
                }
                List<CompanyRisk> risks = new List<CompanyRisk>();
                CompanyPolicy policy;
                List<CompanyPropertyRisk> companyPropertyRisks = new List<CompanyPropertyRisk>();
                CompanyCoverage coverage = new CompanyCoverage();
                coverage.DeclaredAmount = (decimal)companyPolicy.Endorsement.DeclaredValue;
                PendingOperation pendingOperation = new PendingOperation();
                if (companyPolicy?.Endorsement?.TemporalId > 0)
                {
                    policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPolicy.Endorsement.TemporalId, false);
                    if (policy != null)
                    {
                        policy.UserId = companyPolicy.UserId;
                        policy.Endorsement.UserId = companyPolicy.Endorsement.UserId;
                        policy.CurrentTo = Convert.ToDateTime(companyPolicy.Endorsement.CurrentTo);
                        policy.Endorsement.EndorsementReasonId = companyPolicy.Endorsement.EndorsementReasonId;
                        policy.Endorsement.DeclaredValue = companyPolicy.Endorsement.DeclaredValue;
                        policy.Endorsement.RiskId = companyPolicy.Endorsement.RiskId;
                        policy.Endorsement.InsuredObjectId = companyPolicy.Endorsement.InsuredObjectId;
                        policy.Endorsement.CurrentFrom = companyPolicy.Endorsement.CurrentFrom;
                        policy.Endorsement.CurrentTo = companyPolicy.Endorsement.CurrentTo;
                        policy.Endorsement.TicketNumber = companyPolicy.Endorsement.TicketNumber;
                        policy.Endorsement.TicketDate = companyPolicy.Endorsement.TicketDate;
                        policy.Endorsement.Text = new CompanyText
                        {
                            TextBody = companyPolicy.Endorsement.Text?.TextBody,
                            Observations = companyPolicy.Endorsement.Text?.Observations
                        };
                        if (policy.Id != 0)
                        {
                            companyPropertyRisks = DelegateService.propertyService.GetCompanyPropertiesByTemporalId(policy.Id);
                            if (companyPropertyRisks == null || companyPropertyRisks.Count == 0)
                                companyPropertyRisks = DelegateService.propertyService.GetCompanyPropertiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                        }
                        else
                        {
                            companyPropertyRisks = DelegateService.propertyService.GetCompanyPropertiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                        }
                        if (companyPropertyRisks != null && companyPropertyRisks.Any())
                        {
                            CompanyRisk tempRisk = policy.Summary.Risks.Where(x => x.Policy.Endorsement.RiskId == int.Parse(formValues["RiskId"].ToString())).FirstOrDefault();
                            if (tempRisk != null)
                            {
                                if (tempRisk.Policy.Endorsement.InsuredObjectId == int.Parse(formValues["InsuredObjectId"].ToString()))
                                {
                                    policy.Summary.Risks.Where(x => x.Policy.Endorsement.RiskId == int.Parse(formValues["RiskId"].ToString())).FirstOrDefault().Policy.Endorsement = policy.Endorsement;
                                }
                                else
                                {
                                    tempRisk.Policy = new CompanyPolicy { Endorsement = companyPolicy.Endorsement };
                                    policy.Summary.Risks.Add(tempRisk);
                                }
                            }
                            else
                            {
                                CompanyRisk tRisk = new CompanyRisk();
                                tRisk.Policy = new CompanyPolicy { Endorsement = companyPolicy.Endorsement };
                                policy.Summary.Risks.Add(tRisk);
                            }

                            policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                            companyPropertyRisks.AsParallel().ForAll(x => x.Risk.Policy = policy);
                            if (policy.Summary.Risks.Count > 0)
                            {
                                companyPolicy.Summary = policy.Summary;
                            }
                            risks = CreateDeclaration(companyPropertyRisks, companyPolicy);
                        }
                        else
                        {
                            throw new Exception("Error Obteniendo Riesgos");
                        }

                    }
                    else
                    {
                        throw new ArgumentException("Temporal poliza encontrado");
                    }

                }
                else
                {
                    policy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyPolicy.Endorsement.Id);
                    if (policy != null)
                    {
                        policy.CurrentFrom = companyPolicy.Endorsement.CurrentFrom;
                        policy.CurrentTo = Convert.ToDateTime(companyPolicy.Endorsement.CurrentTo);
                        policy.UserId = companyPolicy.UserId;
                        policy.Endorsement.UserId = companyPolicy.Endorsement.UserId;
                        policy.EffectPeriod = DelegateService.commonService.GetExtendedParameterByParameterId(1027).NumberParameter.Value;
                        if (policy.Endorsement == null)
                        {
                            policy.Endorsement = new CompanyEndorsement();
                        }
                        policy.Endorsement.Text = new CompanyText
                        {
                            TextBody = companyPolicy.Endorsement.Text.TextBody,
                            Observations = companyPolicy.Endorsement.Text.Observations
                        };

                        policy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(policy.Product.Id, policy.Prefix.Id);
                        policy.Endorsement.EndorsementReasonId = companyPolicy.Endorsement.EndorsementReasonId;
                        policy.Endorsement.EndorsementType = EndorsementType.DeclarationEndorsement;
                        policy.Endorsement.EndorsementTypeDescription = EnumHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
                        policy.TemporalType = TemporalType.Endorsement;
                        policy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(policy.TemporalType);
                        policy.Endorsement.DeclaredValue = companyPolicy.Endorsement.DeclaredValue;
                        policy.Endorsement.RiskId = companyPolicy.Endorsement.RiskId;
                        policy.Endorsement.InsuredObjectId = companyPolicy.Endorsement.InsuredObjectId;
                        policy.Endorsement.CurrentFrom = companyPolicy.Endorsement.CurrentFrom;
                        policy.Endorsement.CurrentTo = companyPolicy.Endorsement.CurrentTo;
                        policy.Endorsement.TicketNumber = companyPolicy.Endorsement.TicketNumber;
                        policy.Endorsement.TicketDate = companyPolicy.Endorsement.TicketDate;

                        var imapper = ModelAssembler.CreateMapCompanyClause();
                        policy.Clauses = imapper.Map<List<Clause>, List<CompanyClause>>(DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.General, policy.Prefix.Id).Where(x => x.IsMandatory).ToList());

                        policy.Summary = new CompanySummary
                        {
                            RiskCount = 0
                        };
                        CompanyRisk tRisk = new CompanyRisk();
                        tRisk.Policy = new CompanyPolicy { Endorsement = companyPolicy.Endorsement };
                        policy.Summary.Risks = new List<CompanyRisk>() { tRisk };
                        policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                        if (policy != null)
                        {
                            companyPropertyRisks = DelegateService.propertyService.GetCompanyPropertiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                            if (companyPropertyRisks != null && companyPropertyRisks.Any())
                            {
                                companyPropertyRisks.AsParallel().ForAll(x => x.Risk.Policy = policy);
                                if (policy.Summary.Risks.Count > 0)
                                {
                                    companyPolicy.Summary = policy.Summary;
                                }
                                risks = CreateDeclaration(companyPropertyRisks, companyPolicy);
                            }
                            else
                            {
                                throw new Exception("Error Obteniendo Riesgos");
                            }
                        }
                        else
                        {
                            throw new Exception("Error Creando temporal");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Poliza no encontrada");
                    }
                }
                if (policy.InfringementPolicies != null && policy.InfringementPolicies.Count() > 0)
                {
                    risks.AsParallel().ForAll(x => policy.InfringementPolicies.AddRange(x.InfringementPolicies));
                }
                if (risks != null && risks.Count != 0)
                {
                    policy.Summary = risks.Where(x => x.RiskId == (int)formValues["RiskId"]).FirstOrDefault().Policy.Summary;
                    //policy.Summary.AmountInsured = risks.Where(x => x.RiskId == (int)formValues["RiskId"]).FirstOrDefault().AmountInsured;
                }

                return policy;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error  al Crear Endoso de Declaración" + ex);
            }
        }

        /// <summary>
        /// Create Declaration
        /// </summary>
        /// <param name="companyPropertyRisks"></param>
        /// <returns></returns>
        private List<CompanyRisk> CreateDeclaration(List<CompanyPropertyRisk> companyPropertyRisks, CompanyPolicy policy)
        {
            if (companyPropertyRisks != null && companyPropertyRisks.Any())
            {
                List<CompanyRisk> risks = new List<CompanyRisk>();
                PendingOperation pendingOperation = new PendingOperation();

                if ((bool)companyPropertyRisks.First()?.Risk?.Policy.Product.IsCollective)
                {
                    if (companyPropertyRisks.First().Risk.Policy.Endorsement.TemporalId > 0)
                    {
                        companyPropertyRisks = DelegateService.propertyService.GetCompanyPropertiesByTemporalId(companyPropertyRisks.First().Risk.Policy.Endorsement.TemporalId);
                        foreach (var item in companyPropertyRisks)
                        {
                            item.Risk.IsPersisted = true;
                            var risk = GetDataDeclaration(item, policy);
                            risk = DelegateService.propertyService.CreatePropertyTemporal(risk, false);
                            risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                            risks.Add(risk.Risk);
                        }
                        var propertyPolicy = provider.CalculatePolicyAmounts(risks.First().Policy, risks);
                        propertyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(propertyPolicy, false);
                        return risks;
                    }
                    else
                    {
                        companyPropertyRisks = DelegateService.propertyService.GetCompanyPropertiesByPolicyId(companyPropertyRisks.First().Risk.Policy.Endorsement.PolicyId);
                        TP.Parallel.ForEach(companyPropertyRisks, item =>
                        {
                            item.Risk.IsPersisted = true;
                            var risk = GetDataDeclaration(item, policy);
                            risk = DelegateService.propertyService.CreatePropertyTemporal(risk, false);
                            risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                            risks.Add(risk.Risk);
                        });

                        var propertyPolicy = provider.CalculatePolicyAmounts(risks.First().Policy, risks);

                        propertyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(propertyPolicy, false);
                        return risks;
                    }
                }
                else
                {
                    TP.Parallel.ForEach(companyPropertyRisks, item =>
                    {
                        item.Risk.IsPersisted = true;
                        var risk = GetDataDeclaration(item, policy);
                        item.Risk.OriginalStatus = RiskStatusType.Modified;
                        item.Risk.Status = RiskStatusType.Modified;
                        risk = DelegateService.propertyService.CreatePropertyTemporal(risk, false);
                        risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                        risks.Add(risk.Risk);
                    });
                    var propertyPolicy = provider.CalculatePolicyAmounts(risks.Where(x => x.RiskId == policy.Endorsement.RiskId).FirstOrDefault().Policy, risks);
                    if (policy.Summary.Risks.Count > 0)
                    {
                        propertyPolicy.Summary.Risks = policy.Summary.Risks;
                    }
                    /*Actualiza el Pending Operation de la Poliza*/
                    propertyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(propertyPolicy, false);
                    return risks;
                }
            }
            else
            {
                throw new Exception("No existen riesgos");
            }
        }

        private CompanyPropertyRisk GetDataDeclaration(CompanyPropertyRisk risk, CompanyPolicy policy)
        {
            if (risk.Risk?.Beneficiaries?[0].IdentificationDocument == null)
            {
                List<CompanyBeneficiary> beneficiaries = new List<CompanyBeneficiary>();
                ConcurrentBag<string> error = new ConcurrentBag<string>();
                if (risk.Risk.Beneficiaries != null)
                {
                    risk.Risk.Beneficiaries.AsParallel().ForAll(
                        item =>
                        {
                            var beneficiary = DelegateService.underwritingService.GetBeneficiariesByDescription(item.IndividualId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
                            if (beneficiary != null)
                            {
                                item.IdentificationDocument = beneficiary.IdentificationDocument;
                                item.Name = beneficiary.Name;
                            }
                            else
                            {
                                error.Add(Error.ErrorBeneficiaryNotFound);
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
                    throw new Exception(Error.ErrorBeneficiaryNotFound);
                }
            }
            List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(risk.Risk.Policy.Product.Id, risk.Risk.GroupCoverage.Id, risk.Risk.Policy.Prefix.Id);
            if (coverages != null && coverages.Count > 0)
            {
                coverages = coverages.Where(c => (risk.Risk.Coverages.Any(x => x.Id == c.Id))).ToList();
                var ciaCoverages = risk.Risk.Coverages.Where(x => coverages.Select(z => z.Id).Contains(x.Id)).ToList();
                ciaCoverages.AsParallel().ForAll(item =>
                {
                    var coverageLocal = coverages.FirstOrDefault(u => u.Id == item.Id);
                    item.CoverStatus = item.CoverageOriginalStatus;
                    item.CurrentFrom = risk.Risk.Policy.CurrentFrom;
                    item.CurrentTo = risk.Risk.Policy.CurrentTo;
                    item.AccumulatedPremiumAmount = 0;
                    item.FlatRatePorcentage = 0;
                    item.SubLineBusiness = coverageLocal.SubLineBusiness;
                    item.IsSelected = coverageLocal.IsSelected;
                    item.IsMandatory = coverageLocal.IsMandatory;
                    item.IsVisible = coverageLocal.IsVisible;
                    //item.Rate = coverageLocal.Rate;
                    //item.DepositPremiumPercent = coverageLocal.DepositPremiumPercent;
                    if (item.InsuredObject.Id == risk.Risk.Policy.Endorsement.InsuredObjectId)
                    {
                        item.DeclaredAmount = (decimal)risk.Risk.Policy.Endorsement.DeclaredValue;
                        if (item.LimitAmount > 0)
                        {
                            item.LimitAmount = (decimal)risk.Risk.Policy.Endorsement.DeclaredValue;
                        }

                    }
                    //else
                    //{
                    //    item.DeclaredAmount = 0;
                    //    item.LimitAmount = 0;
                    //}
                });
                risk.Risk.Coverages = ciaCoverages;
                risk.Risk.AmountInsured = risk.Risk.Coverages.Sum(x => x.LimitAmount);
            }
            else
            {
                throw new Exception(Error.ErrorCoverages);
            }
            risk = DelegateService.propertyService.QuotateProperty(risk, false, false);

            return risk;
        }
    }
}
