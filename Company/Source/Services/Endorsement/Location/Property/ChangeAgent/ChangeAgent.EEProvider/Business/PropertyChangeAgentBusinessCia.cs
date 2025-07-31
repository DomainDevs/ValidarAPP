using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.PropertyChangeAgentService.EEProvider.Resources;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CancellationEndorsement.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PEM = Sistran.Company.Application.Location.PropertyServices.Models;
using TP = Sistran.Core.Application.Utilities.Utility;



namespace Sistran.Company.Application.PropertyChangeAgentService.EEProvider.Business
{

    public class PropertyChangeAgentBusinessCia
    {
        BaseBusinessCia baseBusinessCia;
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyChangeAgentBusinessCia"/> class.
        /// </summary>
        public PropertyChangeAgentBusinessCia()
        {
            baseBusinessCia = new BaseBusinessCia();
        }

        /// <summary>
        /// Creates the temporal.
        /// </summary>
        /// <param name="companyPolicyBase">The company policy base.</param>
        /// <param name="isMassive">if set to <c>true</c> [is massive].</param>
        /// <returns></returns>
        public CompanyPolicy CreateTemporal(CompanyPolicy companyPolicyBase, bool isMassive = false)
        {
            try
            {
                if (companyPolicyBase == null)
                {
                    throw new Exception("Datos de la Poliza no Enviados");
                }
                companyPolicyBase.Endorsement.EndorsementType = EndorsementType.ChangeAgentEndorsement;
                CompanyPolicy companyPolicy = new CompanyPolicy();

                if (companyPolicyBase.Endorsement?.TemporalId != 0)
                {
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPolicyBase.Endorsement.TemporalId, false);
                }
                else
                {
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyPolicyBase.Endorsement.Id);
                    companyPolicy.Id = 0;
                    companyPolicy.Endorsement.TemporalId = 0;
                    companyPolicy.Summary = new CompanySummary
                    {
                        RiskCount = 0
                    };
                }
                if (companyPolicy != null)
                {
                    if ((companyPolicy.CurrentFrom != Convert.ToDateTime(companyPolicyBase.CurrentFrom))
                       || (companyPolicy.Agencies != companyPolicyBase.Agencies))
                    {
                        companyPolicy.UserId = companyPolicyBase.UserId;
                        companyPolicy.TemporalType = TemporalType.Endorsement;
                        companyPolicy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(TemporalType.Endorsement);
                        companyPolicy.CurrentFrom = companyPolicyBase.CurrentFrom;
                        companyPolicy.CurrentTo = companyPolicyBase.CurrentTo;
                        companyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);
                        companyPolicy.BeginDate = Convert.ToDateTime(companyPolicyBase.CurrentFrom);
                        companyPolicy.Endorsement = companyPolicyBase.Endorsement;
                        companyPolicy.Agencies = companyPolicyBase.Agencies;
                        companyPolicy = ChangeAgentPolicy(companyPolicy);
                        return companyPolicy;
                    }

