using AutoMapper;
using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.PropertyEndorsementExtensionService.EEProvider.Resources;
using Sistran.Company.Application.PropertyEndorsementExtensionService.EEProvider.Services;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TM=System.Threading.Tasks;
using baf = Sistran.Core.Framework.BAF;
using PEM = Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Company.Application.PropertyEndorsementExtensionService3GProvider.Assemblers;
using Sistran.Core.Application.Utilities.Utility;
using Sistran.Core.Framework.Queries;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
using System.Data;
using Sistran.Core.Application.Utilities.DataFacade;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.PropertyEndorsementExtensionService.EEProvider.Business
{
    class ExtensionBusinessCia
    {
        BaseBusinessCia provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyEndorsementBusinessCia" /> class.
        /// </summary>
        public ExtensionBusinessCia()
        {
            provider = new BaseBusinessCia();
        }

        internal bool ValidateDeclarativeInsuredObjects(decimal policyId)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(ISSEN.EndorsementRisk.Properties.PolicyId, "er");
                filter.Equal();
                filter.Constant(policyId);
                filter.And();
                filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, "er");
                filter.Equal();
                filter.Constant(1);
                filter.And();
                filter.Property(QUOEN.InsuredObject.Properties.IsDeclarative, "io");
                filter.Equal();
                filter.Constant(1);
                //columnas que devuelve
                SelectQuery SelectQuery = new SelectQuery();
                SelectQuery.AddSelectValue(new SelectValue(new Column(QUOEN.InsuredObject.Properties.IsDeclarative, "io"), "IsDeclarative"));

                Join join = new Join(new ClassNameTable(typeof(ISSEN.EndorsementRisk), "er"), new ClassNameTable(typeof(ISSEN.RiskInsuredObject), "rio"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(ISSEN.EndorsementRisk.Properties.RiskId, "er")
                    .Equal()
                    .Property(ISSEN.RiskInsuredObject.Properties.RiskId, "rio")
                    .GetPredicate());
                join = new Join(join, new ClassNameTable(typeof(QUOEN.InsuredObject), "io"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(ISSEN.RiskInsuredObject.Properties.InsuredObjectId, "rio")
                    .Equal()
                    .Property(QUOEN.InsuredObject.Properties.InsuredObjectId, "io")
                    .GetPredicate());
                SelectQuery.Table = join;
                SelectQuery.Where = filter.GetPredicate();
                SelectQuery.Distinct = true;
                //SelectQuery.GetFirstSelect();

                bool result = false;
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(SelectQuery))
                {
                    while (reader.Read())
                    {
                        result = (bool)reader["IsDeclarative"];
                        break;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
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
                List<PEM.CompanyPropertyRisk> companyProperty = new List<PEM.CompanyPropertyRisk>();
                PendingOperation pendingOperation = new PendingOperation();

                if (companyPolicy?.Endorsement?.TemporalId > 0)
                {
                    policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPolicy.Endorsement.TemporalId, false);
                    if (policy != null)
                    {
                        policy.UserId = companyPolicy.UserId;
                        policy.CurrentTo = Convert.ToDateTime(companyPolicy.Endorsement.CurrentTo);
                        policy.Endorsement.EndorsementReasonId = companyPolicy.Endorsement.EndorsementReasonId;
                        policy.Endorsement.Text = new CompanyText
                        {
                            TextBody = companyPolicy.Endorsement.Text?.TextBody,
                            Observations = companyPolicy.Endorsement.Text?.Observations
                        };
                        policy.Endorsement.TicketDate = companyPolicy.Endorsement.TicketDate;
                        policy.Endorsement.TicketNumber = companyPolicy.Endorsement.TicketNumber;

                        companyProperty = DelegateService.propertyService.GetCompanyPropertiesByTemporalId(policy.Id);
                        if (companyProperty != null && companyProperty.Any())
                        {
                            policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                            companyProperty.AsParallel().ForAll(x => x.Risk.Policy = policy);
                            risks = CreateExtension(companyProperty);
                        }
                        else
                        {
                            companyProperty = DelegateService.propertyService.GetCompanyPropertiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                            if (companyProperty != null && companyProperty.Any())
                            {
                                policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                                companyProperty.AsParallel().ForAll(x => x.Risk.Policy = policy);
                                risks = CreateExtension(companyProperty);
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
                        policy.UserId = companyPolicy.UserId;
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
                        policy.Endorsement.TicketDate = companyPolicy.Endorsement.TicketDate;
                        policy.Endorsement.TicketNumber = companyPolicy.Endorsement.TicketNumber;
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
                            companyProperty = DelegateService.propertyService.GetCompanyPropertiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                            if (companyProperty != null && companyProperty.Any())
                            {
                                companyProperty.AsParallel().ForAll(x => x.Risk.Policy = policy);
                                risks = CreateExtension(companyProperty);
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

        private List<CompanyRisk> CreateExtension(List<PEM.CompanyPropertyRisk> companyProperty)
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
                            var risk = GetDataExtension(item);
                            risk = DelegateService.propertyService.CreatePropertyTemporal(risk, false);
                            risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                            risks.Add(risk.Risk);
                        });

                        var propertyPolicy = provider.CalculatePolicyAmounts(risks.First().Policy, risks);

                        propertyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(propertyPolicy, false);

                        return risks;
                    }
                    else
                    {
                        companyProperty = DelegateService.propertyService.GetCompanyPropertiesByPolicyId(companyProperty.First().Risk.Policy.Endorsement.PolicyId);
                        TP.Parallel.ForEach(companyProperty, item =>
                        {
                            item.Risk.IsPersisted = true;
                            var risk = GetDataExtension(item);
                            risk.Risk.Status = RiskStatusType.Original;
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
                    TP.Parallel.ForEach(companyProperty, item =>
                    {
                        item.Risk.IsPersisted = true;
                        var risk = GetDataExtension(item);
                        risk.Risk.Status = RiskStatusType.Original;
                        risk = DelegateService.propertyService.CreatePropertyTemporal(risk, false);
                        if (risk != null)
                        {
                            if (risk.Risk.Policy.InfringementPolicies != null && risk.Risk.Policy.InfringementPolicies.Any())
                            {
                                risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                            }
                            risks.Add(risk.Risk);
                        }
                    });
                    var propertyPolicy = provider.CalculatePolicyAmounts(risks.First().Policy, risks);

                    /*Actualiza el Pending Operation de la Poliza*/
                    propertyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(propertyPolicy, false);

                    return risks;
                }
            }
            else
            {
                throw new Exception("No existen Vehiculos");
            }


        }
        private PEM.CompanyPropertyRisk GetDataExtension(PEM.CompanyPropertyRisk risk)
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


            risk = DelegateService.propertyService.QuotateProperty(risk, false, false);

            return risk;
        }
    }
}
