using AutoMapper;
using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.SuretyEndorsementExtensionService.EEProvider.Resources;
using Sistran.Company.Application.SuretyEndorsementExtensionService.EEProvider.Services;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using baf = Sistran.Core.Framework.BAF;
using SEM = Sistran.Company.Application.Sureties.SuretyServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Company.Application.SuretyEndorsementExtensionService3GProvider.Assemblers;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Framework.BAF;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.SuretyEndorsementExtensionService.EEProvider.Business
{
    class ExtensionBusinessCia
    {
        BaseBusinessCia provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="SuretyEndorsementBusinessCia" /> class.
        /// </summary>
        public ExtensionBusinessCia()
        {
            provider = new BaseBusinessCia();
        }
        /// <summary>
        /// Creates the endorsement.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Endoso Vacio</exception>
        public CompanyPolicy CreateEndorsementExtension(CompanyPolicy companyPolicy)
        {
            try
            {
                if (companyPolicy == null)
                {
                    throw new ArgumentException("Poliza Vacia");
                }
                List<CompanyRisk> risks = new List<CompanyRisk>();
                CompanyPolicy policy;
                List<SEM.CompanyContract> companySuretys = new List<SEM.CompanyContract>();
                PendingOperation pendingOperation = new PendingOperation();

                if (companyPolicy?.Endorsement?.TemporalId > 0)
                {
                    policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPolicy.Endorsement.TemporalId, false);
                    if (policy != null)
                    {

                        policy.CurrentTo = Convert.ToDateTime(companyPolicy.Endorsement.CurrentTo);
                        policy.Endorsement.EndorsementReasonId = companyPolicy.Endorsement.EndorsementReasonId;
                        policy.Endorsement.Text = new CompanyText
                        {
                            TextBody = companyPolicy.Endorsement.Text?.TextBody,
                            Observations = companyPolicy.Endorsement.Text?.Observations
                        };

                        companySuretys = DelegateService.suretyService.GetCompanySuretiesByTemporalId(policy.Id);
                        policy.UserId = companyPolicy.UserId;
                        if (companySuretys != null && companySuretys.Any())
                        {
                            policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                            companySuretys.AsParallel().ForAll(x => x.Risk.Policy = policy);
                            risks = CreateExtension(companySuretys);
                        }
                        else
                        {
                            companySuretys = DelegateService.suretyService.GetCompanySuretiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                            if (companySuretys != null && companySuretys.Any())
                            {
                                policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                                companySuretys.AsParallel().ForAll(x => x.Risk.Policy = policy);
                                risks = CreateExtension(companySuretys);
                            }
                            else
                            {
                                throw new Exception("Error Obteniendo vehiculos");
                            }
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
                        policy.UserId = BusinessContext.Current?.UserId ?? companyPolicy.UserId; 

                        policy.UserId = policy.UserId;
                        policy.CurrentFrom = policy.CurrentTo;
                        policy.CurrentTo = Convert.ToDateTime(companyPolicy.Endorsement.CurrentTo);
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
                        policy.Endorsement.EndorsementType = EndorsementType.EffectiveExtension;
                        policy.Endorsement.EndorsementTypeDescription = EnumHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
                        policy.TemporalType = TemporalType.Endorsement;
                        policy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(policy.TemporalType);

                        var immaper = AutoMapperAssembler.CreateMapCompanyClause();
                        policy.Clauses = immaper.Map<List<Clause>, List<CompanyClause>>(DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(Core.Application.UnderwritingServices.Enums.EmissionLevel.General, policy.Prefix.Id).Where(x => x.IsMandatory).ToList());

                        policy.Summary = new CompanySummary
                        {
                            RiskCount = 0
                        };
                        policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                        if (policy != null)
                        {
                            companySuretys = DelegateService.suretyService.GetCompanySuretiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                            if (companySuretys != null && companySuretys.Any())
                            {
                                companySuretys.AsParallel().ForAll(x => x.Risk.Policy = policy);
                                risks = CreateExtension(companySuretys);
                            }
                            else
                            {
                                throw new Exception("Error Obteniendo vehiculos");
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
                    risks.ForEach(x => policy.InfringementPolicies.AddRange(x.InfringementPolicies));
                }
                if (risks != null && risks.Count != 0)
                {
                    policy.Summary = risks.First().Policy.Summary;
                }
                return policy;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private List<CompanyRisk> CreateExtension(List<SEM.CompanyContract> companySuretys)
        {
            if (companySuretys != null && companySuretys.Any())
            {
                List<CompanyRisk> risks = new List<CompanyRisk>();
                PendingOperation pendingOperation = new PendingOperation();

                if ((bool)companySuretys.First()?.Risk?.Policy.Product.IsCollective)
                {
                    if (companySuretys.First().Risk.Policy.Endorsement.TemporalId > 0)
                    {
                        companySuretys = DelegateService.suretyService.GetCompanySuretiesByTemporalId(companySuretys.First().Risk.Policy.Endorsement.TemporalId);
                        TP.Parallel.ForEach(companySuretys, item =>
                        {
                            item.Risk.IsPersisted = true;
                            var risk = GetDataExtension(item);
                            risk = DelegateService.suretyService.CreateSuretyTemporal(risk, false);
                            risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                            risks.Add(risk.Risk);
                        });

                        var suretyPolicy = provider.CalculatePolicyAmounts(risks.First().Policy, risks);

                        suretyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(suretyPolicy, false);

                        return risks;
                    }
                    else
                    {
                        companySuretys = DelegateService.suretyService.GetCompanySuretiesByPolicyId(companySuretys.First().Risk.Policy.Endorsement.PolicyId);
                        TP.Parallel.ForEach(companySuretys, item =>
                        {
                            item.Risk.IsPersisted = true;
                            var risk = GetDataExtension(item);
                            risk = DelegateService.suretyService.CreateSuretyTemporal(risk, false);
                            risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                            risks.Add(risk.Risk);
                        });

                        var suretyPolicy = provider.CalculatePolicyAmounts(risks.First().Policy, risks);

                        suretyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(suretyPolicy, false);

                        return risks;
                    }
                }
                else
                {
                    TP.Parallel.ForEach(companySuretys, item =>
                    {
                        item.Risk.IsPersisted = true;
                        var risk = GetDataExtension(item);
                        risk = DelegateService.suretyService.CreateSuretyTemporal(risk, false);
                        risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                        risks.Add(risk.Risk);
                    });
                    var suretyPolicy = provider.CalculatePolicyAmounts(risks.First().Policy, risks);

                    /*Actualiza el Pending Operation de la Poliza*/
                    suretyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(suretyPolicy, false);

                    return risks;
                }
            }
            else
            {
                throw new Exception("No existen Vehiculos");
            }


        }
        private SEM.CompanyContract GetDataExtension(SEM.CompanyContract risk)
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
                    throw new Exception(Errors.ErrorBeneficiaryEmpty);
                }
            }
            List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(risk.Risk.Policy.Product.Id, risk.Risk.GroupCoverage.Id, risk.Risk.Policy.Prefix.Id);
            if (coverages != null && coverages.Count > 0)
            {
                coverages = coverages.Where(c => (risk.Risk.Coverages.Any(x => x.Id == c.Id))).ToList();
                var ciaCoverages = risk.Risk.Coverages.Where(x => coverages.Select(z => z.Id).Contains(x.Id)).ToList();
                ciaCoverages.AsParallel().ForAll(item =>
                {
                    item.CoverStatus = item.CoverageOriginalStatus;
                    item.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(item.CoverageOriginalStatus.Value));
                    item.CurrentFrom = risk.Risk.Policy.CurrentFrom;
                    item.CurrentTo = risk.Risk.Policy.CurrentTo;
                    item.Description = coverages.FirstOrDefault(u => u.Id == item.Id).Description;
                    item.EndorsementType = risk.Risk.Policy.Endorsement.EndorsementType;
                    item.AccumulatedPremiumAmount = 0;
                    item.FlatRatePorcentage = 0;
                    item.SubLineBusiness = coverages.First(x => x.Id == item.Id).SubLineBusiness;
                    item.IsPrimary = coverages.First(x => x.Id == item.Id).IsPrimary;
                });
                risk.Risk.Coverages = ciaCoverages;
                risk.Risk.AmountInsured = risk.Risk.Coverages.Sum(x => x.LimitAmount);
            }
            else
            {
                throw new Exception(Errors.ErrorCoverages);
            }


            risk = DelegateService.suretyService.QuotateSurety(risk, false, false);

            return risk;
        }
    }
}