                    return null;
                }
                else
                {
                    throw new Exception("Poliza No encontrada");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Changes the agent policy.
        /// </summary>
        /// <param name="companyPolicy">The company policy.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// Poliza Vacia
        /// or
        /// or
        /// Vehiculos no ERncontrados
        /// </exception>
        private CompanyPolicy ChangeAgentPolicy(CompanyPolicy companyPolicy)
        {
            try
            {
                if (companyPolicy == null)
                {
                    throw new Exception("Poliza Vacia");
                }
                List<PoliciesAut> riskInfringementPolicies = new List<PoliciesAut>();
                List<PEM.CompanyPropertyRisk> companyProperty = new List<PEM.CompanyPropertyRisk>();
                if (companyPolicy.Id == 0)
                {
                    companyPolicy.TemporalType = TemporalType.Endorsement;
                    companyPolicy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(TemporalType.Endorsement);
                    companyPolicy.Summary = new CompanySummary
                    {
                        RiskCount = 0
                    };
                    companyProperty = DelegateService.propertyService.GetCompanyPropertiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                    companyProperty.AsParallel().ForAll(
                        x =>
                        {
                            x.Risk.Id = 0;
                            x.Risk.OriginalStatus = x.Risk.Status;
                            x.Risk.Status = RiskStatusType.NotModified;
                        }
                        );
                }
                else
                {
                    companyProperty = DelegateService.propertyService.GetCompanyPropertiesByTemporalId(companyPolicy.Id);
                    if (companyProperty == null && companyProperty.Count == 0)
                    {
                        companyProperty = DelegateService.propertyService.GetCompanyPropertiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                    }
                }
                var risks = DelegateService.changeAgentEndorsementService.QuotateChangeAgentCia(companyPolicy);
                companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);
                if (companyPolicy != null && companyProperty?.Count > 0 && risks?.Count > 0)
                {
                    ConcurrentBag<string> errors = new ConcurrentBag<string>();
                    companyProperty.Where(a => a != null).AsParallel().ForAll(
                    z =>
                    {
                        var companyRisk = risks.FirstOrDefault(x => x.RiskId == z.Risk.RiskId);
                        if (companyRisk != null)
                        {
                            z.Risk.Policy = companyPolicy;
                            z.Risk.RiskId = companyRisk.RiskId;
                            z.Risk.Number = companyRisk.Number;
                            z.Risk.Status = companyRisk.Status;
                            z.Risk.Coverages = companyRisk.Coverages;
                            z.Risk.Premium = companyRisk.Coverages.Where(m => m != null).Sum(x => x.PremiumAmount);

                        }
                        else
                        {
                            errors.Add("Error Riesgo no encontrado");
                        }
                    }
                    );
                    companyProperty.AsParallel().ForAll(item =>
                    {
                        item = DelegateService.propertyModificationService.GetDataModification(item, CoverageStatusType.Included);
                        item.Risk.Status = RiskStatusType.Included;
                        var coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(companyPolicy.Product.Id, item.Risk.GroupCoverage.Id, companyPolicy.Prefix.Id);
                        if (coverages.Count < item.Risk.Coverages.Count)
                        {
                            throw new Exception("Coberturas no existe en el Grupo de Coberturas");
                        }
                        TP.Parallel.ForEach(item.Risk.Coverages, coverage =>
                        {
                            if (coverages != null && coverages.Where(x => x.Id == coverage.Id).FirstOrDefault() != null)
                            {
                                coverage.SubLineBusiness = coverages.First(x => x.Id == coverage.Id).SubLineBusiness;
                                coverage.Number = coverages.First(x => x.Id == coverage.Id).Number;
                            }

                            if (coverage.InsuredObject == null)
                            {

                                coverage.InsuredObject = new CompanyInsuredObject();
                                coverage.InsuredObject.Amount = coverages.First(x => x.Id == coverage.Id).InsuredObject.Amount;
                                coverage.InsuredObject.Description = coverages.First(x => x.Id == coverage.Id).InsuredObject.Description;
                                coverage.InsuredObject.Id = coverages.First(x => x.Id == coverage.Id).InsuredObject.Id;
                                coverage.InsuredObject.IsDeclarative = coverages.First(x => x.Id == coverage.Id).InsuredObject.IsDeclarative;
                                coverage.InsuredObject.IsMandatory = coverages.First(x => x.Id == coverage.Id).InsuredObject.IsMandatory;
                                coverage.InsuredObject.IsSelected = coverages.First(x => x.Id == coverage.Id).InsuredObject.IsSelected;
                                coverage.InsuredObject.ParametrizationStatus = coverages.First(x => x.Id == coverage.Id).InsuredObject.ParametrizationStatus;
                                coverage.InsuredObject.Premium = coverages.First(x => x.Id == coverage.Id).InsuredObject.Premium;
                                coverage.InsuredObject.SmallDescription = coverages.First(x => x.Id == coverage.Id).InsuredObject.SmallDescription;

                            }


                        });
                        item = DelegateService.propertyService.CreatePropertyTemporal(item, false);
                        if (item.Risk.InfringementPolicies != null)
                            riskInfringementPolicies.AddRange(item.Risk?.InfringementPolicies.Where(x => x != null));
                    });

                    if (companyProperty != null && companyProperty.Select(x => x.Risk).Any())
                    {

                        riskInfringementPolicies.AddRange(companyProperty.Select(x => x.Risk).SelectMany(x => x.InfringementPolicies).Where(m => m != null).ToList());

                        companyPolicy = baseBusinessCia.CalculatePolicyAmounts(companyPolicy, companyProperty.Select(x => x.Risk).ToList());

                        companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);

                        companyPolicy.InfringementPolicies.AddRange(riskInfringementPolicies);

                        return companyPolicy;
                    }
                    else
                    {
                        throw new Exception(Errors.UnquotedRisks);
                    }

                }
                else
                {
                    throw new Exception("Vehiculos no ERncontrados");
                }

            }
            catch (Exception)
            {

                throw;
            }

        }


        /// <summary>
        /// Creates the endorsement change agent.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        public List<CompanyPolicy> CreateEndorsementChangeAgent(CompanyEndorsement companyEndorsement)
        {
            try
            {
                List<CompanyPolicy> companyPolicies = new List<CompanyPolicy>();
                var TempId = companyEndorsement.TemporalId;
                var cancelation = CancellationPolicy(companyEndorsement);
                companyEndorsement.TemporalId = TempId;
                try
                {
                    companyEndorsement.Id = cancelation.Id;
                    var createEndorsementChangeAgent = CreatePropertyEndorsementChangeAgent(companyEndorsement);
                    companyPolicies.Add(createEndorsementChangeAgent);
                    companyPolicies.Add(cancelation);
                    return companyPolicies;
                }
                catch (Exception ex)
                {
                    DelegateService.underwritingService.DeleteEndorsementByPolicyIdEndorsementIdEndorsementType(cancelation.Endorsement.PolicyId, cancelation.Endorsement.Id, EndorsementType.Cancellation);
                    throw new Exception("Error Creando Endoso Cambio Agente");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Creates the vehicle endorsement change agent.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        private CompanyPolicy CreatePropertyEndorsementChangeAgent(CompanyEndorsement companyEndorsement)
        {
            var companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyEndorsement.TemporalId, false);
            if (companyPolicy.UserId == 0)
            {
                companyPolicy.UserId = companyEndorsement.UserId;
            }
            List<PEM.CompanyPropertyRisk> property = DelegateService.propertyService.GetCompanyPropertiesByTemporalId(companyPolicy.Id);
            return DelegateService.propertyService.CreateEndorsement(companyPolicy, property);

        }

        /// <summary>
        /// Cancellations the policy.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        private CompanyPolicy CancellationPolicy(CompanyEndorsement companyEndorsement)
        {
            companyEndorsement.TemporalId = 0;
            companyEndorsement.CancellationTypeId = (int)CancellationType.FromDate;
            companyEndorsement.EndorsementReasonId = 1;
            var date = companyEndorsement.CurrentTo - companyEndorsement.CurrentFrom;
            companyEndorsement.EndorsementDays = date.Days;
            var companyPolicy = DelegateService.endorsementPropertyCancellationService.CreateTemporalEndorsementCancellation(companyEndorsement);
            var property = DelegateService.propertyService.GetCompanyPropertiesByTemporalId(companyPolicy.Id);
            return DelegateService.propertyService.CreateEndorsement(companyPolicy, property);
        }
    }
}
