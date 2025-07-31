using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.JudicialSuretyChangeAgentService.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Services.UtilitiesServices.Enums;
using ENUMPOLICIES = Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using JSEM = Sistran.Company.Application.Sureties.JudicialSuretyServices.Models;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.JudicialSuretyChangeAgentService.EEProvider.Business
{

    public class SuretyChangeAgentBusinessCompany
    {
        BaseBusinessCia baseBusinessCompany;
        /// <summary>
        /// Initializes a new instance of the <see cref="SuretyChangeAgentBusinessCompany"/> class.
        /// </summary>
        public SuretyChangeAgentBusinessCompany()
        {
            baseBusinessCompany = new BaseBusinessCia();
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
                    //companyPolicy.Id = 0;
                    //companyPolicy.Endorsement.TemporalId = 0;
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
                        companyPolicy.UserId = companyPolicyBase.UserId;
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

                        companyPolicy.Agencies = companyPolicyBase.Agencies;
                        companyPolicy.Holder = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(companyPolicy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();

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
                    throw new Exception(Errors.ErrorPolicyEmpty);
                }

                var originalCurrentFrom = companyPolicy.CurrentFrom;
                companyPolicy.CurrentFrom = companyPolicy.Endorsement.CurrentFrom;
                companyPolicy.Endorsement.CancelationCurrentFrom = companyPolicy.Endorsement.CurrentFrom;
                companyPolicy.Endorsement.CancelationCurrentTo = companyPolicy.Endorsement.CurrentTo;
                companyPolicy.Endorsement.CancellationTypeId = originalCurrentFrom == companyPolicy.CurrentFrom ? 1 : 0;

                List<PoliciesAut> riskInfringementPolicies = new List<PoliciesAut>();
                List<JSEM.CompanyJudgement> companySuretys = new List<JSEM.CompanyJudgement>();
                if (companyPolicy.Id == 0)
                {
                    companyPolicy.TemporalType = TemporalType.Endorsement;
                    companyPolicy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(TemporalType.Endorsement);
                    companyPolicy.Summary = new CompanySummary
                    {
                        RiskCount = 0
                    };
                    companySuretys = DelegateService.judicialsuretyService.GetCompanyJudicialSuretyByPolicyId(companyPolicy.Endorsement.PolicyId);
                    companySuretys.AsParallel().ForAll(
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
                    //companySuretys = DelegateService.judicialsuretyService.GetCompanyJudgementsByTemporalId(companyPolicy.Id);
                    //if (companySuretys == null && companySuretys.Count < 1)
                    //{
                    companySuretys = DelegateService.judicialsuretyService.GetCompanyJudicialSuretyByPolicyId(companyPolicy.Endorsement.PolicyId);
                    //}
                }
                var risks = DelegateService.changeAgentEndorsementService.QuotateChangeAgentCia(companyPolicy);
                companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);
                if (companyPolicy != null && companySuretys?.Count > 0 && risks?.Count > 0)
                {
                    ConcurrentBag<string> errors = new ConcurrentBag<string>();
                    companySuretys.Where(a => a != null).AsParallel().ForAll(
                    z =>
                    {
                        var companyRisk = risks.FirstOrDefault(x => x.Number == z.Risk.Number);
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
                            errors.Add(Errors.ErrorRiskNotFound);
                        }
                    }
                    );
                    companySuretys.AsParallel().ForAll(item =>
                    {
                        try
                        {
                            item = DelegateService.judicialsuretyModificationService.GetDataModification(item, CoverageStatusType.Included);
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
                                    coverage.EndorsementLimitAmount = coverage.DeclaredAmount;
                                    coverage.EndorsementSublimitAmount = coverage.DeclaredAmount;
                                }
                            });
                            DelegateService.judicialsuretyService.CreateJudgementTemporal(item, false);
                        }
                        catch (Exception ex)
                        {
                            errors.Add(ex.Message);
                        }
                    });

                    if (companySuretys != null && companySuretys.Select(x => x.Risk).Any())
                    {
                        riskInfringementPolicies.AddRange(companySuretys.Select(x => x.Risk).SelectMany(x => x.InfringementPolicies).Where(m => m != null).ToList());
                        companyPolicy = baseBusinessCompany.CalculatePolicyAmounts(companyPolicy, companySuretys.Select(x => x.Risk).ToList());
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
                    throw new Exception(Errors.ErrorRiskNotFound);
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
        public List<CompanyPolicy> CreateEndorsementChangeAgent(CompanyEndorsement companyEndorsement, bool clearPolicies)
        {
            try
            {
                List<CompanyPolicy> companyPolicies = new List<CompanyPolicy>();
                var TempId = companyEndorsement.TemporalId;
                var companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyEndorsement.TemporalId, false);

                if (!clearPolicies)
                {
                    List<JSEM.CompanyJudgement> suretys = DelegateService.judicialsuretyService.GetCompanyJudgementsByTemporalId(companyPolicy.Id);

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
                    companyEndorsement.Id = cancelation.Endorsement.Id;
                    var createEndorsementChangeAgent = CreateSuretyEndorsementChangeAgent(companyEndorsement);
                    companyPolicies.Add(createEndorsementChangeAgent);
                    companyPolicies.Add(cancelation);
                    return companyPolicies;
                }
                catch (Exception)
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
        /// Creates the surety endorsement change agent.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        private CompanyPolicy CreateSuretyEndorsementChangeAgent(CompanyEndorsement companyEndorsement)
        {
            var companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyEndorsement.TemporalId, false);
            List<JSEM.CompanyJudgement> suretys = DelegateService.judicialsuretyService.GetCompanyJudgementsByTemporalId(companyPolicy.Id);
            companyPolicy.InfringementPolicies = new List<PoliciesAut>();
            suretys.ForEach(x => x.Risk.InfringementPolicies = new List<PoliciesAut>());
            companyPolicy.CurrentFrom = companyEndorsement?.CurrentFrom ?? companyPolicy.CurrentFrom;
            return DelegateService.judicialsuretyService.CreateEndorsement(companyPolicy, suretys);

        }

        /// <summary>
        /// Cancellations the policy.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        private CompanyPolicy CancellationPolicy(CompanyEndorsement companyEndorsement)
        {
            companyEndorsement.TemporalId = 0;
            companyEndorsement.EndorsementReasonId = 29;
            var date = companyEndorsement.CurrentTo - companyEndorsement.CurrentFrom;
            companyEndorsement.EndorsementDays = date.Days;
            var companyPolicy = DelegateService.endorsementSuretyCancellationService.CreateTemporalEndorsementCancellation(companyEndorsement);
            var suretys = DelegateService.judicialsuretyService.GetCompanyJudgementsByTemporalId(companyPolicy.Id);
            companyPolicy.InfringementPolicies = new List<PoliciesAut>();
            TP.Parallel.ForEach(companyPolicy.Agencies.ToList(), item =>
            {
                item.Commissions[0].Percentage = 0;
            });
            suretys.ForEach(x => x.Risk.InfringementPolicies = new List<PoliciesAut>());
            return DelegateService.judicialsuretyService.CreateEndorsement(companyPolicy, suretys);
        }
    }
}
