using AutoMapper;
using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.TransportExtensionService.EEProvider;
using Sistran.Company.Application.TransportExtensionService.EEProvider.Resources;
using Sistran.Company.Application.Transports.TransportBusinessService.Models.Base;
using AMA = Sistran.Company.Application.TransportExtensionService.EEProvider.AutoMapperAssembler;
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
using TM=System.Threading.Tasks;
using baf = Sistran.Core.Framework.BAF;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.TransportExtensionService.EEProvider.Business
{
    class TransportExtensionBusinessCia
    {
        BaseBusinessCia provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransportEndorsementBusinessCia" /> class.
        /// </summary>
        public TransportExtensionBusinessCia()
        {
            provider = new BaseBusinessCia();
        }

        /// <summary>
        /// Creates the endorsement.
        /// </summary>
        /// <param name="companyPolicy">The company endorsement.</param>
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
                List<CompanyTransport> companyTransports = new List<CompanyTransport>();
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
                        policy.Endorsement.TicketDate = companyPolicy.TicketDate;
                        policy.Endorsement.TicketNumber = companyPolicy.TicketNumber;

                        //companyTransports = DelegateService.transportService.GetCompanyTransportsByPolicyId(companyPolicy.Endorsement.PolicyId);
                        companyTransports = DelegateService.transportService.GetCompanyTransportsByTemporalId(policy.Id);
                        if (companyTransports != null && companyTransports.Any())
                        {
                            policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                            companyTransports.AsParallel().ForAll(x => x.Risk.Policy = policy);
                            risks = CreateExtension(companyTransports);
                        }
                        else
                        {
                            companyTransports = DelegateService.transportService.GetCompanyTransportsByPolicyId(companyPolicy.Endorsement.PolicyId);
                            if (companyTransports != null && companyTransports.Any())
                            {
                                policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                                companyTransports.AsParallel().ForAll(x => x.Risk.Policy = policy);
                                risks = CreateExtension(companyTransports);
                            }
                            else
                            {
                                throw new Exception("Error Obteniendo transportes");
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
                        policy.Endorsement.EndorsementReasonDescription = companyPolicy.Endorsement.EndorsementReasonDescription;

                        var imapper = AMA.AutoMapperAssembler.CreateMapClause();
                        policy.Clauses = imapper.Map<List<Clause>, List<CompanyClause>>(DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.General, policy.Prefix.Id).Where(x => x.IsMandatory).ToList());

                        policy.Summary = new CompanySummary
                        {
                            RiskCount = 0
                        };
                        policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                        if (policy != null)
                        {
                            companyTransports = DelegateService.transportService.GetCompanyTransportsByPolicyId(companyPolicy.Endorsement.PolicyId);
                            if (companyTransports != null && companyTransports.Any())
                            {
                                companyTransports.AsParallel().ForAll(x => x.Risk.Policy = policy);
                                risks = CreateExtension(companyTransports);
                            }
                            else
                            {
                                throw new Exception("Error Obteniendo transportes");
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
                    policy.Summary = risks.First().Policy.Summary;
                }
                return policy;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Creates Extension.
        /// </summary>
        /// <param name="companyTransports">The company endorsement.</param>
        /// <returns></returns>
        private List<CompanyRisk> CreateExtension(List<CompanyTransport> companyTransports)
        {
            if (companyTransports != null && companyTransports.Any())
            {
                List<CompanyRisk> risks = new List<CompanyRisk>();
                PendingOperation pendingOperation = new PendingOperation();

                if ((bool)companyTransports.First()?.Risk?.Policy.Product.IsCollective)
                {
                    if (companyTransports.First().Risk.Policy.Endorsement.TemporalId > 0)
                    {
                        companyTransports = DelegateService.transportService.GetCompanyTransportsByTemporalId(companyTransports.First().Risk.Policy.Endorsement.TemporalId);
                        TP.Parallel.ForEach(companyTransports, item =>
                        {
                            item.Risk.IsPersisted = true;
                            item.Risk.Status = RiskStatusType.Original;
                            var risk = GetDataExtension(item);
                            risk = DelegateService.transportService.CreateCompanyTransportTemporal(risk);
                            risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                            risks.Add(risk.Risk);
                        });

                        var transportPolicy = provider.CalculatePolicyAmounts(risks.First().Policy, risks);

                        transportPolicy = DelegateService.underwritingService.CreatePolicyTemporal(transportPolicy, false);

                        return risks;
                    }
                    else
                    {
                        companyTransports = DelegateService.transportService.GetCompanyTransportsByPolicyId(companyTransports.First().Risk.Policy.Endorsement.PolicyId);
                        TP.Parallel.ForEach(companyTransports, item =>
                        {
                            item.Risk.IsPersisted = true;
                            var risk = GetDataExtension(item);
                            risk.Risk.Status = RiskStatusType.Original;
                            risk = DelegateService.transportService.CreateCompanyTransportTemporal(risk);
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
                    TP.Parallel.ForEach(companyTransports, item =>
                    {
                        item.Risk.IsPersisted = true;
                        item.Risk.Status = RiskStatusType.Original;
                        var risk = GetDataExtension(item);
                        risk = DelegateService.transportService.CreateCompanyTransportTemporal(risk);
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

        /// <summary>
        /// Gets the data extension
        /// </summary>
        /// <param name="risk">The company endorsement.</param>
        /// <returns></returns>
        private CompanyTransport GetDataExtension(CompanyTransport risk)
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
                    String coverStatusName = String.Empty;
                    if (item.CoverageOriginalStatus.HasValue)
                    {
                        if (Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(item.CoverageOriginalStatus.Value)) == null)
                        {
                            coverStatusName = EnumHelper.GetItemName<CoverageStatusType>(item.CoverageOriginalStatus.Value);
                        }
                        else
                        {
                            coverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(item.CoverageOriginalStatus.Value));
                        }
                    }

                    CompanyCoverage coverageLocal = coverages.FirstOrDefault(u => u.Id == item.Id);
                    item.CoverStatus = item.CoverageOriginalStatus;
                    item.CoverStatusName = coverStatusName;
                    item.CurrentFrom = risk.Risk.Policy.CurrentFrom;
                    item.CurrentTo = risk.Risk.Policy.CurrentTo;
                    item.Description = coverageLocal.Description;
                    item.EndorsementType = risk.Risk.Policy.Endorsement.EndorsementType;

                    item.AccumulatedPremiumAmount = 0;
                    item.FlatRatePorcentage = 0;
                    item.SubLineBusiness = coverageLocal.SubLineBusiness;
                    item.IsSelected = coverageLocal.IsSelected;
                    item.IsMandatory = coverageLocal.IsMandatory;
                    item.IsVisible = coverageLocal.IsVisible;
                    item.EndorsementLimitAmount = 0;
                    item.EndorsementSublimitAmount = 0;
                });
                risk.Risk.Coverages = ciaCoverages;
                risk.Risk.AmountInsured = risk.Risk.Coverages.Sum(x => x.LimitAmount);
            }
            else
            {
                throw new Exception(Errors.ErrorCoverages);
            }


            risk = DelegateService.transportService.QuotateCompanyTransport(risk, false, false);

            return risk;
        }
    }
}
