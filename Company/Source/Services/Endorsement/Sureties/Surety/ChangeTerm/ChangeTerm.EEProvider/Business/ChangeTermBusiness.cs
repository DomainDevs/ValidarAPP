using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.SuretyChangeTermService.EEProvider.Resources;
using Sistran.Company.Application.SuretyChangeTermService.EEProvider.Services;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CancellationEndorsement.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using CSEM = Sistran.Company.Application.Sureties.SuretyServices.Models;
using ENUMPOLICIES = Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using TP = Sistran.Core.Application.Utilities.Utility;
using Sistran.Core.Services.UtilitiesServices.Enums;

namespace Sistran.Company.Application.SuretyChangeTermService.EEProvider.Business
{
    public class SuretyChangeTermBusinessCompany
    {
        BaseBusinessCia baseBusinessCia;

        /// <summary>
        /// Initializes a new instance of the <see cref="SuretyChangeTermBusinessCompany"/> class.
        /// </summary>
        public SuretyChangeTermBusinessCompany()
        {
            baseBusinessCia = new BaseBusinessCia();
        }


        /// <summary>
        /// Creates the temporal.
        /// </summary>
        /// <param name="companyPolicyBase">The company policy base.</param>
        /// <param name="isMassive">if set to <c>true</c> [is massive].</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// Datos de la Poliza no Enviados
        /// or
        /// Poliza No encontrada
        /// </exception>
        public CompanyPolicy CreateTemporal(CompanyPolicy companyPolicyBase, bool isMassive = false)
        {
            try
            {
                if (companyPolicyBase == null)
                {
                    throw new Exception(Errors.LabelWithoutData);
                }
                companyPolicyBase.Endorsement.EndorsementType = EndorsementType.ChangeTermEndorsement;
                CompanyPolicy companyPolicy = new CompanyPolicy();

                if (companyPolicyBase.Endorsement.TemporalId != 0)
                {
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPolicyBase.Endorsement.TemporalId, false);
                }
                else
                {
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyPolicyBase.Endorsement.Id);
                    companyPolicy.Id = 0;
                    companyPolicy.Summary = new CompanySummary
                    {
                        RiskCount = 0
                    };
                }
                if (companyPolicy != null)
                {
                    if (companyPolicy.CurrentFrom != companyPolicyBase.CurrentFrom)
                    {
                        companyPolicy.UserId = BusinessContext.Current?.UserId ?? companyPolicyBase.UserId;
                        companyPolicy.TemporalType = TemporalType.Endorsement;
                        companyPolicy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(TemporalType.Endorsement);
                        int daysDifference = (companyPolicyBase.CurrentFrom - companyPolicy.CurrentFrom).Days;
                        companyPolicy.CurrentFrom = companyPolicyBase.CurrentFrom;
                        companyPolicy.CurrentTo = companyPolicyBase.CurrentTo;
                        companyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);
                        companyPolicy.BeginDate = companyPolicyBase.CurrentFrom;
                        
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

                        companyPolicy.Holder = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(companyPolicy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();
                        companyPolicy = ChangeTermPolicy(companyPolicy, daysDifference);
                        return companyPolicy;
                    }

                    return null;
                }
                else
                {
                    throw new Exception(Errors.PolicyNotFound);
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
        /// <exception cref="Exception">Poliza Vacia
        /// or
        /// or
        /// Vehiculos no ERncontrados</exception>
        private CompanyPolicy ChangeTermPolicy(CompanyPolicy companyPolicy, int daysDifference)
        {
            try
            {
                if (companyPolicy == null)
                {
                    throw new Exception(Errors.ErrorPolicyNoExist);
                }

                companyPolicy.Endorsement.CancelationCurrentFrom = companyPolicy.Endorsement.CurrentFrom;
                companyPolicy.Endorsement.CancelationCurrentTo = companyPolicy.Endorsement.CurrentTo;
                companyPolicy.Endorsement.CancellationTypeId = (int)CancellationType.BeginDate;

                List<PoliciesAut> riskInfringementPolicies = new List<PoliciesAut>();
                List<CSEM.CompanyContract> companySuretys = new List<CSEM.CompanyContract>();
                if (companyPolicy.Id == 0)
                {
                    companyPolicy.Summary = new CompanySummary
                    {
                        RiskCount = 0
                    };
                    companySuretys = DelegateService.suretyService.GetCompanySuretiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                    if (companySuretys == null || companySuretys.Count < 1)
                    {
                        throw new Exception(Errors.NoExistRisk);
                    }
                    companySuretys.AsParallel().ForAll(x => x.Risk.Id = 0);
                }
                else
                {
                    companySuretys = DelegateService.suretyService.GetCompanySuretiesByTemporalId(companyPolicy.Id);
                }
                var risks = DelegateService.changeTermEndorsementService.QuotateChangeTermCia(companyPolicy);
                if (companySuretys != null && companySuretys.Count > 0 && risks != null)
                {
                    companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);
                    if (companyPolicy != null)
                    {
                        ConcurrentBag<string> errors = new ConcurrentBag<string>();
                        companySuretys.Where(a => a != null).AsParallel().WithDegreeOfParallelism(ParallelHelper.DebugParallelFor().MaxDegreeOfParallelism).ForAll(
                        z =>
                        {
                            var companyRisk = risks.FirstOrDefault(x => x.RiskId == z.Risk.RiskId);
                            if (companyRisk != null)
                            {
                                z.Risk.Policy = companyPolicy;
                                z.Risk.RiskId = companyRisk.RiskId;
                                z.Risk.Number = companyRisk.Number;
                                z.Risk.Status = companyRisk.Status;
                                companyRisk.Coverages.ForEach(x => x.CurrentFrom = x.CurrentFromOriginal.AddDays(daysDifference));
                                companyRisk.Coverages.ForEach(x => x.CurrentTo = x.CurrentToOriginal.AddDays(daysDifference));
                                z.Risk.Coverages = companyRisk.Coverages;
                                z.Risk.Premium = companyRisk.Coverages.Where(m => m != null).Sum(x => x.PremiumAmount);
                                z.Risk.AmountInsured = companyRisk.Coverages.Where(m => m != null).Sum(y => y.LimitAmount);
                                z = DelegateService.suretyModificationService.GetDataModification(z, CoverageStatusType.Included);
                                z.Risk.Status = RiskStatusType.Included;
                                var coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(companyPolicy.Product.Id, z.Risk.GroupCoverage.Id, companyPolicy.Prefix.Id);
                                if (coverages.Count < z.Risk.Coverages.Count)
                                {
                                    throw new Exception(Errors.ErrorGetGroupCoverages);
                                }
                                TP.Parallel.ForEach(z.Risk.Coverages, coverage =>
                                {
                                    if (coverages != null && coverages.Where(x => x.Id == coverage.Id).FirstOrDefault() != null)
                                    {
                                        coverage.SubLineBusiness = coverages.First(x => x.Id == coverage.Id).SubLineBusiness;
                                        coverage.Number = coverages.First(x => x.Id == coverage.Id).Number;
                                        coverage.EndorsementLimitAmount = coverage.DeclaredAmount;
                                        coverage.EndorsementSublimitAmount = coverage.DeclaredAmount;
                                    }
                                });
                                z = DelegateService.suretyService.CreateSuretyTemporal(z, false);
                            }
                            else
                            {
                                throw new Exception(Errors.NoExistRisk);

                            }
                        }
                        );
                    }
                    else
                    {
                        throw new Exception(Errors.ErrorCreateTemporary);
                    }
                    if (companySuretys != null && companySuretys.Select(x => x.Risk).Any())
                    {

                        riskInfringementPolicies.AddRange(companySuretys.Select(x => x.Risk).SelectMany(x => x.InfringementPolicies).Where(m => m != null).ToList());

                        companyPolicy = baseBusinessCia.CalculatePolicyAmounts(companyPolicy, companySuretys.Select(x => x.Risk).ToList());

                        companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);
                        if (companyPolicy != null)
                        {
                            companyPolicy.InfringementPolicies.AddRange(riskInfringementPolicies);

                            return companyPolicy;
                        }
                        else
                        {
                            throw new Exception(Errors.ErrorCreateTemporary);
                        }
                    }
                    else
                    {
                        throw new Exception(Errors.UnquotedRisks);
                    }

                }
                else
                {
                    throw new Exception(Errors.ErrorSuretyNotFound);
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
        /// <param name="clearPolicies">If validate Policies</param>
        /// <returns></returns>
        /// <exception cref="Exception">Error Creando Endoso Cambio Agente</exception>
        public List<CompanyPolicy> CreateEndorsementChangeTerm(CompanyEndorsement companyEndorsement, bool clearPolicies)
        {
            if (companyEndorsement == null)
            {
                throw new ArgumentException(Errors.ErrorEndorsement);
            }
            try
            {
                List<CompanyPolicy> companyPolicies = new List<CompanyPolicy>();
                int tempId = companyEndorsement.TemporalId;
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(tempId, false);

                if (!clearPolicies)
                {
                    if (companyPolicy != null)
                    {
                        List<CSEM.CompanyContract> suretys = DelegateService.suretyService.GetCompanySuretiesByTemporalId(companyPolicy.Id);
                        suretys.ForEach(x => companyPolicy.InfringementPolicies.AddRange(x.Risk.InfringementPolicies));
                        if (companyPolicy.InfringementPolicies.Any(x => x.Type == ENUMPOLICIES.TypePolicies.Authorization || x.Type == ENUMPOLICIES.TypePolicies.Restrictive))
                        {
                            companyPolicy.InfringementPolicies = companyPolicy.InfringementPolicies.Where(x => x.Type == ENUMPOLICIES.TypePolicies.Authorization || x.Type == ENUMPOLICIES.TypePolicies.Restrictive).ToList();
                            companyPolicies.Add(companyPolicy);
                            return companyPolicies;
                        }
                    }
                    else
                    {
                        throw new Exception(Errors.ErrorTemporalNotFound);
                    }
                }


                companyEndorsement.CancelationCurrentFrom = companyPolicy.Endorsement.CurrentFrom;
                companyEndorsement.CancelationCurrentTo = companyPolicy.Endorsement.CurrentTo;
                companyEndorsement.CurrentFrom = companyPolicy.CurrentFrom;
                companyEndorsement.CurrentTo = companyPolicy.CurrentTo;
                companyEndorsement.CancellationTypeId = companyPolicy.Endorsement.CancellationTypeId;

                var cancelation = CancellationPolicy(companyEndorsement);
                companyEndorsement.TemporalId = tempId;
                try
                {
                    companyEndorsement.Id = cancelation.Endorsement.Id;
                    var createEndorsementChangeTerm = CreateSuretyEndorsementChangeTerm(companyEndorsement);
                    companyPolicies.Add(createEndorsementChangeTerm);
                    companyPolicies.Add(cancelation);
                    return companyPolicies;
                }
                catch (Exception)
                {
                    DelegateService.underwritingService.DeleteEndorsementByPolicyIdEndorsementIdEndorsementType(cancelation.Endorsement.PolicyId, cancelation.Endorsement.Id, EndorsementType.Cancellation);
                    throw new Exception(Errors.ErrorCreateEndorsement);
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
        private CompanyPolicy CreateSuretyEndorsementChangeTerm(CompanyEndorsement companyEndorsement)
        {
            var companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyEndorsement.TemporalId, false);
            if (companyPolicy != null)
            {
                List<CSEM.CompanyContract> suretys = DelegateService.suretyService.GetCompanySuretiesByTemporalId(companyPolicy.Id);
                if (suretys != null && suretys.Any())
                {
                    companyPolicy.InfringementPolicies = new List<PoliciesAut>();
                    suretys.ForEach(x => x.Risk.InfringementPolicies = new List<PoliciesAut>());
                    companyPolicy.CurrentFrom = companyEndorsement?.CurrentFrom ?? companyPolicy.CurrentFrom;
                    return DelegateService.suretyService.CreateEndorsement(companyPolicy, suretys);
                }
                else
                {
                    throw new Exception(Errors.ErrorCreatePolicy);
                }
            }
            else
            {
                throw new Exception(Errors.ErrorTemporalNotFound);
            }
        }

        /// <summary>
        /// Cancellations the policy.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        private CompanyPolicy CancellationPolicy(CompanyEndorsement companyEndorsement)
        {
            companyEndorsement.TemporalId = 0;
            companyEndorsement.CancellationTypeId = (int)CancellationType.BeginDate;
            companyEndorsement.EndorsementReasonId = 20;
            var date = companyEndorsement.CurrentTo - companyEndorsement.CurrentFrom;
            companyEndorsement.EndorsementDays = date.Days;
            var companyPolicy = DelegateService.endorsementSuretyCancellationService.CreateTemporalEndorsementCancellation(companyEndorsement);
            var suretys = DelegateService.suretyService.GetCompanySuretiesByTemporalId(companyPolicy.Id);
            companyPolicy.InfringementPolicies = new List<PoliciesAut>();
            suretys.ForEach(x => x.Risk.InfringementPolicies = new List<PoliciesAut>());
            return DelegateService.suretyService.CreateEndorsement(companyPolicy, suretys);
        }


        //BaseBusinessCia provider;

        ///// <summary>
        ///// Initializes a new instance of the <see cref="SuretyEndorsementBusinessCia" /> class.
        ///// </summary>
        //public SuretyChangeTermBusinessCompany()
        //{
        //    provider = new BaseBusinessCia();
        //}

        ///// <summary>
        ///// Creates the endorsement.
        ///// </summary>
        ///// <param name="companyEndorsement">The company endorsement.</param>
        ///// <returns></returns>
        ///// <exception cref="ArgumentException">Endoso Vacio</exception>
        //public CompanyPolicy CreateEndorsementChangeTerm(CompanyEndorsement companyEndorsement)
        //{
        //    if (companyEndorsement == null)
        //    {
        //        throw new ArgumentException(Errors.ErrorEndorsement);
        //    }
        //    try
        //    {
        //        CompanyPolicy policy = new CompanyPolicy();
        //        CTSE.CiaChangeTermEndorsementEEProvider CiaChangeTermEndorsementEEProvider = new CTSE.CiaChangeTermEndorsementEEProvider();
        //        policy = CiaChangeTermEndorsementEEProvider.CreateCiaChangeTerms(companyEndorsement);
        //        List<SEM.CompanyContract> companySuretys = null;
        //        companySuretys = DelegateService.suretyService.GetCompanySuretiesByPolicyId(companyEndorsement.PolicyId);
        //        if (companySuretys != null && companySuretys.Count > 0)
        //        {
        //            List<CompanyRisk> risks = new List<CompanyRisk>();
        //            companySuretys.AsParallel().ForAll(item =>
        //            {
        //                item.Risk.Policy = policy;
        //                item.Risk.Status = RiskStatusType.Modified;
        //                item = DelegateService.suretyModificationService.GetDataModification(item, CoverageStatusType.Modified);
        //                var surety = DelegateService.suretyService.CreateSuretyTemporal(item, false);
        //            });
        //            risks = companySuretys.Select(x => x.Risk).ToList();
        //            policy = provider.CalculatePolicyAmounts(policy, risks);
        //            policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
        //            List<CompanyContract> suretys = DelegateService.suretyService.GetCompanySuretiesByTemporalId(policy.Id);

        //            var message = DelegateService.baseEndorsementService.ValidateEndorsement(policy.Id);
        //            if (message != string.Empty)
        //            {
        //                throw new Exception(message);
        //            }
        //            CVSE.SuretyServiceEEProvider suretyServiceEEProvider = new CVSE.SuretyServiceEEProvider();
        //            return suretyServiceEEProvider.CreateEndorsement(policy, suretys);
        //        }
        //        else
        //        {
        //            throw new Exception(Errors.ErrorSuretyNotFound);
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

    }
}
