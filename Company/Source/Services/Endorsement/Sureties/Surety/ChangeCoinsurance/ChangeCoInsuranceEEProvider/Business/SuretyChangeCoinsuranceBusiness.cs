using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.Sureties.SuretyServices.Models;
using Sistran.Company.Application.SuretyChangeCoinsuranceService.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using ENUMPOLICIES = Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using SEM = Sistran.Company.Application.Sureties.SuretyServices.Models;
using TP = Sistran.Core.Application.Utilities.Utility;
using Sistran.Core.Services.UtilitiesServices.Enums;

namespace Sistran.Company.Application.SuretyChangeCoinsuranceService.EEProvider.Business
{
    public class SuretyChangeCoinsuranceBusinessCia
    {
        BaseBusinessCia baseBusinessCia;
        /// <summary>
        /// Initializes a new instance of the <see cref="SuretyChangeCoinsuranceBusinessCia"/> class.
        /// </summary>
        public SuretyChangeCoinsuranceBusinessCia()
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
                companyPolicyBase.Endorsement.EndorsementType = EndorsementType.ChangeCoinsuranceEndorsement;
                CompanyPolicy companyPolicy = new CompanyPolicy();

                if (companyPolicyBase.Endorsement.TemporalId != 0)
                {
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPolicyBase.Endorsement.TemporalId, false);
                }
                else
                {
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyPolicyBase.Endorsement.Id);
                    companyPolicy.Summary = new CompanySummary
                    {
                        RiskCount = 0
                    };
                }
                if (companyPolicy != null)
                {
                    if ((companyPolicy.CurrentFrom != Convert.ToDateTime(companyPolicyBase.Endorsement.CurrentFrom))
                       || (companyPolicy.Agencies != companyPolicyBase.Agencies))
                    {
                        companyPolicy.UserId = BusinessContext.Current?.UserId ?? companyPolicyBase.UserId;
                        companyPolicy.TemporalType = TemporalType.Endorsement;
                        companyPolicy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(TemporalType.Endorsement);
                        companyPolicy.CurrentFrom = companyPolicyBase.CurrentFrom;
                        companyPolicy.CurrentTo = companyPolicyBase.CurrentTo;
                        companyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);
                        companyPolicy.BeginDate = companyPolicyBase.Endorsement.CurrentFrom;
                        
                        companyPolicy.Endorsement.CancelationCurrentFrom = companyPolicyBase.Endorsement.CancelationCurrentFrom;
                        companyPolicy.Endorsement.CancelationCurrentTo = companyPolicyBase.Endorsement.CancelationCurrentTo;
                        companyPolicy.Endorsement.CurrentFrom = companyPolicyBase.Endorsement.CurrentFrom;
                        companyPolicy.Endorsement.CurrentTo = companyPolicyBase.Endorsement.CurrentTo;
                        companyPolicy.Endorsement.EndorsementDays = companyPolicyBase.Endorsement.EndorsementDays;
                        companyPolicy.Endorsement.EndorsementType = companyPolicyBase.Endorsement.EndorsementType;
                        companyPolicy.Endorsement.IssueDate = companyPolicyBase.Endorsement.IssueDate;
                        companyPolicy.Endorsement.TicketDate = companyPolicyBase.Endorsement.TicketDate;
                        companyPolicy.Endorsement.TicketNumber = companyPolicyBase.Endorsement.TicketNumber;
                        companyPolicy.Endorsement.UserId = companyPolicyBase.Endorsement.UserId;
                        companyPolicy.Endorsement.EndorsementReasonId = companyPolicyBase.Endorsement.EndorsementReasonId;
                        companyPolicy.Endorsement.PrevPolicyId = companyPolicyBase.Endorsement.PrevPolicyId;
                        companyPolicy.Endorsement.Text = companyPolicyBase.Endorsement.Text;
                        companyPolicy.Endorsement.OnlyCancelation = companyPolicyBase.Endorsement.OnlyCancelation;
                        companyPolicy.Endorsement.ExchangeRate = companyPolicyBase.Endorsement.ExchangeRate;

                        companyPolicy.CoInsuranceCompanies = companyPolicyBase.CoInsuranceCompanies;
                        companyPolicy.Holder = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(companyPolicy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();
                        companyPolicy = ChangeCoinsurancePolicy(companyPolicy);
                        return companyPolicy;
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
        private CompanyPolicy ChangeCoinsurancePolicy(CompanyPolicy companyPolicy)
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
                List<SEM.CompanyContract> companySuretys = new List<SEM.CompanyContract>();
                if (companyPolicy.Id == 0)
                {
                    companyPolicy.TemporalType = TemporalType.Endorsement;
                    companyPolicy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(TemporalType.Endorsement);
                    companyPolicy.Summary = new CompanySummary
                    {
                        RiskCount = 0
                    };
                    companySuretys = DelegateService.suretyService.GetCompanySuretiesByPolicyId(companyPolicy.Endorsement.PolicyId);

                }
                else
                {
                    companySuretys = DelegateService.suretyService.GetCompanySuretiesByTemporalId(companyPolicy.Id);
                }
                var risks = DelegateService.ciaChangeCoinsuranceEndorsement.QuotateChangeCoinsuranceCia(companyPolicy);
                companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);
                if (companySuretys != null && companySuretys.Count > 0 && risks != null)
                {
                    ConcurrentBag<string> errors = new ConcurrentBag<string>();
                    companySuretys.Where(a => a != null).AsParallel().ForAll(
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

                            if (companyRisk.Coverages != null && companyRisk.Coverages.Count > 0)
                            {
                                z.Risk.AmountInsured = companyRisk.Coverages.Where(m => m != null).Sum(x => x.LimitAmount);
                            }
                        }
                        else
                        {
                            errors.Add("Error Riesgo no encontrado");
                        }
                    }
                    );
                    companySuretys.AsParallel().ForAll(item =>
                    {
                        item = DelegateService.suretyModificationService.GetDataModification(item, CoverageStatusType.Included);
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
                        DelegateService.suretyService.CreateSuretyTemporal(item, false);
                    });

                    if (companySuretys != null && companySuretys.Select(x => x.Risk).Any())
                    {
                        riskInfringementPolicies.AddRange(companySuretys.Select(x => x.Risk).SelectMany(x => x.InfringementPolicies).Where(m => m != null).ToList());
                        companyPolicy = baseBusinessCia.CalculatePolicyAmounts(companyPolicy, companySuretys.Select(x => x.Risk).ToList());
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
        public List<CompanyPolicy> CreateEndorsementChangeCoinsurance(CompanyEndorsement companyEndorsement, bool clearPolicies)
        {
            try
            {
                List<CompanyPolicy> companyPolicies = new List<CompanyPolicy>();
                var TempId = companyEndorsement.TemporalId;
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(TempId, false);

                if (!clearPolicies)
                {
                    List<CompanyContract> suretys = DelegateService.suretyService.GetCompanySuretiesByTemporalId(companyPolicy.Id);

                    suretys.ForEach(x => companyPolicy.InfringementPolicies.AddRange(x.Risk.InfringementPolicies));
                    if (companyPolicy.InfringementPolicies.Any(x => x.Type == ENUMPOLICIES.TypePolicies.Authorization || x.Type == ENUMPOLICIES.TypePolicies.Restrictive))
                    {
                        companyPolicy.InfringementPolicies = companyPolicy.InfringementPolicies.Where(x => x.Type == ENUMPOLICIES.TypePolicies.Authorization || x.Type == ENUMPOLICIES.TypePolicies.Restrictive).ToList();
                        companyPolicies.Add(companyPolicy);
                        return companyPolicies;
                    }
                }

                var originalCurrentFrom = companyEndorsement.CurrentFrom;
                companyEndorsement.CancelationCurrentFrom = companyPolicy.Endorsement.CurrentFrom;
                companyEndorsement.CancelationCurrentTo = companyPolicy.Endorsement.CurrentTo;
                companyEndorsement.CurrentFrom = companyPolicy.CurrentFrom;
                companyEndorsement.CurrentTo = companyPolicy.CurrentTo;
                companyEndorsement.CancellationTypeId = companyPolicy.Endorsement.CancellationTypeId;

                var cancelation = CancellationPolicy(companyEndorsement);
                companyEndorsement.CurrentFrom = originalCurrentFrom;
                companyEndorsement.TemporalId = TempId;
                try
                {
                    companyEndorsement.Id = cancelation.Id;
                    var createEndorsementChangeCoinsurance = CreateSuretyEndorsementChangeCoinsurance(companyEndorsement);
                    companyPolicies.Add(createEndorsementChangeCoinsurance);
                    companyPolicies.Add(cancelation);
                    return companyPolicies;
                }
                catch (Exception)
                {
                    DelegateService.underwritingService.DeleteEndorsementByPolicyIdEndorsementIdEndorsementType(cancelation.Endorsement.PolicyId, cancelation.Endorsement.Id, EndorsementType.Cancellation);
                    throw new Exception("Error Creando Endoso Cambio Coinsurancee");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Creates the surety endorsement change Coinsurance.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        private CompanyPolicy CreateSuretyEndorsementChangeCoinsurance(CompanyEndorsement companyEndorsement)
        {
            var companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyEndorsement.TemporalId, false);
            List<CompanyContract> suretys = DelegateService.suretyService.GetCompanySuretiesByTemporalId(companyPolicy.Id);
            companyPolicy.InfringementPolicies = new List<PoliciesAut>();
            suretys.ForEach(x => x.Risk.InfringementPolicies = new List<PoliciesAut>());

            companyPolicy.CurrentFrom = companyEndorsement?.CurrentFrom ?? companyPolicy.CurrentFrom;
            return DelegateService.suretyService.CreateEndorsement(companyPolicy, suretys);
        }

        /// <summary>
        /// Cancellations the policy.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        private CompanyPolicy CancellationPolicy(CompanyEndorsement companyEndorsement)
        {
            companyEndorsement.TemporalId = 0;
            companyEndorsement.EndorsementReasonId = 32;
            var date = companyEndorsement.CurrentTo - companyEndorsement.CurrentFrom;
            companyEndorsement.EndorsementDays = date.Days;
            var companyPolicy = DelegateService.endorsementSuretyCancellationService.CreateTemporalEndorsementCancellation(companyEndorsement);
            var suretys = DelegateService.suretyService.GetCompanySuretiesByTemporalId(companyPolicy.Id);
            companyPolicy.InfringementPolicies = new List<PoliciesAut>();
            suretys.ForEach(x => x.Risk.InfringementPolicies = new List<PoliciesAut>());
            return DelegateService.suretyService.CreateEndorsement(companyPolicy, suretys);
        }
    }
}
