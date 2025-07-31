using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.LiabilityChangePolicyHolderService.EEProvider.Assemblers;
using Sistran.Company.Application.LiabilityChangePolicyHolderService.EEProvider.Resources;
using Sistran.Company.Application.Location.LiabilityServices.Models;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using ENUMPOLICIES = Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using SEM = Sistran.Company.Application.Location.LiabilityServices.Models;
using TP = Sistran.Core.Application.Utilities.Utility;
using Sistran.Core.Application.CancellationEndorsement.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using COMUT = Sistran.Company.Application.Utilities.Helpers;
using Sistran.Core.Services.UtilitiesServices.Enums;

namespace Sistran.Company.Application.LiabilityChangePolicyHolderService.EEProvider.Business
{
    public class LiabilityChangePolicyHolderBusinessCia
    {
        BaseBusinessCia baseBusinessCia;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiabilityPolicyHolderBusinessCia" /> class.
        /// </summary>
        public LiabilityChangePolicyHolderBusinessCia()
        {
            baseBusinessCia = new BaseBusinessCia();
        }
        /// <summary>
        /// Creates the temporal.
        /// </summary>
        /// <param name="companyChangePolicyHolderBase">The company policy base.</param>
        /// <param name="isMassive">if set to <c>true</c> [is massive].</param>
        /// <returns></returns>
        public CompanyChangePolicyHolder CreateTemporal(CompanyChangePolicyHolder companyChangePolicyHolderBase, bool isMassive = false)
        {
            try
            {
                if (companyChangePolicyHolderBase == null)
                {
                    throw new Exception("Datos de la Poliza no Enviados");
                }
                companyChangePolicyHolderBase.Endorsement.EndorsementType = EndorsementType.ChangePolicyHolderEndorsement;
                CompanyPolicy companyPolicy = new CompanyPolicy();

                if (companyChangePolicyHolderBase.Endorsement.TemporalId != 0)
                {
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyChangePolicyHolderBase.Endorsement.TemporalId, false);
                }
                else
                {
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyChangePolicyHolderBase.Endorsement.Id);
                }
                if (companyPolicy != null)
                {
                    if ((companyPolicy.CurrentFrom != Convert.ToDateTime(companyChangePolicyHolderBase.Endorsement.CurrentFrom)) || (companyPolicy.Holder != companyChangePolicyHolderBase.holder))
                    {
                        companyPolicy.UserId = BusinessContext.Current?.UserId ?? companyChangePolicyHolderBase.UserId;
                        companyPolicy.TemporalType = TemporalType.Endorsement;
                        companyPolicy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(TemporalType.Endorsement);
                        companyPolicy.CurrentFrom = companyChangePolicyHolderBase.CurrentFrom;
                        companyPolicy.CurrentTo = companyChangePolicyHolderBase.CurrentTo;
                        companyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);
                        companyPolicy.BeginDate = companyChangePolicyHolderBase.Endorsement.CurrentFrom;
                        
                        companyPolicy.Endorsement.CancelationCurrentFrom = companyChangePolicyHolderBase.Endorsement.CancelationCurrentFrom;
                        companyPolicy.Endorsement.CancelationCurrentTo = companyChangePolicyHolderBase.Endorsement.CancelationCurrentTo;
                        companyPolicy.Endorsement.CurrentFrom = companyChangePolicyHolderBase.Endorsement.CurrentFrom;
                        companyPolicy.Endorsement.CurrentTo = companyChangePolicyHolderBase.Endorsement.CurrentTo;
                        companyPolicy.Endorsement.EndorsementDays = companyChangePolicyHolderBase.Endorsement.EndorsementDays;
                        companyPolicy.Endorsement.EndorsementType = companyChangePolicyHolderBase.Endorsement.EndorsementType;
                        companyPolicy.Endorsement.IssueDate = companyChangePolicyHolderBase.Endorsement.IssueDate;
                        companyPolicy.Endorsement.TicketDate = companyChangePolicyHolderBase.Endorsement.TicketDate;
                        companyPolicy.Endorsement.TicketNumber = companyChangePolicyHolderBase.Endorsement.TicketNumber;
                        companyPolicy.Endorsement.UserId = companyChangePolicyHolderBase.Endorsement.UserId;
                        companyPolicy.Endorsement.EndorsementReasonId = companyChangePolicyHolderBase.Endorsement.EndorsementReasonId;
                        companyPolicy.Endorsement.PrevPolicyId = companyChangePolicyHolderBase.Endorsement.PrevPolicyId;
                        companyPolicy.Endorsement.Text = companyChangePolicyHolderBase.Endorsement.Text;
                        companyPolicy.Endorsement.OnlyCancelation = companyChangePolicyHolderBase.Endorsement.OnlyCancelation;
                        companyPolicy.Endorsement.ExchangeRate = companyChangePolicyHolderBase.Endorsement.ExchangeRate;

                        companyPolicy.Holder = companyChangePolicyHolderBase.holder;
                        if (companyPolicy.Holder.PaymentMethod.PaymentId == 0)
                        {
                            companyPolicy.Holder.PaymentMethod.PaymentId = 1;
                        }
                        companyPolicy.Holder = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(companyPolicy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();
                        companyPolicy = ChangePolicyHolderPolicy(companyPolicy);
                        return ModelAssembler.CreateCompanyChangePolicyHolder(companyPolicy);
                    }

                    return null;
                }
                else
                {
                    throw new Exception("Poliza No encontrada");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Changes the Coinsurance policy.
        /// </summary>
        /// <param name="companyPolicy">The company policy.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// Poliza Vacia
        /// or
        /// or
        /// Vehiculos no ERncontrados
        /// </exception>
        private CompanyPolicy ChangePolicyHolderPolicy(CompanyPolicy companyPolicy)
        {
            try
            {
                if (companyPolicy == null)
                {
                    throw new Exception("Poliza Vacia");
                }

                var originalCurrentFrom = companyPolicy.CurrentFrom;
                companyPolicy.CurrentFrom = companyPolicy.Endorsement.CurrentFrom;
                companyPolicy.Endorsement.CancelationCurrentFrom = companyPolicy.Endorsement.CurrentFrom;
                companyPolicy.Endorsement.CancelationCurrentTo = companyPolicy.Endorsement.CurrentTo;
                companyPolicy.Endorsement.CancellationTypeId = originalCurrentFrom == companyPolicy.CurrentFrom ? 1 : 0;

                List<PoliciesAut> riskInfringementPolicies = new List<PoliciesAut>();
                List<SEM.CompanyLiabilityRisk> companyLiabilitys = new List<SEM.CompanyLiabilityRisk>();
                if (companyPolicy.Id == 0)
                {
                    companyPolicy.TemporalType = TemporalType.Endorsement;
                    companyPolicy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(TemporalType.Endorsement);
                    companyLiabilitys = DelegateService.LiabilityService.GetCompanyLiebilitiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                }
                else
                {
                    companyLiabilitys = DelegateService.LiabilityService.GetCompanyLiabilitiesByTemporalId(companyPolicy.Id);
                }

                var risks = DelegateService.ciaChangePolicyHolderEndorsement.QuotateChangePolicyHolderCia(companyPolicy);
                companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);
                if (companyLiabilitys != null && companyLiabilitys.Count > 0 && risks != null)
                {
                    ConcurrentBag<string> errors = new ConcurrentBag<string>();
                    companyLiabilitys.Where(a => a != null).AsParallel().ForAll(
                    z =>
                    {
                        var companyRisk = risks.FirstOrDefault(x => x.RiskId == z.Risk.RiskId);
                        if (companyRisk != null)
                        {
                            z.Risk.Policy = companyPolicy;
                            z.Risk.RiskId = companyRisk.RiskId;
                            z.Risk.Number = companyRisk.Number;
                            z.Risk.Status = companyRisk.Status;
                            companyRisk.Coverages.ForEach(x => x.Deductible = z.Risk.Coverages.FirstOrDefault(y => y.Id == x.Id).Deductible);
                            z.Risk.Coverages = companyRisk.Coverages;
                            z.Risk.AmountInsured = companyRisk.AmountInsured;
                            z.Risk.Premium = companyRisk.Coverages.Where(m => m != null).Sum(x => x.PremiumAmount);
                        }
                        else
                        {
                            errors.Add("Error Riesgo no encontrado");
                        }
                    }
                    );
                    companyLiabilitys.AsParallel().ForAll(item =>
                    {
                        item = DelegateService.LiabilityModificationService.GetDataModification(item, CoverageStatusType.Included);
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
                        });
                        item = DelegateService.LiabilityService.CreateLiabilityTemporal(item, false);
                    });

                    if (companyLiabilitys != null && companyLiabilitys.Select(x => x.Risk).Any())
                    {
                        riskInfringementPolicies.AddRange(companyLiabilitys.Select(x => x.Risk).SelectMany(x => x.InfringementPolicies).Where(m => m != null).ToList());
                        companyPolicy = baseBusinessCia.CalculatePolicyAmounts(companyPolicy, companyLiabilitys.Select(x => x.Risk).ToList());
                        companyPolicy.CurrentFrom = originalCurrentFrom;
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
        /// Creates the endorsement change Coinsurance.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        public List<CompanyPolicy> CreateEndorsementChangePolicyHolder(CompanyChangePolicyHolder companyChangePolicyHolder, bool clearPolicies)
        {
            try
            {
                List<CompanyPolicy> companyPolicies = new List<CompanyPolicy>();
                var TempId = companyChangePolicyHolder.Endorsement.TemporalId;
                var companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(TempId, false);

                if (!clearPolicies)
                {
                    List<CompanyLiabilityRisk> Liabilitys = DelegateService.LiabilityService.GetCompanyLiabilitiesByTemporalId(companyPolicy.Id);

                    Liabilitys.ForEach(x => companyPolicy.InfringementPolicies.AddRange(x.Risk.InfringementPolicies));
                    if (companyPolicy.InfringementPolicies.Any(x => x.Type == ENUMPOLICIES.TypePolicies.Authorization || x.Type == ENUMPOLICIES.TypePolicies.Restrictive))
                    {
                        companyPolicy.InfringementPolicies = companyPolicy.InfringementPolicies.Where(x => x.Type == ENUMPOLICIES.TypePolicies.Authorization || x.Type == ENUMPOLICIES.TypePolicies.Restrictive).ToList();
                        companyPolicies.Add(companyPolicy);
                        return companyPolicies;
                    }
                }

                var originalCurrentFrom = companyChangePolicyHolder.Endorsement.CurrentFrom;
                companyChangePolicyHolder.Endorsement.CancelationCurrentFrom = companyPolicy.Endorsement.CurrentFrom;
                companyChangePolicyHolder.Endorsement.CancelationCurrentTo = companyPolicy.Endorsement.CurrentTo;
                companyChangePolicyHolder.Endorsement.CurrentFrom = companyPolicy.CurrentFrom;
                companyChangePolicyHolder.Endorsement.CurrentTo = companyPolicy.CurrentTo;
                companyChangePolicyHolder.Endorsement.CancellationTypeId = companyPolicy.Endorsement.CancellationTypeId;

                var cancelation = CancellationPolicy(companyChangePolicyHolder.Endorsement);
                companyChangePolicyHolder.Endorsement.CurrentFrom = originalCurrentFrom;
                companyChangePolicyHolder.Endorsement.TemporalId = TempId;
                try
                {
                    companyChangePolicyHolder.Endorsement.Id = cancelation.Id;
                    var createEndorsementChangePolicyHolder = CreateLiabilityEndorsementChangePolicyHolder(companyChangePolicyHolder);
                    companyPolicies.Add(createEndorsementChangePolicyHolder);
                    companyPolicies.Add(cancelation);
                    return companyPolicies;
                }
                catch (Exception)
                {
                    DelegateService.underwritingService.DeleteEndorsementByPolicyIdEndorsementIdEndorsementType(cancelation.Endorsement.PolicyId, cancelation.Endorsement.Id, EndorsementType.Cancellation);
                    throw new Exception("Error Creando Endoso Cambio de Afianzado");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Creates the Liability endorsement change Coinsurance.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        private CompanyPolicy CreateLiabilityEndorsementChangePolicyHolder(CompanyChangePolicyHolder companyChangePolicyHolder)
        {
            var companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyChangePolicyHolder.Endorsement.TemporalId, false);
            List<CompanyLiabilityRisk> Liabilitys = DelegateService.LiabilityService.GetCompanyLiabilitiesByTemporalId(companyPolicy.Id);
            companyPolicy.InfringementPolicies = new List<PoliciesAut>();
            Liabilitys.ForEach(x => x.Risk.InfringementPolicies = new List<PoliciesAut>());
            companyPolicy.CurrentFrom = companyChangePolicyHolder.Endorsement?.CurrentFrom ?? companyPolicy.CurrentFrom;
            return DelegateService.LiabilityService.CreateEndorsement(companyPolicy, Liabilitys);
        }

        /// <summary>
        /// Cancellations the policy.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        private CompanyPolicy CancellationPolicy(CompanyEndorsement companyEndorsement)
        {
            companyEndorsement.TemporalId = 0;
            companyEndorsement.EndorsementReasonId = 30;
            var date = companyEndorsement.CurrentTo - companyEndorsement.CurrentFrom;
            companyEndorsement.EndorsementDays = date.Days;
            var companyPolicy = DelegateService.endorsementLiabilityCancellationService.CreateTemporalEndorsementCancellation(companyEndorsement);
            var Liabilitys = DelegateService.LiabilityService.GetCompanyLiabilitiesByTemporalId(companyPolicy.Id);
            companyPolicy.InfringementPolicies = new List<PoliciesAut>();
            Liabilitys.ForEach(x => x.Risk.InfringementPolicies = new List<PoliciesAut>());
            return DelegateService.LiabilityService.CreateEndorsement(companyPolicy, Liabilitys);
        }

    }
}
