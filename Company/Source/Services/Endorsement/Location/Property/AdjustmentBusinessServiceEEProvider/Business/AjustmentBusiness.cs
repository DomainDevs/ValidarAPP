using Sistran.Company.Application.AdjustmentBusinessServiceEEProvider.Assemblers;
using Sistran.Company.Application.AdjustmentBusinessServiceEEProvider.Resources;
using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
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
using Sistran.Company.Application.Location.PropertyServices.EEProvider.DAOs;

namespace Sistran.Company.Application.AdjustmentBusinessServiceEEProvider.Business
{
    public class AjustmentBusiness
    {
        BaseBusinessCia provider;


        public AjustmentBusiness()
        {
            provider = new BaseBusinessCia();
        }

        public CompanyPolicy CreateEndorsementAdjustment(CompanyPolicy companyPolicy, Dictionary<string, object> formValues)
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
                PendingOperation pendingOperation = new PendingOperation();

                if (companyPolicy?.Endorsement?.TemporalId > 0)
                {
                    policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPolicy.Endorsement.TemporalId, false);
                    if (policy != null)
                    {

                        policy.CurrentTo = Convert.ToDateTime(companyPolicy.Endorsement.CurrentTo);
                        policy.Endorsement.EndorsementReasonId = companyPolicy.Endorsement.EndorsementReasonId;
                        policy.UserId = companyPolicy.Endorsement.UserId;
                        policy.Endorsement.Text = new CompanyText
                        {
                            TextBody = companyPolicy.Endorsement.Text?.TextBody,
                            Observations = companyPolicy.Endorsement.Text?.Observations
                        };
                        policy.Endorsement.TicketNumber = companyPolicy.Endorsement.TicketNumber;
                        policy.Endorsement.TicketDate = companyPolicy.Endorsement.TicketDate;

                        companyPropertyRisks = DelegateService.propertyService.GetCompanyPropertiesByTemporalId(policy.Id);
                        if (companyPropertyRisks == null || companyPropertyRisks.Count == 0)
                        {
                            companyPropertyRisks = DelegateService.propertyService.GetCompanyPropertiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                        }
                        if (companyPropertyRisks != null && companyPropertyRisks.Any())
                        {
                            int riskNum = companyPropertyRisks.Where(x => x.Risk.RiskId == (int)formValues["RiskId"]).Select(n => n.Risk.Number).FirstOrDefault();
                            CompanyRisk tempRisk = policy.Summary.Risks.Where(x => x.Policy.Endorsement.RiskId == riskNum || x.Policy.Endorsement.RiskId == (int)formValues["RiskId"]).FirstOrDefault();
                            if (tempRisk != null)
                            {
                                policy.Summary.Risks.Where(x => x.Policy.Endorsement.RiskId == riskNum || x.Policy.Endorsement.RiskId == (int)formValues["RiskId"]).FirstOrDefault().Policy.Endorsement = policy.Endorsement;
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
                            risks = CreateAdjustment(companyPropertyRisks);
                        }
                        else
                        {
                            throw new Exception("Error Obteniendo riesgos");
                            //companyProperty = DelegateService.propertyService.GetCompanyPropertiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                            //if (companyProperty != null && companyProperty.Any())
                            //{
                            //    policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                            //    companyProperty.AsParallel().ForAll(x => x.Risk.Policy = policy);
                            //    risks = CreateAdjustment(companyProperty);
                            //}
                            //else
                            //{
                            //    throw new Exception("Error Obteniendo riesgos");
                            //}
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
                        policy.UserId = policy.UserId;
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
                        policy.Endorsement.EndorsementType = EndorsementType.AdjustmentEndorsement;
                        policy.Endorsement.EndorsementDays = companyPolicy.Endorsement.EndorsementDays;
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
                        policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                        if (policy != null)
                        {
                            companyPropertyRisks = DelegateService.propertyService.GetCompanyPropertiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                            if (companyPropertyRisks != null && companyPropertyRisks.Any())
                            {
                                CompanyPropertyRisk currentRisk = companyPropertyRisks.Where(x => x.Risk.RiskId == (int)formValues["RiskId"]).FirstOrDefault();
                                CompanyInsuredObject insuredObject = currentRisk.InsuredObjects.Where(x => x.Id == (int)formValues["InsuredObjectId"]).FirstOrDefault();
                                companyPolicy.Endorsement.InsuredObjectId = insuredObject.Id;
                                companyPolicy.Endorsement.RiskId = currentRisk.Risk.Number;
                                companyPolicy.Endorsement.DeclaredValue = currentRisk.Risk.AmountInsured;
                                CompanyRisk tRisk = new CompanyRisk();
                                tRisk.Policy = new CompanyPolicy { Endorsement = companyPolicy.Endorsement };
                                policy.Summary.Risks = new List<CompanyRisk>() { tRisk };
                                companyPropertyRisks.AsParallel().ForAll(x => x.Risk.Policy = policy);
                                risks = CreateAdjustment(companyPropertyRisks);
                            }
                            else
                            {
                                throw new Exception("Error Obteniendo riesgos");
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
                    policy.Summary.AmountInsured = risks.Where(x => x.RiskId == (int)formValues["RiskId"]).FirstOrDefault().AmountInsured;
                    CompanyCoverage coverage = risks.Where(x => x.RiskId == (int)formValues["RiskId"]).FirstOrDefault().Coverages.Where(y => y.IsPrimary).FirstOrDefault();
                    policy.Summary.Premium = (decimal)GetPremiumByRiskByInsuredObject(policy.Endorsement.PolicyId, risks.Where(x => x.RiskId == (int)formValues["RiskId"]).FirstOrDefault().Number, (int)formValues["InsuredObjectId"], coverage);
                }
                if (policy.Endorsement.TicketDate == null || policy.Endorsement.TicketNumber == null)
                {
                    policy.Endorsement.TicketDate = (DateTime)formValues["TicketDate"];
                    policy.Endorsement.TicketNumber = (int)formValues["TicketNumber"];
                }
                return policy;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error  al Crear Endoso de Ajuste");
            }
        }
        public decimal GetPremiumByRiskByInsuredObject(decimal policyId, decimal riskId, decimal insuredObject, CompanyCoverage coverage)
        {

            PropertyDAO propertyDAO = new PropertyDAO();
            CompanyEndorsementPeriod period = propertyDAO.GetEndorsementPeriodByPolicyId(policyId);
            List<CompanyEndorsementDetail> details = propertyDAO.GetEndorsementDetailsListByPolicyId(policyId, period.Version);
            details = details.Where(x => x.RiskNum == riskId && x.InsuredObjectId == insuredObject && x.EndorsementType == (int)EndorsementType.DeclarationEndorsement).ToList();
            decimal sumDeclaration = (decimal)details.Sum(x => x.PremiumAmount);
            if (coverage.DepositPremiumPercent > 0)
            {
                return sumDeclaration - (decimal)(coverage.SubLimitAmount * (coverage.DepositPremiumPercent / 100) * (coverage.Rate / 100));
            }
            return 0;

        }
        private List<CompanyRisk> CreateAdjustment(List<CompanyPropertyRisk> companyProperty)
        {
            if (companyProperty != null && companyProperty.Any())
            {
                List<CompanyRisk> risks = new List<CompanyRisk>();
                PendingOperation pendingOperation = new PendingOperation();

                if ((bool)companyProperty.First()?.Risk?.Policy.Product.IsCollective)
                {
                    if (companyProperty.First().Risk.Policy.Endorsement.TemporalId > 0)
                    {
                        companyProperty = DelegateService.propertyService.GetCompanyPropertiesByTemporalId(companyProperty.First().Risk.Policy.Endorsement.TemporalId);
                        TP.Parallel.ForEach(companyProperty, item =>
                        {
                            item.Risk.IsPersisted = true;
                            item.Risk.Status = RiskStatusType.Modified;
                            var risk = GetDataAdjustment(item);
                            risk = DelegateService.propertyService.CreatePropertyTemporal(risk, false);
                            risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                            risks.Add(risk.Risk);
                        });

                        var transportPolicy = provider.CalculatePolicyAmounts(risks.First().Policy, risks);

                        transportPolicy = DelegateService.underwritingService.CreatePolicyTemporal(transportPolicy, false);

                        return risks;
                    }
                    else
                    {
                        companyProperty = DelegateService.propertyService.GetCompanyPropertiesByPolicyId(companyProperty.First().Risk.Policy.Endorsement.PolicyId);
                        TP.Parallel.ForEach(companyProperty, item =>
                        {
                            item.Risk.IsPersisted = true;
                            var risk = GetDataAdjustment(item);
                            risk = DelegateService.propertyService.CreatePropertyTemporal(risk, false);
                            risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                            risks.Add(risk.Risk);
                        });

                        var transportPolicy = provider.CalculatePolicyAmounts(risks.First().Policy, risks);

                        transportPolicy = DelegateService.underwritingService.CreatePolicyTemporal(transportPolicy, false);

                        return risks;
                    }
                }
                else
                {
                    TP.Parallel.ForEach(companyProperty, item =>
                    {
                        item.Risk.IsPersisted = true;
                        var risk = GetDataAdjustment(item);
                        item.Risk.OriginalStatus = RiskStatusType.Modified;
                        item.Risk.Status = RiskStatusType.Modified;
                        risk = DelegateService.propertyService.CreatePropertyTemporal(risk, false);
                        risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                        risks.Add(risk.Risk);
                    });
                    var transportPolicy = provider.CalculatePolicyAmounts(risks.First().Policy, risks);

                    /*Actualiza el Pending Operation de la Poliza*/
                    transportPolicy = DelegateService.underwritingService.CreatePolicyTemporal(transportPolicy, false);

                    return risks;
                }
            }
            else
            {
                throw new Exception("No existen Transportes");
            }
        }


        private CompanyPropertyRisk GetDataAdjustment(CompanyPropertyRisk risk)
        {
            if (risk.Risk.Beneficiaries.Count > 0)
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
                        throw new Exception(Errors.ErrorBeneficiaryNotFound);
                    }
                }
            }
            else
            {
                throw new Exception(Errors.ErrorGetBeneficiary);
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
                    //item.DeclaredAmount=
                });
                risk.Risk.Coverages = ciaCoverages;
                risk.Risk.AmountInsured = risk.Risk.Coverages.Sum(x => x.LimitAmount);

                //companyCoverage = DelegateService.underwritingService.QuotateCompanyCoverage(companyCoverage, companyTransport.Risk.Policy.Endorsement.PolicyId, companyTransport.Risk.RiskId);
                //companyCoverage.PremiumAmount = (companyCoverage.PremiumAmount * companyCoverage.DepositPremiumPercent) / 100;
            }
            else
            {
                throw new Exception(Errors.ErrorCoverages);
            }
            risk = DelegateService.propertyService.QuotateProperty(risk, false, false);
            return risk;
        }

    }
}
